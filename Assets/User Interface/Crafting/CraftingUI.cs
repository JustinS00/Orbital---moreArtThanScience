using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CraftingUI : MonoBehaviour
{   

    private Transform canvas;
    private Transform tradeUI;  
    private Transform scrollRect;
    private Transform view;
    private Transform tradesContent;
    private Transform tradeTemplate;
    private Transform slot;
    

    private int itemGivenXOffset = -30;
    private int itemGivenMultiplierRow = 40;
    private int itemGivenYOffset = 0;

    public Trade[] trades;
    private PlayerController player;
    public bool isShowing;

    private void Awake() {
        canvas = transform.Find("Canvas");
        tradeUI = canvas.Find("tradeUI");
        scrollRect = tradeUI.Find("Scroll Rect");
        view = scrollRect.Find("View");
        tradesContent = view.Find("TradesContent");
        tradeTemplate = tradesContent.Find("tradeTemplate");
        tradeTemplate.gameObject.SetActive(false);
        slot = tradeTemplate.Find("ItemGiven");
        for (int i = 0; i < trades.Length; i++) {
            CreateItemButton(trades[i]);
        }
        gameObject.SetActive(false);       
    }
    
    // Start is called before the first frame update

    private void CreateItemButton(Trade trade) {
        Transform tradeItemTransform = Instantiate(tradeTemplate, tradesContent);      
        //tradeItemRectTransform.anchoredPosition = new Vector3(x, y);

        tradeItemTransform.Find("ItemRecieved").Find("Image").GetComponent<Image>().sprite= trade.itemRecieve.item.itemSprite;
        tradeItemTransform.Find("ItemRecieved").Find("Quantity").GetComponent<Text>().text = trade.itemRecieve.quantity.ToString();
        tradeItemTransform.Find("ItemRecieved").Find("EquipmentDurabilityBar").gameObject.SetActive(false);
        tradeItemTransform.Find("ItemGiven").gameObject.SetActive(false);
        

        for (int i = 0; i < trade.itemsGiven.Length; i++) {
            ItemClass item = trade.itemsGiven[i].item;
            int quantity = trade.itemsGiven[i].quantity;

            Transform newSlot = Instantiate(slot, tradeItemTransform);
            RectTransform newSlotRectTransform = newSlot.GetComponent<RectTransform>();
            newSlotRectTransform.anchoredPosition = new Vector3(itemGivenXOffset + i * itemGivenMultiplierRow, itemGivenYOffset);
            
            newSlot.Find("Image").GetComponent<Image>().sprite= item.itemSprite;
            newSlot.Find("Quantity").GetComponent<Text>().text = quantity.ToString();
            newSlot.Find("EquipmentDurabilityBar").gameObject.SetActive(false);
        }

        tradeItemTransform.gameObject.SetActive(true);

        tradeItemTransform.GetComponent<Button>().onClick.AddListener(() => TryTradeItem(trade));
    }

    private void TryTradeItem(Trade trade) {
        if (player != null) {
            bool haveItemsRequired = FindItems(player, trade.itemsGiven);
            if (haveItemsRequired) {
                RemoveItems(player, trade.itemsGiven);
                AddItems(player, trade.itemRecieve);
                ToolTip.HideToolTip_Static();
            } else {
                ToolTip.ShowToolTip_Static("  Insufficent Items");
            }
        }
    }

    private bool FindItems(PlayerController player, ItemSet[] items) {
        foreach(ItemSet item in items) {
            if (!player.inventory.HasItemInInventory(item.item, item.quantity)) {
                return false;
            }
        }
        return true;
    }

    private void RemoveItems(PlayerController player, ItemSet[] items) {
        foreach(ItemSet item in items) {
            player.inventory.RemoveItemFromInventory(item.item, item.quantity);
        }
    }

    private void AddItems(PlayerController player, ItemSet item) {
        ItemClass newItem = Instantiate(item.item);
        player.inventory.AddedItems(newItem, item.quantity);
    }


    public void Show(PlayerController player) {
        this.player = player;
        gameObject.SetActive(true);
    }

    public void Hide(PlayerController player) {
        player = null;
        gameObject.SetActive(false);
    }
}
