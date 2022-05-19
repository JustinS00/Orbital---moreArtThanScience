using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {   

    private int maxItemsPerStack = 64;

    public ToolClass tool;

    public Vector2 offsetInv;
    public Vector2 offsetHotbar;
    public Vector2 multiplierInv;
    public Vector2 multiplierHotbar;

    public GameObject InventoryUI;
    public GameObject HotbarUI;
    public GameObject inventorySlotPrefab;

    public int inventoryWidth;
    public int inventoryHeight;

    public InventorySlot[] hotbar;
    public InventorySlot[,] inventory;
    

    public GameObject[] hotbarUISlots;
    public GameObject[,] uiSlots;
    
    // Start is called before the first frame update
    private void Start() {
        inventory = new InventorySlot[inventoryWidth, inventoryHeight];
        hotbar = new InventorySlot[inventoryWidth];
        uiSlots = new GameObject[inventoryWidth, inventoryHeight];
        hotbarUISlots = new GameObject[inventoryWidth];
        SetupUI();
        UpdateInventoryUI();
        Add(new ItemClass(tool));

    }

    void SetupUI() {
        //set up hot bar
        for (int x = 0; x < inventoryWidth; x++) {
            GameObject inventorySlot = Instantiate(inventorySlotPrefab, HotbarUI.transform.GetChild(0).transform);
            inventorySlot.GetComponent<RectTransform>().localPosition = new Vector3(x * multiplierHotbar.x + offsetHotbar.x , offsetHotbar.y);
            hotbarUISlots[x] = inventorySlot;
            hotbar[x] = null;
        }

        //set up main inventory
        for (int x = 0; x < inventoryWidth; x++) {
            for (int y = 0; y < inventoryHeight; y++) {
                GameObject inventorySlot = Instantiate(inventorySlotPrefab, InventoryUI.transform.GetChild(0).transform);
                inventorySlot.GetComponent<RectTransform>().localPosition = new Vector3(x * multiplierInv.x + offsetInv.x ,y * multiplierInv.y + offsetInv.y);
                uiSlots[x,y] = inventorySlot;
                inventory[x,y] = null;
            }
        }
    }

    void UpdateInventoryUI() {
        //Update main inventory
        for (int x = 0; x < inventoryWidth; x++) {
            for (int y = 0; y < inventoryHeight; y++) {
                if (inventory[x,y] != null && inventory[x,y].quantity <= 0) {
                    inventory[x,y] = null;
                }
                if (inventory[x,y] == null) {
                    uiSlots[x,y].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    uiSlots[x,y].transform.GetChild(0).GetComponent<Image>().enabled = false;

                    uiSlots[x,y].transform.GetChild(1).GetComponent<Text>().text = new string("0");
                    uiSlots[x,y].transform.GetChild(1).GetComponent<Text>().enabled = false;
                } else if (inventory[x,y] != null) {
                    uiSlots[x,y].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    uiSlots[x,y].transform.GetChild(0).GetComponent<Image>().sprite = inventory[x,y].item.sprite;

                    uiSlots[x,y].transform.GetChild(1).GetComponent<Text>().text = inventory[x,y].quantity.ToString();
                    uiSlots[x,y].transform.GetChild(1).GetComponent<Text>().enabled = true;
                }
            }
        }
        //Update hot bar
        for (int x = 0; x < inventoryWidth; x++) {
            if (inventory[x, inventoryHeight - 1] == null || inventory[x, inventoryHeight - 1].quantity <= 0) {
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().enabled = false;

                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().text = new string("0");
                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().enabled = false;
            } else if (inventory[x, inventoryHeight - 1] != null) {
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().sprite = inventory[x, inventoryHeight - 1].item.sprite;

                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().text = inventory[x, inventoryHeight - 1].quantity.ToString();
                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().enabled = true;
            }
        }
    }


    public bool Add(ItemClass item) {
        for (int y = inventoryHeight - 1; y >= 0; y--) {
            for (int x = 0; x < inventoryWidth; x++) {
                //if slot is empty
                if (inventory[x,y] != null && inventory[x,y].item.name == item.name && inventory[x,y].quantity < maxItemsPerStack && item.isStackable) {
                    inventory[x,y].quantity += 1;
                    UpdateInventoryUI();
                    return true;
                } else if (inventory[x,y] == null) {
                    inventory[x,y] = new InventorySlot(item, new Vector2Int(x,y), 1);
                    UpdateInventoryUI();
                    return true;
                }
            }
        }
        return false;
    }

    public void Remove(ItemClass item, int index) {
        if (inventory[index, inventoryHeight - 1] != null && inventory[index, inventoryHeight - 1].item.name == item.name) {
            inventory[index, inventoryHeight - 1].quantity = inventory[index, inventoryHeight - 1].quantity - 1;
            UpdateInventoryUI();
        } 
    }
}
