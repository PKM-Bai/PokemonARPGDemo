using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikachuSkill : Skill
{
    private Transform electroBallPrefab;

    private void OnEnable() {
        EventHandler.AttackEnemy += OnAttackEnemy;
    }

    private void OnDisable() {
        EventHandler.AttackEnemy -= OnAttackEnemy;
        enemyPokemon = null;
    }

    private void FixedUpdate()
    {
        Dash();
        Protect();
        ElectroBall();
    }

    private void OnAttackEnemy(PokemonAttribute enemyPokemon)
    {
        this.enemyPokemon = enemyPokemon;
    }

    public override void Skill_1()
    {
        ReleaseSkillMethod(equSkills.skillDatabase[0].skillName);
    }

    public override void Skill_2()
    {
        ReleaseSkillMethod(equSkills.skillDatabase[1].skillName);
    }

    public override void Skill_3()
    {
        ReleaseSkillMethod(equSkills.skillDatabase[2].skillName);
    }

    public override void Skill_4()
    {
        ReleaseSkillMethod(equSkills.skillDatabase[3].skillName);
    }

    //* 皮卡丘所有释放技能的方法 根据装备的技能名字来释放相应的技能
    public void ReleaseSkillMethod(SkillName skillName)
    {
        if(movement.xVelocity != 0)
            filpDirction = movement.xVelocity > 0 ? 1 : -1;
        else
            filpDirction = transform.localScale.x;
        transform.localScale = new Vector3(filpDirction, transform.localScale.y);

        switch (skillName)
        {
            case SkillName.守住:
                ReadyProtect(equSkills.GetSkill(SkillName.守住));
                break;
            case SkillName.皮卡皮:
                ReadyToDash(equSkills.GetSkill(SkillName.皮卡皮));
                break;
            case SkillName.电球:
                ReadyElectroBall(equSkills.GetSkill(SkillName.电球));
                break;
            default:

                break;
        }
    }

    //* 技能-电球
    public void ReadyElectroBall(Skill_SO skill)
    {   
        if (Time.time >= (skill.lastSkillReleaseTimer + skill.skillCD))
        {
            if (protectPrefab != null)
                ProtectOver(equSkills.GetSkill(SkillName.守住));
            
            if (attactMethod.isAttack)
            {
                attactMethod.AttackOver();
                attactMethod.AtkAnimOver();
            }
            animator.SetTrigger("ElectroBall");
            skill.skillPressed = true;
            flightFilpDirction = filpDirction;  // 电球的朝向
            // 技能图标进入CD
            SkillUI.Instance.GetSkill_Slot(skill.skillName).iconCD.fillAmount = 1;

            playerControl.isMove = false;
            playerControl.isJump = false;
            movement.isFilpDirction = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;  // 刚体x,y,z轴全锁定

            skill.skillReleaseTimerLeft = skill.skillReleaseTimer;
            skill.lastSkillReleaseTimer = Time.time;

            SkillEffectPrefab skillPrefab = SkillManager.Instance.skillEffectDB.GetSkillEffectPrefab(SkillName.电球);
            if (skillPrefab != null)
            {
                electroBallPrefab = Instantiate(skillPrefab.prefab, transform);
                electroBallPrefab.SetParent(null);
                electroBallPrefab.GetComponent<SkillHurt>().pokemon = pokemon;
            }

            Enemy.state = AtkStatusEnum.None;
        }
    }


    public void ElectroBall()
    {
        Skill_SO skill = equSkills.GetSkill(SkillName.电球);
        if (skill.skillPressed)
        {
            info = animator.GetCurrentAnimatorStateInfo(1);

            skill.skillReleaseTimerLeft -= Time.deltaTime;
            if (info.normalizedTime >= .4f)
            {
                electroBallPrefab.GetComponent<Animator>().Play("ElectroBall");
                electroBallPrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(flightFilpDirction * skill.skillSpeed, 1);
            }
            if (info.normalizedTime >= .7f)
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (info.normalizedTime >= .9f)
                animator.StopPlayback();
            
            // 技能释放结束
            if (skill.skillReleaseTimerLeft <= 0)
                ElectroBallOver(skill);
        }
    }

    public void ElectroBallOver(Skill_SO skill)
    {
        StartCoroutine(ElectroBallDestroy());
        skill.skillPressed = false;
        playerControl.isMove = true;
        playerControl.isJump = true;
        movement.isFilpDirction = true;
    }

    private IEnumerator ElectroBallDestroy()
    {
        info = electroBallPrefab.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(0.6f - info.normalizedTime);
        electroBallPrefab.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        if (electroBallPrefab != null)
            Destroy(electroBallPrefab.gameObject, 0.4f);
    }


    //* 技能-皮卡皮
    // 准备冲刺（皮卡皮）
    public void ReadyToDash(Skill_SO skill)
    {
        if (Time.time >= (skill.lastSkillReleaseTimer + skill.skillCD))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (protectPrefab != null)
                ProtectOver(equSkills.GetSkill(SkillName.守住));
            // 按下冲刺键
            skill.skillPressed = true;
            priorityPressed = true;
            // 技能图标进入CD
            SkillUI.Instance.GetSkill_Slot(skill.skillName).iconCD.fillAmount = 1;

            // 记下按下按键的时间
            skill.skillReleaseTimerLeft = skill.skillReleaseTimer;
            skill.lastSkillReleaseTimer = Time.time;

            Enemy.state = AtkStatusEnum.None;
        }
    }
    // 冲刺（皮卡皮）
    public void Dash()
    {
        Skill_SO skill = equSkills.GetSkill(SkillName.皮卡皮);
        if (skill.skillPressed)
        {
            movement.isFilpDirction = false;
            // 已进入冲刺状态
            playerControl.isDashing = true;
            playerControl.notEnterHit = true;

            animator.SetBool("isDashing", true);
            
            if(enemyPokemon != null)
            {
                attactMethod.atkHurt = PokemonManager.Instance.CalculateSkillHurt(pokemon, enemyPokemon, SkillManager.Instance.skillDB.GetSkill(SkillName.皮卡皮));
            }

            if (skill.skillReleaseTimerLeft >= 0)
            {
                rb.velocity = new Vector3(skill.skillSpeed * filpDirction, 1);
                skill.skillReleaseTimerLeft -= Time.deltaTime;
                // 从对象池中调出一个对象
                ShadowPool.instance.GetFormPool();
            }
        }

    }
    // 冲刺结束
    public void DashOver()
    {
        movement.isFilpDirction = true;
        playerControl.isControl = true;

        equSkills.GetSkill(SkillName.皮卡皮).skillPressed = false;
        priorityPressed = false;
        
        playerControl.isDashing = false;
        animator.SetBool("isDashing", false);
        playerControl.notEnterHit = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 只锁定Z轴
    }

}
