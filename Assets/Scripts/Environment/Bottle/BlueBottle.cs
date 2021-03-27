using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBottle : Bottle
{
    [Header("Attack Power Enhance")]
    public float attackPowerScale;
    public float continueSecs;

    BlueBottle()
    {
        type = Type.BlueBottle;
    }

    public override void DrinkBy(PlayerController player)
    {
        base.DrinkBy(player);
        StartCoroutine("MorePowerfulBomb", player);
    }

    IEnumerator MorePowerfulBomb(PlayerController player)
    {
        var originalDamage = player.bombPrefab.GetComponent<Bomb>().damageAmount;
        player.bombPrefab.GetComponent<Bomb>().damageAmount *= attackPowerScale;
        yield return new WaitForSeconds(continueSecs);
        player.bombPrefab.GetComponent<Bomb>().damageAmount = originalDamage;
        // Buff 完成后销毁自身
        Destroy(gameObject);
    }
}
