using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerControl : MonoBehaviour
{
    public float speed = 100;
    float horizontalInput;
    public float horizontalMultiplier = 30;
    public Material redMaterial;  // Reference to the red material
    public Material greenMaterial;  // Reference to the green material
    public Material blueMaterial;  // Reference to the blue material
    public Material whiteMaterial;  // Reference to the white material
    private Renderer playerRenderer;  // Reference to the player's renderer
    public bool alive = true;
    public string state;
    public bool powerUsed = false;
    public GameObject PausePanel;
    public GameObject muteIcon;
    public GameObject PauseButtonPanel;
    public GameObject UsePowerPanel;
    public GameObject SwitchColorPanel;
    public bool pause = false;
    public GameObject sheild;
    private bool mute = false;
    private bool cheating = false;
    private bool IsPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the renderer component of the player
        AudioSource temp = GameObject.FindGameObjectWithTag("backgroundSound").GetComponent<AudioSource>();
        temp.Play();
        playerRenderer = GetComponent<Renderer>();
        PausePanel.SetActive(false);
        muteIcon.SetActive(false);
        PauseButtonPanel.SetActive(false);
        UsePowerPanel.SetActive(false);
        SwitchColorPanel.SetActive(false);
        pause = false;
        state = "normal";
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        #if UNITY_ANDROID
            // Enable the panels on Android only
            PauseButtonPanel.SetActive(true);
            UsePowerPanel.SetActive(true);
            SwitchColorPanel.SetActive(true);
        #endif
        #if UNITY_EDITOR
            // Enable the panels in the Unity Editor for testing
            PauseButtonPanel.SetActive(true);
            UsePowerPanel.SetActive(true);
            SwitchColorPanel.SetActive(true);
        #endif
    }

    private void FixedUpdate()
    {
        if (pause) return;

        Vector3 moveForward = transform.forward * speed * Time.deltaTime;
        Vector3 moveHorizontal = transform.right * horizontalInput * Time.deltaTime * speed * horizontalMultiplier;
        transform.position += moveForward;
        if ((moveHorizontal + transform.position).x < 5 && (moveHorizontal + transform.position).x > -5)
        {
            transform.position += moveHorizontal;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("blue") || other.gameObject.CompareTag("red") || other.gameObject.CompareTag("green"))
        {
            CollectOrb(other);
            AudioSource temp = GameObject.FindGameObjectWithTag("collectOrbSound").GetComponent<AudioSource>();
            temp.Play();
        }
        else if(other.gameObject.CompareTag("obstacle") && cheating)
        {
            return;
        }
        else if (other.gameObject.CompareTag("obstacle") && state == "normal")
        {
            
            AudioSource temp = GameObject.FindGameObjectWithTag("hitObctacle").GetComponent<AudioSource>();
            temp.Play();
            SceneManager.LoadScene("GameOver");
        }
        else if (other.gameObject.CompareTag("obstacle") && state != "normal")
        {
            AudioSource temp = GameObject.FindGameObjectWithTag("hitObctacle").GetComponent<AudioSource>();
            temp.Play();
            handleObstacle(other);
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch
            if (touch.phase == TouchPhase.Moved)
            {
                float swipeDelta = touch.deltaPosition.x;
                float movement = swipeDelta * speed * Time.deltaTime * 0.1f;
                Vector3 vector3 = transform.position + Vector3.right * movement;
                if(vector3.x <= 5 && vector3.x>=-5)
                    transform.Translate(Vector3.right * movement);
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (gameManager.instance.red == 5)
            {
                if (GameObject.FindGameObjectWithTag("sheild")) Destroy(GameObject.FindGameObjectWithTag("sheild"));
                powerUsed = false;
                gameManager.instance.powerUsed.text = "";
                AudioSource temp = GameObject.FindGameObjectWithTag("switchcolor").GetComponent<AudioSource>();
                temp.Play();
                // Change the player's color to red
                gameManager.instance.DecreamentRedPower(1);
                playerRenderer.material = redMaterial;
                state = "red";    
            }
            else
            {
                AudioSource temp = GameObject.FindGameObjectWithTag("wrong").GetComponent<AudioSource>();
                temp.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (gameManager.instance.green == 5)
            {
                if(GameObject.FindGameObjectWithTag("sheild")) Destroy(GameObject.FindGameObjectWithTag("sheild"));
                powerUsed = false;
                gameManager.instance.powerUsed.text = "";
                AudioSource temp = GameObject.FindGameObjectWithTag("switchcolor").GetComponent<AudioSource>();
                temp.Play();
                // Change the player's color to green
                gameManager.instance.DecreamentGreenPower(1);
                playerRenderer.material = greenMaterial;
                state = "green";
            }
            else
            {
                AudioSource temp = GameObject.FindGameObjectWithTag("wrong").GetComponent<AudioSource>();
                temp.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (gameManager.instance.blue == 5)
            {
                powerUsed = false;
                gameManager.instance.powerUsed.text = "";
                AudioSource temp = GameObject.FindGameObjectWithTag("switchcolor").GetComponent<AudioSource>();
                temp.Play();
                // Change the player's color to blue
                gameManager.instance.DecreamentBluePower(1);
                playerRenderer.material = blueMaterial;
                state = "blue";
            }
            else
            {
                AudioSource temp = GameObject.FindGameObjectWithTag("wrong").GetComponent<AudioSource>();
                temp.Play();
            }

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Using the red power
            if(state == "red")
            {
                UsingRedPower();
            }
            else if( state == "green" && !powerUsed)
            {
                UsingGreenPower();
            }
            else if(state == "blue" && !powerUsed)
            {

                UsingBluePower();
            }
            AudioSource temp = GameObject.FindGameObjectWithTag("wrong").GetComponent<AudioSource>();
            temp.Play();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanel.SetActive(!PausePanel.activeSelf);
            pause = !pause;
            if (pause)
            {
                AudioSource temp = GameObject.FindGameObjectWithTag("backgroundSound").GetComponent<AudioSource>();
                temp.Pause();
                AudioSource temp2 = GameObject.FindGameObjectWithTag("pause").GetComponent<AudioSource>();
                temp2.Play();
            }
            else
            {
                AudioSource temp = GameObject.FindGameObjectWithTag("pause").GetComponent<AudioSource>();
                temp.Pause();
                AudioSource temp2 = GameObject.FindGameObjectWithTag("backgroundSound").GetComponent<AudioSource>();
                temp2.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            mute = !mute;
            muteIcon.SetActive(!muteIcon.activeSelf);
            if (mute)
            {
                AudioSource temp = GameObject.FindGameObjectWithTag("backgroundSound").GetComponent<AudioSource>();
                temp.Pause();
            }
            else
            {
                AudioSource temp = GameObject.FindGameObjectWithTag("backgroundSound").GetComponent<AudioSource>();
                temp.Play();
            }
        }
        //cheats
        if(Input.GetKeyDown(KeyCode.U))
        {
            cheating = !cheating;
        }
        if(Input.GetKey(KeyCode.I) && !IsPressed)
        {
            IsPressed = true;
            if(gameManager.instance.red < 5)
            {
                gameManager.instance.IncreamentRedPower(1);
            }
        }
        if(Input.GetKeyDown(KeyCode.O) && !IsPressed)
        {
            IsPressed = true;
            if (gameManager.instance.green < 5)
            {
                gameManager.instance.IncreamentGreenPower(1);
            }
        }
        if(Input.GetKeyDown(KeyCode.P) && !IsPressed)
        {
            IsPressed = true;
            if(gameManager.instance.blue < 5)
            {
                gameManager.instance.IncreamentBluePower(1);
            }
        }
        if(Input.GetKeyUp(KeyCode.I) || Input.GetKeyUp(KeyCode.O) || Input.GetKeyUp(KeyCode.P)) {
            IsPressed = false;
        }
    }
    public void RedButton()
    {
        if (gameManager.instance.red == 5)
        {
            if (GameObject.FindGameObjectWithTag("sheild")) Destroy(GameObject.FindGameObjectWithTag("sheild"));
            powerUsed = false;
            gameManager.instance.powerUsed.text = "";
            AudioSource temp = GameObject.FindGameObjectWithTag("switchcolor").GetComponent<AudioSource>();
            temp.Play();
            // Change the player's color to red
            gameManager.instance.DecreamentRedPower(1);
            playerRenderer.material = redMaterial;
            state = "red";
        }
        else
        {
            AudioSource temp = GameObject.FindGameObjectWithTag("wrong").GetComponent<AudioSource>();
            temp.Play();
        }
    }
    public void GreenButton()
    {
        if (gameManager.instance.green == 5)
        {
            if (GameObject.FindGameObjectWithTag("sheild")) Destroy(GameObject.FindGameObjectWithTag("sheild"));
            powerUsed = false;
            gameManager.instance.powerUsed.text = "";
            AudioSource temp = GameObject.FindGameObjectWithTag("switchcolor").GetComponent<AudioSource>();
            temp.Play();
            // Change the player's color to green
            gameManager.instance.DecreamentGreenPower(1);
            playerRenderer.material = greenMaterial;
            state = "green";
        }
        else
        {
            AudioSource temp = GameObject.FindGameObjectWithTag("wrong").GetComponent<AudioSource>();
            temp.Play();
        }
    }
    public void BlueButton()
    {
        if (gameManager.instance.blue == 5)
        {
            powerUsed = false;
            gameManager.instance.powerUsed.text = "";
            AudioSource temp = GameObject.FindGameObjectWithTag("switchcolor").GetComponent<AudioSource>();
            temp.Play();
            // Change the player's color to blue
            gameManager.instance.DecreamentBluePower(1);
            playerRenderer.material = blueMaterial;
            state = "blue";
        }
        else
        {
            AudioSource temp = GameObject.FindGameObjectWithTag("wrong").GetComponent<AudioSource>();
            temp.Play();
        }
    }
    public void UsePowerButton()
    {
        // Using the red power
        if (state == "red")
        {
            UsingRedPower();
        }
        else if (state == "green" && !powerUsed)
        {
            UsingGreenPower();
        }
        else if (state == "blue" && !powerUsed)
        {

            UsingBluePower();
        }
        AudioSource temp = GameObject.FindGameObjectWithTag("wrong").GetComponent<AudioSource>();
        temp.Play();
    }
    public void PauseButton()
    {
        PausePanel.SetActive(!PausePanel.activeSelf);
        pause = !pause;
        if (pause)
        {
            AudioSource temp = GameObject.FindGameObjectWithTag("backgroundSound").GetComponent<AudioSource>();
            temp.Pause();
            AudioSource temp2 = GameObject.FindGameObjectWithTag("pause").GetComponent<AudioSource>();
            temp2.Play();
        }
        else
        {
            AudioSource temp = GameObject.FindGameObjectWithTag("pause").GetComponent<AudioSource>();
            temp.Pause();
            AudioSource temp2 = GameObject.FindGameObjectWithTag("backgroundSound").GetComponent<AudioSource>();
            temp2.Play();
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    public void Resume()
    {
        PausePanel.SetActive(false);
        pause = false;
        AudioSource temp = GameObject.FindGameObjectWithTag("pause").GetComponent<AudioSource>();
        temp.Pause();
        AudioSource temp2 = GameObject.FindGameObjectWithTag("backgroundSound").GetComponent<AudioSource>();
        temp2.Play();
    }
    void CollectOrb(Collider other) 
    {
        Destroy(other.gameObject);
        if (state == "green" && powerUsed)
        {
            if (other.gameObject.CompareTag("blue") || other.gameObject.CompareTag("red"))
            {
                gameManager.instance.IncreamentScore(5);
                if (other.gameObject.CompareTag("red")) gameManager.instance.IncreamentRedPower(2);
                if (other.gameObject.CompareTag("blue")) gameManager.instance.IncreamentBluePower(2);
            }
            else
            {
                gameManager.instance.IncreamentScore(10);
            }
            EndGreeenPowe();
        }
        else if(state != "normal" && other.gameObject.CompareTag(state))
        {
            gameManager.instance.IncreamentScore(2);
        }
        else
        {
            gameManager.instance.IncreamentScore(1);
            if (other.gameObject.CompareTag("blue")) gameManager.instance.IncreamentBluePower(1);
            if (other.gameObject.CompareTag("red")) gameManager.instance.IncreamentRedPower(1);
            if (other.gameObject.CompareTag("green")) gameManager.instance.IncreamentGreenPower(1);
        }
    }
    void handleObstacle(Collider other)
    {
        if(state == "blue" && powerUsed)
        {
            Destroy(other.gameObject);
            EndBluePower();
        }
        else
        {
            state = "normal";
            powerUsed = false;
            gameManager.instance.powerUsed.text = "";
            playerRenderer.material = whiteMaterial;
            Destroy(other.gameObject);
        }
    }
    void UsingRedPower ()
    {
        
        AudioSource temp = GameObject.FindGameObjectWithTag("usePower").GetComponent<AudioSource>();
        temp.Play();
        foreach (var obstacle in GameObject.FindGameObjectsWithTag("obstacle"))
        {
            Destroy(obstacle);
        }
        gameManager.instance.DecreamentRedPower(1);
        if (gameManager.instance.red == 0)
        {
            state = "normal";
            playerRenderer.material = whiteMaterial;
        }
    }
    void UsingGreenPower ()
    {
       
        AudioSource temp = GameObject.FindGameObjectWithTag("usePower").GetComponent<AudioSource>();
        temp.Play(); 
        powerUsed = true;
        gameManager.instance.DecreamentGreenPower(1);
        gameManager.instance.powerUsed.text = "Multiplier";
        gameManager.instance.powerUsed.color = Color.green;
    } 
    void EndGreeenPowe()
    {
        gameManager.instance.powerUsed.text = "";
        powerUsed = false;
        if(gameManager.instance.green == 0)
        {
            state = "normal";
            playerRenderer.material = whiteMaterial;
        }
    }
    void UsingBluePower ()
    {
        
        AudioSource temp = GameObject.FindGameObjectWithTag("usePower").GetComponent<AudioSource>();
        temp.Play();
        powerUsed = true;
        Vector3 SheildPostion = transform.position;
        SheildPostion.y = 1;
        Instantiate(sheild, SheildPostion,Quaternion.identity);
        gameManager.instance.DecreamentBluePower(1);
        gameManager.instance.powerUsed.text = "Sheild";
        gameManager.instance.powerUsed.color = Color.blue;
    }
    void EndBluePower ()
    {
        gameManager.instance.powerUsed.text = "";
        powerUsed = false;
        Destroy(GameObject.FindGameObjectWithTag("sheild"));
        if (gameManager.instance.blue == 0)
        {
            state = "normal";
            playerRenderer.material = whiteMaterial;
        }
    }
}
