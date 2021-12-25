using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class EnemyController : MonoBehaviour
{
    // stuff that player 2 needs to access
    public GameObject finishLine;
    public GameObject player1;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI pointsText;
    public GameObject scorePanel;

    // points logic variables
    private string result;
    private int points;

    // See when level starts
    int startTime = 0;
    int levelTime = 0;

    // Player properties
    private Rigidbody rb;
    public float speed = 0;
    Vector3 initialPosition;

    // Collectibles effect state
    bool bonusFast = false;
    bool bonusSlow = false;
    bool bonusJumper = false;

    // Collectibles effect duration time
    float bonusTime = 100;
    bool raceFinished = false;
    bool raceStart = false;
    
    // Movement Forces
    Vector3 rotationRight = new Vector3(0, 100, 0);
    Vector3 rotationLeft = new Vector3(0, -100, 0);
    Vector3 backward = new Vector3(0, 0, 1);
    Vector3 forward = new Vector3(0, 0, -1);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        levelTime = (int) Time.realtimeSinceStartup;
        initialPosition = new Vector3 (transform.position.x,transform.position.y,transform.position.z);

    }

    private void FixedUpdate()
    {
        // Wait for race start sound
        startCountdown();

        if (raceStart)
        {
            // Key inputs to move car
            moveCar();

            // Set Points
            SetPoints();
            
            // Speed or slow down car when collectible is picked
            bonusEffect();

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

            if (!raceFinished)
            {
                if (transform.position.z < player1.transform.position.z)
                {
                    // Set the text value of your 'winText'
                    result = "PLAYER 2 WINS";
                    finalScoreText.text = points.ToString();
                    resultText.text = result;
                    // Activate score panel
                    scorePanel.SetActive(true);
                }
                else
                {
                    Debug.Log("nao cheguei");
                    // Set the text value of your 'winText'
                    result = "PLAYER 1 WINS";
                    finalScoreText.text = points.ToString();
                    resultText.text = result;
                    // Activate score panel
                    scorePanel.SetActive(true);
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
        points = 10000 - startTime;

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
        if (Input.GetKey("up"))
        {
            transform.Translate(forward * speed * Time.deltaTime);
        }
        if (Input.GetKey("down"))
        {
            transform.Translate(backward * speed * Time.deltaTime);
        }

        if (Input.GetKey("right"))
        {
            Quaternion deltaRotationRight = Quaternion.Euler(rotationRight * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotationRight);
        }

        if (Input.GetKey("left"))
        {
            Quaternion deltaRotationLeft = Quaternion.Euler(rotationLeft * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotationLeft);
        }
    }

void bonusEffect ()
    {

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
        
        startTime = (int) Time.realtimeSinceStartup - levelTime;
        if (startTime > 14)
        {
            int time = 21 - startTime;

            if (time == 0){
                raceStart = true;
            }
        }
    }

    public void showFinalScore()
    {
        // Activate score panel
        scorePanel.SetActive(true);
    }
}