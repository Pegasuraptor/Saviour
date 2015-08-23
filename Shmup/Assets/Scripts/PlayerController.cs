using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
    
    public float tilt;
    public Boundary screenBoundary;
    public Transform mainGun;
    public Transform[] sidekickSlots;
    public GameObject shield;
    public GameObject bolt;
    public GameObject specialBolt;
    public GameObject sidekick;

    public int shotsUntilSpecial;

    private float timeUntilNextShot;
    private int shotsFired;
    private int numSidekicks;
    public LevelManager levelManager;
    private float[] multiFireAngles;

    void Start()
    {
        levelManager.SetUp();
        levelManager.sidekickMax = sidekickSlots.Length;
        shield.SetActive(false);
        shotsFired = 0;
        numSidekicks = 0;
        multiFireAngles = new float[] {-40f, 40f, 20f, -20f };
    }

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3(-moveVertical, 0.0f, moveHorizontal);
        GetComponent<Rigidbody>().velocity = movement * levelManager.speed;

        float xBounds = Mathf.Clamp(GetComponent<Rigidbody>().position.x, screenBoundary.xMin, screenBoundary.xMax);
        float zBounds = Mathf.Clamp(GetComponent<Rigidbody>().position.z, screenBoundary.zMin, screenBoundary.zMax);
        GetComponent<Rigidbody>().position = new Vector3(xBounds, 0.0f, zBounds);

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * tilt);

        Transform tempTransform;
        for (int i = 0; i < sidekickSlots.Length; i++)
        {
            tempTransform = sidekickSlots[i];
            if (tempTransform.childCount > 0)
            {
                tempTransform.GetChild(0).gameObject.GetComponent<Rigidbody>().position = tempTransform.position;
                tempTransform.GetChild(0).gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * tilt);
            }
        }
	}

    void Update()
    {
        levelManager.isShieldActive = shield.activeSelf;
        
        if(Input.GetKey(KeyCode.Q))
        {
            levelManager.LevelUp(0);
        }
        else if(Input.GetKey(KeyCode.E))
        {
            levelManager.LevelUp(1);
        }

        shield.SetActive(levelManager.isShieldActive);

        if(numSidekicks != levelManager.sidekickLevel)
        {
            AddSidekick();
        }

        if (Input.GetButton("Fire1") && Time.time > timeUntilNextShot)
        {
            //StandardFire
            timeUntilNextShot = Time.time + levelManager.fireRate;
            Instantiate(bolt, mainGun.position, mainGun.rotation);
            GetComponent<AudioSource>().Play();

            //MultiFire
            for (int i = 0; i < levelManager.multiLevel; i++)
            {
                Instantiate(bolt, mainGun.position, Quaternion.Euler(0, multiFireAngles[i], 0));
            }

            //HomingFire
            if (levelManager.homingLevel > 0)
            {
                shotsFired++;

                if (shotsFired >= (shotsUntilSpecial - (2 * levelManager.homingLevel)))
                {
                    shotsFired = 0;
                    Instantiate(specialBolt, mainGun.position, mainGun.rotation);
                }
            }
   
            //sidekickFire
            if(levelManager.sidekickLevel > 0)
            {
                Transform tempTransform;
                for (int i = 0; i < sidekickSlots.Length; i++)
                {
                    tempTransform = sidekickSlots[i];
                    if (tempTransform.childCount > 0)
                    {

                        Instantiate(bolt, tempTransform.GetChild(0).position, tempTransform.GetChild(0).rotation);
                    }
                }
            }
        }
    }

    void AddSidekick()
    {
        GameObject temp;
        Transform tempTransform;

        for (int i = 0; i < sidekickSlots.Length; i++)
        {
            tempTransform = sidekickSlots[i];
            if (tempTransform.childCount == 0)
            {
                temp = (GameObject)Instantiate(sidekick, tempTransform.position, tempTransform.rotation);
                temp.transform.parent = tempTransform;
                temp = null;
                numSidekicks++;
                return;
            }

            tempTransform = null;
        }
    }

    public void RemoveSidekick(GameObject o)
    {
        levelManager.sidekickLevel--;
        numSidekicks--;
        Transform tempTransform;

        for (int i = 0; i < sidekickSlots.Length; i++)
        {
            tempTransform = sidekickSlots[i];
            if (tempTransform.childCount > 0)
            {
                tempTransform = tempTransform.GetChild(0);
                if (tempTransform == o.transform)
                {
                    tempTransform = null;
                    Destroy(o);
                    return;
                }
            }

            tempTransform = null;
        }
    }
}
