using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToScen1 : MonoBehaviour
{
    public void EndLess()
    {
        SceneManager.LoadScene("EndLess");
    }
    public void LoadGameTutorial()
    {
        SceneManager.LoadScene("Tutorial1");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadLeaderboardScene()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }
    public void LoadRamdomEndlessScene()
    {
        SceneManager.LoadScene("Endless_MapGena");
    }
}
