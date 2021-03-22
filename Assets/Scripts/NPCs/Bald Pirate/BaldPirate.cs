using UnityEngine;

public class BaldPirate : Enemy
{
    [Header("Special Settings")]
    public float kickForce;

    public void KickOffBomb()
    {
        if (targetPoint.CompareTag("Bomb"))
        {
            int dir = targetPoint.transform.position.x < transform.position.x ? -1 : 1;
            targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * kickForce, ForceMode2D.Impulse);
        }
    }
}
