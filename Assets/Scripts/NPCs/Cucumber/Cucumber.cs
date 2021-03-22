using System;

public class Cucumber : Enemy
{
    public void BlowOutBomb() // Animation Event
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
}
