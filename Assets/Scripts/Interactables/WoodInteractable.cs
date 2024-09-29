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

	public override void InteractOnperformed(InputAction.CallbackContext obj)
	{
		base.InteractOnperformed(obj);
		UIInventory.Instance.AddDraggableElement(woodItem);
	}

	public override void PlayerUnCollisioned(PlayerController player)
	{
		player.controls.Interact.Interact.performed -= InteractOnperformed;
	}
}