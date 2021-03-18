using UnityEngine;

public class Captain : Enemy, IDamageable
{
    private SpriteRenderer sprite;

    public override void Init()
    {
        base.Init();
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void Update()
    {
        base.Update();
        sprite.flipX = anim.GetCurrentAnimatorStateInfo(1).IsName("captain_scare_run");
    }

    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }

    public override void SkillAction()
    {
        base.SkillAction();

        if (anim.GetCurrentAnimatorStateInfo(1).IsName("captain_scare_run"))
        {
            Vector3 dir = transform.position.x > targetPoint.position.x ? Vector3.right : Vector3.left;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + dir, speed * 2 * Time.deltaTime);
        }
    }
}
