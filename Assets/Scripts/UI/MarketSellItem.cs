using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MarketSellItem : MarketItem, IPointerDownHandler
{
    public override void InitPrice()
    {
        base.InitPrice();

        priceText.text = "" + item.priceSell;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        UIInventory.Instance.RemoveDraggable(item);
        MoneyManager.instance.AddMoney(item.priceSell);
        Destroy(gameObject);
    }
}
