using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public Enemy character;

    [Header("Hit Bomb")]
    public bool bombAvailable;
    public float hitForce;

    private int dir;

    private void OnTriggerEnter2D(Collider2D other)
    {
        dir = transform.position.x > other.transform.position.x ? -1 : 1;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().GetHit(character.damageAmount);
        }

        if (other.CompareTag("Bomb") && bombAvailable)
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * hitForce, ForceMode2D.Impulse);
        }
    }
}
