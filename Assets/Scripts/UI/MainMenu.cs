using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Components")]
    public Button continueGameButton;

    private void Start()
    {
        continueGameButton.enabled = PlayerPrefs.HasKey("scene_index");
    }
}
