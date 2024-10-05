using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Sprite))]
public class Interactables : MonoBehaviour
{
    public GameObject objectToActivate;

    protected bool objectIsNearby = false;
    
	public EventReference sfx;

    public virtual void PlayerCollisioned(PlayerController player)
    {
        objectIsNearby = true;
		player.ShowE(true);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            PlayerCollisioned(player);
        }

    }

	private void OnTriggerExit2D(Collider2D collision)
	{
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            PlayerUnCollisioned(player);
        }
	}

	public virtual void PlayerUnCollisioned(PlayerController player)
    {
        objectIsNearby = false;
        player.controls.Interact.Disable();
        player.ShowE(false);
	}

	public virtual void InteractOnperformed(InputAction.CallbackContext obj)
	{
		if (!sfx.IsNull)
		{
			FMODUnity.RuntimeManager.PlayOneShot(sfx);
		}
	}
    
}
