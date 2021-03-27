using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBottle : Bottle
{
    [Header("Attack Frequence Enhance")]
    public float attackRateScale;
    public float continueSecs;

    GreenBottle()
    {
        type = Type.GreenBottle;
    }

    public override void DrinkBy(PlayerController player)
    {
        base.DrinkBy(player);
        StartCoroutine("MoreBomb", player);
    }

    IEnumerator MoreBomb(PlayerController player)
    {
        var originalRate = player.attackRate;
        player.attackRate *= attackRateScale;
        yield return new WaitForSeconds(continueSecs);
        player.attackRate = originalRate;
        // Buff 完成后销毁自身
        Destroy(gameObject);
    }
}
