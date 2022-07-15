using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHurt : MonoBehaviour
{
    public PokemonAttribute pokemon;
    private Rigidbody2D rb;
    private Animator animator;
    public Skill_SO skill;
    int hurt;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            hurt = PokemonManager.Instance.CalculateSkillHurt(pokemon, other.GetComponent<Enemy>().enmeyPokemon, skill);
            animator.speed = 2;     // 动画播放速度加快
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            if (transform.localScale.x > 0)
                other.GetComponent<Enemy>().GetHit(Vector2.right, hurt, PokemonManager.Instance.GetHurtMagnification(other.GetComponent<Enemy>().enmeyPokemon, skill));
            else if (transform.localScale.x < 0)
                other.GetComponent<Enemy>().GetHit(Vector2.left, hurt, PokemonManager.Instance.GetHurtMagnification(other.GetComponent<Enemy>().enmeyPokemon, skill));
        }
    }
}
