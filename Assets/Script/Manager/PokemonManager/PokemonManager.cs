using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MGame.Save;

public class PokemonManager : Singleton<PokemonManager>, ISaveable
{
    public PokemonDataBase_SO allPokemon;
    public PokemonDataBase_SO pokemonTeam;
    public List<PokemonDetails> teamData;
    public PokemonType_SO type_SO;
    public Sprite[] sexSprites;

    public string GUID => GetComponent<DataGUID>().guid;

    private void OnEnable()
    {
        EventHandler.AddBasePoints += OnAddBasePoints;
        EventHandler.GetExp += OnGetExp;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }
    private void OnDisable()
    {
        EventHandler.AddBasePoints -= OnAddBasePoints;
        EventHandler.GetExp -= OnGetExp;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    protected override void Awake()
    {
        base.Awake();
        
        InitTeamData();
    }

    private void Start()
    {
        foreach (var pokemon in allPokemon.pokemons)
        {
            InitPokemonAttribute(pokemon);
        }
        ISaveable saveable = this;
        saveable.RegisterSaveable();
    }

    private void OnStartNewGameEvent(int index)
    {
        pokemonTeam.pokemons[0].level = 5;
        pokemonTeam.pokemons[0].experience = 100;
        pokemonTeam.pokemons[0].currentExperience = 0;
        pokemonTeam.pokemons[0].upLevelExperience = 500;
        pokemonTeam.pokemons[0].holdingsItem = string.Empty;

        pokemonTeam.pokemons[0].basePoints = new BasePoints();
        pokemonTeam.pokemons[0].equippedSkills.skillDatabase.Clear();
        pokemonTeam.pokemons[0].equippedSkills.skillDatabase.Add(SkillManager.Instance.skillDB.GetSkill(SkillName.皮卡皮));
        pokemonTeam.pokemons[0].equippedSkills.skillDatabase.Add(SkillManager.Instance.skillDB.GetSkill(SkillName.守住));
        pokemonTeam.pokemons[0].equippedSkills.skillDatabase.Add(SkillManager.Instance.skillDB.GetSkill(SkillName.电球));

        InitPokemonAttribute(pokemonTeam.pokemons[0]);
    }

    private void OnAddBasePoints(PokemonAttribute pokemon)
    {
        OverStepBasePointsRange(pokemon);
    }
    
    public void InitTeamData()
    {
        teamData = new List<PokemonDetails>();
        foreach (var pokemon in pokemonTeam.pokemons)
        {
            PokemonDetails newPokemon = new PokemonDetails();
            newPokemon.InitPokemonTeam(pokemon);
            teamData.Add(newPokemon);
        }
    }

    public void CallRefreshPokemonTeam()
    {
        if(teamData != null)   
        {
            foreach (var pokemonDetails in teamData)
            {
                PokemonAttribute pokemon = pokemonTeam.GetPokemon(pokemonDetails.PUID);
                pokemon = pokemonDetails.SetPokemonTeam(pokemon);
            }
        }
    }


    /// <summary>
    ///* 计算技能伤害
    /// </summary>
    /// <param name="attacker">攻击者</param>
    /// <param name="defender">防御者</param>
    /// <param name="skill">造成伤害的技能</param>
    /// <returns></returns>
    public int CalculateSkillHurt(PokemonAttribute attacker, PokemonAttribute defender, Skill_SO skill)
    {
        int hurt = 0;
        int power = skill.power != "—" ? int.Parse(skill.power) : 1;

        // 技能与宝可梦属性一致加成 1.5倍
        float typeEqualAddition = (attacker.firstType == skill.type || attacker.secondType == skill.type) ? 1.5f : 1f;
        // 属性相克加成
        float typeSuppressAddition = type_SO.GetHurtMagnification(skill.type, defender.firstType) * type_SO.GetHurtMagnification(skill.type, defender.secondType);
        //? 其他加成
        // 随机数 [0.85 ~ 1]
        float randomAddition = Random.Range(0.85f, 1.01f);

        if (skill.hurtCategory == SkillType.物理招式)
        {
            hurt = (int)Mathf.Floor(((( (2 * attacker.level / 5 + 2) * power * attacker.Stat.Attack / defender.Stat.Defense) / 50) + 2) * typeEqualAddition * typeSuppressAddition * randomAddition);
        }

        if (skill.hurtCategory == SkillType.特殊招式)
        {
            // 电球伤害计算：攻击者速度与防御者的速度比，小于1最低伤害40、大于等于1小于2伤害60、大于等于2小于3伤害80、大于等于3小于4伤害120、大于4伤害150
            if (skill.skillName == SkillName.电球)
            {
                float speedD = attacker.Stat.Speed / defender.Stat.Speed;
                if (speedD < 1)
                    power = 40;
                else if (speedD >= 1 && speedD < 2)
                    power = 60;
                else if (speedD >= 2 && speedD < 3)
                    power = 80;
                else if (speedD >= 3 && speedD < 4)
                    power = 120;
                else if (speedD >= 4)
                    power = 150;
            }

            hurt = (int)Mathf.Floor(((((2 * attacker.level / 5 + 2) * power * attacker.Stat.SpecialAttack / defender.Stat.SpecialDefense) / 50) + 2) * typeEqualAddition * typeSuppressAddition * randomAddition);
        }
        return hurt;
    }

    public float GetHurtMagnification(PokemonAttribute defender, Skill_SO skill)
    {
        return type_SO.GetHurtMagnification(skill.type, defender.firstType) * type_SO.GetHurtMagnification(skill.type, defender.secondType);
    }

    //* 获得经验
    public void OnGetExp(PokemonAttribute pokemon, int exp)
    {
        exp = pokemon.upLevelExperience - (exp + pokemon.currentExperience);
        while (exp <= 0) //? 减剩下的值如果小于等于0，代表获得到的经验比升级所需要的经验多，执行升级
        {
            pokemon.level++;
            pokemon.currentExperience = 0;
            pokemon.upLevelExperience = pokemon.experience * pokemon.level;
            QuestManager.Instance.UpdatePokemonLevelProgress(pokemon, pokemon.level);

            CalculationStatistic(pokemon);
            exp = pokemon.upLevelExperience - Mathf.Abs(exp);

        }
        //? 当前经验等于减剩下的值
        pokemon.currentExperience = pokemon.upLevelExperience - exp;
    }

    //* 获取队伍中最高等级
    public int GetTeamMaxLevel()
    {
        var level = 0;
        for (int i = 0; i < pokemonTeam.pokemons.Count; i++)
        {
            if (i == 0)
                level = pokemonTeam.pokemons[i].level;
            else if (pokemonTeam.pokemons[i].level > level)
                level = pokemonTeam.pokemons[i].level;
        }
        return level;
    }

    //* 使用宝可梦名获取当前队伍中的宝可梦
    public PokemonAttribute GetPokemonTeamByName(string name)
    {
        PokemonAttribute pokemon = pokemonTeam.pokemons.Find(p => p.pokmeonName == name);

        return pokemon;
    }

    #region 宝可梦能力相关
    //* 宝可梦初始能力值 随机获得性别、性格、个体值
    public void InitPokemonAttribute(PokemonAttribute pokemon)
    {
        if (pokemon != null)
        {
            // 性别-随机
            pokemon.sex = (RoleSexType)Random.Range(1, 3);
            // 性格-随机
            pokemon.nature = (Nature)Random.Range(0, 25);
            // 个体值-随机
            RandomGenerateIndividual(pokemon);

            // 得到最终能力值
            CalculationStatistic(pokemon);
            pokemon.currentHP = pokemon.Stat.HP;
            // EditorUtility.SetDirty(pokemon);
        }
    }

    //* 随机生成个体值
    public void RandomGenerateIndividual(PokemonAttribute pokemon)
    {
        pokemon.individual.HPIV = Random.Range(0, 32);
        pokemon.individual.AttackIV = Random.Range(0, 32);
        pokemon.individual.DefenseIV = Random.Range(0, 32);
        pokemon.individual.SpecialAttackIV = Random.Range(0, 32);
        pokemon.individual.SpecialDefenseIV = Random.Range(0, 32);
        pokemon.individual.SpeedIV = Random.Range(0, 32);
    }

    //* 计算能力值 宝可梦正作游戏 能力值是采取向下取整的方式，舍去小数点
    public void CalculationStatistic(PokemonAttribute pokemon)
    {
        //? HP能力值=((种族值*2 + 个体值 + 努力值/4 ) * 等级) / 100 + 10 + 等级
        pokemon.Stat.HP = ((pokemon.speciesStrength.HP * 2 + pokemon.individual.HPIV + (int)(pokemon.basePoints.HP / 4)) * pokemon.level) / 100 + 10 + pokemon.level;
        //? 其他能力值=(((种族值*2 + 个体值 + 努力值/4) * 等级) / 100 +5) * 性格修正
        pokemon.Stat.Attack = (int)Mathf.Floor((((pokemon.speciesStrength.Attack * 2 + pokemon.individual.AttackIV + (int)(pokemon.basePoints.Attack / 4)) * pokemon.level) / 100 + 5) * NatureCorrection(pokemon.nature, StatisticType.攻击));

        pokemon.Stat.Defense = (int)Mathf.Floor((((pokemon.speciesStrength.Defense * 2 + pokemon.individual.DefenseIV + (int)(pokemon.basePoints.Defense / 4)) * pokemon.level) / 100 + 5) * NatureCorrection(pokemon.nature, StatisticType.防御));

        pokemon.Stat.SpecialAttack = (int)Mathf.Floor((((pokemon.speciesStrength.SpecialAttack * 2 + pokemon.individual.SpecialAttackIV + (int)(pokemon.basePoints.SpecialAttack / 4)) * pokemon.level) / 100 + 5) * NatureCorrection(pokemon.nature, StatisticType.特攻));

        pokemon.Stat.SpecialDefense = (int)Mathf.Floor((((pokemon.speciesStrength.SpecialDefense * 2 + pokemon.individual.SpecialDefenseIV + (int)(pokemon.basePoints.SpecialDefense / 4)) * pokemon.level) / 100 + 5) * NatureCorrection(pokemon.nature, StatisticType.特防));

        pokemon.Stat.Speed = (int)Mathf.Floor((((pokemon.speciesStrength.Speed * 2 + pokemon.individual.SpeedIV + (int)(pokemon.basePoints.Speed / 4)) * pokemon.level) / 100 + 5) * NatureCorrection(pokemon.nature, StatisticType.速度));

    }

    //* 性格修正
    public float NatureCorrection(Nature nature, StatisticType correctionStatType)
    {
        float correctionValue = 1;
        switch (nature)
        {
            case Nature.勤奋:    //? 没有能力修正
                break;
            case Nature.怕寂寞:    //? +攻击 -防御
                if (correctionStatType == StatisticType.攻击) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.防御) correctionValue = 0.9f;
                break;
            case Nature.固执:    //? +攻击 -特攻
                if (correctionStatType == StatisticType.攻击) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.特攻) correctionValue = 0.9f;
                break;
            case Nature.顽皮:    //? +攻击 -特防
                if (correctionStatType == StatisticType.攻击) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.特防) correctionValue = 0.9f;
                break;
            case Nature.勇敢:    //? +攻击 -速度
                if (correctionStatType == StatisticType.攻击) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.速度) correctionValue = 0.9f;
                break;
            case Nature.大胆:    //? +防御 -攻击
                if (correctionStatType == StatisticType.防御) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.攻击) correctionValue = 0.9f;
                break;
            case Nature.坦率:    //? 没有能力修正
                break;
            case Nature.淘气:    //? +防御 -特攻
                if (correctionStatType == StatisticType.防御) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.特攻) correctionValue = 0.9f;
                break;
            case Nature.乐天:    //? +防御 -特防
                if (correctionStatType == StatisticType.防御) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.特防) correctionValue = 0.9f;
                break;
            case Nature.悠闲:    //? +防御 -速度
                if (correctionStatType == StatisticType.防御) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.速度) correctionValue = 0.9f;
                break;
            case Nature.内敛:    //? +特攻 -攻击
                if (correctionStatType == StatisticType.特攻) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.攻击) correctionValue = 0.9f;
                break;
            case Nature.慢吞吞:    //? +特攻 -防御
                if (correctionStatType == StatisticType.特攻) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.防御) correctionValue = 0.9f;
                break;
            case Nature.害羞:    //? 没有能力修正
                break;
            case Nature.马虎:    //? +特攻 -特防
                if (correctionStatType == StatisticType.特攻) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.特防) correctionValue = 0.9f;
                break;
            case Nature.冷静:    //? +特攻 -速度
                if (correctionStatType == StatisticType.特攻) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.速度) correctionValue = 0.9f;
                break;
            case Nature.温和:    //? +特防 -攻击
                if (correctionStatType == StatisticType.特防) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.攻击) correctionValue = 0.9f;
                break;
            case Nature.温顺:    //? +特防 -防御
                if (correctionStatType == StatisticType.特防) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.防御) correctionValue = 0.9f;
                break;
            case Nature.慎重:    //? +特防 -特攻
                if (correctionStatType == StatisticType.特防) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.特攻) correctionValue = 0.9f;
                break;
            case Nature.浮躁:    //? 没有能力修正
                break;
            case Nature.自大:    //? +特防 -速度
                if (correctionStatType == StatisticType.特防) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.速度) correctionValue = 0.9f;
                break;
            case Nature.胆小:    //? +速度 -攻击
                if (correctionStatType == StatisticType.速度) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.攻击) correctionValue = 0.9f;
                break;
            case Nature.急躁:    //? +速度 -防御
                if (correctionStatType == StatisticType.速度) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.防御) correctionValue = 0.9f;
                break;
            case Nature.爽朗:    //? +速度 -特攻
                if (correctionStatType == StatisticType.速度) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.特攻) correctionValue = 0.9f;
                break;
            case Nature.天真:    //? +速度 -特防
                if (correctionStatType == StatisticType.速度) correctionValue = 1.1f;
                if (correctionStatType == StatisticType.特防) correctionValue = 0.9f;
                break;
            case Nature.认真:    //? 没有能力修正 
                break;
            default: break;
        }

        return correctionValue;
    }

    public string[] NatureCorrection(Nature nature)
    {
        string[] natureCorrections = new string[2];
        //* 性格修正+
        if (nature == Nature.怕寂寞 || nature == Nature.固执 || nature == Nature.顽皮 || nature == Nature.勇敢)
            natureCorrections.SetValue("攻击+", 0);
        if (nature == Nature.大胆 || nature == Nature.淘气 || nature == Nature.乐天 || nature == Nature.悠闲)
            natureCorrections.SetValue("防御+", 0);
        if (nature == Nature.内敛 || nature == Nature.慢吞吞 || nature == Nature.马虎 || nature == Nature.冷静)
            natureCorrections.SetValue("特攻+", 0);
        if (nature == Nature.温和 || nature == Nature.温顺 || nature == Nature.慎重 || nature == Nature.自大)
            natureCorrections.SetValue("特防+", 0);
        if (nature == Nature.胆小 || nature == Nature.急躁 || nature == Nature.爽朗 || nature == Nature.天真)
            natureCorrections.SetValue("速度+", 0);
        //* 性格修正-
        if (nature == Nature.大胆 || nature == Nature.内敛 || nature == Nature.温和 || nature == Nature.胆小)
            natureCorrections.SetValue("攻击-", 1);
        if (nature == Nature.怕寂寞 || nature == Nature.慢吞吞 || nature == Nature.温顺 || nature == Nature.急躁)
            natureCorrections.SetValue("防御-", 1);
        if (nature == Nature.固执 || nature == Nature.淘气 || nature == Nature.慎重 || nature == Nature.爽朗)
            natureCorrections.SetValue("特攻-", 1);
        if (nature == Nature.顽皮 || nature == Nature.乐天 || nature == Nature.马虎 || nature == Nature.天真)
            natureCorrections.SetValue("特防-", 1);
        if (nature == Nature.勇敢 || nature == Nature.悠闲 || nature == Nature.冷静 || nature == Nature.自大)
            natureCorrections.SetValue("速度-", 1);
        //* 平衡
        if (nature == Nature.勤奋 || nature == Nature.坦率 || nature == Nature.害羞 || nature == Nature.浮躁 || nature == Nature.认真)
            natureCorrections.SetValue("平衡", 0);

        return natureCorrections;
    }


    //* 监听努力值
    private bool OverStepBasePointsRange(PokemonAttribute pokemon)
    {
        var sumBasePoints = pokemon.basePoints.HP + pokemon.basePoints.Attack + pokemon.basePoints.Defense + pokemon.basePoints.SpecialAttack + pokemon.basePoints.SpecialDefense + pokemon.basePoints.Speed;
        return sumBasePoints > 510 ? true : false;
    }
    #endregion

    public GameSaveData GenerateSaveData()
    {
        InitTeamData();
        GameSaveData saveData = new GameSaveData();
        saveData.pokemonTeamDict = new Dictionary<string, List<PokemonDetails>>();
        saveData.pokemonTeamDict.Add(pokemonTeam.name, teamData);
        return saveData;
    }

    public void RestoreLoadData(GameSaveData saveData)
    {
        teamData = saveData.pokemonTeamDict[pokemonTeam.name];
        CallRefreshPokemonTeam();
    }
}
