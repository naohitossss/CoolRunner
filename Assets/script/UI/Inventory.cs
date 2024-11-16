using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Dictionary<ItemData, int> itemToCountMap = new Dictionary<ItemData, int>();
    private GameObject inventoryPanel;         
    public ItemData iceItem;          
    public ItemData energyDrinkItem;
    private HeatStroke playerHeatstroke;
    private LaneMovement playerLaneMovement;
    public GameObject itemRowPrefab;

    private void Awake()
    {
        // 同じGameObjectから必要なコンポーネントを取得
        playerHeatstroke = GetComponent<HeatStroke>();
        playerLaneMovement = GetComponent<LaneMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RemoveItem(energyDrinkItem, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Item itemGet = other.GetComponent<Item>();
        if (itemGet != null)
        {
            if(itemGet.GetisStackable())
                AddItem(itemGet.GetitemData(), 1);

            if(itemGet.GetitemName() == "Ice")
            {
                playerHeatstroke.currentStroke -= itemGet.GetitemValue();
            }
            Destroy(other.gameObject);
        }
    }

    public void AddItem(ItemData item, int count)
    {
        if (itemToCountMap.ContainsKey(item))
        {
            itemToCountMap[item] += count;

            if (itemToCountMap[item] > item.maxStackSize)
            {
                itemToCountMap[item] = item.maxStackSize;
            }
        }
        else
        {
            itemToCountMap[item] = Mathf.Min(count, item.maxStackSize);
        }

        UpdateInventoryUI(itemToCountMap);
    }

    public void RemoveItem(ItemData item, int count)
    {
        if (itemToCountMap.ContainsKey(item))
        {
            if (itemToCountMap[item] <= 0)
            {
                itemToCountMap[item] = 0;
            }
            else {
                Debug.Log("use");
                itemToCountMap[item] -= count;
                switch (item.itemName)
                {
                    case "EnergyDrink":
                        playerLaneMovement.DrinkEnergy(item.value);
                        break;
                }
            }
        }

        UpdateInventoryUI(itemToCountMap);
    }

    public void UpdateInventoryUI(Dictionary<ItemData, int> items)
    {
        Debug.Log("UIupdate");
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var itemEntry in items)
        {
            GameObject itemRow = Instantiate(itemRowPrefab, inventoryPanel.transform);
            int itemCount = itemEntry.Value;
            Debug.Log(itemCount);
            for (int i = 0; i < itemEntry.Value; i++)
            {
                GameObject slot = Instantiate(itemEntry.Key.itemSlot, itemRow.transform);
            }
        }
    }

    public void SetInventoryPanel(GameObject panel)
    {
        inventoryPanel = panel;
    }
}
    


