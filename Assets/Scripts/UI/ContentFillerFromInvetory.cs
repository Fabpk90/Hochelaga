using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContentFillerFromInvetory : MonoBehaviour
{
	public MarketSellItem itemPrefab;

	public List<Item> items = new List<Item>();
	// Start is called before the first frame update
	void Start()
	{
		PopulateList();

		foreach (var item in items)
		{
			var itemSpawn = Instantiate<MarketItem>(itemPrefab, transform);
			itemSpawn.item = item; // could have been in the ctor oopsi
			itemSpawn.Init();
		}
	}

	void PopulateList()
	{
		items = UIInventory.Instance.GetInventory();
		foreach (var item in items)
		{
			if (!item.canBeSold)
			{
				items.Remove(item);
			}
		}
	}

}
