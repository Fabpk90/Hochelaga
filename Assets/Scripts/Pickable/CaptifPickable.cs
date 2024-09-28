using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptifPickable : Pickable
{
    [SerializeField] private Item item;
    public override void PickedUp(PlayerController _player)
    {
        base.PickedUp(_player);

        UIInventory.Instance.AddDraggableElement(item);
    }
}
