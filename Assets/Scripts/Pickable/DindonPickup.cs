using UnityEngine;
public class DindonPickup : Pickable
{
    public override void PickedUp(PlayerController _player)
    {
        base.PickedUp(_player);
        
        print("dindon picked up");

        _player.PickupDindon();
    }
}