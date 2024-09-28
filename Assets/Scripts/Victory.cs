using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
	public static Victory Instance;

	private void Awake()
	{
		Instance = this;
	}

	public void Verify()
	{
		(Item, Item, Item) items = UIInventory.Instance.GetOrder();
	}
}
