using UnityEngine;

public class PikachuControl : PlayerControl
{


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attactMethod = GetComponent<AttactMethod>();
        movement = GetComponent<Movement>();
    }

    // 第一段攻击的事件
    void AtkOne()
    {
        attactMethod.AtkOneAudio.Play();
    }
    void AtkOneEvent()
    {
        
    }
    // 第二段攻击的事件
    void AtkTwo()
    {
        // 前突进
        rb.velocity = new Vector2(transform.localScale.x * (attactMethod.atkMoveSpeed + 2f), rb.velocity.y);
        attactMethod.AtkTwoAudio.Play();
    }
    // 第三段攻击的事件
    void AtkThree()
    {
        rb.velocity = new Vector2(transform.localScale.x * attactMethod.atkMoveSpeed, rb.velocity.y);
        // 升龙
        Enemy.state = AtkStatusEnum.Strikefly;
        attactMethod.AtkThreeAudio.Play();
    }





    
}
