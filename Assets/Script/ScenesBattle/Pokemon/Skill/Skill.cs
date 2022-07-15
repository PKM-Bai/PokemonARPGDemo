using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Skill : MonoBehaviour
{
    protected SkillDataBase_SO equSkills;
    protected PokemonAttribute pokemon;
    protected Rigidbody2D rb;
    protected PlayerControl playerControl;
    protected AttactMethod attactMethod;
    protected Movement movement;
    protected Animator animator;
    protected PokemonAttribute enemyPokemon;

    public ReleaseSkill releaseSkill;   // 释放远程技能参数
    public float skillTimer;                  // 技能释放持续时间
    protected float skillTimeLeft;             // 技能释放剩余时间
    protected AnimatorStateInfo info;
    protected float filpDirction;       // 朝向
    protected float flightFilpDirction;     //飞行物的朝向
    [Header("守住")]
    protected Transform protectPrefab;

    protected bool priorityPressed;

    protected void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        pokemon = playerControl.playerAttribute;
        equSkills = playerControl.playerAttribute.equippedSkills;
        rb = GetComponent<Rigidbody2D>();
        attactMethod = GetComponent<AttactMethod>();
        movement = GetComponent<Movement>();
        animator = playerControl.animator;

        foreach(Skill_SO skill in equSkills.skillDatabase)
        {
            skill.skillReleaseTimerLeft = 0;
            skill.lastSkillReleaseTimer = -100;
        }
        
    }

    private void Update()
    {
        if (playerControl.isDeath) return;
        if (Input.GetButtonDown("Skill_1") && equSkills.skillDatabase.Count > 0) // 按下技能1
            Skill_1();

        if (Input.GetButtonDown("Skill_2") && equSkills.skillDatabase.Count > 1) // 按下技能2
            Skill_2();

        if (Input.GetButtonDown("Skill_3") && equSkills.skillDatabase.Count > 2) // 按下技能3
            Skill_3();

        if (Input.GetButtonDown("Skill_4") && equSkills.skillDatabase.Count > 3) // 按下技能4
            Skill_4();
    }


    public virtual void Skill_1()
    {

    }

    public virtual void Skill_2()
    {

    }

    public virtual void Skill_3()
    {

    }

    public virtual void Skill_4()
    {

    }

    //* 技能-保护
    public void ReadyProtect(Skill_SO skill)
    {
        if (Time.time >= (skill.lastSkillReleaseTimer + skill.skillCD))
        {
            if(priorityPressed) return;
            
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            skill.skillPressed = true;
            playerControl.notHit = true;

            animator.SetBool("protect", skill.skillPressed);
            // 技能图标进入CD
            SkillUI.Instance.GetSkill_Slot(skill.skillName).iconCD.fillAmount = 1;
            // 记下按下按键的时间
            skill.skillReleaseTimerLeft = skill.skillReleaseTimer;
            skill.lastSkillReleaseTimer = Time.time;

            SkillEffectPrefab skillPrefab = SkillManager.Instance.skillEffectDB.GetSkillEffectPrefab(SkillName.守住);
            if (skillPrefab != null)
            {
                protectPrefab = Instantiate(skillPrefab.prefab, transform);
                protectPrefab.SetParent(null);
            }
        }
    }
    public void Protect()
    {
        Skill_SO skill = equSkills.GetSkill(SkillName.守住);
        if (skill.skillPressed)
        {
            protectPrefab.transform.position = transform.position;
            
            playerControl.isMove = false;
            playerControl.isJump = false;
            movement.isFilpDirction = false;

            skill.skillReleaseTimerLeft -= Time.deltaTime;
            // 保护结束
            if (skill.skillReleaseTimerLeft <= 0)
            {
                ProtectOver(skill);
            }
        }
    }

    public void ProtectOver(Skill_SO skill)
    {
        playerControl.isMove = true;
        playerControl.isJump = true;
        movement.isFilpDirction = true;
        skill.skillPressed = false;
        animator.SetBool("protect", skill.skillPressed);
        playerControl.notHit = false;
        playerControl.HitOver();

        attactMethod.AttackOver();
        attactMethod.AtkAnimOver();
        
        if(protectPrefab != null)
            Destroy(protectPrefab.gameObject);
    }
}