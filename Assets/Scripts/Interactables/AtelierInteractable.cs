using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AtelierInteractable : Interactables
{
	[Serializable]
	public class Recipe
	{
		public string nameButton;
		public List<Item> needed;
		public Item result;
	}

	public List<Recipe> recipes = new();
	public static AtelierInteractable Instance;
	private void Awake()
	{
		Instance = this;
	}
	public EventReference sfxCrafter;

	//interraction
	public override void PlayerCollisioned(PlayerController _player)
	{
		base.PlayerCollisioned(_player);

		_player.controls.Interact.Enable();
		_player.controls.Interact.Interact.performed += InteractOnperformed;
		_player.controls.Interact.Close.performed += CloseOnperformed;
	}

	private void CloseOnperformed(InputAction.CallbackContext obj)
	{
		UIInventory.Instance.OpenAtelier(false);
	}

	public override void InteractOnperformed(InputAction.CallbackContext obj)
	{
		base.InteractOnperformed(obj);
		PlayerController.instance.controls.Move.Disable();
		UIInventory.Instance.OpenAtelier(true);
		PlayerController.instance?.TwinkleE();
	}

	public override void PlayerUnCollisioned(PlayerController player)
	{
		player.controls.Interact.Interact.performed -= InteractOnperformed;
	}

	public void CreateObject(string buttonName)
	{
		Recipe recipeAsked = null;
		foreach (var recipe in recipes)
		{
			if (recipe.nameButton == buttonName)
			{
				recipeAsked = recipe;
				break;
			}
		}
		if (recipeAsked == null) return;

		bool canCraft = true;
		foreach (var item in recipeAsked.needed)
		{
			if (!UIInventory.Instance.HasObjectInInventory(item))
			{
				canCraft = false;
				break;
			}
		}
		if (!canCraft) return;

		foreach (var item in recipeAsked.needed)
		{
			UIInventory.Instance.RemoveDraggable(item);
		}

		UIInventory.Instance.AddDraggableElement(recipeAsked.result);
		if (!sfxCrafter.IsNull)
		{
			FMODUnity.RuntimeManager.PlayOneShot(sfxCrafter);
		}
	}

}
