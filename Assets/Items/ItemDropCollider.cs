using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropCollider : MonoBehaviour {
    public bool touchingPlayer;
    public bool touchingOtherDrop;
    public ItemClass item;
    public int quantity;

    /*
    Ignore triggers/collision for other layers
    private void Start() {
        Physics2D.IgnoreLayerCollision(7, 8, true);
    }
    */

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            touchingPlayer = true;
            //Add to player inv
            int addedItems = col.GetComponent<Inventory>().AddedItems(item, quantity);
            if (addedItems == quantity) {
                Destroy(this.gameObject);
            } else {
                quantity -= addedItems;
            }
        } else if (col.gameObject.CompareTag("Drop")) {
            touchingOtherDrop = true;
            ItemDropCollider other = col.gameObject.GetComponent<ItemDropCollider>();
            ItemClass otherItem = other.item;
            Vector3 otherItemPosition = col.gameObject.GetComponent<Transform>().position;
            Vector3 thisItemPosition = this.gameObject.GetComponent<Transform>().position;
            if (thisItemPosition.magnitude < otherItemPosition.magnitude) {
                if (otherItem.itemName == this.item.itemName && this.item.isStackable && this.quantity + other.quantity <= this.item.maxItemsPerStack) {
                    this.quantity += other.quantity;
                    Destroy(col.gameObject);
                }
            }
        } else if (col.gameObject.CompareTag("Enemy")) {
            Physics2D.IgnoreCollision(col.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        }
    }
}
