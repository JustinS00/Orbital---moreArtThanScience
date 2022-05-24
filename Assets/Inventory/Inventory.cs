using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {   
    //should not be here
    private int maxItemsPerStack = 64;
    //starter tools
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

    public int THRESHOLD = 40; // half of the size of each slot
    private InventorySlot movingSlot;
    private bool isMovingItem;

    public GameObject itemCursor;
    
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

    private void Update() {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem) {
            itemCursor.GetComponent<Image>().sprite = movingSlot.item.sprite;
        }
        if (Input.GetMouseButtonDown(0)) {
            if (isMovingItem) {
                EndItemMove();
            } else {
                BeginItemMove();
            }
        } else if (Input.GetMouseButtonDown(1)) {
            if (isMovingItem) {
                EndItemMoveSingle();

            } else {
                BeginItemMoveHalf();
            }
        }
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
            for (int y = inventoryHeight - 1; y >= 0; y--) {
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
            for (int y = inventoryHeight - 1; y >= 0; y--) {
                if (inventory[x,y] != null && inventory[x,y].quantity <= 0) {
                    inventory[x,y] = null;
                }
                if (inventory[x,y] == null) {
                    uiSlots[x,y].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    uiSlots[x,y].transform.GetChild(0).GetComponent<Image>().enabled = false;

                    uiSlots[x,y].transform.GetChild(1).GetComponent<Text>().text = new string("0");
                    uiSlots[x,y].transform.GetChild(1).GetComponent<Text>().enabled = false;
                } else if (inventory[x,y] != null) { // minecraft style if quantity == 1 do not display
                    uiSlots[x,y].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    uiSlots[x,y].transform.GetChild(0).GetComponent<Image>().sprite = inventory[x,y].item.sprite;

                    uiSlots[x,y].transform.GetChild(1).GetComponent<Text>().text = inventory[x,y].quantity.ToString();
                    uiSlots[x,y].transform.GetChild(1).GetComponent<Text>().enabled = true;
                }
            }
        }
        //Update hot bar
        for (int x = 0; x < inventoryWidth; x++) {
            if (inventory[x, 0] == null || inventory[x, 0].quantity <= 0) {
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().enabled = false;

                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().text = new string("0");
                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().enabled = false;
            } else if (inventory[x, 0] != null) {
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().sprite = inventory[x, 0].item.sprite;

                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().text = inventory[x, 0].quantity.ToString();
                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().enabled = true;
            }
        }
    }

    public bool Add(ItemClass item) {
        for (int y = 0; y < inventoryHeight; y++) {
            for (int x = 0; x < inventoryWidth; x++) {
                //if slot is empty
                if (inventory[x,y] != null && inventory[x,y].item.name == item.name && inventory[x,y].quantity < maxItemsPerStack && item.isStackable) {
                    inventory[x,y].quantity += 1;
                    UpdateInventoryUI();
                    return true;
                } else if (inventory[x,y] == null) {
                    inventory[x,y] = new InventorySlot(item, 1);
                    UpdateInventoryUI();
                    return true;
                }
            }
        } //inventory is full
        return false;
    }

    public void RemoveFromHotBar(ItemClass item, int index) {
        if (inventory[index, 0] != null && inventory[index, 0].item.name == item.name) {
            inventory[index, 0].quantity = inventory[index, 0].quantity - 1;
            UpdateInventoryUI();
        } 
    }

    private Vector2Int GetClosestSlot() {
        //Debug.Log("Called GetClosestSlot()");
        for (int i = 0; i < inventoryWidth; i ++) {
            for (int j = 0; j < inventoryHeight; j ++) {
                if (Input.mousePosition != null && uiSlots[i,j].transform.position != null && Vector2.Distance(uiSlots[i,j].transform.position, Input.mousePosition) <= THRESHOLD) {
                    return new Vector2Int(i,j);
                }
            }
        }
        return new Vector2Int(-1,-1);
    }

    private bool BeginItemMove() {
        //Debug.Log("Called BeginItemMove()");
        Vector2Int pos = GetClosestSlot();
        if (pos == new Vector2Int(-1,-1) ) {
            return false;
        }
        //Debug.Log(pos);
        InventorySlot originalSlot = inventory[pos.x, pos.y];
        //Debug.Log(originalSlot.item);
        if (originalSlot == null || originalSlot.item == null) {
            return false;
        }
        movingSlot = new InventorySlot(originalSlot);
        inventory[pos.x, pos.y] = null;
        UpdateInventoryUI();
        isMovingItem = true;
        return true;
    }
    
    private bool BeginItemMoveHalf() {
        //Debug.Log("Called BeginItemMove()");
        Vector2Int pos = GetClosestSlot();
        if (pos == new Vector2Int(-1,-1) ) {
            return false;
        }
        //Debug.Log(pos);
        InventorySlot originalSlot = inventory[pos.x, pos.y];
        //Debug.Log(originalSlot.item);
        if (originalSlot == null || originalSlot.item == null) {
            return false;
        }
        int quantityMove = Mathf.FloorToInt(originalSlot.quantity / 2);
        if (quantityMove == 0) {
            return false;
        }
        
        movingSlot = new InventorySlot(originalSlot.item, quantityMove);
        inventory[pos.x, pos.y] = new InventorySlot(originalSlot.item, originalSlot.quantity - quantityMove);
        UpdateInventoryUI();
        isMovingItem = true;
        return true;
    }

    private bool EndItemMove() {
        Vector2Int pos = GetClosestSlot();
        if (pos == new Vector2Int(-1,-1) ) { 
            // click outside inventory, either add back to inventory or (-> this) throw away (To be implemented)
            return false;
        }
         InventorySlot originalSlot = inventory[pos.x, pos.y];
        if (originalSlot == null) {
            inventory[pos.x, pos.y] = new InventorySlot(movingSlot);
            movingSlot = null;
            isMovingItem = false;
            UpdateInventoryUI();
            return true;
        } else if (originalSlot.item != null && movingSlot.item != null) {
            if (originalSlot.item.name == movingSlot.item.name && originalSlot.item.isStackable
                && originalSlot.quantity < maxItemsPerStack) {
                int totalQuantity = originalSlot.quantity + movingSlot.quantity;
                originalSlot.quantity = Mathf.Min(totalQuantity, maxItemsPerStack);
                movingSlot.quantity = totalQuantity - originalSlot.quantity;
                if (movingSlot.quantity == 0) {
                    movingSlot = null;
                    UpdateInventoryUI();
                    isMovingItem = false;
                    return true;
                }
                UpdateInventoryUI();
                return false;
            } else if (originalSlot.item.name != movingSlot.item.name)  {
                InventorySlot tempSlot = new InventorySlot(originalSlot);
                inventory[pos.x, pos.y] = new InventorySlot(movingSlot);
                movingSlot = tempSlot;
                UpdateInventoryUI();
                return false;
            }
        } 
        return false;
    }

    private bool EndItemMoveSingle() {
        Vector2Int pos = GetClosestSlot();
        if (pos == new Vector2Int(-1,-1) ) {
            return false;
        } 
        InventorySlot originalSlot = inventory[pos.x, pos.y];
        if (originalSlot == null) {
            inventory[pos.x, pos.y] = new InventorySlot(movingSlot.item, 1);
            movingSlot.quantity -= 1;
        } else if (originalSlot.item.name == movingSlot.item.name 
        && originalSlot.item.isStackable && originalSlot.quantity < maxItemsPerStack) {
            movingSlot.quantity -= 1;
            originalSlot.quantity += 1;
        }
        if (movingSlot.quantity == 0) {
            isMovingItem = false;
            movingSlot = null;
        } else {
            isMovingItem = true;
        }

        UpdateInventoryUI();
        return true;
        
    }

} 
