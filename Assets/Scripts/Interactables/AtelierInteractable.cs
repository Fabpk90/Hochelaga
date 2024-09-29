using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AtelierInteractable : Interactables
{
	[Serializable]
	private class Recipe
	{
		public string nameButton;
		public List<Item> needed;
		public Item result;
	}

	[SerializeField] private List<Recipe> recipes = new();

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
		UIInventory.Instance.OpenStele(false);
	}

	private void InteractOnperformed(InputAction.CallbackContext obj)
	{
		PlayerController.instance.controls.Move.Disable();
		UIInventory.Instance.OpenStele(true);
	}

	public override void PlayerUnCollisioned(PlayerController player)
	{
		player.controls.Interact.Interact.performed -= InteractOnperformed;
	}
}
