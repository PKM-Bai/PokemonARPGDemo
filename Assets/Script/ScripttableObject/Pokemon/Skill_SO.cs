using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Skill_SO", menuName = "Pokemon/Skill/Skill_SO")]
public class Skill_SO : ScriptableObject
{
    [LabelText("编号")] public int id;
    [LabelText("技能名")] public SkillName skillName;
    [LabelText("图标")] public Sprite image;
    [LabelText("技能属性")] public PokemonType type;
    [LabelText("技能类别")] public SkillType category;
    [LabelText("伤害类型")] public SkillType hurtCategory;
    [LabelText("招式类型")] public List<MoveType> moveType;
    [LabelText("威力")] public string power;
    [LabelText("PP")] public int maxPP;
    [LabelText("当前PP")] public int currentPP;
    [LabelText("说明"), TextArea] public string info;

    [LabelText("追加效果")] public AddedEffect_SO addedEffect;
    [LabelText("追加效果触发概率"), Range(0, 100)] public int triggerProbability;

    [LabelText("到达等级解锁")] public int reachLevel;

    [Header("释放技能参数")]
    [LabelText("技能CD")] public float skillCD;
    [LabelText("技能速度")] public float skillSpeed;
    [LabelText("技能释放时间")] public float skillReleaseTimer;
    [LabelText("技能释放剩余时间")] public float skillReleaseTimerLeft;
    [LabelText("上一次释放技能的时间")] public float lastSkillReleaseTimer;
    [LabelText("是否开始释放技能")] public bool skillPressed;
}

