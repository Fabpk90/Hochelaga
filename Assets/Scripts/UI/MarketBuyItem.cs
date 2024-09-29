using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MarketBuyItem : MarketItem, IPointerDownHandler
{
    public override void InitPrice()
    {
        base.InitPrice();

        priceText.text = "" + item.priceBuy;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MoneyManager.instance.amount >= item.priceBuy)
        {
            MoneyManager.instance.RemoveMoney(item.priceSell);    
            UIInventory.Instance.AddDraggableElement(item);
        }
    }
}
