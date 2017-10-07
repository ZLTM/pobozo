using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuNav : MonoBehaviour
{    
    public void ClickExit()
    {
        Application.Quit();
    }

    public void StartNewGame()
    {
        GMScript.InBattle = false;
        Application.LoadLevel("SchoolInterior");
    }

    public void BackMenu()
    {
        GMScript.InBattle = false;
        Application.LoadLevel("StartMenu");
    }

    public void Retry()
    {
        GMScript.InBattle = false;
        Application.LoadLevel("ToyBox");
    }
}
