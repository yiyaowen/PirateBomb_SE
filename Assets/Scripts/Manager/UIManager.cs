using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject healthBar;

    [Header("UI Elements")]
    public GameObject pauseMenu;
    public Slider bossHealthBar;
    public Image bossHealthBarFill;
    public GameObject gameOverPanel;

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

    public void EnableBossHealthBar()
    {
        bossHealthBar.transform.parent.gameObject.SetActive(true);
    }

    public void UpdateHealth(float currentHealth) // nb!
    {
        switch (currentHealth)
        {
            case 0:
                healthBar.transform.GetChild(0).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                healthBar.transform.GetChild(0).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 2:
                healthBar.transform.GetChild(0).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 3:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1;
    }

    public void SetBossHealth(float health)
    {
        bossHealthBar.maxValue = health;
    }

    public void UpdateBossHealth(float health)
    {
        bossHealthBar.value = health;
        float healthPercent = health / bossHealthBar.maxValue;
        // 00950D r 0 g 149 b 13 // green
        // FFA800 r 255 g 168 b 0 // yellow
        // FF3E00 r 255 g 62 b 0 // red
        if (healthPercent > 0.6)
        {
            bossHealthBarFill.color = new Color(0.0f, 0.58f, 0.05f);
        }
        else if (healthPercent > 0.3)
        {
            bossHealthBarFill.color = new Color(1.0f, 0.66f, 0.0f);
        }
        else
        {
            bossHealthBarFill.color = new Color(1.0f, 0.24f, 0.0f);
        }
    }

    public void GameOverUI(bool gameOver)
    {
        gameOverPanel.SetActive(gameOver);
    }
}
