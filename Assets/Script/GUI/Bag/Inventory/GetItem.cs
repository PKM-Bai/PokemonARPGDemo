using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPokemon.Inventory
{
    public class GetItem : MonoBehaviour
    {
        Item items;
        bool isPickUp;

        private void Update()
        {
            if (isPickUp)
            {
                if (Input.GetButtonDown("Interactive"))
                {
                    if (items.isDestory)
                    {
                        Destroy(items.gameObject);
                    }

                    if (items != null && items.thisItems != null)
                    {
                        // 拾取道具
                        if (items.getItemType == GetItemType.All)
                            InventoryManager.Instance.AllAddItem(items.thisItems);
                        if (items.getItemType == GetItemType.Random)
                            InventoryManager.Instance.RandomAddItem(items.thisItems);
                        if (items.getItemType == GetItemType.Only)
                            InventoryManager.Instance.AddNewItem(items.thisItems[0]);
                    }
                }

            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("WorldItem"))
            {
                isPickUp = true;
                items = other.GetComponent<Item>();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("WorldItem"))
            {
                isPickUp = false;
            }
        }
    }

}
