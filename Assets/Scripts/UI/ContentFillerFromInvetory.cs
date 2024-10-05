using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContentFillerFromInvetory : MonoBehaviour
{
	public MarketSellItem itemPrefab;

	public List<Item> items = new List<Item>();

	private List<MarketItem> itemsSpawned = new List<MarketItem>();

	void Start()
	{
		PopulateList();
		
		UIInventory.Instance.OnItemInInventoryAdded += OnItemInInventoryAdded;
		UIInventory.Instance.OnItemInInventoryRemoved += OnItemInInventoryRemoved;
	}

	private void OnItemInInventoryAdded(object sender, Item item)
	{
		if (!item.canBeSold) return;

		var itemSpawn = Instantiate<MarketItem>(itemPrefab, transform);
		itemSpawn.item = item; // could have been in the ctor oopsi
		itemSpawn.Init();
		
		itemsSpawned.Add(itemSpawn);
	}

	private void OnItemInInventoryRemoved(object sender, Item item)
	{
		foreach (var marketItem in itemsSpawned)
		{
			if (marketItem.item == item)
			{
				itemsSpawned.Remove(marketItem);
				items.Remove(item);
				Destroy(marketItem.gameObject);
				return;
			}
		}
	}

	void PopulateList()
	{
		foreach (var marketItem in itemsSpawned)
		{
			Destroy(marketItem.gameObject);
		}
		
		itemsSpawned.Clear();
		items.Clear();
		
		var inventory = UIInventory.Instance.GetInventory();
		foreach (var item in inventory)
		{
			if (item.canBeSold)
			{
				items.Add(item);
			}
		}
		
		foreach (var item in items)
		{
			var itemSpawn = Instantiate<MarketItem>(itemPrefab, transform);
			itemSpawn.item = item; // could have been in the ctor oopsi
			itemSpawn.Init();
			
			itemsSpawned.Add(itemSpawn);
		}
	}

}
