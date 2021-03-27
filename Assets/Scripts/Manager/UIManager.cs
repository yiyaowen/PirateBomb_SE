using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Buttons")]
    public GameObject pauseButton;
    public GameObject joystick;
    public GameObject attackButton;
    public GameObject jumpButton;
    public GameObject buffButton;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject settingsMenu;

    [Header("Widgets")]
    public PlayerHealthBar playerHealthBar;
    public BossHealthBar bossHealthBar;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateBuffButtonIcon()
    {
        buffButton.GetComponent<BuffButton>().UpdateIcon(GameManager.instance.player.availableBottle);
    }

    public void UpdateVolumeSettings()
    {
        var mainVolumeSlider = settingsMenu.transform.GetChild(1).GetChild(1).GetComponent<Slider>();
        mainVolumeSlider.value = PlayerPrefs.GetFloat("Main_Volume");

        var backgroundVolumeSlider = settingsMenu.transform.GetChild(2).GetChild(1).GetComponent<Slider>();
        backgroundVolumeSlider.value = PlayerPrefs.GetFloat("Background_Volume");

        var effectsVolumeSlider = settingsMenu.transform.GetChild(3).GetChild(1).GetComponent<Slider>();
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("Effects_Volume");
    }

    public float GetMainVolumeSliderValue()
    {
        return settingsMenu.transform.GetChild(1).GetChild(1).GetComponent<Slider>().value;
    }

    public float GetBackgroundVolumeSliderValue()
    {
        return settingsMenu.transform.GetChild(2).GetChild(1).GetComponent<Slider>().value;
    }

    public float GetEffectsVolumeSliderValue()
    {
        return settingsMenu.transform.GetChild(3).GetChild(1).GetComponent<Slider>().value;
    }

    public void SetPlayerHealthBarActive(bool value)
    {
        playerHealthBar.gameObject.SetActive(value);
    }

    public void SetPlayerMaxHealth(float health)
    {
        playerHealthBar.SetMaxHealth(health);
    }

    public void UpdatePlayerHealth(float health)
    {
        playerHealthBar.UpdateHealth(health);
    }

    public void SetBossHealthBarActive(bool value)
    {
        bossHealthBar.gameObject.SetActive(value);
    }

    public void SetBossMaxHealth(float health)
    {
        bossHealthBar.SetMaxHealth(health);
    }

    public void UpdateBossHealth(float health)
    {
        bossHealthBar.UpdateHealth(health);
    }

    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void UpdateTouchButtons_MobileDevice(PlayerController.State state)
    {
        switch (state)
        {
            case PlayerController.State.Static:
                joystick.SetActive(false);
                attackButton.SetActive(false);
                jumpButton.SetActive(false);
                buffButton.SetActive(false);
                break;
            case PlayerController.State.Roaming:
                joystick.SetActive(true);
                attackButton.SetActive(false);
                jumpButton.SetActive(true);
                buffButton.SetActive(false);
                break;
            case PlayerController.State.Battle:
                joystick.SetActive(true);
                attackButton.SetActive(true);
                jumpButton.SetActive(true);
                buffButton.SetActive(true);
                break;
            default:
                break;
        }
    }
}
