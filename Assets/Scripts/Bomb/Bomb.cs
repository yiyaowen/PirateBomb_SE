using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;

    [Header("Properties")]
    public float waitTime;
    public float bombForce;
    public float damageAmount;

    public float startTime { get; set; }

    [Header("Explosion Area")]
    public float explosionRaidus;
    public LayerMask explosionTargetLayer;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("bomb_on"))
        {
            if (Time.time > startTime + waitTime)
            {
                anim.Play("bomb_explosion");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRaidus);
    }

    public void Explosion() // animation event
    {
        coll.enabled = false;
        rb.gravityScale = 0;

        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, explosionRaidus, explosionTargetLayer);

        foreach (var item in aroundObjects)
        {
            Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();
            if (itemRb)
            {
                Vector3 pos = item.transform.position - transform.position;
                itemRb.AddForce((pos + Vector3.up) * bombForce, ForceMode2D.Impulse);
            }

            if (item.CompareTag("Bomb") && item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bomb_off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }
            else if (item.CompareTag("Player") || item.CompareTag("NPCs"))
            {
                item.GetComponent<IDamageable>().GetHit(damageAmount);
            }
            else if (item.CompareTag("Mask"))
            {
                item.gameObject.SetActive(false);
                item.GetComponent<TerrainMask>().OnTriggerEvent();
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject); 
    }

    public void TurnOff()
    {
        anim.Play("bomb_off");
        gameObject.layer = LayerMask.NameToLayer("Environment");
    }

    public void TurnOn()
    {
        startTime = Time.time;
        anim.Play("bomb_on");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
    }
}
