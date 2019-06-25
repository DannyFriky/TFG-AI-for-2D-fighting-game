using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{


    public Button button1, button2, button3, button4, button5;


    public void PlayGame(int mode)
    {
        ModeSelection.height = Screen.height;
        ModeSelection.width = Screen.width;
        ModeSelection.fullScreen = Screen.fullScreen;
        ModeSelection.modeSelected = mode;
        SceneManager.LoadScene("Fight_Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectButton1()
    {
        button1.Select();
    }

    public void SelectButton2()
    {
        button2.Select();
    }

    public void SelectButton3()
    {
        button3.Select();
    }

    public void SelectButton4()
    {
        button4.Select();
    }

    public void SelectButton5()
    {
        button5.Select();
    }
}
