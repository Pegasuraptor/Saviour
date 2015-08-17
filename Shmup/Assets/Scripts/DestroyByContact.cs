using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour 
{
    public GameObject asteroidExplosion;
    public GameObject playerExplosion;
    public GameObject shieldExplosion;
    public int scoreValue;

    private GameController gameController;
    private PlayerController playerController;

    void Start ()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if(!gameController)
        {
            Debug.Log("DestroyByContact cannot find GameController script.");
        }

        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<PlayerController>();
        }

        if (!playerController)
        {
            Debug.Log("DestroyByContact cannot find PlayerController script.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        string otherTag = other.tag;

        switch(otherTag)
        {
            case "Boundary":
                break;
            case "Player":
                Destroy(other.gameObject);
                Destroy(gameObject);
                Instantiate(asteroidExplosion, transform.position, transform.rotation);
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.GameOver();
                break;
            case "PlayerWeaponFire":
                Destroy(other.gameObject);
                Destroy(gameObject);
                Instantiate(asteroidExplosion, transform.position, transform.rotation);
                gameController.AddScore(scoreValue);
                break;
            case "Option":
                playerController.RemoveOption(other.gameObject);
                Destroy(gameObject);
                Instantiate(asteroidExplosion, transform.position, transform.rotation);
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                break;
            case "PlayerShield":
                other.gameObject.SetActive(false);
                Destroy(gameObject);
                Instantiate(asteroidExplosion, transform.position, transform.rotation);
                Instantiate(shieldExplosion, other.transform.position, other.transform.rotation);
                break;
            default:
                Destroy(other.gameObject);
                Destroy(gameObject);
                Instantiate(asteroidExplosion, transform.position, transform.rotation);
                break;
        }
    }
}
