using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TradingUI : MonoBehaviour
{   
    [System.Serializable]
    public struct ItemSet {
        public ItemClass item;
        public int quantity;
    }
 
    [System.Serializable]
    public struct Trade {
        public ItemSet itemRecieve;
        public ItemSet[] itemsGiven;

    }
    private Transform slot;
    private Transform tradeTemplate;
    private Transform tradeUI;
    private Transform canvas;

    private int colOffset = -100;
    private int rowOffset = 125;
    private int multiplierRow = -50;
    private int multiplierCol = 200;
    private int itemsPerCol = 6;
    private int noOfCols = 2;

    private int itemGivenXOffset = -10;
    private int itemGivenYOffset = -0;
    private int itemGivenMultiplierRow = 40;

    public Trade[] trades;

    private void Awake() {
        canvas = transform.Find("Canvas");
        tradeUI = canvas.Find("tradeUI");
        tradeTemplate = tradeUI.Find("tradeTemplate");
        tradeTemplate.gameObject.SetActive(false);
        slot = tradeTemplate.Find("ItemGiven");
    }

    private void Start() {
        for (int i = 0; i < trades.Length; i++) {
        CreateItemButton(trades[i], colOffset + multiplierCol * (i % noOfCols) , rowOffset + multiplierRow * (i / noOfCols));
        }
    }
    // Start is called before the first frame update

    private void CreateItemButton(Trade trade, int x, int y) {
        Transform tradeItemTransform = Instantiate(tradeTemplate, tradeUI);
        RectTransform tradeItemRectTransform = tradeItemTransform.GetComponent<RectTransform>();
      
        tradeItemRectTransform.anchoredPosition = new Vector3(x, y);

        tradeItemTransform.Find("ItemRecieved").Find("Image").GetComponent<Image>().sprite= trade.itemRecieve.item.itemSprite;
        tradeItemTransform.Find("ItemRecieved").Find("Quantity").GetComponent<Text>().text = trade.itemRecieve.quantity.ToString();
        tradeItemTransform.Find("ItemRecieved").Find("EquipmentDurabilityBar").gameObject.SetActive(false);
        tradeItemTransform.Find("ItemGiven").gameObject.SetActive(false);
        

        for (int i = 0; i < trade.itemsGiven.Length; i++) {
            ItemClass item = trade.itemsGiven[i].item;
            int quantity = trade.itemsGiven[i].quantity;

            Transform newSlot = Instantiate(slot, tradeItemTransform);
            RectTransform newSlotRectTransform = newSlot.GetComponent<RectTransform>();
            newSlotRectTransform.localPosition = new Vector3(itemGivenXOffset + i * itemGivenMultiplierRow, itemGivenYOffset);
            
            newSlot.Find("Image").GetComponent<Image>().sprite= item.itemSprite;
            newSlot.Find("Quantity").GetComponent<Text>().text = quantity.ToString();
            newSlot.Find("EquipmentDurabilityBar").gameObject.SetActive(false);
        }

        tradeItemTransform.gameObject.SetActive(true);

        tradeItemTransform.GetComponent<Button>().onClick.AddListener(() => TryTradeItem(trade));
    }

    private void TryTradeItem(Trade trade) {
        string s = "Trading ";
        foreach (ItemSet items in trade.itemsGiven) {
            s += items.quantity.ToString() + " " + items.item.itemName;
        }
        s += " for ";
        s += trade.itemRecieve.item.itemName;

        Debug.Log(s);
    }
}
