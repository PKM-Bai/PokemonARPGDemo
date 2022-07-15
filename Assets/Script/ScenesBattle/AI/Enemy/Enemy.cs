using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour
{
    private EnemyFSM enemyFSM;
    private BoxCollider2D coll;

    protected Rigidbody2D rb;
    public Animator animator;

    public PokemonAttribute enmeyPokemon;
    public string enemyName;
    [Header("敌人生命条")]
    [LabelText("血条画布")] public EnemyHPCanvas enemyHPCanvas;
    public EnemyHPBar HPBar;     // 血条
    [Header("受击")]
    [LabelText("受击音效")] public AudioSource hitAudio;
    [LabelText("伤害画布")]public Transform hurtCanvas;
    [LabelText("伤害数字")]public Transform hurtValuePos;
    [LabelText("伤害数字浮动")] public GameObject FloatPoint;
    [LabelText("受击硬直时间")] public float hitTimer;
    [LabelText("受击方向")] private Vector2 direction;
    [LabelText("受伤状态")] public bool isHit;
    [LabelText("受击浮空高度")] public float hitForce;

    [Header("参数")]
    [LabelText("当前血量")] public int currentHP;
    [LabelText("移动速度")] public float moveSpeed = 2;             // 移动速度
    [LabelText("追击速度")] public float chaseSpeed = 4;            // 追击速度
    [LabelText("攻击力")] public float attack;
    public static AtkStatusEnum state;

    [Header("攻击技能")]
    public ReleaseSkill skill;
    public List<Transform> skillPerfabs;
    public GameObject skill_1Perfab;
    public GameObject skill_2Perfab;
    public GameObject skill_3Perfab;
    public GameObject skill_4Perfab;   
    
    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        enemyName = enmeyPokemon.pokmeonName;
        enemyHPCanvas = FindObjectOfType<EnemyHPCanvas>();
        animator = transform.GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody2D>();
        enemyFSM = GetComponent<EnemyFSM>();
        
        skill.thisObject = gameObject;
        if (SkillManager.Instance)
            skill.animator = SkillManager.Instance.skillAnim;
    }

    void Start()
    {
        // 创建生命条
        enemyHPCanvas.CreateEnemyHP();
        HPBar = enemyHPCanvas.transform.GetChild(enemyHPCanvas.EnemyHPList.Count - 1).GetComponent<EnemyHPBar>();
        currentHP = enmeyPokemon.Stat.HP * 2;

        // 初始化
        HPBar.HPValue.maxValue = enmeyPokemon.Stat.HP * 2;
        HPBar.HPValue.value = currentHP;
        HPBar.HPValue.minValue = 0;
        HPBar.level.text = "Lv." + enmeyPokemon.level;
        HPBar.pkmName.text = enmeyPokemon.pokmeonName;
        HPBar.sex.sprite = enmeyPokemon.sex != RoleSexType.无性别 ? enmeyPokemon.sex == RoleSexType.雄性 ? PokemonManager.Instance.sexSprites[1] : PokemonManager.Instance.sexSprites[0] : null;
        
        if(enmeyPokemon.firstType != PokemonType.None)
        {
            Image typeIcon =  GameObject.Instantiate(HPBar.typeIconPrefab, HPBar.transform.position, Quaternion.identity, HPBar.typeIcon.transform);
            typeIcon.sprite = PokemonManager.Instance.type_SO.GetTypeIcon(enmeyPokemon.firstType);
        }
        if(enmeyPokemon.secondType != PokemonType.None)
        {
            Image typeIcon =  GameObject.Instantiate(HPBar.typeIconPrefab, HPBar.transform.position, Quaternion.identity, HPBar.typeIcon.transform);
            typeIcon.sprite = PokemonManager.Instance.type_SO.GetTypeIcon(enmeyPokemon.secondType);
        }

    }

    void Update()
    {
        if (HPBar)
        {
            // 血条跟随 ui:屏幕坐标   游戏对象：世界坐标
            Vector3 enemyPosition = new Vector3(transform.position.x - 0.2f, transform.position.y, 0);
            // 将世界坐标转换成屏幕坐标
            HPBar.transform.position = Camera.main.WorldToScreenPoint(enemyPosition + Vector3.up * coll.size.y);

            HPBar.HPValue.value = enmeyPokemon.Stat.HP * 2 * ((float)currentHP / (float)(enmeyPokemon.Stat.HP*2));
        }
    }

    public virtual void Attack()
    {

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

    public void OverSkill()
    {
        if (skill_1Perfab != null)
            Destroy(skill_1Perfab.gameObject);
        if (skill_2Perfab != null)
            Destroy(skill_2Perfab.gameObject);
        if (skill_3Perfab != null)
            Destroy(skill_3Perfab.gameObject);
        if (skill_4Perfab != null)
            Destroy(skill_4Perfab.gameObject);
    }

    /// <summary>
    ///* 敌人受伤函数
    /// </summary>
    /// <param name="direction">收到伤害来源的朝向</param>
    /// <param name="lossHP">扣血量</param>
    /// <param name="typeHurt">属性伤害：大于1：效果绝佳、等于1：一般、小于1大于0：效果不好、等于0：无效</param>
    public void GetHit(Vector2 direction, int lossHP, float typeHurt)
    {
        if(state == AtkStatusEnum.Strikefly)
            rb.velocity = new Vector2(rb.velocity.x, hitForce);
        hitAudio.Play();

        //* 伤害数字浮动
        GameObject newFloat = GameObject.Instantiate(FloatPoint, hurtValuePos.position, Quaternion.identity, hurtCanvas);
        TextMesh textMesh = newFloat.transform.GetChild(0).GetComponent<TextMesh>();
        
        var str = "";
        if(typeHurt > 1) { str = "效果绝佳:"; textMesh.color = Color.red; }
        else if(typeHurt == 1 ) { textMesh.color = new Color(0.94f, 0.67f, 0.25f); }
        else if(typeHurt < 1 ) { str = "效果不好:"; textMesh.color = Color.green; }
        else if(typeHurt == 0 ) { str = "无效:"; textMesh.color = Color.white; }

        textMesh.text = str + lossHP.ToString();


        transform.localScale = new Vector3(direction.x, 1, 1);
        this.direction = direction;
        currentHP -= lossHP;

        enemyFSM.TransitionState(StateType.Hit);
        if (currentHP <= 0)
        {
            DestroyEnemy();
            QuestManager.Instance.UpdateItemAndEnemyProgress(enemyName, 1);
        }
    }

    public void DestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject, 3f);
        Destroy(enemy.HPBar.gameObject);
        Destroy(enemy.transform.GetComponent<Rigidbody2D>());
        enemy.transform.GetComponent<Collider2D>().enabled = false;
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject, 3f);
        Destroy(HPBar.gameObject);
        Destroy(transform.GetComponent<Rigidbody2D>());
        transform.GetComponent<Collider2D>().enabled = false;
        
    }

    private void OnDrawGizmos()
    {
        if (skill.releasePoint != null)
            Gizmos.DrawWireCube(skill.releasePoint.position, skill.skillArea);
    }

}
