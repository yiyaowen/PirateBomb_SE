using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool gameOver { get; set; }

    public PlayerController player { get; set; }

    public NewGameMenu newGameMenu { get; set; }

    public List<GameObject> objectsToFinish = new List<GameObject>();

    private AudioSource audioSource;
    [Header("Music Settings")]
    public AudioClip normalBackSound;
    public AudioClip bossBackSound;
    public AudioClip mestoryBackSound;

    float mainVolume, backgroundVolume, effectsVolume;
    public float realBackgroundVolume { get { return mainVolume * backgroundVolume; } }
    public float realEffectsVolume { get { return mainVolume * effectsVolume; } }

    public string[] gameLevelPrefKeys { get; set; } = { "Player_Name", "Player_Health", "Scene_Index" };
    // 各个音量的实际值 = 各个音量的设定值 * 主音量值
    public Dictionary<string, float> volumePrefs { get; set; } = new Dictionary<string, float>()
    { { "Main_Volume", 1.0f }, { "Background_Volume", 1.0f }, { "Effects_Volume", 0.5f } };

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
        // 全局初始化音量设置
        foreach (var key in volumePrefs.Keys)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetFloat(key, volumePrefs[key]);
            }
        }
        mainVolume = PlayerPrefs.GetFloat("Main_Volume");
        backgroundVolume = PlayerPrefs.GetFloat("Background_Volume");
        effectsVolume = PlayerPrefs.GetFloat("Effects_Volume");
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = realBackgroundVolume;
        audioSource.clip = normalBackSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        audioSource.volume = mainVolume * backgroundVolume;
    }

    public void AdjustMainVolume()
    {
        mainVolume = UIManager.instance.GetMainVolumeSliderValue();
        PlayerPrefs.SetFloat("Main_Volume", mainVolume);
    }

    public void AdjustBackgroundVolume()
    {
        backgroundVolume = UIManager.instance.GetBackgroundVolumeSliderValue();
        PlayerPrefs.SetFloat("Background_Volume", backgroundVolume);
    }

    public void AdjustEffectsVolume()
    {
        effectsVolume = UIManager.instance.GetEffectsVolumeSliderValue();
        PlayerPrefs.SetFloat("Effects_Volume", effectsVolume);
    }

    public void IsBoss(Enemy boss)
    {
        audioSource.volume = 0.75f;
        audioSource.clip = bossBackSound;
        audioSource.Play();
    }

    public void BossDead(Enemy boss, bool isFirstTime)
    {
        if (isFirstTime)
        {
            audioSource.volume = 1.0f;
            audioSource.clip = normalBackSound;
            audioSource.Play();
        }
    }

    public void IsPlayer(PlayerController controller)
    {
        player = controller;
    }

    public float LoadPlayerHealth()
    {
        if (PlayerPrefs.HasKey("Player_Health"))
            return PlayerPrefs.GetFloat("Player_Health");
        else
            return player.maxHealth;
    }

    public void PlayerDead()
    {
        gameOver = true;
        player.gameObject.layer = LayerMask.NameToLayer("NPCs");

        UIManager.instance.UpdateTouchButtons_MobileDevice(PlayerController.State.Static);

        StartCoroutine("ShowGameOverMenu");
    }

    public void PlayerResurge()
    {
        gameOver = false;
        player.isDead = false;
        player.GetHeal(player.maxHealth);
        player.gameObject.layer = LayerMask.NameToLayer("Player");

        UIManager.instance.UpdateTouchButtons_MobileDevice(PlayerController.State.Battle);
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
        SceneManager.LoadScene(1);
    }

    public void IsNewGameMenu(NewGameMenu menu)
    {
        newGameMenu = menu;
    }

    public void NewGame()
    {
        if (newGameMenu.GetName().Length != 0)
        {
            PlayerPrefs.SetString("Player_Name", newGameMenu.GetName());
            PlayerPrefs.SetInt("Scene_Index", 2);
            PlayerPrefs.SetFloat("Player_Health", player.maxHealth);
            GoToNextScene();
        }
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("Scene_Index"))
            SceneManager.LoadScene(PlayerPrefs.GetInt("Scene_Index"));
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 仅在每次进入下一个关卡时更新游戏存档
    public void GoToNextScene()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex > 1) SaveGameData();
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void SaveGameData()
    {
        PlayerPrefs.SetFloat("Player_Health", player.health);
        PlayerPrefs.SetInt("Scene_Index", SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.Save();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        player.ChangeToStaticState();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        player.ChangeToBattleState();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
