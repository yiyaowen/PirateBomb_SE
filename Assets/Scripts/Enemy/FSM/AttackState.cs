using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 2;
        enemy.targetPoint = enemy.attackList[0];
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (enemy.hasBomb) return;
        try
        {
            if (enemy.attackList.Count == 0)
            {
                enemy.TransitionToState(enemy.patrolState);
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
            // 优先采取攻击行动
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
                enemy.TransitionToState(enemy.attackState);
            }
            else
            {
                enemy.TransitionToState(enemy.attackState);
            }
        }
    }
}
