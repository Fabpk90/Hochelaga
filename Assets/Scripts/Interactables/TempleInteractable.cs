using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempleInteractable : Interactables
{
    public override void PlayerCollisioned(PlayerController _player)
    {
        base.PlayerCollisioned(_player);
        
        _player.controls.Interact.Enable();
        _player.controls.Interact.Interact.performed += InteractOnperformed;
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
