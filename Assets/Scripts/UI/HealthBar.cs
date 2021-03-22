using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    protected Slider slider;
    protected Text text;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<Text>();
    }

    public void SetActive(bool value)
    {
        transform.parent.gameObject.SetActive(value);
    }

    // Notice: max health is equal to slider's max value, but min health isn't.
    // slider's min value is always set to 0, while min health should be a nonnegative.
    // The character's health will not be lower than this value, which means that
    // if the character's health has already reached the min health, then no harm
    // will be done to it whatever the damage is.

    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    public void SetMinHealth(float minHealth)
    {

    }

    public virtual void UpdateHealth(float health)
    {
        SetHealth(health);
        SetText($"{health} / {slider.maxValue}");
    }

    protected void SetHealth(float health)
    {
        slider.value = health;
        slider.transform.GetChild(1).gameObject.SetActive(health != 0);
    }

    protected void SetText(string content)
    {
        text.text = content;
    }

    protected void SetColor(Color color)
    {
        slider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = color;
    }
}
