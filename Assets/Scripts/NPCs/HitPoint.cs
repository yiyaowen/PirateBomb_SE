using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitPoint : MonoBehaviour
{
    public Enemy parentCharacter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().GetHit(parentCharacter.damageAmount);
        }
    }
}
