using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {  

    public bool showInv;
    public Inventory inventory;
    public ItemClass selectedItem;
    public GameObject selectedItemDisplay;

    [Header("Armour Display")]

    public GameObject helmetDisplay;
    public GameObject chestplate1Display;
    public GameObject chestplate2_0_Display;
    public GameObject chestplate2_1_Display;
    public GameObject leggings_0_Display;
    public GameObject leggings_1_Display;
    public GameObject boots_0_Display;
    public GameObject boots_1_Display;

    public HelmetClass helmet;
    public ChestplateClass chestplate;
    public LeggingsClass leggings;
    public BootsClass boots;

    public int armourProtectionValue;

    public int selectionIndex = 0;
    public GameObject hotBarSelector;

    public float moveSpeed = 5;
    public float jumpForce = 10;
    public bool onGround;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    public bool hit;
    public bool place;
    public float jump;
    public bool siu;

    [Header("Health")]
    public int maxHealth = 100;
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
		healthBar.SetMaxHealth(maxHealth);
        health = GetComponent<Health>();
        health.SetFullHealth();
    }

    public void Respawn() {
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
            showInv = !showInv;
            inventory.isShowing = !inventory.isShowing;
        }
        inventory.InventoryUI.SetActive(showInv);

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
        if (!showInv) {
            if (hit) {
                //terrain.destroyBlock(mousePos.x, mousePos.y);
                if (selectedItem && selectedItem.itemType == ItemClass.ItemType.equipment) {
                    EquipmentClass temp = (EquipmentClass) selectedItem;
           
                    if (temp.equipmentType == EquipmentClass.EquipmentType.weapon) {
                        WeaponClass weapon = (WeaponClass) temp;
                        hit = false;
                        anim.SetBool("hit", hit);
                        playerCombat.Attack(weapon);
                    } else if (temp.equipmentType == EquipmentClass.EquipmentType.tool) {
                        ToolClass tool = (ToolClass) temp;
                        mineBlock(mousePos.x, mousePos.y, tool);
                    }
                } else {
                    mineBlock(mousePos.x, mousePos.y);
                }
            } else if (place && selectedItem != null && selectedItem.itemType == ItemClass.ItemType.block) {
                //check not placing on player
                int minX = Mathf.FloorToInt(GetComponent<Transform>().position.x);
                int maxX = Mathf.CeilToInt(GetComponent<Transform>().position.x);
                
                int minY = Mathf.CeilToInt(GetComponent<Transform>().position.y - 1);
                int maxY = Mathf.FloorToInt(GetComponent<Transform>().position.y + 1);

                if(!((mousePos.x >= minX) && (mousePos.x < maxX) && (mousePos.y >= minY) && (mousePos.y <= maxY))){
                    if(gameManager.terrain.canPlace(mousePos.x, mousePos.y)) { //Not sure why needed but giving errors if not included
                        gameManager.terrain.placeBlock(mousePos.x, mousePos.y, (BlockClass) selectedItem);
                        inventory.RemoveFromHotBar(selectedItem, selectionIndex);
                    }
                }
                timeElapsed = 0f;
            } else {
                timeElapsed = 0f;
            }
        }

        if (horizontal > 0) {
            transform.eulerAngles = new Vector3(0,-180,0);
        } else if (horizontal < 0)  {
            transform.eulerAngles = new Vector3(0,0,0);
        }

        if (GetComponent<Transform>().position.y < 0 || GetComponent<Health>().getHealth() <= 0) {
            Respawn();
        }

        int curHealth = GetComponent<Health>().getHealth();
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
        
        if (chestplate!= null) {
            chestplate1Display.GetComponent<SpriteRenderer>().sprite = chestplate.chestplate1_sideview;
            chestplate2_0_Display.GetComponent<SpriteRenderer>().sprite = chestplate.chestplate2_0_sideview;
            chestplate2_1_Display.GetComponent<SpriteRenderer>().sprite = chestplate.chestplate2_1_sideview;
            armourProtectionValue += chestplate.protectionValue;
        } else {
            chestplate1Display.GetComponent<SpriteRenderer>().sprite = null; 
            chestplate2_0_Display.GetComponent<SpriteRenderer>().sprite = null;
            chestplate2_1_Display.GetComponent<SpriteRenderer>().sprite = null;
        }
        
        if (leggings!= null) {
            leggings_0_Display.GetComponent<SpriteRenderer>().sprite = leggings.leggings_0_sideview;
            leggings_1_Display.GetComponent<SpriteRenderer>().sprite = leggings.leggings_1_sideview;
            armourProtectionValue += leggings.protectionValue;
        } else {
            leggings_0_Display.GetComponent<SpriteRenderer>().sprite = null;
            leggings_1_Display.GetComponent<SpriteRenderer>().sprite = null;
        }
        
        if (boots!= null) {
            boots_0_Display.GetComponent<SpriteRenderer>().sprite = boots.boots_0_sideview;
            boots_1_Display.GetComponent<SpriteRenderer>().sprite = boots.boots_1_sideview;
            armourProtectionValue += boots.protectionValue;
        } else {
            boots_0_Display.GetComponent<SpriteRenderer>().sprite = null;
            boots_1_Display.GetComponent<SpriteRenderer>().sprite = null;
        }

        health.protectionValue = armourProtectionValue;
    }

    private float timeElapsed = 0f;
    

    public void mineBlock(int x, int y) {
        mineBlock(x, y, null);
    }

    public void mineBlock(int x, int y, ToolClass tool) {
        Vector2Int target = new Vector2Int(x,y);
        BlockClass block = gameManager.terrain.GetBlock(x,y);
        if (target == currentTarget && block != null && block.isBreakable) {
            bool isPreferredTool = false;
            float miningSpeed = DEFAULT_MINING_SPEED;
            if (tool) {
                isPreferredTool = (block.preferredTool == ToolType.all || block.preferredTool == tool.toolType);
                miningSpeed = isPreferredTool ? tool.miningSpeed : DEFAULT_MINING_SPEED;
            }
            timeElapsed += Time.deltaTime * 1 / miningSpeed;
            if (timeElapsed > block.hardness) {
                gameManager.terrain.mineBlock(x,y, isPreferredTool);
                timeElapsed = 0f;
                if (tool) {
                    tool.reduceDurability(1);
                }
            }
        } else {
            currentTarget = target;
            timeElapsed = 0;
            Debug.Log("Reset target block");
        }
    }


    /*
    public void mineBlock(int x, int y) {
         if (worldBlocks.Contains(new Vector2(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldHeight) {
            BlockClass block = worldBlockClasses[worldBlocks.IndexOf(new Vector2(x, y))];   
            if (block.isBreakable) {
                destroyBlock(x,y);
            }    
         }
    }
    */
}
