using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{  
    public bool showInv = false;
    public Inventory inventory;
    public ItemClass selectedItem;

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

    public Vector2 spawnPos;
    public Vector2Int mousePos;

    public Terrain terrain;


    public void Spawn() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GetComponent<Transform>().position = spawnPos;
        inventory = GetComponent<Inventory>();
    }



    private void FixedUpdate() {
        //destroy or place blocks
        if (hit) {
            terrain.destroyBlock(mousePos.x, mousePos.y);
        } else if (place && selectedItem != null && selectedItem.itemType == ItemClass.ItemType.block) {
            //check not placing on player
            int minX = Mathf.FloorToInt(GetComponent<Transform>().position.x);
            int maxX = Mathf.CeilToInt(GetComponent<Transform>().position.x);
            int minY = Mathf.FloorToInt(GetComponent<Transform>().position.y);
            int maxY = Mathf.CeilToInt(GetComponent<Transform>().position.y);

            if(!((mousePos.x >= minX) && (mousePos.x < maxX) && (mousePos.y >= minY) && (mousePos.y <= maxY))){
                if(selectedItem != null && selectedItem.block != null && terrain.canPlace(mousePos.x, mousePos.y)) { //Not sure why needed but giving errors if not included
                    terrain.placeBlock(mousePos.x, mousePos.y, selectedItem.block);
                    inventory.Remove(selectedItem, selectionIndex);
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

        hotBarSelector.transform.position = inventory.hotbarUISlots[selectionIndex].transform.position;
        if (inventory) { // Not really needed but some times giving error at the start
            InventorySlot selected = inventory.inventory[selectionIndex, inventory.inventoryHeight - 1];
            if (selected != null) {
                selectedItem = selected.item;
            } else {
                selectedItem = null;
            }
            //Debug.Log(selectedItem);
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

}
