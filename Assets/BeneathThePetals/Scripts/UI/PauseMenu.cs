using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;

    [HideInInspector]
    public bool isPaused = false;

    private GameObject gameOverMenu;
    private GameObject killMenu;
    private GameObject restartMenu;
    private GameObject settingsMenu;
    private InventoryUI inventoryUI;
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        isPaused = false;
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        gameOverMenu = GameObject.Find("GameOverMenu");
        killMenu = GameObject.Find("KillMenu");
        restartMenu = GameObject.Find("RestartMenu");
        settingsMenu = GameObject.Find("SettingsMenu");
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        SetActiveIfNotNull(ref pauseMenu, false);
        SetActiveIfNotNull(ref gameOverMenu, false);
        SetActiveIfNotNull(ref killMenu, false);
        SetActiveIfNotNull(ref restartMenu, false);
        SetActiveIfNotNull(ref settingsMenu, false);

        inventoryUI = GameObject.FindAnyObjectByType<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused && !restartMenu.activeSelf && !gameOverMenu.activeSelf && 
                !killMenu.activeSelf && !settingsMenu.activeSelf && !playerController.DialogueBox.activeSelf)
                PauseGame();
            else if(isPaused && pauseMenu.activeSelf)
                ResumeGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenInventory()
    {
        inventoryUI.ToggleCamera();
    }

    public void LoadScene(string name)
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        SceneManager.LoadScene(name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetPause(bool setPause)
    {
        isPaused = setPause;
    }

    public void StartGameOver()
    {
        SetActiveIfNotNull(ref gameOverMenu, true);
    }
    public void ShowRestartMenu()
    {
        restartMenu.GetComponent<CanvasGroup>().alpha = 0f;
        SetActiveIfNotNull(ref restartMenu, true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetKillTransition(bool setBool)
    {
        killMenu.SetActive(setBool);
    }
    private void SetActiveIfNotNull(ref GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            obj.SetActive(isActive);
        }
        else
        {
            obj = pauseMenu;
        }
    }
}
