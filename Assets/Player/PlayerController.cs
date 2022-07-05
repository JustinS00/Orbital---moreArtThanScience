using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public bool showInv;
    public Inventory inventory;
    public CraftingUI crafting;
    public ItemClass selectedItem;
    public GameObject selectedItemDisplay;

    [Header("Armour Display")]

    [HideInInspector] public GameObject helmetDisplay;
    [HideInInspector] public GameObject chestplate1Display;
    [HideInInspector] public GameObject chestplate2_0_Display;
    [HideInInspector] public GameObject chestplate2_1_Display;
    [HideInInspector] public GameObject leggings_0_Display;
    [HideInInspector] public GameObject leggings_1_Display;
    [HideInInspector] public GameObject boots_0_Display;
    [HideInInspector] public GameObject boots_1_Display;

    public HelmetClass helmet;
    public ChestplateClass chestplate;
    public LeggingsClass leggings;
    public BootsClass boots;

    public int armourProtectionValue;

    public int selectionIndex = 0;
    public GameObject hotBarSelector;

    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public bool onGround;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    private bool hit;
    private bool place;
    private float jump;
    private bool siu;

    [Header("Health")]
    public int maxHealth = 40;
    public int currentHealth;

    public HealthBar healthBar;
    public Health health;

    public Vector2 spawnPos;
    public Vector2Int mousePos;
    private Vector2Int currentTarget;
    private float DEFAULT_MINING_SPEED = 2;

    //public Terrain terrain;
    public GameManager gameManager;
    public PlayerCombat playerCombat;

    //tempEquipment
    public ConsumableClass apple;

    //Mining, breaking of blocks
    private float timeElapsedBlockBreak = 0f;
    private float eatRate = 2.0f;
    private float nextEatTime;

    /*
    to make into knockback script
    public float knockback = 30;
    private float knockbackLength;
    private float knockbackCount;
    private bool knockFromRight;
    */

    public void Spawn() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GetComponent<Transform>().position = spawnPos;
        inventory = GetComponent<Inventory>();
        inventory.InventoryUI.SetActive(showInv);
        healthBar.SetMaxHealth(maxHealth);
        health = GetComponent<Health>();
        health.SetFullHealth();

    }

    public void moveTo(Vector2 location) {
        GetComponent<Transform>().position = location;
    }

    public void Respawn() {
        Achievement.instance.UnlockAchievement(Achievement.AchievementType.emotionaldamage);
        
        //not clearing inventory for now
        GetComponent<Transform>().position = spawnPos;
        healthBar.SetMaxHealth(maxHealth);
        health.SetFullHealth();
    }

    private void FixedUpdate() {
        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        rb.velocity = movement;
        /*
        to make into knockback script
        if (knockbackCount <= 0) {
            
        } else {
            if (knockFromRight) {
                rb.velocity = new Vector2(-knockback, knockback/3);
            } else { 
                rb.velocity = new Vector2(knockback, knockback/3);
            }
            knockbackCount--;
        }
        */

    }

    private void Update() {
        //Hotbar
        onGround = -0.1f <= rb.velocity.y && rb.velocity.y <= 0.1f;

        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            selectionIndex = (selectionIndex + 1) % inventory.inventoryWidth;
        } else if ((Input.GetAxis("Mouse ScrollWheel") < 0)) {
            selectionIndex = (selectionIndex - 1 + inventory.inventoryWidth) % inventory.inventoryWidth;
        }

        if (Input.GetKeyDown("1")) {
            selectionIndex = 0;
        } else if (Input.GetKeyDown("2")) {
            selectionIndex = 1;
        } else if (Input.GetKeyDown("3")) {
            selectionIndex = 2;
        } else if (Input.GetKeyDown("4")) {
            selectionIndex = 3;
        } else if (Input.GetKeyDown("5")) {
            selectionIndex = 4;
        } else if (Input.GetKeyDown("6")) {
            selectionIndex = 5;
        } else if (Input.GetKeyDown("7")) {
            selectionIndex = 6;
        } else if (Input.GetKeyDown("8")) {
            selectionIndex = 7;
        } else if (Input.GetKeyDown("9")) {
            selectionIndex = 8;
        }

        hotBarSelector.transform.position = inventory.hotbarUISlots[selectionIndex].transform.position;
        if (inventory != null && inventory.inventory != null) { // Not really needed but some times giving error at the start
            InventorySlot selected = inventory.inventory[selectionIndex, 0];
            if (selected != null) {
                selectedItem = selected.item;
                if (selectedItem.itemName == "diamond_ore")
                    Achievement.instance.UnlockAchievement(Achievement.AchievementType.diamondhands);
            } else {
                selectedItem = null;
            }
        }

        if (selectedItem != null) {
            selectedItemDisplay.GetComponent<SpriteRenderer>().sprite = selectedItem.itemSprite;
            selectedItemDisplay.transform.localScale = Vector3.one * 0.5f;
            if (selectedItem.itemType == ItemClass.ItemType.equipment) {
                EquipmentClass equipment = (EquipmentClass) selectedItem;
                if (equipment.equipmentType == EquipmentClass.EquipmentType.tool || equipment.equipmentType == EquipmentClass.EquipmentType.weapon) {
                    selectedItemDisplay.transform.localScale = Vector3.one;
                }
            }
        } else {
            selectedItemDisplay.GetComponent<SpriteRenderer>().sprite = null;
        }

        // Toggle Inventory
        if (Input.GetKeyDown(KeyCode.E)) {
            ToggleUI();

            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (showInv) {
                ToggleUI();
                TogglePause();
            }
        }

        helmet = inventory.GetHelmet();
        chestplate = inventory.GetChestplate();
        leggings = inventory.GetLeggings();
        boots = inventory.GetBoots();

        // Movement and action
        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");
        hit = Input.GetMouseButton(0);
        place = Input.GetMouseButton(1);
        siu = Input.GetKeyDown(KeyCode.UpArrow);

        anim.SetFloat("horizontal", horizontal);
        anim.SetBool("siu", siu);
        anim.SetBool("hit", hit);

        if (siu) {
            jump = 1.0f;
            Achievement.instance.UnlockAchievement(Achievement.AchievementType.siu);
            AudioManager.instance.PlaySound("SIU");
        }

        if (jump > 0.1f) {
            if (onGround) {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);
                onGround = false;
            }
        }

        mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f);
        mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);

        //destroy or place blocks
        if (!showInv) { //unity does not support covariant, will have to do a lot of type casting
            if (hit) {
                //terrain.destroyBlock(mousePos.x, mousePos.y);
                if (selectedItem) {
                    if (selectedItem.itemType == ItemClass.ItemType.equipment) {
                        EquipmentClass tempEquipment = (EquipmentClass) selectedItem;
                        if (tempEquipment.equipmentType == EquipmentClass.EquipmentType.weapon) {
                            WeaponClass weapon = (WeaponClass) tempEquipment;
                            if (weapon.weaponType == WeaponClass.WeaponType.melee) {
                                hit = false;
                                anim.SetBool("hit", hit);
                                playerCombat.Attack(weapon);
                            } else {
                                BowClass bow = (BowClass) tempEquipment;
                                ItemClass arrowItem =  inventory.HasItemInInventoryByString("arrow");
                                if (arrowItem != null && playerCombat.canFire()) {
                                    inventory.RemoveItemFromInventory(arrowItem, 1);
                                    ArrowClass arrow = (ArrowClass) arrowItem; //of type mob drop not arrow
                                    playerCombat.Shoot(bow, arrow);
                                }
                            }
                        } else if (tempEquipment.equipmentType == EquipmentClass.EquipmentType.tool) {
                            ToolClass tool = (ToolClass) tempEquipment;
                            mineBlock(mousePos.x, mousePos.y, tool);
                        }
                    } else if (selectedItem.itemType == ItemClass.ItemType.consumable && nextEatTime < Time.time) {
                        ConsumableClass consumable = (ConsumableClass) selectedItem;
                        Consume(consumable, selectionIndex);
                    }
                } else {
                    TryHit(mousePos.x, mousePos.y);
                }
            } else if (place && selectedItem != null) {
                if (selectedItem.itemType == ItemClass.ItemType.block) {
                    //check not placing on player
                    int minX = Mathf.FloorToInt(GetComponent<Transform>().position.x);
                    int maxX = Mathf.CeilToInt(GetComponent<Transform>().position.x);

                    int minY = Mathf.CeilToInt(GetComponent<Transform>().position.y - 1);
                    int maxY = Mathf.FloorToInt(GetComponent<Transform>().position.y + 1);

                    if (!((mousePos.x >= minX) && (mousePos.x < maxX) && (mousePos.y >= minY) && (mousePos.y <= maxY))) {
                        if (gameManager.terrain.canPlace(mousePos.x, mousePos.y)) { //Not sure why needed but giving errors if not included
                            gameManager.terrain.placeBlock(mousePos.x, mousePos.y, (BlockClass) selectedItem);
                            inventory.RemoveFromHotBar(selectedItem, selectionIndex);
                        }
                    }
                    timeElapsedBlockBreak = 0f;
                }
            } else {
                timeElapsedBlockBreak = 0f;
            }
        }

        if (horizontal > 0) {
            transform.eulerAngles = new Vector3(0, -180, 0);
        } else if (horizontal < 0) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (GetComponent<Transform>().position.y < 0) {
            Achievement.instance.UnlockAchievement(Achievement.AchievementType.luna);
            Respawn();
        } 

        int curHealth = GetComponent<Health>().GetHealth();
        healthBar.SetHealth(curHealth);
        if (curHealth <= 0) {
            Respawn();
        }

        //Armour Related
        armourProtectionValue = 0;
        if (helmet != null) {
            helmetDisplay.GetComponent<SpriteRenderer>().sprite = helmet.helmet_sideview;
            armourProtectionValue += helmet.protectionValue;
        } else {
            helmetDisplay.GetComponent<SpriteRenderer>().sprite = null;
        }

        if (chestplate != null) {
            chestplate1Display.GetComponent<SpriteRenderer>().sprite = chestplate.chestplate1_sideview;
            chestplate2_0_Display.GetComponent<SpriteRenderer>().sprite = chestplate.chestplate2_0_sideview;
            chestplate2_1_Display.GetComponent<SpriteRenderer>().sprite = chestplate.chestplate2_1_sideview;
            armourProtectionValue += chestplate.protectionValue;
        } else {
            chestplate1Display.GetComponent<SpriteRenderer>().sprite = null;
            chestplate2_0_Display.GetComponent<SpriteRenderer>().sprite = null;
            chestplate2_1_Display.GetComponent<SpriteRenderer>().sprite = null;
        }

        if (leggings != null) {
            leggings_0_Display.GetComponent<SpriteRenderer>().sprite = leggings.leggings_0_sideview;
            leggings_1_Display.GetComponent<SpriteRenderer>().sprite = leggings.leggings_1_sideview;
            armourProtectionValue += leggings.protectionValue;
        } else {
            leggings_0_Display.GetComponent<SpriteRenderer>().sprite = null;
            leggings_1_Display.GetComponent<SpriteRenderer>().sprite = null;
        }

        if (boots != null) {
            boots_0_Display.GetComponent<SpriteRenderer>().sprite = boots.boots_0_sideview;
            boots_1_Display.GetComponent<SpriteRenderer>().sprite = boots.boots_1_sideview;
            armourProtectionValue += boots.protectionValue;
        } else {
            boots_0_Display.GetComponent<SpriteRenderer>().sprite = null;
            boots_1_Display.GetComponent<SpriteRenderer>().sprite = null;
        }
        health.protectionValue = armourProtectionValue;
        if (armourProtectionValue == 20)
            Achievement.instance.UnlockAchievement(Achievement.AchievementType.deckedout);

    }

    private void TryHit(int x, int y) {
        if (gameManager.terrain.GetBlock(x, y) != null) {
            mineBlock(x, y, null);
        } else {
            playerCombat.Attack();
        }
    }

    public void mineBlock(int x, int y, ToolClass tool) {
        Vector2Int target = new Vector2Int(x, y);
        BlockClass block = gameManager.terrain.GetBlock(x, y);
        if (target == currentTarget && block != null && block.isBreakable) {
            bool isPreferredTool = false;
            float miningSpeed = DEFAULT_MINING_SPEED;
            if (tool) {
                isPreferredTool = (block.preferredTool == ToolType.all || block.preferredTool == tool.toolType || tool.toolType == ToolType.all);
                miningSpeed = isPreferredTool ? tool.miningSpeed : DEFAULT_MINING_SPEED;
            }
            timeElapsedBlockBreak += Time.deltaTime * 1 / miningSpeed;
            if (timeElapsedBlockBreak > block.hardness) {
                gameManager.terrain.mineBlock(x, y, isPreferredTool);
                timeElapsedBlockBreak = 0f;
                if (tool) {
                    tool.reduceDurability(1);
                }
            }
        } else {
            currentTarget = target;
            timeElapsedBlockBreak = 0f;
        }
    }


    public void ToggleUI() {
        showInv = !showInv;
        ToggleInventory();
        ToggleSideUI();
    }

    public void ToggleInventory() {
        inventory.isShowing = !inventory.isShowing;
        inventory.InventoryUI.SetActive(showInv);
    }

    public void ToggleSideUI() {
        crafting.isShowing = !crafting.isShowing;
        if (showInv) {
            crafting.Show(this);
        } else {
            crafting.Hide(this);
        }
        
    }

    public void armourDamage(int value) {
        if (helmet) helmet.reduceDurability(value);
        if (chestplate) chestplate.reduceDurability(value);
        if (leggings) leggings.reduceDurability(value);
        if (boots) boots.reduceDurability(value);
    }

    private void Consume(ConsumableClass consumable, int selectionIndex) {
        nextEatTime = Time.time + eatRate;
        inventory.RemoveFromHotBar(consumable, selectionIndex);
        health.Heal(consumable.healthAdded);
    }

    private bool gameIsPausedFromPlayer;
    private void TogglePause() {
        gameIsPausedFromPlayer = !gameIsPausedFromPlayer;
        if (gameIsPausedFromPlayer != gameManager.isGamePaused())
            gameManager.TogglePause();

    }
}
