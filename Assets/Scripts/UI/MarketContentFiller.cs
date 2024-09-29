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
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/ScriptableObjects" });
        foreach (string SOName in assetNames)
        {
            var SOpath    = AssetDatabase.GUIDToAssetPath(SOName);
            var item = AssetDatabase.LoadAssetAtPath<Item>(SOpath);

            if (item.canBeBought == isBuySide) // we could have used the filter here (two folders for this) but eh, game jam right ?
            {
                items.Add(item);    
            }
            else if (item.canBeSold)
            {
                items.Add(item);
            }
        }
    }
}
