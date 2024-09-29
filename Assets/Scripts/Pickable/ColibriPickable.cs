using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColibriPickable : Pickable
{
	public override void PickedUp(PlayerController _player)
	{
		base.PickedUp(_player);

		Destroy(gameObject);
	}
}

