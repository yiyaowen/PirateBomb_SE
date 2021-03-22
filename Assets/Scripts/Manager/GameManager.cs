using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool gameOver { get; set; }

    public PlayerController player { get; set; }
    public List<GameObject> objectsToFinish = new List<GameObject>();

    public NewGameMenu newGameMenu { get; set; }

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

    private void Update()
    {

    }

    public void IsPlayer(PlayerController controller)
    {
        player = controller;
    }

    public float LoadPlayerHealth()
    {
        if (PlayerPrefs.HasKey("player_health"))
            return PlayerPrefs.GetFloat("player_health");
        else
            return player.maxHealth;
    }

    public void PlayerDead()
    {
        gameOver = true;
        player.gameObject.layer = LayerMask.NameToLayer("Default");

        StartCoroutine("ShowGameOverMenu");
    }

    IEnumerator ShowGameOverMenu()
    {
        yield return new WaitForSeconds(2);
        UIManager.instance.pauseButton.SetActive(false);
        UIManager.instance.gameOverMenu.SetActive(true);
    }

    public void IsObjectToFinish(GameObject target)
    {
        if (!objectsToFinish.Contains(target))
            objectsToFinish.Add(target);
    }

    public void ObjectFinish(GameObject target)
    {
        if (objectsToFinish.Contains(target))
            objectsToFinish.Remove(target);

        if (objectsToFinish.Count == 0)
        {
            SaveGameData();
            foreach (var door in FindObjectsOfType<Door>())
            {
                if (door.CompareTag("Exit Door"))
                {
                    door.Open();
                }
            }
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void IsNewGameMenu(NewGameMenu menu)
    {
        newGameMenu = menu;
    }

    public void NewGame()
    {
        if (newGameMenu.GetName().Length != 0)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("player_name", newGameMenu.GetName());
            GoToNextScene();
        }
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("scene_index"))
            SceneManager.LoadScene(PlayerPrefs.GetInt("scene_index"));
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SaveGameData()
    {
        PlayerPrefs.SetFloat("player_health", player.health);
        PlayerPrefs.SetInt("scene_index", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();
    }

    public void DeleteGameData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
