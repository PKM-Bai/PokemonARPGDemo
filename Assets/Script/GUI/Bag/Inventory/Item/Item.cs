using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyPokemon.Inventory
{
    [System.Serializable]
    public class Item : MonoBehaviour
    {
        public string[] itemNames;
        [HideInInspector]
        public List<ItemDetails> thisItems;
        public Inventory_SO thisInventory;
        public GetItemType getItemType;
        public bool isDestory;
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D coll;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (itemNames.Length != 0)
            {
                Init(itemNames);
            }
        }

        public void Init(string[] names)
        {
            itemNames = names;

            //Inventory获得当前数据
            thisItems = InventoryManager.Instance.getItems(itemNames);
            if (thisItems != null)
            {
                //修改碰撞体尺寸
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
                coll.size = newSize;
                coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
            }
        }
    }
}