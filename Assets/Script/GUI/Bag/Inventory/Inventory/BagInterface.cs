using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MyPokemon.Inventory
{
    public class BagInterface : MonoBehaviour
    {

        public List<GameObject> itemCategory;
        public List<GameObject> itemCategory_active;
        public TextMeshProUGUI bagTabText;

        int bagTab_num;

        private void OnEnable()
        {
            EventHandler.SwitchInventoryTab += OnSwitchInventoryTab;
            SwitchBagTab(InventoryManager.Instance.tabType);

            if (InventoryManager.Instance)
                InventoryManager.RefreshInventory();
        }

        private void OnDisable()
        {
            EventHandler.SwitchInventoryTab -= OnSwitchInventoryTab;
        }

        private void OnSwitchInventoryTab(TabTypeEnum tabType)
        {
            SwitchBagTab(tabType);
        }

        /// <summary>
        /// 切换背包标签图片
        /// </summary>
        /// <param name="tabType"></param>
        public void SwitchBagTab(TabTypeEnum tabType)
        {
            bagTabText.text = tabType.ToString();

            bagTab_num = (int)tabType;
            itemCategory[bagTab_num].SetActive(false);
            itemCategory_active[bagTab_num].SetActive(true);

            for (int i = 0; i < itemCategory.Count; i++)
            {
                if (i != bagTab_num)
                {
                    itemCategory[i].SetActive(true);
                    itemCategory_active[i].SetActive(false);
                }
            }

        }


    }
}