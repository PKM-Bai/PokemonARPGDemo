using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerControl : MonoBehaviour
{
    protected AttactMethod attactMethod;
    protected Movement movement;

    [SerializeField, ReadOnly] protected Rigidbody2D rb;
    public Animator animator;
    public PokemonAttribute playerAttribute;


    [Header("状态")]
    [LabelText("是否站在地面")] public bool isOnGround;                 // 是否站在地面
    [LabelText("是否可以跳跃")] public bool isJump = true;                     // 是否可以跳跃
    [LabelText("是否蹲下")] public bool isCrouch;                     // 是否蹲下
    [LabelText("是否可以移动")] public bool isMove = true;              // 是否可以移动
    [LabelText("是否可以蹲下移动")] public bool isGrouchMove = true;              // 是否可以蹲下移动
    [LabelText("头部上方是否有障碍物")] public bool isHeadBlocked;              // 头部上方是否有物体
    [LabelText("是否冲刺")] public bool isDashing;                 // 冲刺
    [LabelText("是否死亡")] public bool isDeath;                 // 死亡
    public bool isControl = true;

    [Header("受伤状态")]
    [LabelText("不会受伤")] public bool notHit;                // 不会受伤
    [LabelText("无法被选中状态")] public bool notEnterHit;     // 无法被选中状态（无敌）
    [LabelText("是否为受伤状态")] public bool isHit;           // 是否为受伤状态
    [LabelText("受伤击退距离")] public float hitRepel;

    private void Awake()
    {
        isJump = true;
        isCrouch = false;
        isMove = true;
        isControl = true;
        isDeath = false;
    }

    public void OnHit(Vector2 direction, int actHurt)
    {
        if (playerAttribute.currentHP > 0 && !notEnterHit && !notHit)
        {
            if (!attactMethod.isAttack && !animator.GetBool("isDashing"))
                transform.localScale = new Vector3(direction.x, 1, 1);
            isHit = true;
            isControl = false;
            
            playerAttribute.currentHP -= actHurt;
            animator.SetTrigger("hit");
            if (playerAttribute.currentHP <= 0)
            {
                EventHandler.CallGameOverEvent();
                isDeath = true;
                playerAttribute.currentHP = 0;
                animator.SetTrigger("death");
                isControl = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            }
            else
            {
                // 击退
                rb.velocity = new Vector2(-direction.x * hitRepel, rb.velocity.y);
                if (attactMethod.atkPressed)
                {
                    attactMethod.AttackOver();
                    attactMethod.AtkAnimOver();
                }
            }
        }
    }

    public void HitOver()
    {
        if(attactMethod.isAttack && playerAttribute.currentHP > 0)
        {
            attactMethod.AttackOver();
            attactMethod.AtkAnimOver();
        }
        isHit = false;
        isControl = true;
        isJump = true;
        isCrouch = false;
        isMove = true;
    }



}


