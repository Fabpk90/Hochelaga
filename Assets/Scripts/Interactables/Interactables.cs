using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

[RequireComponent(typeof(Sprite))]
public class Interactables : MonoBehaviour
{
    public GameObject objectToActivate;
	public EventReference sfx;

    public virtual void PlayerCollisioned(PlayerController _player)
    {
        _player.objectInteractingWith = objectToActivate;
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
        player.objectInteractingWith = null;
	}

	public virtual void InteractOnperformed(InputAction.CallbackContext obj)
	{
		if (!sfx.IsNull)
		{
			FMODUnity.RuntimeManager.PlayOneShot(sfx);
		}
	}
    
}
