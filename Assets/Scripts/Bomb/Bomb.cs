using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    [Header("Properties")]
    public float waitTime;
    public float bombForce;
    public float damageAmount;

    public float startTime { get; set; }
    public bool isCharging { get; set; } = true;

    [Header("Explosion Area")]
    public float explosionRadius;
    public LayerMask explosionTargetLayer;

    [Header("Sound Effects")]
    public AudioClip explosionSound;
    public float advancePlaySecs;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = explosionSound;
        audioSource.Play();
        startTime = Time.time;
    }

    bool isAdvance; // 判断爆炸音效是否已经提前（只需提前一次即可）
    void Update()
    {
        audioSource.volume = GameManager.instance.realEffectsVolume;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("bomb_on"))
        {
            if (Time.time > startTime + waitTime - advancePlaySecs && !isAdvance)
            {
                isAdvance = true;
                audioSource.time = audioSource.clip.length - 1;
            }
            if (Time.time > startTime + waitTime)
            {
                anim.SetTrigger("explosion");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void Explosion() // animation event
    {
        coll.enabled = false;
        rb.gravityScale = 0;

        transform.GetChild(0).gameObject.SetActive(false);

        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionTargetLayer);

        foreach (var item in aroundObjects)
        {
            if (item.CompareTag("Bomb") && !item.GetComponent<Bomb>().isCharging)
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
                var mask = item.GetComponent<TerrainMask>();
                if (mask.canTrigger)
                {
                    item.gameObject.SetActive(false);
                    mask.OnTriggerEvent();
                }
            }
            else if (item.CompareTag("Cannon Ball"))
            {
                item.GetComponent<CannonBall>().Explosion();
            }

            Rigidbody2D itemRb = item.GetComponent<Rigidbody2D>();
            if (itemRb != null)
            {
                Vector3 pos = item.transform.position - transform.position;
                itemRb.AddForce((pos + Vector3.up) * bombForce, ForceMode2D.Impulse);
            }
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject); 
    }

    public void TurnOff()
    {
        isCharging = false;

        audioSource.Pause();
        audioSource.time = 0;
        isAdvance = false;

        anim.SetTrigger("off");

        gameObject.layer = LayerMask.NameToLayer("Inactive Bomb");
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void TurnOn()
    {
        startTime = Time.time;
        isCharging = true;

        audioSource.Play();

        anim.SetTrigger("on");

        gameObject.layer = LayerMask.NameToLayer("Bomb");
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
