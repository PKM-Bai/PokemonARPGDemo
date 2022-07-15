
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace MyPokemon.Inventory
{
    public class Slot : MonoBehaviour, IPointerClickHandler
    {
        [LabelText("道具")]
        public ItemDetails slotItem;
        public TextMeshProUGUI slotItemName;
        public Image slotImage;
        public Text slotNum;

        public Button slotBtn;
        public Slot thisSlot;

        private void Start()
        {
            slotBtn.onClick.AddListener(callShowItemInfo);
        }

        void callShowItemInfo()
        {
            InventoryManager.Instance.showItemInfo(thisSlot, slotItem);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            callShowItemInfo();
            
        }
    }
}