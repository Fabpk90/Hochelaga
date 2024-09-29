using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuetzalPickup : Pickable
{
	public override void PickedUp(PlayerController _player)
	{
		base.PickedUp(_player);

		Destroy(gameObject);
	}
}

