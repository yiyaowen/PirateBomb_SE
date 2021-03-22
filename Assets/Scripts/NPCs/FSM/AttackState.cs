using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState()
    {
        type = Types.AttackState;
    }

    public override void EnterState(Enemy enemy)
    {
        enemy.animatorState = (int)AnimatorTypes.AttackState;
        enemy.targetPoint = enemy.attackList[0];
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (!enemy.canAttack) return;
        try
        {
            if (enemy.attackList.Count == 0)
            {
                enemy.ChangeState(enemy.patrolState);
            }
            // 获取最近的攻击目标
            if (enemy.attackList.Count > 1)
            {
                for (int i = 0; i < enemy.attackList.Count; ++i)
                {
                    if (Mathf.Abs(enemy.transform.position.x - enemy.attackList[i].position.x)
                        < Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                    {
                        enemy.targetPoint = enemy.attackList[i];
                    }
                }
            }
            if (enemy.attackList.Count == 1)
            {
                enemy.targetPoint = enemy.attackList[0];
            }
            // 优先对玩家进行普通攻击，再释放技能
            if (enemy.targetPoint.CompareTag("Player"))
            {
                enemy.AttackAction();
            }
            else if (enemy.targetPoint.CompareTag("Bomb"))
            {
                enemy.SkillAction();
            }
            // 切换为移动状态并追随目标
            enemy.MoveToTarget();
        }
        catch (MissingReferenceException)
        {
            // 如果攻击目标在函数执行期间被销毁，则重新获取新的攻击目标
            if (enemy.attackList.Count > 0)
            {
                enemy.ChangeState(enemy.attackState);
            }
        }
    }
}
