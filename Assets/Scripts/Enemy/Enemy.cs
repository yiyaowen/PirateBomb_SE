using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;

    public Animator anim;
    public int animState;

    private GameObject alarmSign;

    [Header("Base State")]
    public float health;
    public bool isDead;
    public bool hasBomb;
    public bool isBoss;

    [Header("Movement")]
    public float speed;

    public Transform pointA, pointB;
    public Transform targetPoint;

    private float nextAttack = 0;
    private float nextSkill = 0;
    [Header("Attack Setting")]
    public float attackRate;
    public float attackRange;
    public float damageAmount;

    [Header("Skill Setting")]
    public float skillRate;
    public float skillRange;

    public List<Transform> attackList = new List<Transform>();

    public PatrolState patrolState = new PatrolState();
    public AttackState attackState = new AttackState();

    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;
    }

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        GameManager.instance.IsEnemy(this);
        TransitionToState(patrolState);
        if (isBoss)
        {
            UIManager.instance.EnableBossHealthBar();
            UIManager.instance.SetBossHealth(health);
        }
    }

    public virtual void Update()
    {
        if (isBoss)
        {
            UIManager.instance.UpdateBossHealth(health);
        }
        anim.SetBool("dead", isDead);
        if (isDead)
        {
            GameManager.instance.EnemyDead(this);
            return;
        }
        currentState.OnUpdate(this);
        anim.SetInteger("state", animState);
    }

    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FlipDirection();
    }

    public virtual void AttackAction() // 普通攻击
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                // 播放普通攻击动画
                anim.SetTrigger("attack");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    public virtual void SkillAction() // 释放技能
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextSkill)
            {
                // 播放技能攻击动画
                anim.SetTrigger("skill");
                nextSkill = Time.time + skillRate;
            }
        }
    }

    public void FlipDirection()
    {
        if (transform.position.x < targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            attackList.Clear();
            return;
        }
        StartCoroutine("OnAlarm");
        attackList.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isDead)
        {
            attackList.Clear();
            return;
        }
        attackList.Remove(collision.transform);
    }

    IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }
}
