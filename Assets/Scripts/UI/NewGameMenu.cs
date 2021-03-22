using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenu : MonoBehaviour
{
    InputField inputField;
    Button okButton;

    private void Awake()
    {
        inputField = GetComponentInChildren<InputField>();
        okButton = transform.GetChild(1).GetComponent<Button>();
        okButton.enabled = false;
        inputField.onValueChanged.AddListener(delegate
        {
            okButton.enabled = (inputField.text.Length != 0);
        });
        GameManager.instance.IsNewGameMenu(this);
    }

    public string GetName()
    {
        return inputField.text;
    }
}
