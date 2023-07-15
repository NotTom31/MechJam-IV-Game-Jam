using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Start Menu")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject controlsMenu;
    //public GameObject fadeOut;

    [Header("In Game Menu")]
    public GameObject pauseMenu;
    public GameObject HUD;
    public GameObject dialogueBox;

    [Header("Loading Screen")]
    public GameObject LoadingScreen;
    public Image LoadingBarFill;

    private bool isPaused = false;
    private bool isDialogue = false;

    public void MenuNext()
    {
        SoundManager.Instance.PlaySound("menuNext", 1.0f);
    }

    public void MenuBack()
    {
        SoundManager.Instance.PlaySound("menuBack", 1.0f);
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsDialogue()
    {
        return isDialogue;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            BackToMainMenu();
        }
        else
        {
            StartOfScene();
        }
    }
    public void StartOfScene()
    {
        InGameSwitch("HUD");
    }

    public void Pause()
    {
        SoundManager.Instance.PlaySound("menuPause", 1.0f);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        //GameManager.Instance.SetGameState(GameState.PAUSED);
        InGameSwitch("Pause");
        Time.timeScale = 0; // Make sure Input System Package setting "Update Mode" is set to "Dynamic Update", otherwise it will not work
    }

    public void Unpause()
    {
        SoundManager.Instance.PlaySound("menuPause", 1.0f);
        if (!isPaused)
            return;
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        //GameManager.Instance.SetGameStateByContext();
        InGameSwitch("HUD");
    }

    public void OpenDialogue(string name, string[] text)
    {
        dialogueBox.GetComponent<Animator>().Play("OpenDialogue");
        //GameManager.Instance.SetGameState(GameState.DIALOGUE);
        isPaused = false; isDialogue = true;
        //playerController.SetSpeedZero();
        //Time.timeScale = 0;
        InGameSwitch("Dialogue");
        //dialogueName.SetText(name);
        //dialogue.SetText(text);
        //dialogue.StartDialogue();
    }

    public void CloseDialogue()
    {
        dialogueBox.GetComponent<Animator>().Play("CloseDialogue");
        //GameManager.Instance.SetGameStateByContext();
        isPaused = false; isDialogue = false;
        //Time.timeScale = 1;
        Invoke("CloseDialogueDelay", 0.3f);
    }

    private void CloseDialogueDelay()
    {
        InGameSwitch("HUD");
        //openDialogue.DoneTalking();
    }

    public void NextDialogue()
    {
        //dialogue.Click();
    }

    public void InGameSwitch(string ui)
    {
        switch (ui)
        {
            case "HUD":
                HUD.SetActive(true);
                pauseMenu.SetActive(false);
                dialogueBox.SetActive(false);
                LoadingScreen.SetActive(false);
                break;
            case "Pause":
                HUD.SetActive(false);
                pauseMenu.SetActive(true);
                break;
            case "Dialogue":
                HUD.SetActive(true);
                pauseMenu.SetActive(false);
                dialogueBox.SetActive(true);
                break;
            default:
                break;
        }
    }


    public void MoveToScene(string SceneName)
    {
        //GameManager.Instance.ChangeScene(SceneName);
        StartCoroutine(LoadNextScene(SceneName));
    }

    private IEnumerator LoadNextScene(string SceneName)
    {
        LoadingScreen.SetActive(true);
        LoadingBarFill.fillAmount = 0;
        //yield return new WaitForSeconds(1.0f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName); //does not change the game state, need to fix
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
        //MoveToScene(SceneName); //might conflict with loading bar
    }

    public void SendToMainMenu(string SceneName)
    {
        SoundManager.Instance.PlayMusic("menuMusic");
        //GameManager.Instance.SetGameState(GameState.MAIN_MENU);
        Time.timeScale = 1;
        //GameManager.Instance.ChangeScene(SceneName);
        MoveToScene(SceneName);
    }

    public void NewGame(string SceneName)
    {
        Debug.Log("Starting new game!");
        StartCoroutine(NewGameFade(SceneName));

    }

    private IEnumerator NewGameFade(string SceneName)
    {

        //fadeOut.SetActive(true);
        LoadingScreen.SetActive(true);
        //fadeOut.GetComponent<Animator>().Play("MenuFade");
        LoadingBarFill.fillAmount = 0;
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlayMusic("hubMusic");
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName); //does not change the game state, need to fix
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progressValue;

            yield return null;
        }
        //MoveToScene(SceneName); //might conflict with loading bar
    }

    public void OpenCredits()
    {
        MenuSwitch("Credits");
    }

    public void OpenControls()
    {
        MenuSwitch("Controls");
    }

    public void BackToMainMenu()
    {
        MenuSwitch("Main Menu");
    }

    public void OpenSettings()
    {
        MenuSwitch("Settings");
    }

    private void MenuSwitch(string ui)
    {
        switch (ui)
        {
            case "Controls":
                controlsMenu.SetActive(true);
                creditsMenu.SetActive(false);
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                LoadingScreen.SetActive(false);
                break;
            case "Credits":
                controlsMenu.SetActive(false);
                creditsMenu.SetActive(true);
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                LoadingScreen.SetActive(false);
                break;
            case "Main Menu":
                controlsMenu.SetActive(false);
                creditsMenu.SetActive(false);
                mainMenu.SetActive(true);
                settingsMenu.SetActive(false);
                LoadingScreen.SetActive(false);
                break;
            case "Settings":
                controlsMenu.SetActive(false);
                creditsMenu.SetActive(false);
                mainMenu.SetActive(false);
                settingsMenu.SetActive(true);
                LoadingScreen.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}