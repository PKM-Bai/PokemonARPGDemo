using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList_SO", menuName = "Inventory/ItemDataList")]
public class ItemDataList_SO : ScriptableObject {
    public List<ItemDetails> itemList;
    List<ItemDetails> searchItems = new List<ItemDetails>();

    // 使用id获取道具
    public ItemDetails GetItemDetails(int itemID)
    {
        return itemList.Find(i => i.itemID == itemID);
    }

    /// <summary>
    /// 使用ID或者名称查询物品 模糊查询
    /// </summary>
    /// <param name="searchStr">输入查询的字符串</param>
    /// <returns></returns>
    public List<ItemDetails> SearchItemBy_ID_ItemName(string searchStr)
    {
        // 如果查询物品列表不为空，就先将它清空
        if(searchItems.Count != 0)
            searchItems.Clear();
        
        // 使用foreach循环遍历
        foreach (var item in itemList)
        {
            // 如果当前遍历的道具id或者name，含有输入的字符，就将它添加到查询列表中
            if(item.itemID.ToString().Contains(searchStr) || item.itemName.Contains(searchStr))
                searchItems.Add(item);
        }

        return searchItems;
    }
}