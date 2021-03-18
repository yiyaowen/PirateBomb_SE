using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private Animator anim;
    private FixedJoystick joystick;

    public float speed;
    public float jumpForce;

    [Header("Player State")]
    public float health;
    public bool isDead;

    [Header("Ground Check")]
    public Transform groundCheck;
    public Vector2 checkSize;
    public LayerMask groundLayer;

    [Header("States Check")]
    public bool isGround;
    public bool canJump;

    [Header("Jump FX")]
    public GameObject jumpFX;
    public GameObject landFX;

    [Header("Attack Settings")]
    public bool canAttack;
    public GameObject bombPrefab;
    public float nextAttack = 0;
    public float attackRate;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>();

        GameManager.instance.IsPlayer(this);

        health = GameManager.instance.LoadHealth();
        UIManager.instance.UpdateHealth(health);
    }

    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead) return;
        PhysicsCheck();
        CheckInput();
    }

    void CheckInput()
	{
        if (Input.GetButtonDown("Jump") && isGround)
		{
            canJump = true;
		}

        if (canAttack && Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
	}

	private void FixedUpdate()
	{
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Movement();
        Jump();
	}

	void Movement()
    {
        // 键盘操作
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 ~ 1 including float number

        // 操纵杆
        //float horizontalInput = joystick.Horizontal;

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        //if (horizontalInput != 0)
		//{
            //transform.localScale = new Vector3(horizontalInput, 1, 1);
		//}

        if (horizontalInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (horizontalInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public void ButtonJump()
    {
        if (isGround)
        {
            JumpDirectly();
        }
    }

    public void Jump()
	{
        if (canJump)
		{
            JumpDirectly();
            canJump = false;
		}
	}

    public void JumpDirectly()
    {
        jumpFX.SetActive(true);
        jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void Attack()
    {
        if (Time.time > nextAttack)
        {
            Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);

            nextAttack = Time.time + attackRate;
        }
    }

    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapBox(groundCheck.position, checkSize, 0.0f, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
        }
        else
        {
            rb.gravityScale = 4;
        }
    }

    public void LandFX()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, checkSize);
    }

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("player_hit"))
        {
            health -= damage;
            if (health < 1)
            {
                health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");

            UIManager.instance.UpdateHealth(health);
        }
    }
}
