using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    public Animator skillAnim;
    public SkillDataBase_SO skillDB;
    public SkillEffectDataBase_SO skillEffectDB;
    

    protected override void Awake()
    {
        base.Awake();
        skillAnim = transform.GetComponent<Animator>();
    }

}
