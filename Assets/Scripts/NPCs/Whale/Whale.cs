using UnityEngine;

public class Whale : Enemy
{
    public float swellScale;

    public void SwalowBomb() // Animation Event
    {
        targetPoint.GetComponent<Bomb>().TurnOff();
        targetPoint.gameObject.SetActive(false);

        transform.localScale *= swellScale;
    }
}
