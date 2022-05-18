using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{  
    public bool showInv = false;
    public Inventory inventory;
    public BlockClass selectedBlock;

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

    private void OnTriggerStay2D(Collider2D col) {
        if (col.CompareTag("Ground")) {
            onGround = true;
        }

    }

    private void FixedUpdate() {

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (jump > 0.1f) {
            if (onGround) {
                movement.y = jumpForce;
                onGround = false;
            }
        }
        rb.velocity = movement;

        //destroy or place blocks
        if (hit) {
            terrain.destroyBlock(mousePos.x, mousePos.y);
        } else if (place) {
            //Debug.Log(GetComponent<Transform>().position.GetType());
            int minX = Mathf.FloorToInt(GetComponent<Transform>().position.x);
            int maxX = Mathf.CeilToInt(GetComponent<Transform>().position.x);
            int minY = Mathf.FloorToInt(GetComponent<Transform>().position.y);
            int maxY = Mathf.CeilToInt(GetComponent<Transform>().position.y);
            if(!((mousePos.x >= minX) && (mousePos.x < maxX) && (mousePos.y >= minY) && (mousePos.y <= maxY))){
                terrain.placeBlock(mousePos.x, mousePos.y, selectedBlock.blockSprite);
            }
        }
    }

    private void Update() {
        horizontal = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");
        hit = Input.GetMouseButton(0);
        place = Input.GetMouseButton(1);
        
        if (Input.GetKeyDown(KeyCode.E)) {
            showInv = !showInv;
        }
        

        /*
        if (horizontal > 0) {
            transform.localScale = new Vector3(-1,1,1);
        } else if (horizontal < 0)  {
            transform.localScale = new Vector3(1,1,1);
            
        }
        */

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

        inventory.InventoryUI.SetActive(showInv);
    }

}
