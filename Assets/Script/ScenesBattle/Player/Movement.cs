using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MGame.Save;

public class Movement : MonoBehaviour, ISaveable
{
    PlayerControl playerControl;
    AttactMethod attactMethod;

    public Rigidbody2D rb;
    public Animator animator;
    public PhysicsMaterial2D p1;    // 有摩擦力的物理材质
    public PhysicsMaterial2D p2;    // 无摩擦力的物理材质
    public CapsuleCollider2D coll;
    protected RaycastHit2D leftCheck;              // 左脚射线检测
    protected RaycastHit2D rightCheck;             // 右脚射线检测
    protected RaycastHit2D headCheck;           // 头部射线检测

    [Header("移动参数")]
    [LabelText("速度")]
    public float speed = 8f;
    [HideInInspector]
    public float xVelocity;

    [Header("跳跃参数")]
    [LabelText("跳跃高度")]
    public float jumpForce = 6.3f;          // 基本跳跃高度的参数
    [LabelText("跳跃高度的加成")]
    public float jumpHoldForce = 1.9f;      // 长按跳跃按键对跳跃高度的加成
    [LabelText("跳跃高度加成的时间")]
    public float jumpHoldDuration = 2f;     // 跳跃高度加成的时间
    [LabelText("跳跃时间")]
    public float timer;                     // 跳跃定时器时间
    public float jumpTimer;                  // 跳跃时间
    [LabelText("可进行跳跃的次数")]
    public int jumpNum;                     // 可进行跳跃的次数
    public int jumpCount;                          // 记录已跳跃次数


    [Header("环境监测")]
    [LabelText("脚部检测之间的x轴距离")]
    public float footOffsetX;               // 两只脚之间的距离
    [LabelText("脚部检测的y轴距离")]
    public float footOffsetY;
    [LabelText("头部检测之间的x轴距离")]
    public float headoffsetX;
    [LabelText("头部检测的距离")]
    public float headClearance = 0.1f;      // 头部检测的距离
    [LabelText("地面检测的距离")]
    public float groundDistance = 0.1f;     // 地面检测的距离

    public LayerMask groundLayer;           // 地面检测

    [LabelText("是否可以转向")] public bool isFilpDirction = true;

    // 按键设置
    bool jumpPressed;     // 单次按下跳跃键
    bool jumpHeld;        // 长按跳跃键
    bool crouchPressed;     // 按下下蹲

    public string GUID => GetComponent<DataGUID>().guid;

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
        attactMethod = GetComponent<AttactMethod>();
        rb.sharedMaterial = p1;
        // 一些参数初始化
        jumpCount = jumpNum;
    }

    void Update()
    {
        if (!playerControl.isControl) return;

        if (Input.GetButtonDown("Jump") && timer == 0 && playerControl.isJump)
        {
            jumpPressed = true;
            playerControl.isJump = false;

            attactMethod.AttackOver();
            attactMethod.AtkAnimOver();
        }

        if (Input.GetButton("Crouch") && playerControl.isOnGround)
            crouchPressed = true;
        else
            crouchPressed = false;

        // 当在地面上时
        if (playerControl.isOnGround)
        {
            rb.sharedMaterial = p2;
            animator.SetBool("isJumpTwo", false);
        }
        else
            rb.sharedMaterial = p1;

    }

    void FixedUpdate()
    {
        // -1：向左    1：向右
        xVelocity = Input.GetAxis("Horizontal");  // 获得键盘输入 左右移动的值 -1f~1f 朝向

        if (!playerControl.isControl) return;

        StartJumpTimer();
        PhysicsCheck();     // 碰撞检测

        if (playerControl.isMove)
            GroundMovement();   // 地面移动

        // Dash();             // 冲刺
        MidAirMovement();   // 跳跃
    }

    public void StartJumpTimer()
    {
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
                playerControl.isJump = true;
            }
        }
    }

    private void OnMoveToPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void OnAfterSceneLoadedEvent()
    {
        animator.StopPlayback();
        playerControl.isMove = true;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        playerControl.isMove = false;
    }

    // 碰撞检测
    public void PhysicsCheck()
    {
        // 左脚射线检测
        leftCheck = Raycast(new Vector2(-footOffsetX, -footOffsetY), Vector2.down, groundDistance, groundLayer);
        // 右脚射线检测
        rightCheck = Raycast(new Vector2(footOffsetX, -footOffsetY), Vector2.down, groundDistance, groundLayer);

        // 地面检测
        if (leftCheck || rightCheck)
            playerControl.isOnGround = true;
        else
            playerControl.isOnGround = false;

        // 头部射线检测
        if (!playerControl.isMove)
        {
            headCheck = Raycast(new Vector2(0f, coll.size.y - headoffsetX), Vector2.up, headClearance, groundLayer);
        }
        else
        {
            headCheck = Raycast(new Vector2(xVelocity * 0.3f, coll.size.y - headoffsetX), Vector2.up, headClearance, groundLayer);
        }
        if (headCheck) playerControl.isHeadBlocked = true;
        else playerControl.isHeadBlocked = false;
    }

    // 地面移动
    public void GroundMovement()
    {
                                                  // 跑动状态
        if ((xVelocity != 0 && playerControl.isOnGround))
        {
            footOffsetX = 0.4f;
            playerControl.isGrouchMove = true;
        }
        // 未跑动状态
        else
        {
            footOffsetX = 0.2f;
            playerControl.isGrouchMove = false;
        }
        if (crouchPressed)
        {
            playerControl.isCrouch = true;
            animator.SetBool("isCrouching", true);
        }
        else
        {
            playerControl.isCrouch = false;
            animator.SetBool("isCrouching", false);
        }

        animator.SetBool("isCrouchMove", playerControl.isGrouchMove);

        if (playerControl.isMove)
        {
            if (playerControl.isCrouch && playerControl.isGrouchMove)
                rb.velocity = new Vector2(xVelocity * speed / 2, rb.velocity.y);
            else
                rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);
        }

        if (isFilpDirction)
        {
            FilpDirction();   // 转向
        }

    }

    // 人物转向
    public void FilpDirction()
    {
        if (xVelocity < 0)
            transform.localScale = new Vector2(-1, 1);
        if (xVelocity > 0)
            transform.localScale = new Vector2(1, 1);
    }

    // 跳跃
    public void MidAirMovement()
    {
        if (playerControl.isOnGround && timer == 0)
            jumpCount = jumpNum;

        if (jumpPressed && jumpCount > 0)
        {
            if (jumpCount == jumpNum)
            {
                jumpCount--;
                playerControl.isOnGround = false;
                // 按下跳跃按键时间   Time.time=当前系统游戏时间    jumpHoldDuration=跳跃高度加成的时间
                // jumpTime = Time.time + jumpHoldDuration;
                // 进行跳跃
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpPressed = false;
                timer = jumpTimer / 3f;
            }
            // 空中运动 二段跳
            else if (jumpCount > 0 && !playerControl.isOnGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.SetBool("isJumpTwo", true);
                jumpCount--;
                jumpPressed = false;

                timer = jumpTimer * 1.5f;
            }
        }


    }



    // 检测射线
    public RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDiraction * length, color);
        return hit;
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();

        return saveData;
    }

    public void RestoreLoadData(GameSaveData saveData)
    {
        transform.position = new Vector2(-5.78f, -5.95f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
