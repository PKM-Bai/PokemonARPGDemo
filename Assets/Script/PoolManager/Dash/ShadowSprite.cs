using UnityEngine;
using Sirenix.OdinInspector;

public class ShadowSprite : MonoBehaviour
{
  private Transform player;

  private SpriteRenderer thisSprite;
  private SpriteRenderer playerSprite;

  private Color color;

  [Header("时间控制参数")]
  [LabelText("显示时间")]
  public float activeTime;  // 显示时间
  [LabelText("开始显示的时间点")]
  public float activeStart; // 开始显示的时间点

  [Header("不透明度控制")]
  [LabelText("设置初始透明度")]
  public float alphaSet;  // 透明度初始值
  [LabelText("透明度")]
  private float alpha;    // 透明度
  [LabelText("渐变速度")]
  public float alphaMultiplier; // 渐变速度

  // 对象池创建
  private void OnEnable()
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    thisSprite = GetComponent<SpriteRenderer>();
    playerSprite = player.GetComponent<SpriteRenderer>();

    // 不透明度
    alpha = alphaSet;

    thisSprite.sprite = playerSprite.sprite;

    transform.position = player.position;
    transform.localScale = player.localScale;
    transform.rotation = player.rotation;

    activeStart = Time.time;  // 开始时间=当前游戏时间

  }
  
  void FixedUpdate()
  {
    // 透明度变化
    alpha *= alphaMultiplier;

    // 颜色变化
    color = new Color(1, 1, 1, alpha);

    // 将变化后的颜色进行赋值
    thisSprite.color = color;

    // 如果当前游戏时间 >= 启动的时间+显示时间
    if (Time.time >= activeStart + activeTime)
    {
      // 返回对象池
      ShadowPool.instance.ReturnPool(this.gameObject);

    }

  }
}
