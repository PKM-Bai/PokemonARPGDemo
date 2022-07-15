using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public enum AtkStatusEnum
{
    [LabelText("无")]
    None,
    [LabelText("击飞")]
    Strikefly
}
public class AttactMethod : MonoBehaviour
{
    private Movement movement;
    private PlayerControl player;
    private Skill skill;
    
    public Animator animator;
    public AudioSource AtkOneAudio, AtkTwoAudio, AtkThreeAudio;
    public GameObject attackArea;

    public float filpDirction;

    [Header("状态")]
    public bool isAttack;
    public bool isJumpAttack;

    public static AtkStatusEnum atkStatus;

    [Header("攻击")]
    public int atkHurt;         // 攻击伤害
    public float atkMoveSpeed;      // 攻击时的速度补偿
    public int comboStep;       // 招式连击数
    public float interval; // 允许连续combo的时间
    protected float timer;        // 计时器

    public float jumpAtkInterval; // 允许连续combo的时间
    protected float jumpAtkTimer;        // 计时器
    public bool isCrouch;

    public bool atkAnimOver = true; // 停止攻击
    [HideInInspector]
    public static bool atkThree = false;

    // 按键设置s
    bool jumpPressed;     // 单次按下跳跃键
    bool jumpHeld;        // 长按跳跃键
    bool dashPressed;     // 按下冲刺键
    public bool atkPressed;        // 按下攻击键

    private void Start()
    {
        player = GetComponent<PlayerControl>();
        movement = GetComponent<Movement>();
        skill = GetComponent<Skill>();
        attackArea = transform.GetChild(0).gameObject;
    }

    protected void Update()
    {
        if (!player.isControl) return;
        
        if (Input.GetButton("ButtonA"))
        {
            atkPressed = true;
            Enemy.state = AtkStatusEnum.None;
            
            //* 普通攻击的攻击伤害为宝可梦攻击或特攻最高值的五分之一，最低伤害为1
            if(player.playerAttribute.Stat.Attack < player.playerAttribute.Stat.SpecialAttack)
                atkHurt = player.playerAttribute.Stat.SpecialAttack / 5 <= 0 ? 1 : player.playerAttribute.Stat.SpecialAttack / 5;
            else
                atkHurt = player.playerAttribute.Stat.Attack / 5 <= 0 ? 1 : player.playerAttribute.Stat.Attack / 5;
            
        }

    }

    private void FixedUpdate()
    {
        if (atkPressed && !isAttack)
        {
            isCrouch = player.isCrouch;
            if(SkillManager.Instance)
                skill.ProtectOver(SkillManager.Instance.skillDB.GetSkill(SkillName.守住));

            if(movement.xVelocity != 0)
                filpDirction = movement.xVelocity > 0 ? 1 : -1;
            else
                filpDirction = transform.localScale.x;
            transform.localScale = new Vector3(filpDirction, transform.localScale.y);

            if (!player.isOnGround && !isJumpAttack)
                JumpAttack();
            else if (isCrouch)
                GrouchAttack();
            else if (player.isOnGround)
                Attack();           // 攻击
            
            StartAttack();
        }
        StartTimer();
    }

    private void StartAttack()
    {
        isAttack = true;
        atkAnimOver = false;
        player.isMove = false;
        movement.isFilpDirction = false;
    }

    void StartTimer()
    {
        atkPressed = false;
        // 判断是否开始计数，若timer的值不为0则让它持续减去当前帧的时间
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                comboStep = 0;
            }
        }

        if (jumpAtkTimer != 0)
        {
            jumpAtkTimer -= Time.deltaTime;
            if (jumpAtkTimer <= 0)
            {
                jumpAtkTimer = 0;
                isJumpAttack = false;
            }
        }
    }

    // 攻击
    public void Attack()
    {
        comboStep++;
        if (comboStep > 3)
            comboStep = 1;
        timer = interval;

        animator.SetTrigger("groundAttack");
        animator.SetInteger("comboStep", comboStep);
    }

    // 蹲下状态-攻击
    public void GrouchAttack()
    {
        // StartAttack();
        animator.SetBool("crouchAtk", true);
    }
    public void GrouchAttackOver()
    {
        animator.SetBool("crouchAtk", false);
        AttackOver();
    }

    // 跳跃状态-攻击
    public void JumpAttack()
    {
        // StartAttack();
        isJumpAttack = true;
        jumpAtkTimer = jumpAtkInterval;
        animator.SetBool("jumpAtk", true);
    }
    public void JumpAtkOver()
    {
        animator.SetBool("jumpAtk", false);
        AttackOver();
    }

    // 结束攻击
    public void AttackOver()
    {
        atkPressed = false;
        isAttack = false;
        atkThree = false;
        attackArea.SetActive(false);
        movement.isFilpDirction = true;
    }
    // 攻击动画结束
    public void AtkAnimOver()
    {
        player.isMove = true;
        atkAnimOver = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EventHandler.CallAttackEnemy(other.GetComponent<Enemy>().enmeyPokemon);
            if (transform.localScale.x > 0)
                other.GetComponent<Enemy>().GetHit(Vector2.right, atkHurt, 1);
            else if (transform.localScale.x < 0)
                other.GetComponent<Enemy>().GetHit(Vector2.left, atkHurt, 1);
        }
    }

}
