using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WoodInteractable : Interactables
{
	public Item woodItem;

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
		UIInventory.Instance.AddDraggableElement(woodItem);
	}

	public override void PlayerUnCollisioned(PlayerController player)
	{
		player.controls.Interact.Interact.performed -= InteractOnperformed;
	}
}