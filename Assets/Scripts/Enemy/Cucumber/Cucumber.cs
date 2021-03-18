using System;

public class Cucumber : Enemy, IDamageable
{
    public void SetOff() // Animation Event
    {
        try
        {
            targetPoint.GetComponent<Bomb>().TurnOff();
        }
        catch (NullReferenceException)
        {
            // 不进行任何行动
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
        anim.SetTrigger("hit");
    }
}
