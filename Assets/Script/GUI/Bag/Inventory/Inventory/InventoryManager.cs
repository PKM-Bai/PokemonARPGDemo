using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MGame.Save;

namespace MyPokemon.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>, ISaveable
    {
        public ItemDataList_SO itemDataList;
        public List<ItemDetails> playInventory;
        public Inventory_SO playerBag;   // 背包
        public TextMeshProUGUI playerMoney;
        public GameObject slotGrid;         // 背包的格子
        public Slot slotPrefab;             // 背包中的物品

        public TabTypeEnum tabType;     // 当前背包选中的口袋类型
        public ItemInfomation itemInfomation;
        public DialogueData_SO getItemDialogue;

        bool isContains;
        int itemIndex;

        public string GUID => GetComponent<DataGUID>().guid;

        private void OnEnable()
        {
            EventHandler.StartGameEvent += OnStartGameEvent;
            EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        }

        private void OnDisable()
        {
            EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        }

        private void Start()
        {
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }

        private void OnStartGameEvent(int index)
        {
            Init();
        }

        private void OnStartNewGameEvent(int index)
        {
            playerBag.itemList.Clear();
            playerBag.money = 0;
        }

        public void Init()
        {
            tabType = TabTypeEnum.道具口袋;
            RefreshInventory();
        }

        public List<ItemDetails> GetIventoryItem()
        {
            List<ItemDetails> itemList = new List<ItemDetails>();
            foreach (var item in playerBag.itemList)
            {
                itemList.Add(itemDataList.GetItemDetails(item.itemID));
            }
            return itemList;
        }

        //* 根据道具id获得对应道具实体
        public ItemDetails GetItemDetails(int id)
        {
            return itemDataList.itemList.Find(i => i.itemID == id);
        }

        //* 根据多个道具名称获得每个对应道具实体
        public List<ItemDetails> getItems(string[] items)
        {
            List<ItemDetails> thisItems = new List<ItemDetails>();
            for (int i = 0; i < items.Length; i++)
            {
                var item = itemDataList.itemList.Find(o => o.itemName == items[i]);
                if (item != null)
                    thisItems.Add(item);
            }
            return thisItems;
        }

        #region 向背包添加物品
        /// <summary>
        ///*  添加道具箱中的所有物品
        /// </summary>
        /// <param name="thisItems">物品的List列表</param>
        public void AllAddItem(List<ItemDetails> thisItems)
        {
            if (thisItems.Count == 1)
                AddNewItem(thisItems[0]);
            else
            {
                for (int i = 0; i < thisItems.Count; i++)
                    AddNewItem(thisItems[i]);
            }
        }
        /// <summary>
        ///*  随机添加物品
        /// </summary>
        /// <param name="thisItems">物品的List列表</param>
        public void RandomAddItem(List<ItemDetails> thisItems)
        {
            if (thisItems.Count == 1)
            {
                AddNewItem(thisItems[0]);
            }
            else if (thisItems.Count > 1)
            {
                itemIndex = Random.Range(0, thisItems.Count);
                AddNewItem(thisItems[itemIndex]);
            }

        }

        /// <summary>
        ///* 添加新物品
        /// </summary>
        /// <param name="thisItem">添加物品</param>
        public void AddNewItem(ItemDetails thisItem)
        {
            if (playerBag.itemList.Count != 0)
            {
                isContains = false;
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == thisItem.itemID)
                    {
                        isContains = true;
                        if (playerBag.itemList[i].itemCount < 999)
                            playerBag.itemList[i].itemCount++;
                        break;
                    }
                }
            }
            if (playerBag.itemList.Count == 0 || !isContains)
            {
                ItemStack itemStack = new ItemStack();
                itemStack.itemID = thisItem.itemID;
                itemStack.itemCount = 1;
                playerBag.itemList.Add(itemStack);
            }

            DialogueUI.Instance.PickUpItem(thisItem);

            QuestManager.Instance.UpdateItemAndEnemyProgress(thisItem.itemName, 1);
            InventoryManager.RefreshInventory();
        }
        //* 添加多个物品
        public void AddNewItem(ItemDetails thisItem, int count)
        {
            if (playerBag.itemList.Count != 0)
            {
                isContains = false;
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == thisItem.itemID)
                    {
                        isContains = true;
                        if (playerBag.itemList[i].itemCount < 999)
                            playerBag.itemList[i].itemCount += count;
                        break;
                    }
                }
            }
            if (playerBag.itemList.Count == 0 || !isContains)
            {
                ItemStack itemStack = new ItemStack();
                itemStack.itemID = thisItem.itemID;
                itemStack.itemCount = count;
                playerBag.itemList.Add(itemStack);
            }

            QuestManager.Instance.UpdateItemAndEnemyProgress(thisItem.itemName, count);
            InventoryManager.RefreshInventory();
        }
        #endregion


        /// <summary>
        ///* 在背包中创建增加新物品
        /// </summary>
        /// <param name="item">物品</param>
        public static void CreateNewItem(ItemDetails item)
        {
            // 创建一个Slot实例对象，将Slot生成到slotGrid的transform.position位置，角度不变,将slot设置在slotGrid下作为子集
            Slot newItem = Instantiate(InventoryManager.Instance.slotPrefab, InventoryManager.Instance.slotGrid.transform.position, Quaternion.identity, InventoryManager.Instance.slotGrid.transform);

            newItem.slotItem = item;
            newItem.slotImage.sprite = item.itemImage;
            newItem.slotItemName.text = item.itemName;
            ItemStack newItemStack = InventoryManager.Instance.playerBag.GetItemStack(item.itemID);

            newItem.slotNum.text = newItemStack.itemCount.ToString();

            // 将Bag物体下的itemInfomation（显示物品信息）物体并添加到创建的Slot中
            Transform[] transform = InventoryManager.Instance.gameObject.GetComponentsInParent<Transform>();
            foreach (Transform t in transform)
            {
                if (t.name == "Bag")
                {
                    foreach (Transform i in t.gameObject.GetComponentInChildren<Transform>())
                    {
                        if (i.name == "itemInformation")
                            InventoryManager.Instance.itemInfomation = i.gameObject.GetComponent<ItemInfomation>();
                    }
                }
            }
        }

        /// <summary>
        ///* 显示物品说明信息
        /// </summary>
        /// <param name="thisSlot">物品所在格</param>
        /// <param name="item">物品</param>
        public void showItemInfo(Slot thisSlot, ItemDetails item)
        {
            InventoryManager.Instance.itemInfomation.gameObject.SetActive(true);
            InventoryManager.Instance.itemInfomation.itmeImage.sprite = item.itemImage;
            InventoryManager.Instance.itemInfomation.itemName.text = item.itemName;
            InventoryManager.Instance.itemInfomation.itemInfo.text = item.itemInfo;

            InventoryManager.Instance.hideActiveImg(thisSlot);
            thisSlot.GetComponent<Image>().enabled = true;
        }

        /// <summary>
        ///* 隐藏物品说明信息
        /// </summary>
        public void hideItemInfo()
        {
            InventoryManager.Instance.itemInfomation.gameObject.SetActive(false);
        }

        /// <summary>
        ///* 隐藏物品选中效果
        /// </summary>
        /// <param name="thisSlot">物品所在格</param>
        public void hideActiveImg(Slot thisSlot)
        {
            foreach (Transform child in slotGrid.gameObject.transform)
            {
                Slot childSlot = child.GetComponent<Slot>();
                // 把除了点击到的物品以外所有选中效果图片隐藏
                if (childSlot.slotItemName.text != thisSlot.slotItemName.text)
                {
                    childSlot.GetComponent<Image>().enabled = false;
                }
            }
        }

        /// <summary>
        ///* 刷新背包
        /// </summary>
        public static void RefreshInventory()
        {
            InventoryManager.Instance.playerMoney.text = InventoryManager.Instance.playerBag.money.ToString();
            if (InventoryManager.Instance != null)
            {
                // 将背包中所有的物品都销毁
                for (int i = 0; i < InventoryManager.Instance.slotGrid.transform.childCount; i++)
                {
                    Destroy(InventoryManager.Instance.slotGrid.transform.GetChild(i).gameObject);
                }
                // 再重新根据标签类别生成物品
                InventoryManager.Instance.SwitchTabInventory();

                if (InventoryManager.Instance.itemInfomation != null)
                    InventoryManager.Instance.hideItemInfo();
            }
        }

        /// <summary>
        ///* 切换标签背包
        /// </summary>
        public void SwitchTabInventory()
        {
            EventHandler.CallSwitchInventoryTab(tabType);
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                //? 这个物品的数量小于等于0，将他移除背包
                if (playerBag.itemList[i].itemCount <= 0)
                {
                    playerBag.itemList.Remove(playerBag.itemList[i]);
                    continue;
                }
                ItemDetails item = itemDataList.GetItemDetails(playerBag.itemList[i].itemID);
                if (item.tabType == tabType)
                {
                    CreateNewItem(item);
                }
            }
        }

        /// <summary>
        ///* 检查任务物品在背包中有多少个 
        /// </summary>
        public void CheckQuestItemInBag(string questItemName)
        {
            foreach (var item in playerBag.itemList)
            {
                ItemDetails newItem = itemDataList.GetItemDetails(item.itemID);
                if (newItem.itemName == questItemName)
                    QuestManager.Instance.UpdateItemAndEnemyProgress(newItem.itemName, item.itemCount);
            }
        }

        //* 使用物品名字列表获取对应物品实体的列表
        public List<ItemDetails> WithItemNameGetItemList(List<string> itemNames)
        {
            List<ItemDetails> newItems = new List<ItemDetails>(); ;

            for (int i = 0; i < itemNames.Count; i++)
            {
                var item = itemDataList.itemList.Find(item => item.itemName == itemNames[i]);
                if (item != null)
                    newItems.Add(item);
            }
            return newItems;
        }
        public ItemDetails WithItemNameGetItemList(string itemName)
        {
            ItemDetails item = itemDataList.itemList.Find(item => item.itemName == itemName);
            if (item != null)
                return item;
            else
                return null;
        }

        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.playerBagDict = new Dictionary<string, List<ItemStack>>();
            saveData.playerBagDict.Add(playerBag.name, playerBag.itemList);

            saveData.moneyDict = new Dictionary<string, long>();
            saveData.moneyDict.Add(playerBag.name, playerBag.money);
            return saveData;
        }

        public void RestoreLoadData(GameSaveData saveData)
        {
            playerBag.itemList = saveData.playerBagDict[playerBag.name];
            playerBag.money = saveData.moneyDict[playerBag.name];
        }
    }

}