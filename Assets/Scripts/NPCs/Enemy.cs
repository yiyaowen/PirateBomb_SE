using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public Animator animator { get; set; }
    public int animatorState { get; set; }

    public GameObject alarmSign { get; set; }

    [Header("Base State")]
    public float health;
    public bool isBoss;

    public bool isDead { get; set; }

    [Header("Move Settings")]
    public float moveSpeed;
    public Transform pointA, pointB;

    public Transform targetPoint { get; set; }

    [Header("Attack Setting")]
    public bool canAttack;
    public float attackRate;
    public float attackRange;
    public float damageAmount;

    float nextAttack = 0;
    float nextSkill = 0;

    public List<Transform> attackList { get; set; } = new List<Transform>();

    [Header("Skill Setting")]
    public float skillRate;
    public float skillRange;

    public EnemyState.Types initStateType = EnemyState.Types.IdleState;

    public EnemyState currentState { get; set; }
    public IdleState idleState { get; set; } = new IdleState();
    public PatrolState patrolState { get; set; } = new PatrolState();
    public AttackState attackState { get; set; } = new AttackState();

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        alarmSign = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        GameManager.instance.IsObjectToFinish(gameObject);
        ChangeState(initStateType);
        if (isBoss)
        {
            UIManager.instance.SetBossHealthBarActive(true);
            UIManager.instance.SetBossMaxHealth(health);
        }
    }

    protected virtual void Update()
    {
        if (isBoss)
        {
            UIManager.instance.UpdateBossHealth(health);
        }
        animator.SetBool("dead", isDead);
        if (isDead)
        {
            GameManager.instance.ObjectFinish(gameObject);
            gameObject.layer = LayerMask.NameToLayer("Default");
            return;
        }
        currentState.OnUpdate(this);
        animator.SetInteger("state", animatorState);
    }

    public void ChangeState(EnemyState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void ChangeState(EnemyState.Types type)
    {
        switch (type)
        {
            case EnemyState.Types.IdleState:
                ChangeState(idleState);
                break;
            case EnemyState.Types.PatrolState:
                ChangeState(patrolState);
                break;
            case EnemyState.Types.AttackState:
                ChangeState(attackState);
                break;
            default:
                break;
        }
    }

    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
        FlipDirection();
    }

    public virtual void AttackAction()
    {
        if (Time.time > nextAttack && Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            animator.SetTrigger("attack");
            nextAttack = Time.time + attackRate;
        }
    }

    public virtual void SkillAction()
    {
        if (Time.time > nextSkill && Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            animator.SetTrigger("skill");
            nextSkill = Time.time + skillRate;
        }
    }

    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
        }
        animator.SetTrigger("hit");
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

    public void UpdateTargetPoint()
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
}
