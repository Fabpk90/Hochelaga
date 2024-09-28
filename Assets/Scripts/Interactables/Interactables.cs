using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sprite))]
public class Interactables : MonoBehaviour
{
    public GameObject objectToActivate;
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
}
