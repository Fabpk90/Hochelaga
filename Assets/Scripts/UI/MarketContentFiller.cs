using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MarketContentFiller : MonoBehaviour
{
    public bool isBuySide = true;

    public MarketItem prefabToSpawn;
    
    public List<Item> items = new List<Item>();
    // Start is called before the first frame update
    void Start()
    {
        PopulateList();

        foreach (var item in items)
        {
            var itemSpawn = Instantiate<MarketItem>(prefabToSpawn, transform);
            itemSpawn.item = item; // could have been in the ctor oopsi
            itemSpawn.Init();
        }
    }

    void PopulateList()
    {
        var itemsLoaded = Resources.LoadAll<Item>("ScriptableObjects");
        foreach (var item in itemsLoaded)
        {
            if (item.canBeBought == isBuySide)
            {
                items.Add(item);    
            }
        }
    }
}
