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

    public virtual void PlayerCollisioned(PlayerController _player)
    {
        objectIsNearby = true;
        PlayerController.instance.ShowE(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            PlayerCollisioned(player);
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            PlayerUnCollisioned(player);
        }
    }

    public virtual void PlayerUnCollisioned(PlayerController player)
    {
        objectIsNearby = false;
        player.controls.Interact.Disable();
		PlayerController.instance.ShowE(false);
	}

	public virtual void InteractOnperformed(InputAction.CallbackContext obj)
	{
		if (!sfx.IsNull)
		{
			FMODUnity.RuntimeManager.PlayOneShot(sfx);
		}
	}
    
}
