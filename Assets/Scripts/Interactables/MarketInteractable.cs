using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarketInteractable : Interactables
{
    public override void PlayerCollisioned(PlayerController _player)
    {
        base.PlayerCollisioned(_player);
        
        _player.controls.Interact.Enable();
        _player.controls.Interact.Interact.performed += InteractOnperformed; // This should have been in the interactables class eheh
        _player.controls.Interact.Close.performed += CloseOnperformed;
    }

    private void CloseOnperformed(InputAction.CallbackContext obj)
    {
        objectToActivate?.SetActive(false);
        PlayerController.instance.controls.Move.Enable();
    }

    public override void InteractOnperformed(InputAction.CallbackContext obj)
    {
        if (objectIsNearby)
		{
			base.InteractOnperformed(obj);
			objectToActivate?.SetActive(true);
            PlayerController.instance.controls.Move.Disable();    
        }
        
    }

    public void UiCloseButton()
    {
        objectToActivate?.SetActive(false);
        PlayerController.instance.controls.Move.Enable();
    }
}
