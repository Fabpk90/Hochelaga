using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pickable : MonoBehaviour
{
	[SerializeField] private Item item;

	public EventReference sfx;
    public virtual void PickedUp(PlayerController _player)
    {
        if (!sfx.IsNull)
        {
            FMODUnity.RuntimeManager.PlayOneShot(sfx);
		}
		
        if(item!=null) UIInventory.Instance.AddDraggableElement(item);

	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if(player)
        {
            PickedUp(player);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if(player)
        {
            PickedUp(player);
        }
    }
}
