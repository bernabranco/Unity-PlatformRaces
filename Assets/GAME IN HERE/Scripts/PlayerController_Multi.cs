using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController_Multi : MonoBehaviour
{
    // Player properties
    private Rigidbody rb;
    public float speed = 0;
    Vector3 initialPosition;

    // Audio Player
    AudioSource Soundtrack;
    AudioSource Cars;

    // Collectibles effect state
    bool bonusFast = false;
    bool bonusSlow = false;
    bool bonusJumper = false;

    // Collectibles effect duration time
    float bonusTime = 150;

    // Check when race is finished
    bool raceFinished = false;

    // Game points and result (Winner/Looser)
    private int points;
    private string result;

    // UI Elements
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI pointsText;
    public GameObject scorePanel;
    public GameObject pointsUI;
    public GameObject countdown1;
    public GameObject countdown2;
    public GameObject countdown3;
    public GameObject countdown4;
    public GameObject countdown5;
    public GameObject countdownGO;
    public GameObject finishLine;
    public GameObject player2;

    // Check when race starts
    bool raceStart = false;
    int startTime = 0;
    int levelTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Time passed since level started
        levelTime = (int) Time.realtimeSinceStartup;

        // Rigidbidy of component
        rb = GetComponent<Rigidbody>();

        // Save initial position for respawn
        initialPosition = new Vector3 (transform.position.x,transform.position.y,transform.position.z);

        // Fetch the AudioSource from the GameObject
        Soundtrack = GameObject.Find("Soundtrack").GetComponent<AudioSource>();
        Cars = GameObject.Find("Car Sound").GetComponent<AudioSource>();

        // Set the count to zero 
		points = 0;

        // Draw Points UI
		SetPoints();

        // Hide UI's in beginning of game
        scorePanel.SetActive(false);
        pointsUI.SetActive(false);
        countdown1.SetActive(false);
        countdown2.SetActive(false);
        countdown3.SetActive(false);
        countdown4.SetActive(false);
        countdown5.SetActive(false);
        countdownGO.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Start countdown text
        startCountdown();
        
        // Wait for race start sound
        if (raceStart)
        {
            if(!raceFinished)
            {
                // Draw points UI
                pointsUI.SetActive(true);
                // Set points value in UI
		        SetPoints();
            } else {
                pointsUI.SetActive(false);
            }
            
            // Key inputs to move car
            moveCar();
            // Speed or slow down car when collectible is picked
            bonusEffect();

            // Loose Game / Time runs out
            if(points < 0)
            {
                resultText.text = "LOOSER";
                finalScoreText.text = "0";
                showFinalScore();
                points = 0;
                pointsUI.SetActive(false);
            }
        }

        // Respawn when fall
        if(transform.position.y < -50)
        {
            Vector3 pos = new Vector3();
            pos = initialPosition;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        // Trigger when touch finish line
        if (other.gameObject.CompareTag("Finish Line")) 
        {
            Soundtrack.Stop();
            pointsUI.SetActive(false);

            if (!raceFinished)
            {
                if (transform.position.z > player2.transform.position.z)
                {
                    // Set the text value of your 'winText'
                    result = "PLAYER 2 WINS";
                    finalScoreText.text = points.ToString();
                    resultText.text = result;
                    showFinalScore();
                }
                else
                {
                    // Set the text value of your 'winText'
                    result = "PLAYER 1 WINS";
                    finalScoreText.text = points.ToString();
                    resultText.text = result;
                    showFinalScore();
                } 
            }

            // Race is finished
            raceFinished = true;
            
        }

        // Trigger when touch trampolines
        if (other.gameObject.CompareTag("Jumper")) 
        {
            // Jump!
            bonusJumper = true;
            Debug.Log("JUMP!");
        }

        // Trigger when touch sphere bonus
        if (other.gameObject.CompareTag("Collectible")) 
        {
            // Deactivate collectible
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Collectible Fast")) 
        {
            // Deactivate collectible
            other.gameObject.SetActive(false);

            // Set bonus state
            bonusFast = true;
        }

        if (other.gameObject.CompareTag("Collectible Slow")) 
        {
            // Deactivate collectible
            other.gameObject.SetActive(false);

            // Set bonus state
            bonusSlow = true;
        }
    }

    void SetPoints()
	{
        // Set points based on time since start of game
        startTime = (int) Time.realtimeSinceStartup - levelTime;
        points = 10000 - startTime*5;

        // set point value in UI
        pointsText.text = points.ToString();
	}

    void moveCar()
    {
        // Movement Forces
        Vector3 rotationRight = new Vector3(0, 100, 0);
        Vector3 rotationLeft = new Vector3(0, -100, 0);
        Vector3 backward = new Vector3(0, 0, 1);
        Vector3 forward = new Vector3(0, 0, -1);

        // Key controls
        if (Input.GetKey("w"))
        {
            transform.Translate(forward * speed * Time.deltaTime);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(backward * speed * Time.deltaTime);
        }

        if (Input.GetKey("d"))
        {
            Quaternion deltaRotationRight = Quaternion.Euler(rotationRight * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotationRight);
        }

        if (Input.GetKey("a"))
        {
            Quaternion deltaRotationLeft = Quaternion.Euler(rotationLeft * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotationLeft);
        }
    }

    void bonusEffect (){

        if (bonusJumper)
        {
            // Dont let car rotate itself
            Quaternion rotation = transform.rotation;
            rotation.x = 0;
            rotation.y = 0;
            rotation.z = 0;
            transform.rotation = rotation;
            
            if(bonusTime >= 0)
            {
                Vector3 position = transform.position;
                position.y += 1;
                transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
                transform.position = position;
                bonusTime -= 1;
            }
            else
            {
                bonusJumper = false;
                bonusTime = 150;
            }
        }

        // Consequences of catching sphere collectibles
        if (bonusFast)
        {
            if(bonusTime >= 0)
            {
                bonusTime -= 1;
                speed = 100;
            }
            else
            {
                speed = 30;
                bonusFast = false;
                bonusTime = 100;
            }
        }

        if (bonusSlow)
        {
            if(bonusTime >= 0)
            {
                bonusTime -= 1;
                speed = 1;
            }
            else
            {
                speed = 30;
                bonusSlow = false;
                bonusTime = 100;
            }
        }
    }

    void startCountdown(){
        
        // Start countdown when narrator says "5"
        startTime = (int) Time.realtimeSinceStartup - levelTime;
        
        if (startTime > 14)
        {
            int time = 21 - startTime;

            if(time > 4 && time <= 5)
            {
                countdown1.SetActive(false);
                countdown2.SetActive(false);
                countdown3.SetActive(false);
                countdown4.SetActive(false);
                countdown5.SetActive(true);
                countdownGO.SetActive(false);
            }

            if(time > 3 && time <= 4)
            {
                countdown1.SetActive(false);
                countdown2.SetActive(false);
                countdown3.SetActive(false);
                countdown4.SetActive(true);
                countdown5.SetActive(false);
                countdownGO.SetActive(false);
            }

            if(time > 2 && time <= 3)
            {
                countdown1.SetActive(false);
                countdown2.SetActive(false);
                countdown3.SetActive(true);
                countdown4.SetActive(false);
                countdown5.SetActive(false);
                countdownGO.SetActive(false);
            }

            if(time > 1 && time <= 2)
            {
                countdown1.SetActive(false);
                countdown2.SetActive(true);
                countdown3.SetActive(false);
                countdown4.SetActive(false);
                countdown5.SetActive(false);
                countdownGO.SetActive(false);   
            }

            if(time > 0 && time <= 1)
            {
                countdown1.SetActive(true);
                countdown2.SetActive(false);
                countdown3.SetActive(false);
                countdown4.SetActive(false);
                countdown5.SetActive(false);
                countdownGO.SetActive(false);
            }

            if (time == 0)
            {
                countdown1.SetActive(false);
                countdown2.SetActive(false);
                countdown3.SetActive(false);
                countdown4.SetActive(false);
                countdown5.SetActive(false);
                countdownGO.SetActive(true);
                raceStart = true;
            }

            if (time < 0){
                countdown1.SetActive(false);
                countdown2.SetActive(false);
                countdown3.SetActive(false);
                countdown4.SetActive(false);
                countdown5.SetActive(false);
                countdownGO.SetActive(false);
            }
        }
    }

    public void showFinalScore()
    {
        // Activate score panel
        scorePanel.SetActive(true);
    }

    void restartLevel(){
         SceneManager.LoadScene("Level1");
    }

}