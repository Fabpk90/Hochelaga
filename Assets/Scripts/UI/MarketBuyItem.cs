using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MarketBuyItem : MarketItem, IPointerClickHandler
{
    public override void InitPrice()
    {
        base.InitPrice();

        priceText.text = "" + item.priceBuy;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MoneyManager.instance.amount >= item.priceBuy)
        {
            MoneyManager.instance.RemoveMoney(item.priceBuy);    
            UIInventory.Instance.AddDraggableElement(item);
        }
    }
}
