using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rigidbody2D;

public class PauseMenu : MonoBehaviour
{
    bool isSetting = false;
    bool isPaused = false;
    public GameObject pauseCanvas;
    public GameObject settingsCanvas;
    public GameObject exitMenu;
    public GameObject mainMenu;
    public GameObject PanelControls;
    public GameObject PanelGame;
    public GameObject lineControls;
    public GameObject lineGame;
    public GameObject musicSlider;
    public GameObject sensitivityXSlider;
    public GameObject sensitivityYSlider;
    public FirstPersonController personController;
    [Header("SFX")]
    [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
    public AudioSource hoverSound;
    [Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
    public AudioSource sliderSound;
    [Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
    public AudioSource swooshSound;
    public AudioSource playerSound;
    //public AudioSource weaponSound;
    void Start()
    {
        if (!isPaused)
        {
            pauseCanvas.SetActive(false);
        }
        musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
        sensitivityXSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("XSensitivity");
        sensitivityYSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("YSensitivity");
        UpdateSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsCanvas.activeSelf)
            {
                Return();
                return;
            }
            if (exitMenu.activeSelf || mainMenu.activeSelf)
            {
                ReturnMenu();
                return;
            }

            isPaused = !isPaused;
            PauseMenuScreen();
        }
    }

    void PauseMenuScreen()
    {
        if (!isPaused && !isSetting)
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            personController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            pauseCanvas.SetActive(true);
            settingsCanvas.SetActive(false);
            personController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    
    public void Resume()
    {
        isPaused = false;
        PauseMenuScreen();
    }
    public void Settings()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
        isSetting = true;
    }
    public void GamePanel()
    {
        DisablePanels();
        PanelGame.SetActive(true);
        lineGame.SetActive(true);
    }
    public void ControlsPanel()
    {
        DisablePanels();
        PanelControls.SetActive(true);
        lineControls.SetActive(true);
    }
    void DisablePanels()
    {
        PanelControls.SetActive(false);
        PanelGame.SetActive(false);

        lineGame.SetActive(false);
        lineControls.SetActive(false);

    }
    public void PlayHover()
    {
        hoverSound.Play();
    }

    public void PlaySFXHover()
    {
        sliderSound.Play();
    }

    public void PlaySwoosh()
    {
        swooshSound.Play();
    }
    public void MusicSlider()
    {

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.GetComponent<Slider>().value);
    }
    public void UpdateSound()
    {
        playerSound.volume = PlayerPrefs.GetFloat("MusicVolume");
    }
    public void Return()
    {
        settingsCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        isSetting = false;
    }
    public void AreYouSure()
    {
        //Time.timeScale = 1;
        mainMenu.SetActive(false);
        exitMenu.SetActive(true);
    }
    public void AreYouSure2()
    {
        //Time.timeScale = 1;
        exitMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
    }
    public void ReturnMenu()
    {
        exitMenu.SetActive(false);
        pauseCanvas.SetActive(true);
    }
    public void ReturnmainMenu()
    {
        mainMenu.SetActive(false);
        pauseCanvas.SetActive(true);
    }
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
