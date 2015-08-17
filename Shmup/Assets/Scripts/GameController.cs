using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
    public GameObject hazard;
    public GameObject hazardContainer;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
  
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    private int score;
    private bool gameOver;
    private bool restart;
    private GameObject tempHazardRef;

    void Start()
    {
        gameOver = false;
        restart = false;

        restartText.text = "";
        gameOverText.text = "";

        score = 0;
        UpdateScore();
        StartCoroutine (SpawnWaves());
    }

    void Update()
    {
        if(restart)
        {
            if(Input.GetKeyDown (KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    IEnumerator SpawnWaves ()
    {
        yield return new WaitForSeconds(spawnWait);
        while(true)
        {
            for(int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;

                tempHazardRef = (GameObject)Instantiate(hazard, spawnPosition, spawnRotation);
                tempHazardRef.transform.parent = hazardContainer.transform;
                yield return new WaitForSeconds(spawnWait);
            }

            if(gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScore();
    }

    public void GameOver ()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
