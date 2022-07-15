using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBloodLoss : MonoBehaviour
{
    BoxCollider2D coll;
    public int lossHP;

    private void OnEnable() {
        EventHandler.FinishSceneLoadedEvent += InitPlayerBloodLoss;
    }

    private void OnDisable() {
        EventHandler.FinishSceneLoadedEvent -= InitPlayerBloodLoss;
    }

    private void InitPlayerBloodLoss()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            if(transform.GetComponentInParent<Enemy>().transform.localScale.x > 0)
                other.GetComponent<PlayerControl>().OnHit(Vector2.right, lossHP);
            else if(transform.GetComponentInParent<Enemy>().transform.localScale.x < 0)
                other.GetComponent<PlayerControl>().OnHit(Vector2.left, lossHP);
        }
    }

    
}
