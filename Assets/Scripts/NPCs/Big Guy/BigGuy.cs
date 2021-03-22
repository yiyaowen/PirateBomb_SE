using UnityEngine;

public class BigGuy : Enemy
{
    [Header("Special Settings")]
    public float throwForce;
    public Transform pickUpPoint;

    bool hasBomb;
    Transform bombOriginalParent;

    public void PickUpBomb() // Animation Event
    {
        if (targetPoint.CompareTag("Bomb") && !hasBomb)
        {
            bombOriginalParent = targetPoint.parent;
            targetPoint.SetParent(pickUpPoint);

            targetPoint.position = pickUpPoint.position;

            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            hasBomb = true;
            canAttack = false;
        }
    }

    public void ThrowAwayBomb() // Animation Event
    {
        if (hasBomb)
        {
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            targetPoint.SetParent(bombOriginalParent);

            int dir = FindObjectOfType<PlayerController>().transform.position.x - transform.position.x < 0 ? -1 : 1;
            targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir, 1) * throwForce, ForceMode2D.Impulse);

            hasBomb = false;
            canAttack = true;
        }
    }
}
