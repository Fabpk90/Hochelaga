using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public string idName;
    public string idDescr;
    public VisualTreeAsset visualAsset;
}
