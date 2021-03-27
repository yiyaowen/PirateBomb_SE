using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffButton : MonoBehaviour
{
    [Header("Buff Images")]
    public Sprite redBottle;
    public Sprite greenBottle;
    public Sprite blueBottle;
    public Sprite nullIcon;

    public void UpdateIcon(Bottle buff)
    {
        var icon = transform.GetChild(0).GetComponent<Image>();
        if (buff == null)
        {
            icon.sprite = nullIcon;
            icon.SetNativeSize();
            return;
        }
        switch (buff.type)
        {
            case Bottle.Type.RedBottle:
                icon.sprite = redBottle;
                break;
            case Bottle.Type.GreenBottle:
                icon.sprite = greenBottle;
                break;
            case Bottle.Type.BlueBottle:
                icon.sprite = blueBottle;
                break;
            default:
                break;
        }
        icon.SetNativeSize();
    }
}
