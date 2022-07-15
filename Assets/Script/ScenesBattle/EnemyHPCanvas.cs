using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyHPCanvas : MonoBehaviour
{   
    public GameObject EnemyHP;
    public List<GameObject> EnemyHPList;


    public void CreateEnemyHP()
    {  
        EnemyHPList.Add(Instantiate(EnemyHP, transform.position, Quaternion.identity, transform));
    }



}
