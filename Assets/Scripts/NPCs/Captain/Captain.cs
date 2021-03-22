using UnityEngine;

public class Captain : Enemy
{
    [Header("Special Settings")]
    public float scareRunSpeedScale;

    SpriteRenderer sprite;

    protected override void Awake()
    {
        base.Awake();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        sprite.flipX = animator.GetCurrentAnimatorStateInfo(1).IsName("captain_scare_run");
    }

    public override void SkillAction()
    {
        base.SkillAction();

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("captain_scare_run"))
        {
            Vector3 dir = transform.position.x > targetPoint.position.x ? Vector3.right : Vector3.left;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + dir, moveSpeed * scareRunSpeedScale * Time.deltaTime);
        }
    }
}
