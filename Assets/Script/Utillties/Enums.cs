//* 城镇名
public enum TownNameEnum
{
    None, 未白镇 
}
//* 音乐|音效类型
public enum SoundName
{
    none, OpenMenu, CloseMenu, Select, SelectActive, SwitchBag,
    CoutinueDialogue, GetItem, PokemonCenter,
    MainMenu, LittlerootTown,
    Battle01, Battle02, Battle03
}
//* 特效
public enum ParticleEffectType
{
    Footstep
}

//* 道具类型
public enum TabTypeEnum
{
    // Items, Recoverys, TMs, Berrys, Treasures, KeyItems
    道具口袋, 回复口袋, 技能机口袋, 树果口袋, 宝物口袋, 重要道具口袋
}

//* 道具口袋类别，中文
public enum ItemPocketEnum
{
    道具口袋, 回复口袋, 技能机口袋, 树果口袋, 宝物口袋, 重要道具口袋
}

//* 使用道具类型
public enum ItemActionType
{
    None, Use, Carryon
}

//* 获取道具方式
public enum GetItemType
{
    All, Random, Only
}

//* 天气
public enum Weather
{
    雨天, 晴天, 沙暴, 雪天
}

//* 季节
public enum Season
{
    春天, 夏天, 秋天, 冬天
}
//* 灯光转换
public enum LightShift
{
    Morning, Night
}
//* 传送场景类型
public enum TeleportType
{
    Menu, TownScene, BattleScene
}

//* 任务类型
public enum QuestType
{
    主线, 支线, 小道消息
}
//* 任务目标类型
public enum QuestRequireType
{
    收集道具, 击败敌人, 到达特定等级, 找特定的人对话, 到达目的地
}
//* 任务奖励类型
public enum QuestRewardType
{
    零钱, 经验, 道具
}

//********宝可梦角色属性**********
//*性别
public enum RoleSexType
{
    无性别, 雄性, 雌性
}
//*性格
public enum Nature
{
    勤奋, 怕寂寞, 固执, 顽皮, 勇敢,
    大胆, 坦率, 淘气, 乐天, 悠闲,
    内敛, 慢吞吞, 害羞, 马虎, 冷静,
    温和, 温顺, 慎重, 浮躁, 自大,
    胆小, 急躁, 爽朗, 天真, 认真
}
//* 属性
public enum PokemonType
{
    None,
    一般, 火, 水, 电, 草, 冰,
    格斗, 毒, 地面, 飞行, 超能力, 虫,
    岩石, 幽灵, 龙, 恶, 钢, 妖精
}
//* 能力值类型
public enum StatisticType
{
    HP, 攻击, 防御, 特攻, 特防, 速度
}
//* 技能类型
public enum SkillType
{
    物理招式, 特殊招式, 变化招式, 通用招式, 秘技招式
}
//* 招式种类
public enum MoveType
{
    接触类招式, 回复HP的招式, 吸取HP的招式, 反作用力伤害的招式, 啃咬类招式,
    拳类招式, 波动和波导类招式, 声音类招式, 粉末类招式, 球和弹类招式, 防住类招式,
    免疫伤害, 降低伤害, 位移招式, 传送招式
}
//* 附加效果类型
public enum AddedEffectType
{
    提高能力变化, 降低能力变化, 反作用力, 陷入异常状态, 陷入状态变化
}
//* 异常状态
public enum StatusConditionType
{
    灼伤, 冰冻, 冻伤, 麻痹, 中毒, 剧毒, 睡眠, 瞌睡
}
//* 使用对象（招式|效果）
public enum UseObjectType
{
    自己, 敌人, 范围
}

//* 技能名
public enum SkillName
{
    守住, 皮卡皮, 十万伏特, 电球, 电击, 劈瓦, 电光一闪, 铁尾, 叫声, 摇尾巴, 撞击, 飞叶快刀
}





