using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleInteractable : Interactables
{
    public override void PlayerCollisioned(PlayerController _player)
    {
        base.PlayerCollisioned(_player);
        
        _player.controls.Disable();
        UIInventory.Instance.OpenStele(true);
    }
}
