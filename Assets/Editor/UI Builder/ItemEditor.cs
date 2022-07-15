using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
public class ItemEditor : EditorWindow
{
    private ItemDataList_SO dataBase;           // 受到编辑的so文件 
    private List<ItemDetails> itemList;         // 当前编辑器展示的列表数据
    private VisualTreeAsset itemRowTemplate;    // 物品列表中 物品行的模板文件
    private ListView itemListView;              // 
    private ScrollView itemDetailsSection;
    private ItemDetails activeItem;
    private TextField searchBar;
    private DropdownField sortDropdown;
    private string currentSortMode;
    private string[] sortMode;

    // *默认预览图片
    private Sprite defaultIcon;
    private VisualElement iconPreview;

    [MenuItem("My Editor/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemDatabaseEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
        // 拿到物品列表中 物品的模板
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRowTemplate.uxml");
        // 拿到默认Icon图片
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/Image/Pokemon/items/Bag_未知_Sprite.png");

        // 变量赋值-获取组件
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        itemDetailsSection = root.Q<ScrollView>("ScrollView");
        iconPreview = itemDetailsSection.Q<VisualElement>("Icon");
        // 物品搜索栏
        searchBar = root.Q<TextField>("Search");
        searchBar.RegisterValueChangedCallback<string>(OnSearchBarTextChange);
        // 列表排序
        sortDropdown = root.Q<DropdownField>("Sort");
        sortDropdown.RegisterValueChangedCallback(OnSortModeChange);
        sortMode = sortDropdown.choices.ToArray();
        currentSortMode = sortDropdown.choices[0];

        // 获得按钮
        root.Q<Button>("AddButton").clicked += OnAddItemClicked;
        root.Q<Button>("DeleteButton").clicked += OnDelItemClicked;

        // 加载数据
        LoadDataBase();
        
        // 生成ListView
        GenerateListView();
    }


    // *搜索物品
    private void OnSearchBarTextChange(ChangeEvent<string> evt)
    {
        if (evt.newValue != string.Empty || evt.newValue != "")
        {
            itemList = dataBase.SearchItemBy_ID_ItemName(evt.newValue);
        }
        else
        {
            itemList = dataBase.itemList;
        }

        GenerateListView();
    }

    // *物品列表排序
    private void OnSortModeChange(ChangeEvent<string> evt)
    {
        currentSortMode = evt.newValue;

        //? 升序（按ID）
        if (evt.newValue == sortMode[0])
        {
            // 先降序排序
            itemList.Sort(delegate (ItemDetails a, ItemDetails b) {
                return ((a.itemID > b.itemID) ? 1 : -1);
            });
        }
        //? 降序（按ID）
        else if (evt.newValue == sortMode[1])
        {
            // 降序排序
            itemList.Sort(delegate (ItemDetails a, ItemDetails b) {
                return ((b.itemID > a.itemID) ? 1 : -1);
            });
        }

        GenerateListView();
    }


    // *加载基本数据
    private void LoadDataBase()
    {
        var dataArray = AssetDatabase.FindAssets("ItemDataList_SO");
        if (dataArray.Length > 0)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            dataBase = (ItemDataList_SO)AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO));
        }

        itemList = dataBase.itemList;
        // 如果不标记则无法保存数据
        EditorUtility.SetDirty(dataBase);
    }

    // *生成ListView中的数据
    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();    // 复制

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < itemList.Count)
            {
                if (itemList[i].itemImage != null)
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemImage.texture;
                e.Q<Label>("Name").text = itemList[i] == null ? "No Name" : itemList[i].itemID + "_" + itemList[i].itemName;
            }
        };

        itemListView.itemsSource = itemList;    // 源文件
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;

        // 当点击物品时触发
        itemListView.onSelectionChange += OnListSelectionChange;

        // 右侧信息面板不可见
        itemDetailsSection.visible = false;
    }



    #region 按钮事件
    private void OnDelItemClicked()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        itemDetailsSection.visible = false;
    }

    private void OnAddItemClicked()
    {
        ItemDetails newItem = new ItemDetails();
        newItem.itemID = itemList[itemList.Count - 1].itemID + 1; // 当前列表中最后一个物品的ID往后+1
        newItem.itemName = "New Item";

        itemList.Add(newItem);
        itemListView.Rebuild();
    }
    #endregion

    private void OnListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (ItemDetails)selectedItem.First();
        GetItemDetails();
        EditorUtility.SetDirty(dataBase);
        itemDetailsSection.visible = true;
    }


    // *获取物品的详情数据
    private void GetItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();  // 

        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        // 回调函数 如果有更改数据，随之更新
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemID = evt.newValue;
        });

        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });


        itemDetailsSection.Q<EnumField>("TabType").Init(activeItem.tabType);
        itemDetailsSection.Q<EnumField>("TabType").value = activeItem.tabType;
        itemDetailsSection.Q<EnumField>("TabType").RegisterValueChangedCallback(evt =>
        {
            activeItem.tabType = (TabTypeEnum)evt.newValue;
        });

        itemDetailsSection.Q<EnumField>("ActionType").Init(activeItem.itemType);
        itemDetailsSection.Q<EnumField>("ActionType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ActionType").RegisterValueChangedCallback(evt =>
        {
            // Debug.Log(evt.newValue);
            activeItem.itemType = (ItemActionType)evt.newValue;
        });

        iconPreview.style.backgroundImage = activeItem.itemImage == null ? defaultIcon.texture : activeItem.itemImage.texture;
        itemDetailsSection.Q<ObjectField>("Image").value = activeItem.itemImage;
        itemDetailsSection.Q<ObjectField>("Image").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemImage = newIcon;
            itemDetailsSection.Q<VisualElement>("Icon").style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
            itemListView.Rebuild();
        });

        itemDetailsSection.Q<TextField>("Info").value = activeItem.itemInfo;
        itemDetailsSection.Q<TextField>("Info").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemInfo = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("BuyMoney").value = activeItem.buyMoney;
        itemDetailsSection.Q<IntegerField>("BuyMoney").RegisterValueChangedCallback(evt =>
        {
            activeItem.buyMoney = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("SellMoney").value = activeItem.sellMoney;
        itemDetailsSection.Q<IntegerField>("SellMoney").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellMoney = evt.newValue;
        });
    }



}