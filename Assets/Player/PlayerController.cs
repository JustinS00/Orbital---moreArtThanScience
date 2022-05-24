using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{  
    public bool showInv = false;
    public Inventory inventory;
    public ItemClass selectedItem;
    public GameObject selectedItemDisplay;

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

    public int maxHealth = 100;
	public int currentHealth;

	public HealthBar healthBar;

    public Vector2 spawnPos;
    public Vector2Int mousePos;

    public Terrain terrain;

    void Start() {
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
    }

    public void Spawn() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GetComponent<Transform>().position = spawnPos;
        inventory = GetComponent<Inventory>();
    }



    private void FixedUpdate() {
        //destroy or place blocks
        if (hit && !showInv) {
            terrain.destroyBlock(mousePos.x, mousePos.y);
        } else if (place && !showInv && selectedItem != null && selectedItem.itemType == ItemClass.ItemType.block) {
            //check not placing on player
            int minX = Mathf.FloorToInt(GetComponent<Transform>().position.x);
            int maxX = Mathf.CeilToInt(GetComponent<Transform>().position.x);
            int minY = Mathf.FloorToInt(GetComponent<Transform>().position.y);
            int maxY = Mathf.CeilToInt(GetComponent<Transform>().position.y);

            if(!((mousePos.x >= minX) && (mousePos.x < maxX) && (mousePos.y >= minY) && (mousePos.y <= maxY))){
                if(selectedItem != null && selectedItem.block != null && terrain.canPlace(mousePos.x, mousePos.y)) { //Not sure why needed but giving errors if not included
                    terrain.placeBlock(mousePos.x, mousePos.y, selectedItem.block);
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
    }

    private void Update() {
        
        //Hotbar
        //can use number keys as well to add later
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
            selectedItemDisplay.GetComponent<SpriteRenderer>().sprite = selectedItem.sprite;
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
        }
        inventory.InventoryUI.SetActive(showInv);

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
        anim.SetBool("hit", hit);
        anim.SetBool("siu", siu);
        if (siu) {
            jump = 1.0f;
        }
        mousePos.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f);
        mousePos.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);
    }

    void TakeDamage(int damage) {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
