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

        /*For Testing*/
    }

    public void Respawn() {
        //not clearing inventory for now
        GetComponent<Transform>().position = spawnPos;
		healthBar.SetMaxHealth(maxHealth);
        health.SetFullHealth();
    }

    private void FixedUpdate() {
        //destroy or place blocks
        if (hit && !showInv) {
            //terrain.destroyBlock(mousePos.x, mousePos.y);
            gameManager.terrain.mineBlock(mousePos.x, mousePos.y);
        } else if (place && !showInv && selectedItem != null && selectedItem.itemType == ItemClass.ItemType.block) {
            //check not placing on player
            int minX = Mathf.FloorToInt(GetComponent<Transform>().position.x);
            int maxX = Mathf.CeilToInt(GetComponent<Transform>().position.x);
            int minY = Mathf.FloorToInt(GetComponent<Transform>().position.y);
            int maxY = Mathf.CeilToInt(GetComponent<Transform>().position.y);

            if(!((mousePos.x >= minX) && (mousePos.x < maxX) && (mousePos.y >= minY) && (mousePos.y <= maxY))){
                if(gameManager.terrain.canPlace(mousePos.x, mousePos.y)) { //Not sure why needed but giving errors if not included
                    gameManager.terrain.placeBlock(mousePos.x, mousePos.y, (BlockClass) selectedItem);
                    inventory.RemoveFromHotBar(selectedItem, selectionIndex);
                }
            }
        }

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (jump > 0.1f) {
            if (onGround) {
                movement.y = jumpForce;
                onGround = false;
            }
        }
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
            //Debug.Log(selectedItem);
        }

        if (selectedItem != null) {
            selectedItemDisplay.GetComponent<SpriteRenderer>().sprite = selectedItem.itemSprite;
            if (selectedItem.itemType == ItemClass.ItemType.block) {
                selectedItemDisplay.transform.localScale = Vector3.one * 0.5f;
            } else { //might have to change in the future depending on the other items implemented
                selectedItemDisplay.transform.localScale = Vector3.one;
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


        
        if (helmet != null) {
            helmetDisplay.GetComponent<SpriteRenderer>().sprite = helmet.helmet_sideview;
        } 
        
        if (chestplate!= null) {
            chestplate1Display.GetComponent<SpriteRenderer>().sprite = chestplate.chestplate1_sideview;
            chestplate2_0_Display.GetComponent<SpriteRenderer>().sprite = chestplate.chestplate2_0_sideview;
            chestplate2_1_Display.GetComponent<SpriteRenderer>().sprite = chestplate.chestplate2_1_sideview;
        }
        
        if (leggings!= null) {
            leggings_0_Display.GetComponent<SpriteRenderer>().sprite = leggings.leggings_0_sideview;
            leggings_1_Display.GetComponent<SpriteRenderer>().sprite = leggings.leggings_1_sideview;
        }
        
        if (boots!= null) {
            boots_0_Display.GetComponent<SpriteRenderer>().sprite = boots.boots_0_sideview;
            boots_1_Display.GetComponent<SpriteRenderer>().sprite = boots.boots_1_sideview;
        }

        // Movement
        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");
        hit = Input.GetMouseButton(0);
        place = Input.GetMouseButton(1);
        
        if (horizontal > 0) {
            transform.eulerAngles = new Vector3(0,-180,0);
        } else if (horizontal < 0)  {
            transform.eulerAngles = new Vector3(0,0,0);
        }
        
        siu = Input.GetKeyDown(KeyCode.UpArrow);
        anim.SetFloat("horizontal", horizontal);
        anim.SetBool("siu", siu);

        if (selectedItem && selectedItem.itemType == ItemClass.ItemType.equipment) {
            EquipmentClass temp = (EquipmentClass) selectedItem;
            if (hit && temp.equipmentType == EquipmentClass.EquipmentType.weapon) {
                playerCombat.Attack();
            }
        } else {
            anim.SetBool("hit", hit);
        }
        
        if (siu) {
            jump = 1.0f;
        }
        mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f);
        mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);

        if (GetComponent<Transform>().position.y < 0 || GetComponent<Health>().getHealth() <= 0) {
            Respawn();
        }

        int curHealth = GetComponent<Health>().getHealth();
        healthBar.SetHealth(curHealth);
        if (curHealth <= 0) {
            Respawn();
        }
    }
}
