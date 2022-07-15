using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Turtwig : Enemy
{
    public override void Attack()
    {
        rb.velocity = new Vector2(-transform.localScale.x * 10f, rb.velocity.y);
    }

    public override void Skill_1()
    {
        skill.skillArea = new Vector3(4.5f, 0.8f, 0);
        // 技能释放位置
        skill.releasePoint.transform.position = new Vector3(
            skill.thisObject.transform.position.x + (skill.thisObject.transform.localScale.x * -2.7f), skill.thisObject.transform.position.y + 0.7f, 0
        );
        skill_1Perfab = GameObject.Instantiate(skillPerfabs[0].gameObject, skill.releasePoint.transform);

        skill_1Perfab.GetComponent<Animator>().Play("RazorLeaf");
       
    }

    public override void Skill_2()
    {

    }

    public override void Skill_3()
    {

    }

    public override void Skill_4()
    {

    }
}
