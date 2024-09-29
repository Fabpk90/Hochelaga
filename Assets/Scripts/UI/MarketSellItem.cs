using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketSellItem : MarketItem
{
    public override void InitPrice()
    {
        base.InitPrice();

        priceText.text = "" + item.priceSell;
    }

    private void OnMouseDown()
    {
        //UIInventory.Instance.Removeitem
        MoneyManager.instance.AddMoney(item.priceSell);
        Destroy(gameObject);
    }
}
