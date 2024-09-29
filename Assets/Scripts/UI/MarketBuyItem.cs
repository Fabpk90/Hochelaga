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
}
