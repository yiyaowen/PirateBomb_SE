using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius;
    public LayerMask explosionTargetLayer;
    public float explosionForce;
    public float damageAmount;

    Collider2D coll;
    Animator anim;
    Rigidbody2D rb;
    AudioSource audioSource;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSource.volume = GameManager.instance.realEffectsVolume;

        var collisionList = new List<Collider2D>();
        coll.OverlapCollider(new ContactFilter2D(), collisionList);
        foreach (var collision in collisionList)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("NPCs") || collision.CompareTag("Cannon Ball"))
            {
                audioSource.time = audioSource.clip.length - 1;
                audioSource.Play();
                Explosion();
            }
        }
    }

    public void Explosion()
    {
        coll.enabled = false;
        rb.gravityScale = 0;

        anim.SetTrigger("explosion");

        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionTargetLayer);

        foreach (var item in aroundObjects)
        {
            if (item.CompareTag("Bomb") && item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }
            else if (item.CompareTag("Player") || item.CompareTag("NPCs"))
            {
                var damagePoint = item.GetComponent<IDamageable>();
                if (damagePoint != null)
                {
                    damagePoint.GetHit(damageAmount);
                }
            }
            else if (item.CompareTag("Mask"))
            {
                item.gameObject.SetActive(false);
                item.GetComponent<TerrainMask>().OnTriggerEvent();
            }
            else if (item.CompareTag("Cannon Ball"))
            {
                item.GetComponent<CannonBall>().Explosion();
            }

            Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();
            if (itemRb != null)
            {
                Vector3 pos = item.transform.position - transform.position;
                itemRb.AddForce((pos + Vector3.up) * explosionForce, ForceMode2D.Impulse);
            }
        }
    }

    public void DestroySelf() // animation event
    {
        Destroy(gameObject);
    }
}
