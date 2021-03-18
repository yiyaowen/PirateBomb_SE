using UnityEngine;

public class BigGuy : Enemy, IDamageable
{
    public Transform pickupPoint;
    public float power;

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

    public void PickUpBomb() // Animation Event
    {
        if (targetPoint.CompareTag("Bomb") && !hasBomb)
        {
            targetPoint.gameObject.transform.position = pickupPoint.position;

            targetPoint.SetParent(pickupPoint);

            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            hasBomb = true;
        }
    }

    public void ThrowAway() // Animation Event
    {
        if (hasBomb)
        {
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            targetPoint.SetParent(transform.parent.parent); // nb!

            int dir = FindObjectOfType<PlayerController>().gameObject.transform.position.x - transform.position.x < 0 ? -1 : 1;
            targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * power, ForceMode2D.Impulse);
        }

        hasBomb = false;
    }
}
