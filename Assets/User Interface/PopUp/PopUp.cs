using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp: MonoBehaviour
{   
    private static PopUp instance;

    [SerializeField]
    private Camera uiCamera;

    private Text popUpText;
    private RectTransform backgroundRectTransform;
 

    private void Awake() {
        instance = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        popUpText = transform.Find("Text").GetComponent<Text>();
        HidePopUp();
    }

    private void ShowPopUp(string popUpString) {
        popUpText.text = popUpString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(popUpText.preferredWidth + textPaddingSize * 3f, 
            popUpText.preferredHeight + textPaddingSize * 3f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        gameObject.SetActive(true);
        
    }

    private void HidePopUp() {
        gameObject.SetActive(false);
    }

    public static void ShowPopUp_Static(string popUpString) {
        instance.ShowPopUp(popUpString);
    }

    public static void HidePopUp_Static() {
        instance.HidePopUp(); 
    }
}
