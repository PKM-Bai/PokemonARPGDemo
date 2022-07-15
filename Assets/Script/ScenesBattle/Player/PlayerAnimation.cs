using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    Movement movement;
    PlayerControl player;
    Rigidbody2D rb;
    // int groundID;
    int fallID;
    int dashID;
    
    

    void Start()
    {
      anim = GetComponent<Animator>();
      movement = GetComponentInParent<Movement>();
      rb = GetComponentInParent<Rigidbody2D>();
      player = GetComponent<PlayerControl>();
      //groundID = Animator.StringToHash("isOnGround");   // 获得这个动画参数的id
      //fallID = Animator.StringToHash("verticalVelocity");
    //   dashID = Animator.StringToHash("isDashing");
    }


    private void FixedUpdate() {
        anim.SetFloat("speed", Mathf.Abs(movement.xVelocity));
        // anim.SetBool("isJump", movement.isJump);
        anim.SetBool("isOnGround", player.isOnGround);
        anim.SetFloat("verticalVelocity", rb.velocity.y);
    }
}
