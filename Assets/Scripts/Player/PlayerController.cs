using System.Collections.Generic;
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

    [Header("Sound Effects")]
    public AudioClip hurtSound;

    public Bottle availableBottle { get; set; }

    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private FloatingJoystick joystick;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        GameManager.instance.IsPlayer(this);
        if (state >= State.Battle)
        {
            UIManager.instance.SetPlayerHealthBarActive(true);
            InitHealth();
        }

        // 操纵杆放到更新完触摸按钮后获取
#if UNITY_ANDROID || UNITY_IOS
        UIManager.instance.UpdateTouchButtons_MobileDevice(state);
#endif
        joystick = FindObjectOfType<FloatingJoystick>();
    }

    void Update()
    {
        audioSource.volume = GameManager.instance.realEffectsVolume;

        if (state <= State.Static) return;

        anim.SetBool("dead", isDead);
        if (isDead) return;

        PhysicsCheck();
        CheckAvailableBuff();
#if !UNITY_ANDROID && !UNITY_IOS
        GetUserInput();
#endif
    }

    private void FixedUpdate()
    {
        if (state <= State.Static) return;

        if (isDead)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = gravityScaleInAir;
            return;
        }
        Move();
#if !UNITY_ANDROID && !UNITY_IOS
        Jump();
#endif
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(checkPoint.position, checkSize);
    }

    // 此方法用于非移动设备
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

        if (Input.GetKeyDown(KeyCode.K) && availableBottle != null && !availableBottle.isUsed)
        {
            availableBottle.DrinkBy(this);
            availableBottle = null;
        }
	}

	void Move()
    {
#if UNITY_ANDROID || UNITY_IOS
        // 操纵杆
        float horizontalInput = joystick.Horizontal;
#else
        // 键盘操作
        float horizontalInput = Input.GetAxisRaw("Horizontal");
#endif

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

    // 用于移动设备的 UI 按钮的点击事件
    public void Jump_MobileDevice() // button click event
    {
        if (isGround)
        {
            JumpDirectly();
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

    // 用于移动设备的 UI 按钮的点击事件
    public void Attack_MobileDevice() // button click event
    {
        if (state >= State.Battle)
        {
            Attack();
        }
    }

    public void CheckAvailableBuff()
    {
        bool hasBottle = false;
        var targetList = new List<Collider2D>();
        coll.OverlapCollider(new ContactFilter2D(), targetList);

        foreach (var target in targetList)
        {
            if (target.CompareTag("Buff"))
            {
                hasBottle = true;
                var bottle = target.GetComponent<Bottle>();
                if (bottle != null && !bottle.isUsed)
                {
                    availableBottle = bottle;
                    break;
                }
            }
        }
        if (!hasBottle) availableBottle = null;
        UIManager.instance.UpdateBuffButtonIcon();
    }

    // 用于移动设备的 UI 按钮的点击事件
    public void UseBuff_MobileDevice() // button click event
    {
        if (availableBottle != null && !availableBottle.isUsed)
        {
            availableBottle.DrinkBy(this);
            availableBottle = null;
            UIManager.instance.UpdateBuffButtonIcon();
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

    public void InitHealth()
    {
        health = GameManager.instance.LoadPlayerHealth();
        UIManager.instance.SetPlayerMaxHealth(maxHealth);
        UIManager.instance.UpdatePlayerHealth(health);
    }

    public void GetHit(float damage)
    {
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("player_hit"))
        {
            audioSource.clip = hurtSound;
            audioSource.Play();

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

    public void GetHeal(float value)
    {
        health += value;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        UIManager.instance.UpdatePlayerHealth(health);
    }

    public void ChangeToStaticState()
    {
        state = State.Static;
        UIManager.instance.UpdateTouchButtons_MobileDevice(state);
    }

    public void ChangeToRoamingState()
    {
        state = State.Roaming;
        UIManager.instance.UpdateTouchButtons_MobileDevice(state);
        joystick = FindObjectOfType<FloatingJoystick>();
    }

    public void ChangeToBattleState()
    {
        state = State.Battle;
        UIManager.instance.UpdateTouchButtons_MobileDevice(state);
        joystick = FindObjectOfType<FloatingJoystick>();
    }
}
