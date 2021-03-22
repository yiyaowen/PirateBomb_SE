using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyState
{
    public PatrolState()
    {
        type = Types.IdleState;
    }

    public override void EnterState(Enemy enemy)
    {
        enemy.animatorState = (int)AnimatorTypes.IdleState;
        enemy.UpdateTargetPoint();
    }

    public override void OnUpdate(Enemy enemy)
    {
        AnimatorStateInfo stateInfo = enemy.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("idle") && stateInfo.normalizedTime >= 1.0f)
        {
            enemy.animatorState = (int)AnimatorTypes.RunState;
        }
        else if (stateInfo.IsName("run"))
        {
            enemy.MoveToTarget();
        }

        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f)
        {
            enemy.ChangeState(enemy.patrolState);
        }
        if (enemy.attackList.Count > 0 && enemy.canAttack)
        {
            enemy.ChangeState(enemy.attackState);
        }
    }
}
