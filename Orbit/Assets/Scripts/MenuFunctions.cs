using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void Quit(){
        Debug.Log("Game exiting");
        Application.Quit();
    }
    public void Restart(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeLevel(int sceneNum){
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneNum);
    }
}
