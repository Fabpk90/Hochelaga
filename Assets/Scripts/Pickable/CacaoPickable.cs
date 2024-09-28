using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacaoPickable : Pickable
{
    public int amount = 1;
    public override void PickedUp(PlayerController _player)
    {
        base.PickedUp(_player);
        
        MoneyManager.instance.AddMoney(amount);
        
        print("Pickup Cacao");
        Destroy(gameObject);
    }
}
