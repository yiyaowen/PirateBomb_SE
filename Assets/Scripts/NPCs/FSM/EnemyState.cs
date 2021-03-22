public abstract class EnemyState
{
    public enum AnimatorTypes
    {
        IdleState= 0,
        RunState = 1,
        AttackState = 2
    }

    public enum Types
    {
        IdleState = 0,
        PatrolState = 1,
        AttackState = 2
    }

    public Types type { get; set; } = Types.IdleState;

    public abstract void EnterState(Enemy enemy);

    public abstract void OnUpdate(Enemy enemy);
}
