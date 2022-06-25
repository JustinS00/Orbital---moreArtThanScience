using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    //starter tools
    public EquipmentClass[] starter_equipment;
    public ConsumableClass apple;
    public ArrowClass arrow;
    public bool isShowing;

    private Vector2 offsetInv = new Vector2(-160, -110);
    private Vector2 offsetHotbar = new Vector2(-160, 0);
    private Vector2 multiplierInv = new Vector2(40, 40);
    private Vector2 multiplierHotbar = new Vector2(40, 40);
    private int EQUIPMENT_SECTION_Y_OFFSET = 30;

    public GameObject InventoryUI;
    public GameObject HotbarUI;
    public GameObject inventorySlotPrefab;
    public GameObject helmetSlotPrefab;
    public GameObject chestplateSlotPrefab;
    public GameObject leggingsSlotPrefab;
    public GameObject bootsSlotPrefab;

    [HideInInspector] public int inventoryWidth = 9;
    [HideInInspector] public int inventoryHeight = 4;
    private int NO_PIECES_OF_ARMOUR = 4;

    [HideInInspector] public InventorySlot[] hotbar;
    [HideInInspector] public InventorySlot[,] inventory;
    [HideInInspector] public InventorySlot[] armourSlots;

    [HideInInspector] public GameObject[] hotbarUISlots;
    [HideInInspector] public GameObject[,] uiSlots;
    [HideInInspector] public GameObject[] armourSlotsUI;

    private int THRESHOLD = 20; //slot size
    private InventorySlot movingSlot;
    private bool isMovingItem;

    public GameObject itemCursor;


    // Start is called before the first frame update
    private void Awake() {
        inventory = new InventorySlot[inventoryWidth, inventoryHeight];
        hotbar = new InventorySlot[inventoryWidth];
        armourSlots = new InventorySlot[NO_PIECES_OF_ARMOUR];
        uiSlots = new GameObject[inventoryWidth, inventoryHeight];
        hotbarUISlots = new GameObject[inventoryWidth];
        armourSlotsUI = new GameObject[NO_PIECES_OF_ARMOUR];
        SetupUI();
        UpdateInventoryUI();
        isShowing = false;
        foreach (EquipmentClass equipment in starter_equipment) {
            Add(Instantiate(equipment));
        }
        for (int i = 0; i < 20; i++) {
            Add(Instantiate(apple));
            Add(Instantiate(arrow));
        }
    }

    void SetupUI() {
        //set up hot bar
        for (int x = 0; x < inventoryWidth; x++) {
            GameObject inventorySlot = Instantiate(inventorySlotPrefab, HotbarUI.transform.GetChild(0).transform);
            inventorySlot.GetComponent<RectTransform>().localPosition = new Vector3(x * multiplierHotbar.x + offsetHotbar.x, offsetHotbar.y);
            inventorySlot.transform.GetChild(2).gameObject.SetActive(false);
            hotbarUISlots[x] = inventorySlot;
            hotbar[x] = null;
        }
        //set up main inventory
        for (int x = 0; x < inventoryWidth; x++) {
            for (int y = inventoryHeight - 1; y >= 0; y--) {
                GameObject inventorySlot = Instantiate(inventorySlotPrefab, InventoryUI.transform.GetChild(0).transform);
                inventorySlot.transform.GetChild(2).gameObject.SetActive(false);
                inventorySlot.GetComponent<RectTransform>().localPosition = new Vector3(x * multiplierInv.x + offsetInv.x, y * multiplierInv.y + offsetInv.y);
                uiSlots[x, y] = inventorySlot;
                inventory[x, y] = null;
            }
        }
        //set up equipment page
        GameObject helmetSlot = Instantiate(helmetSlotPrefab, InventoryUI.transform.GetChild(0).transform);
        GameObject chestplateSlot = Instantiate(chestplateSlotPrefab, InventoryUI.transform.GetChild(0).transform);
        GameObject leggingsSlot = Instantiate(leggingsSlotPrefab, InventoryUI.transform.GetChild(0).transform);
        GameObject bootsSlot = Instantiate(bootsSlotPrefab, InventoryUI.transform.GetChild(0).transform);
        armourSlotsUI[0] = helmetSlot;
        armourSlotsUI[1] = chestplateSlot;
        armourSlotsUI[2] = leggingsSlot;
        armourSlotsUI[3] = bootsSlot;
        for (int i = 0; i < NO_PIECES_OF_ARMOUR; i++) {
            armourSlotsUI[i].GetComponent<RectTransform>().localPosition = new Vector3(i * multiplierInv.x + offsetInv.x, inventoryHeight * multiplierInv.y + offsetInv.y + EQUIPMENT_SECTION_Y_OFFSET);
            armourSlotsUI[i].transform.GetChild(1).GetComponent<Text>().enabled = false;
            armourSlotsUI[i].transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    void UpdateInventoryUI() {
        //Update main inventory
        for (int x = 0; x < inventoryWidth; x++) {
            for (int y = inventoryHeight - 1; y >= 0; y--) {
                uiSlots[x, y].transform.GetChild(2).gameObject.SetActive(false);
                if (inventory[x, y] != null && inventory[x, y].quantity <= 0) {
                    inventory[x, y] = null;
                }
                if (inventory[x, y] == null) {
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = false;

                    uiSlots[x, y].transform.GetChild(1).GetComponent<Text>().text = new string("0");
                    uiSlots[x, y].transform.GetChild(1).GetComponent<Text>().enabled = false;

                } else if (inventory[x, y] != null) {
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    uiSlots[x, y].transform.GetChild(0).GetComponent<Image>().sprite = inventory[x, y].item.itemSprite;
                    if (!inventory[x, y].item.isStackable) {
                        uiSlots[x, y].transform.GetChild(1).GetComponent<Text>().enabled = false;
                        if (inventory[x, y].item.itemType == ItemClass.ItemType.equipment) {
                            EquipmentClass item = (EquipmentClass) inventory[x, y].item;
                            if (item.isBroken()) {
                                inventory[x, y] = null;
                            } else {
                                uiSlots[x, y].transform.GetChild(2).gameObject.SetActive(true);
                                uiSlots[x, y].transform.GetChild(2).GetComponent<EquipmentDurabilityBar>().SetMaxDurability(item.getTotalDurability());
                                uiSlots[x, y].transform.GetChild(2).GetComponent<EquipmentDurabilityBar>().SetDurability(item.getCurrentDurability());
                            }
                        }
                    } else {
                        uiSlots[x, y].transform.GetChild(1).GetComponent<Text>().text = inventory[x, y].quantity.ToString();
                        uiSlots[x, y].transform.GetChild(1).GetComponent<Text>().enabled = true;
                    }
                }
            }
        }

        for (int x = 0; x < NO_PIECES_OF_ARMOUR; x++) {
            armourSlotsUI[x].transform.GetChild(2).gameObject.SetActive(false);
            if (armourSlots[x] == null) {
                armourSlotsUI[x].transform.GetChild(0).GetComponent<Image>().color = new Color32(0, 0, 0, 100);
            } else {
                armourSlotsUI[x].transform.GetChild(0).GetComponent<Image>().sprite = armourSlots[x].item.itemSprite;
                armourSlotsUI[x].transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 225, 255);
                EquipmentClass item = (EquipmentClass) armourSlots[x].item;
                if (item.isBroken()) {
                    armourSlots[x] = null;
                } else {
                    armourSlotsUI[x].transform.GetChild(2).gameObject.SetActive(true);
                    armourSlotsUI[x].transform.GetChild(2).GetComponent<EquipmentDurabilityBar>().SetMaxDurability(item.getTotalDurability());
                    armourSlotsUI[x].transform.GetChild(2).GetComponent<EquipmentDurabilityBar>().SetDurability(item.getCurrentDurability());
                }
            }
        }

        //Update hot bar
        for (int x = 0; x < inventoryWidth; x++) {
            hotbarUISlots[x].transform.GetChild(2).gameObject.SetActive(false);
            if (inventory[x, 0] == null || inventory[x, 0].quantity <= 0) {
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().enabled = false;

                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().text = new string("0");
                hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().enabled = false;
            } else if (inventory[x, 0] != null) {
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarUISlots[x].transform.GetChild(0).GetComponent<Image>().sprite = inventory[x, 0].item.itemSprite;

                if (!inventory[x, 0].item.isStackable) {
                    hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().enabled = false;
                    if (inventory[x, 0].item.itemType == ItemClass.ItemType.equipment) {
                        EquipmentClass item = (EquipmentClass) inventory[x, 0].item;
                        hotbarUISlots[x].transform.GetChild(2).gameObject.SetActive(true);
                        hotbarUISlots[x].transform.GetChild(2).GetComponent<EquipmentDurabilityBar>().SetMaxDurability(item.getTotalDurability());
                        hotbarUISlots[x].transform.GetChild(2).GetComponent<EquipmentDurabilityBar>().SetDurability(item.getCurrentDurability());

                    }
                } else {
                    hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().text = inventory[x, 0].quantity.ToString();
                    hotbarUISlots[x].transform.GetChild(1).GetComponent<Text>().enabled = true;
                }
            }
        }
    }


    private void Update() {
        if (isShowing) {
            itemCursor.SetActive(isMovingItem);
            itemCursor.transform.position = Input.mousePosition;
            if (isMovingItem) {
                itemCursor.GetComponent<Image>().sprite = movingSlot.item.itemSprite;
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
        UpdateInventoryUI();
    }


    public int AddedItems(ItemClass item, int quantity) {
        int addedItems = 0;
        for (int i = 0; i < quantity; i++) {
            addedItems += Add(item) ? 1 : 0;
        }
        return addedItems;
    }

    public bool Add(ItemClass item) {
        for (int y = 0; y < inventoryHeight; y++) {
            for (int x = 0; x < inventoryWidth; x++) {
                //if slot is empty
                if (inventory[x, y] != null && inventory[x, y].item.itemName == item.itemName && item.isStackable && inventory[x, y].quantity < item.maxItemsPerStack) {
                    inventory[x, y].quantity += 1;
                    UpdateInventoryUI();
                    return true;
                } else if (inventory[x, y] == null) {
                    inventory[x, y] = new InventorySlot(item, 1);
                    UpdateInventoryUI();
                    return true;
                }
            }
        } //inventory is full
        return false;
    }

    public bool HasItemInInventory(ItemClass item, int quantity) {
        int quantityRemaining = quantity;

        for (int y = 0; y < inventoryHeight; y++) {
            for (int x = 0; x < inventoryWidth; x++) {
                if (inventory[x, y] != null && inventory[x, y].item.itemName == item.itemName) {
                    quantityRemaining -= inventory[x, y].quantity;
                }
            }
        }
        return (quantityRemaining <= 0);
    }

    public ItemClass HasItemInInventoryByString(string itemName) {

        for (int y = 0; y < inventoryHeight; y++) {
            for (int x = 0; x < inventoryWidth; x++) {
                if (inventory[x, y] != null && inventory[x, y].item.itemName == itemName && inventory[x, y].quantity > 0) {
                    return inventory[x, y].item;
                }
            }
        }

        return null;
    }


    public void RemoveItemFromInventory(ItemClass item, int quantity) {
        int quantityRemaining = quantity;

        for (int y = 0; y < inventoryHeight; y++) {
            for (int x = 0; x < inventoryWidth; x++) {
                if (quantityRemaining > 0 && inventory[x, y] != null && inventory[x, y].item.itemName == item.itemName) {
                    int currentQuantity = inventory[x, y].quantity;
                    if (currentQuantity <= quantityRemaining) {
                        inventory[x, y] = null;
                        quantityRemaining -= currentQuantity;
                    } else {
                        inventory[x, y].quantity -= quantityRemaining;
                        quantityRemaining = 0;
                    }
                }
            }
        }
        UpdateInventoryUI();
    }

    public void RemoveFromHotBar(ItemClass item, int index) {
        if (inventory[index, 0] != null && inventory[index, 0].item.itemName == item.itemName) {
            inventory[index, 0].quantity = inventory[index, 0].quantity - 1;
            UpdateInventoryUI();
        }
    }

    private Vector2Int GetClosestSlot() {

        Vector2Int slot = new Vector2Int(-1, -1);
        float distance = float.MaxValue;
        //Debug.Log("Called GetClosestSlot()");
        for (int i = 0; i < inventoryWidth; i++) {
            for (int j = 0; j < inventoryHeight; j++) {
                float currDistance = Vector2.Distance(uiSlots[i, j].transform.position, Input.mousePosition);
                if (Input.mousePosition != null && uiSlots[i, j].transform.position != null && currDistance <= THRESHOLD && currDistance <= distance) {
                    slot = new Vector2Int(i, j);
                    distance = currDistance;
                }
            }
        }
        for (int i = 0; i < NO_PIECES_OF_ARMOUR; i++) {
            float currDistance = Vector2.Distance(armourSlotsUI[i].transform.position, Input.mousePosition);
            if (Input.mousePosition != null && armourSlotsUI[i].transform.position != null && currDistance <= THRESHOLD) {
                slot = new Vector2Int(i, -1);
                distance = currDistance;
            }
        }
        return slot;
    }

    #region Move Items
    private bool BeginItemMove() {
        //Debug.Log("Called BeginItemMove()");
        Vector2Int pos = GetClosestSlot();
        if (pos == new Vector2Int(-1, -1)) {
            return false;
        }
        bool isArmourSlot = pos.y < 0;
        InventorySlot originalSlot = isArmourSlot ? armourSlots[pos.x] : inventory[pos.x, pos.y];

        if (originalSlot == null || originalSlot.item == null) {
            return false;
        }
        movingSlot = new InventorySlot(originalSlot);
        if (isArmourSlot) {
            armourSlots[pos.x] = null;
        } else {
            inventory[pos.x, pos.y] = null;
        }
        UpdateInventoryUI();
        isMovingItem = true;
        return true;
    }

    private bool BeginItemMoveHalf() {
        //Debug.Log("Called BeginItemMove()");
        Vector2Int pos = GetClosestSlot();
        if (pos.y == -1) {
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
        if (pos == new Vector2Int(-1, -1)) {
            // click outside inventory, either add back to inventory or (-> this) throw away (To be implemented)
            return false;
        }
        bool isArmourSlot = pos.y < 0;
        InventorySlot originalSlot = isArmourSlot ? armourSlots[pos.x] : inventory[pos.x, pos.y];

        if (isArmourSlot) {
            if (movingSlot.item.itemType == ItemClass.ItemType.equipment) {
                EquipmentClass equipment = (EquipmentClass) movingSlot.item;
                if (equipment.equipmentType == EquipmentClass.EquipmentType.armour) {
                    ArmourClass armour = (ArmourClass) equipment;
                    if (pos.x == 0 && armour.armourType == ArmourClass.ArmourType.helmet) {
                        if (originalSlot == null) {
                            armourSlots[pos.x] = new InventorySlot(movingSlot);
                            movingSlot = null;
                            isMovingItem = false;
                            UpdateInventoryUI();
                            return true;
                        } else {
                            InventorySlot tempSlot = new InventorySlot(originalSlot);
                            armourSlots[pos.x] = new InventorySlot(movingSlot);
                            movingSlot = tempSlot;
                            UpdateInventoryUI();
                            return false;
                        }
                    } else if (pos.x == 1 && armour.armourType == ArmourClass.ArmourType.chestplate) {
                        if (originalSlot == null) {
                            armourSlots[pos.x] = new InventorySlot(movingSlot);
                            movingSlot = null;
                            isMovingItem = false;
                            UpdateInventoryUI();
                            return true;
                        } else {
                            InventorySlot tempSlot = new InventorySlot(originalSlot);
                            armourSlots[pos.x] = new InventorySlot(movingSlot);
                            movingSlot = tempSlot;
                            UpdateInventoryUI();
                            return false;
                        }
                    } else if (pos.x == 2 && armour.armourType == ArmourClass.ArmourType.leggings) {
                        if (originalSlot == null) {
                            armourSlots[pos.x] = new InventorySlot(movingSlot);
                            movingSlot = null;
                            isMovingItem = false;
                            UpdateInventoryUI();
                            return true;
                        } else {
                            InventorySlot tempSlot = new InventorySlot(originalSlot);
                            armourSlots[pos.x] = new InventorySlot(movingSlot);
                            movingSlot = tempSlot;
                            UpdateInventoryUI();
                            return false;
                        }
                    } else if (pos.x == 3 && armour.armourType == ArmourClass.ArmourType.boots) {
                        if (originalSlot == null) {
                            armourSlots[pos.x] = new InventorySlot(movingSlot);
                            movingSlot = null;
                            isMovingItem = false;
                            UpdateInventoryUI();
                            return true;
                        } else {
                            InventorySlot tempSlot = new InventorySlot(originalSlot);
                            armourSlots[pos.x] = new InventorySlot(movingSlot);
                            movingSlot = tempSlot;
                            UpdateInventoryUI();
                            return false;
                        }
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        if (originalSlot == null) {
            inventory[pos.x, pos.y] = new InventorySlot(movingSlot);
            movingSlot = null;
            isMovingItem = false;
            UpdateInventoryUI();
            return true;
        } else if (originalSlot.item != null && movingSlot.item != null) {
            if (originalSlot.item.itemName == movingSlot.item.itemName && originalSlot.item.isStackable
                && originalSlot.quantity < originalSlot.item.maxItemsPerStack) {
                int totalQuantity = originalSlot.quantity + movingSlot.quantity;
                originalSlot.quantity = Mathf.Min(totalQuantity, originalSlot.item.maxItemsPerStack);
                movingSlot.quantity = totalQuantity - originalSlot.quantity;
                if (movingSlot.quantity == 0) {
                    movingSlot = null;
                    UpdateInventoryUI();
                    isMovingItem = false;
                    return true;
                }
                UpdateInventoryUI();
                return false;
            } else if (originalSlot.item.itemName != movingSlot.item.itemName) {
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
        if (pos == new Vector2Int(-1, -1)) {
            return false;
        }
        InventorySlot originalSlot = inventory[pos.x, pos.y];
        if (originalSlot == null) {
            inventory[pos.x, pos.y] = new InventorySlot(movingSlot.item, 1);
            movingSlot.quantity -= 1;
        } else if (originalSlot.item.itemName == movingSlot.item.itemName
        && originalSlot.item.isStackable && originalSlot.quantity < originalSlot.item.maxItemsPerStack) {
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
    #endregion

    #region Getters
    public HelmetClass GetHelmet() {
        if (armourSlots[0] != null) {
            return (HelmetClass) armourSlots[0].item;
        }
        return null;
    }

    public ChestplateClass GetChestplate() {
        if (armourSlots[1] != null) {
            return (ChestplateClass) armourSlots[1].item;
        }
        return null;
    }

    public LeggingsClass GetLeggings() {
        if (armourSlots[2] != null) {
            return (LeggingsClass) armourSlots[2].item;
        }
        return null;
    }

    public BootsClass GetBoots() {
        if (armourSlots[3] != null) {
            return (BootsClass) armourSlots[3].item;
        }
        return null;
    }

    #endregion

}
