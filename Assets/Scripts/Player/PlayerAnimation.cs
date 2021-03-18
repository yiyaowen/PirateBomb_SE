using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController controller;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x)); // Notice if velocity.x is less than 0
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("ground", controller.isGround);
    }
}
