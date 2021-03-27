using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBottle : Bottle
{
    [Header("Healing Settings")]
    public float singleAmount;
    public float totalCount;
    public float intervalSecs;

    RedBottle()
    {
        type = Type.RedBottle;
    }

    public override void DrinkBy(PlayerController player)
    {
        base.DrinkBy(player);
        StartCoroutine("Heal", player);
    }

    IEnumerator Heal(PlayerController player)
    {
        for (int i = 0; i< totalCount; ++i)
        {
            player.GetHeal(singleAmount);
            yield return new WaitForSeconds(intervalSecs);
        }
        // Buff 完成后销毁自身
        Destroy(gameObject);
    }
}
