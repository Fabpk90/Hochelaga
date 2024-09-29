using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketBuyItem : MarketItem
{
    public override void InitPrice()
    {
        base.InitPrice();

        priceText.text = "" + item.priceBuy;
    }

    private void OnMouseDown()
    {
        if (MoneyManager.instance.amount >= item.priceBuy)
        {
            MoneyManager.instance.RemoveMoney(item.priceSell);    
        }
        
    }
}