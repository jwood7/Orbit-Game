using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTab(){
        SceneManager.LoadScene("Level 1");
    }

    public void OnExit ()
    {
        Debug.Log("Game exiting");
        Application.Quit();
    }
}
