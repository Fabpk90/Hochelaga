using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MarketItem : MonoBehaviour
{
    public Item item;
    // Start is called before the first frame update

    public TextMeshProUGUI descriptionText;
    public Image imageItem;

    public Sprite imageCacao;
    
    public Image imageCacaoUi;
    public TextMeshProUGUI priceText;

    public void Init()
    {
        descriptionText.text = TextManager.GetTextByID(item.idName);
        imageItem.sprite = item.imageForOldUiSystem;

        imageCacaoUi.sprite = imageCacao;
        InitPrice();
    }
    
    public virtual void InitPrice(){}
}
