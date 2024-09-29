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
	// Start is called before the first frame update

	private void Awake()
	{
		PopulateList();
	}

	void Start()
	{
		PopulateList();
		
		UIInventory.Instance.OnItemInInventoryAdded += OnItemInInventoryAdded;
	}

	private void OnItemInInventoryAdded(object sender, Item item)
	{
		var itemSpawn = Instantiate<MarketItem>(itemPrefab, transform);
		itemSpawn.item = item; // could have been in the ctor oopsi
		itemSpawn.Init();
		
		itemsSpawned.Add(itemSpawn);
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
