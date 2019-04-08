using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
public class SceneChanger : MonoBehaviour
{


    private static SceneChanger instance = new SceneChanger();

    private SceneChanger() { }

    public static SceneChanger GetInstance()
    {
        return instance;
    }

	public void SceneMainMenu()
    {
        SteamVR_LoadLevel.Begin("MainMenu");
    }

    public void SceneMain()
    {
        SteamVR_LoadLevel.Begin("MAIN");
     }

    public void QuitGame()
    {
        Application.Quit();
    }
}
