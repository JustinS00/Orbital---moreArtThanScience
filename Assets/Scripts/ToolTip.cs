using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{   
    private static ToolTip instance;

    [SerializeField]
    private Camera uiCamera;

    private Text toolTipText;
    private RectTransform backgroundRectTransform;

    private float showTimer = 2f;

    private void Awake() {
        instance = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        toolTipText = transform.Find("Text").GetComponent<Text>();
        HideToolTip();
    }

    private void Update() {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
        showTimer -= Time.deltaTime;
        if (showTimer <= 0f) {
            HideToolTip();
        }
    }

    private void ShowToolTip(string toolTipString) {
        gameObject.SetActive(true);
        toolTipText.text = toolTipString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + textPaddingSize * 3f, toolTipText.preferredHeight + textPaddingSize * 3f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        showTimer = 2f;
    }

    private void HideToolTip() {
        gameObject.SetActive(false);
    }

    public static void ShowToolTip_Static(string toolTipString) {
        instance.ShowToolTip(toolTipString);
    }

    public static void HideToolTip_Static() {
        instance.HideToolTip(); 
    }
}
