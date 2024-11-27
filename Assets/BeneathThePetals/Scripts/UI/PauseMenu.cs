using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;

    [HideInInspector]
    public bool isPaused = false;

    private GameObject gameOverMenu;
    private GameObject killMenu;
    private InventoryUI inventoryUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        isPaused = false;
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        gameOverMenu = GameObject.Find("GameOverMenu");
        killMenu = GameObject.Find("KillMenu");
    }

    void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        killMenu.SetActive(false);
        inventoryUI = GameObject.FindAnyObjectByType<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
                PauseGame();
            else 
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
        gameOverMenu.SetActive(true);
    }

    public void StartKillTransition()
    {
        killMenu.SetActive(true);
    }
}
