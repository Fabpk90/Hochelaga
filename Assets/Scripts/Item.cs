using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public string idName;
    public string idDescr;
    public VisualTreeAsset visualAsset;
    public Sprite imageForOldUiSystem;
    public bool canBeBought = true;
    public bool canBeSold = true;
    public int priceBuy = 1;
    public int priceSell = 0;
}
