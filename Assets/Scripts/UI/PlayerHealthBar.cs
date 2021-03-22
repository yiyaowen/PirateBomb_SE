using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    public Color favorableColor;
    public float warningThreshold;
    public Color warningColor;
    public float dangerThreshold;
    public Color dangerColor;

    public override void UpdateHealth(float health)
    {
        base.UpdateHealth(health);

        float healthPercent = health / slider.maxValue;
        if (healthPercent < dangerThreshold)
        {
            SetColor(dangerColor);
        }
        else if (healthPercent < warningThreshold)
        {
            SetColor(warningColor);
        }
        else
        {
            SetColor(favorableColor);
        }
    }
}
