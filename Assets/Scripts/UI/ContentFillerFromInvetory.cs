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
		
		UIInventory.Instance.OnItemInInventoryAdded += OnItemInInventoryAdded;

		foreach (var item in items)
		{
			var itemSpawn = Instantiate<MarketItem>(itemPrefab, transform);
			itemSpawn.item = item; // could have been in the ctor oopsi
			itemSpawn.Init();
		}
	}

	private void OnItemInInventoryAdded(object sender, Item item)
	{
		var itemSpawn = Instantiate<MarketItem>(itemPrefab, transform);
		itemSpawn.item = item; // could have been in the ctor oopsi
		itemSpawn.Init();
	}

	void PopulateList()
	{
		items.Clear();
		var inventory = UIInventory.Instance.GetInventory();
		foreach (var item in inventory)
		{
			if (item.canBeSold)
			{
				items.Add(item);
			}
		}
	}

}
