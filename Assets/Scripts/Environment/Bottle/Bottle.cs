using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public virtual void DrinkBy(PlayerController player)
    {
        isUsed = true;
        GetComponent<SpriteRenderer>().enabled = false;
        // Buff 增益应该保留到下一关，直到持续时间结束
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    public bool isUsed { get; set; }

    public enum Type
    {
        RedBottle = 0,
        GreenBottle = 1,
        BlueBottle = 2
    }

    public Type type { get; set; }
}
