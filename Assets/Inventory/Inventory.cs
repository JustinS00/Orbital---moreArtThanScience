using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {   

    private int maxItemsPerStack = 64;

    public ToolClass tool;
    public Vector2 offset;
    public Vector2 multiplier;
    public GameObject InventoryUI;
    public GameObject inventorySlotPrefab;

    public int inventoryWidth;
    public int inventoryHeight;
    public InventorySlot[,] inventory;
    public GameObject[,] uiSlots;
    
    // Start is called before the first frame update
    private void Start() {
        inventory = new InventorySlot[inventoryWidth, inventoryHeight];
        uiSlots = new GameObject[inventoryWidth, inventoryHeight];
        SetupUI();
        UpdateInventoryUI();
        Add(new ItemClass(tool));
        Add(new ItemClass(tool));
        Add(new ItemClass(tool));
    }

    void SetupUI() {
        for (int x = 0; x < inventoryWidth; x++) {
            for (int y = 0; y < inventoryHeight; y++) {
                GameObject inventorySlot = Instantiate(inventorySlotPrefab, InventoryUI.transform.GetChild(0).transform);
                inventorySlot.GetComponent<RectTransform>().localPosition = new Vector3(x * multiplier.x + offset.x ,y * multiplier.y + offset.y);
                uiSlots[x,y] = inventorySlot;
                inventory[x,y] = null;
            }
        }
    }

    void UpdateInventoryUI() {
        for (int x = 0; x < inventoryWidth; x++) {
            for (int y = 0; y < inventoryHeight; y++) {
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
    }
    /*
    public void Add(ItemClass item) {
        for (int y = inventoryHeight - 1; y >= 0; y--) {
            for (int x = 0; x < inventoryWidth; x++) {
                //if slot is empty
                if (inventory[x,y] == null) {
                    inventory[x,y] = new InventorySlot(item, new Vector2Int(x,y), 1);
                    UpdateInventoryUI();
                    return;
                }
            }
        }
    }
    */

    public void Add(ItemClass item) {
        for (int y = inventoryHeight - 1; y >= 0; y--) {
            for (int x = 0; x < inventoryWidth; x++) {
                //if slot is empty
                if (inventory[x,y] != null && inventory[x,y].item.name == item.name && inventory[x,y].quantity < maxItemsPerStack && item.isStackable) {
                    inventory[x,y].quantity += 1;
                    UpdateInventoryUI();
                    return;
                } else if (inventory[x,y] == null) {
                    inventory[x,y] = new InventorySlot(item, new Vector2Int(x,y), 1);
                    UpdateInventoryUI();
                    return;
                }
            }
        }
    }


    public void Remove(ItemClass item) {

    }
}
