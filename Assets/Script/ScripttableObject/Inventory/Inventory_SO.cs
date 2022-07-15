using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPokemon.Inventory
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/New Inventory")]
    public class Inventory_SO : ScriptableObject
    {
        public long money;
        public List<ItemStack> itemList;

        public ItemStack GetItemStack(int itemID)
        {
            return itemList.Find(i => i.itemID == itemID);
        }
    }

    [System.Serializable]
    public class ItemStack
    {
        public int itemID;
        public int itemCount;
    }
}