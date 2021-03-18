using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    BoxCollider2D coll;

    public bool newGame;
    public bool continueGame;
    public bool isExit;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();

        if (isExit) GameManager.instance.IsExitDoor(this);

        coll.enabled = false;
    }

    public void OpenDoor()
    {
        anim.SetTrigger("opening");
        coll.enabled = true;
    }

    public void CloseDoor()
    {
        anim.SetTrigger("closing");
        coll.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (newGame)
            {
                GameManager.instance.NewGame();
            }
            else if (continueGame)
            {
                GameManager.instance.ContinueGame();
            }
            else
            {
                GameManager.instance.NextLevel();
            }
        }
    }
}
