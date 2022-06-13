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
 

    private void Awake() {
        instance = this;
        popUpText = transform.Find("Text").GetComponent<Text>();
        HidePopUp();
    }

    private void ShowPopUp(string popUpString) {
        gameObject.SetActive(true);
        popUpText.text = popUpString;
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
