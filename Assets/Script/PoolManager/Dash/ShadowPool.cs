using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{

  public static ShadowPool instance;

  public GameObject shadowPrefab;

  public int shadowCount;

  private Queue<GameObject> availableObjects = new Queue<GameObject>();

  void Awake() 
  {
    instance = this;

    // 初始化对象池
    FillPool();
  }

  // 初始化对象池
  public void FillPool()
  {
    for (int i = 0; i < shadowCount; i++)
    {
        var newShadow = Instantiate(shadowPrefab);
        // 将新创建的shadow归为ShadowPool的子集
        newShadow.transform.SetParent(transform);

        // 取消启用，返回对象池
        ReturnPool(newShadow);
    }
  }

  public void ReturnPool(GameObject gameObject)
  {
    // 取消启用
    gameObject.SetActive(false);
    // 将对象保存到对象池中
    availableObjects.Enqueue(gameObject);

  }


  public GameObject GetFormPool()
  {
      if (availableObjects.Count == 0)
      {
          FillPool();
      }
      // 从对象池中取出
      var outShadow = availableObjects.Dequeue();
      // 选择启用
      outShadow.SetActive(true);

      return outShadow;
  }
}
