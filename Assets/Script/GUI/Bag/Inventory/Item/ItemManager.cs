using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MGame.Save;

namespace MyPokemon.Inventory
{
    public class ItemManager : MonoBehaviour, ISaveable
    {
        public Item itemPrefab;
        private Transform itemParent;
        //记录场景Item
        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();

        public string GUID => GetComponent<DataGUID>().guid;

        private void OnEnable() {
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadItemEvent;
            EventHandler.FinishSceneLoadedEvent += OnFinishSceneLoadedItemEvent;
            EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        }

        private void OnDisable() {
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadItemEvent;
            EventHandler.FinishSceneLoadedEvent -= OnFinishSceneLoadedItemEvent;
            EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        }

        private void Start() {
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }

        private void OnBeforeSceneUnloadItemEvent()
        {
            GetAllSceneItems();
        }

        private void OnFinishSceneLoadedItemEvent()
        {
            if (GameObject.FindWithTag("ItemParent") != null)
                itemParent = GameObject.FindWithTag("ItemParent").transform;
            
            RecreateAllItems();
        }

        private void OnStartNewGameEvent(int index)
        {
            sceneItemDict.Clear();
        }


        /// <summary>
        ///* 获得当前场景所有物品
        /// </summary>
        private void GetAllSceneItems()
        {
            List<SceneItem> currentScaeneItems = new List<SceneItem>();
            
            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemNames = item.itemNames,
                    position = new SerializableVector3(item.transform.position)

                };
                currentScaeneItems.Add(sceneItem);
            }
            if(sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
                // 如果字典存储的已经有这个场景，那么久更新这个场景内的物品数据
                sceneItemDict[SceneManager.GetActiveScene().name] = currentScaeneItems;
            
            else// 如果没有存储过这个场景，那么就把他添加进去
                sceneItemDict.Add(SceneManager.GetActiveScene().name, currentScaeneItems);
        }

        /// <summary>
        ///* 重新创建所有场景物品
        /// </summary>
        private void RecreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
            {
                if(currentSceneItems != null)
                {
                    // 把场景中所有物品删除
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }
                    // 重新生成创建
                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.itemNames);
                    }
                }
            }
        }

        public GameSaveData GenerateSaveData()
        {
            GetAllSceneItems();
            GameSaveData saveData = new GameSaveData();
            saveData.sceneItemDict = new Dictionary<string, List<SceneItem>>();
            saveData.sceneItemDict = sceneItemDict;

            return saveData;
        }

        public void RestoreLoadData(GameSaveData saveData)
        {
            this.sceneItemDict = saveData.sceneItemDict;

            RecreateAllItems();
        }
    }
}

