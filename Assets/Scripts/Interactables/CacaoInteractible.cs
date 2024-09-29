using UnityEngine.InputSystem;

public class CacaoInteractible : Interactables
{

	public int amount = 1;

	public override void PlayerCollisioned(PlayerController _player)
	{
		base.PlayerCollisioned(_player);

		_player.controls.Interact.Enable();
		_player.controls.Interact.Interact.performed += InteractOnperformed; // This should have been in the interactables class eheh
		_player.controls.Interact.Close.performed += CloseOnperformed;
	}

	private void CloseOnperformed(InputAction.CallbackContext obj)
	{
	}

	public override void InteractOnperformed(InputAction.CallbackContext obj)
	{
		if (objectIsNearby)
		{
			base.InteractOnperformed(obj);

			MoneyManager.instance.AddMoney(amount);
		}

	}
}
