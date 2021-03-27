using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject cannonBallPrefab;
    public float cannonBallOffset;
    public bool canAttack;
    public float attackRate;
    public float throwAngle;
    public float throwForce;
    public BoxCollider2D attackArea;

    float nextAttack = 0;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 检测是否有炮手
        canAttack = false;
        var cannoneerList = new List<Collider2D>();
        GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), cannoneerList);
        foreach (var cannoneer in cannoneerList)
        {
            Enemy tmpEnemy;
            if (cannoneer.CompareTag("NPCs") && (tmpEnemy = cannoneer.GetComponent<Enemy>()) != null && !tmpEnemy.isDead)
            {
                canAttack = true;
                break;
            }
        }

        // 进行攻击
        if (canAttack && Time.time > nextAttack + attackRate)
        {
            nextAttack = Time.time;

            var targetList = new List<Collider2D>();
            attackArea.OverlapCollider(new ContactFilter2D(), targetList);

            foreach (var target in targetList)
            {
                if (target.CompareTag("Player"))
                {
                    anim.SetTrigger("attack");
                }
            }
        }
    }

    public void Attack() // animation event
    {
        var throwDir = cannonBallOffset < 0 ? Vector2.left : Vector2.right;
        var cannonBallOriginPos = new Vector3(transform.position.x + cannonBallOffset, transform.position.y, transform.position.z);
        var cannonBall = Instantiate(cannonBallPrefab, cannonBallOriginPos, transform.rotation);
        var theta = throwAngle * Mathf.PI / 180.0f;

        cannonBall.GetComponent<Rigidbody2D>().AddForce(
            new Vector2(throwDir.x * Mathf.Cos(theta), Mathf.Abs(throwDir.x * Mathf.Sin(theta))) * throwForce,
            ForceMode2D.Impulse
        );
    }
}
