using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public enum State
    {
        Static = 0,
        Roaming = 1,
        Battle = 2
    }

    [Header("State Settings")]
    public State state;

    [Header("Health Settings")]
    public float maxHealth;
    public float health { get; set; }
    public bool isDead { get; set; }

    [Header("Move Settings")]
    public float moveSpeed;
    public float jumpForce;
    public float gravityScaleOnGround;
    public float gravityScaleInAir;

    [Header("Attack Settings")]
    public GameObject bombPrefab;
    public float bombOffsetX;
    public float attackRate;

    private float nextAttack = 0;

    [Header("Ground Check")]
    public Transform checkPoint;
    public Vector2 checkSize;
    public LayerMask groundLayer;

    public bool isGround { get; set; }
    public bool canJump { get; set; }

    [Header("Special Effects")]
    public GameObject jumpFX;
    public GameObject landFX;

    private Rigidbody2D rb;
    private Animator anim;
    //private FixedJoystick joystick;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //joystick = FindObjectOfType<FixedJoystick>();

        GameManager.instance.IsPlayer(this);
        if (state >= State.Battle)
        {
            UIManager.instance.SetPlayerHealthBarActive(true);
            UpdateHealth();
        }
    }

    void Update()
    {
        if (state <= State.Static) return;

        anim.SetBool("dead", isDead);
        if (isDead) return;

        PhysicsCheck();
        GetUserInput();
    }

    private void FixedUpdate()
    {
        if (state <= State.Static) return;

        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Move();
        Jump();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(checkPoint.position, checkSize);
    }

    void GetUserInput()
	{
        if (Input.GetButtonDown("Jump") && isGround)
		{
            canJump = true;
		}

        if (Input.GetKeyDown(KeyCode.J) && state >= State.Battle)
        {
            Attack();
        }
	}

	void Move()
    {
        // 键盘操作
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // 操纵杆
        //float horizontalInput = joystick.Horizontal;

        if (horizontalInput != 0)
        {
            float flipAngle = horizontalInput < 0 ? 180 : 0;
            float offsetDirection = horizontalInput < 0 ? 1 : -1;
            transform.eulerAngles = new Vector3(0, flipAngle, 0);
            bombOffsetX = offsetDirection * Mathf.Abs(bombOffsetX);
        }

        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
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
        ShowJumpFX();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void Attack()
    {
        if (Time.time > nextAttack)
        {
            var bombPos = transform.position;
            bombPos.x += bombOffsetX;
            Instantiate(bombPrefab, bombPos, bombPrefab.transform.rotation);

            nextAttack = Time.time + attackRate;
        }
    }

    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapBox(checkPoint.position, checkSize, 0.0f, groundLayer);
        rb.gravityScale = isGround ? gravityScaleOnGround : gravityScaleInAir;
    }

    public void ShowJumpFX()
    {
        jumpFX.SetActive(true);
        jumpFX.transform.position = transform.position + new Vector3(0, -0.45f, 0);
    }

    public void ShowLandFX()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
    }

    public void UpdateHealth()
    {
        health = GameManager.instance.LoadPlayerHealth();
        UIManager.instance.SetPlayerMaxHealth(maxHealth);
        UIManager.instance.UpdatePlayerHealth(health);
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
                GameManager.instance.PlayerDead();
            }

            anim.SetTrigger("hit");
            UIManager.instance.UpdatePlayerHealth(health);
        }
    }

    public void ChangeToStaticState()
    {
        state = State.Static;
    }

    public void ChangeToRoamingState()
    {
        state = State.Roaming;
    }

    public void ChangeToBattleState()
    {
        state = State.Battle;
    }
}
