using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject menuUI;

	// Use this for initialization
	void Start () {
        unPauseGame();
        hideMenuUI();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playOneSet()
    {
        SceneManager.LoadScene(0); //Load scene called Game
    }

    public void playTwoSet()
    {
        SceneManager.LoadScene(1); //Load scene called Game
    }

    public void showMenuUI()
    {
        Debug.Log("Show Menu");
        menuUI.SetActive(true);
        pauseGame();
    }

    public void hideMenuUI()
    {
        menuUI.SetActive(false);
        unPauseGame();
    }

    public void reStartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void pauseGame()
    {
        Time.timeScale = 0;
    }

    private void unPauseGame()
    {
        Time.timeScale = 1;
    }

}
