using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyPokemon.Inventory
{
    public class TabType : MonoBehaviour
    {
        [LabelText("口袋名称")]
        public TextMeshProUGUI itemPocketText;
        [LabelText("类别标签按钮")]
        public Button tabButton;
        [LabelText("类别标签类型")]
        public TabTypeEnum tabType;

        private void Awake() {
            itemPocketText.text = TabTypeEnum.道具口袋.ToString();
        }
        
        private void Start()
        {
            tabButton.onClick.AddListener(onClickTab);
        }

        public void onClickTab()
        {
            InventoryManager.Instance.tabType = tabType;
            itemPocketText.text = tabType.ToString();
            InventoryManager.RefreshInventory();
        }
        
    }

}