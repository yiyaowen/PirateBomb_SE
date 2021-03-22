using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState()
    {
        type = Types.IdleState;
    }

    public override void EnterState(Enemy enemy)
    {
        enemy.animatorState = (int)AnimatorTypes.IdleState;
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (enemy.attackList.Count > 0 && enemy.canAttack)
        {
            enemy.ChangeState(enemy.attackState);
        }
    }
}
