using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text scoreText;
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        scoreText.text = "Score : " + gameManager.instance.score;
    }
    public void Restart()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
