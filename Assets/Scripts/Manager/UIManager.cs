using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Buttons")]
    public GameObject pauseButton;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

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
}
