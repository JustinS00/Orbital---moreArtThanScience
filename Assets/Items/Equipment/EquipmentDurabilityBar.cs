using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentDurabilityBar : MonoBehaviour {

	public Slider slider;
	public Gradient gradient;
	public Image fill;

	public void SetMaxDurability(int durability) {
		this.gameObject.SetActive(false);
		slider.maxValue = durability;
		slider.value = durability;

		fill.color = gradient.Evaluate(1f);
	}

    public void SetDurability(int durability) {
		if (durability < slider.maxValue) {
			this.gameObject.SetActive(true);
		}

		slider.value = durability;

		fill.color = gradient.Evaluate(slider.normalizedValue);
	}

}