using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int score;
    public int red;
    public int green;
    public int blue;
    public TMP_Text scoreText;
    public TMP_Text redText;
    public TMP_Text blueText;
    public TMP_Text greenText;
    public TMP_Text powerUsed;
    public static gameManager instance;
    public bool muteAll;
    void Start()
    {
        //scoreText.text = "Score : 0";
        //redText.text = "RedPower : 0";
        //blueText.text = "BluePower : 0";
        //greenText.text = "GreenPower : 0";
        //powerUsed.text = "";
        muteAll = false;
    }

    public void IncreamentScore(int amount)
    {
        score += amount;
        scoreText.text = "Score : " + score;
    }
    public void IncreamentRedPower(int amount)
    {
        if (red + amount < 5)
        {
            red += amount;
            redText.text = "Red Power : " + red;
        }
        else
        {
            red = 5;
            redText.text = "Red Power : " + red;
        }
    }
    public void IncreamentGreenPower(int amount)
    {
        if (green + amount < 5)
        {
            green += amount;
            greenText.text = "Green Power : " + green;
        }
        else
        {
            green = 5;
            greenText.text = "Green Power : " + green;
        }
    }
    public void IncreamentBluePower(int amount)
    {
        if (blue + amount < 5)
        {
            blue += amount;
            blueText.text = "Blue Power : " + blue;
        }
        else
        {
            blue = 5;
            blueText.text = "Blue Power : " + blue;
        }
    }
    public void DecreamentRedPower(int amount)
    {
        red -= amount;
        redText.text = "Red Power : " + red;
    }
    public void DecreamentGreenPower(int amount)
    {
        green -= amount;
        greenText.text = "Green Power : " + green;
    }
    public void DecreamentBluePower(int amount)
    {
        blue -= amount;
        blueText.text = "Blue Power : " + blue;
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
