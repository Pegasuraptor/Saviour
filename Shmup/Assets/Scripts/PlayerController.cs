using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
    private const int OPTION_MAX = 2;
    private const int HOMING_MAX = 4;
    private const int MULTI_MAX = 4;

    public float speed;
    public float tilt;
    public Boundary screenBoundary;
    public Transform mainGun;
    public Transform optionSlot1;
    public Transform optionSlot2;
    public GameObject shield;
    public GameObject bolt;
    public GameObject specialBolt;
    public GameObject option;
    public float fireRate;
    public int shotsUntilSpecial;

    private float timeUntilNextShot;
    private int shotsFired;
    public int homingLevel;
    public int multiLevel;
    public int optionLevel;
    private float[] multiFireAngles;

    void Start()
    {
        shield.SetActive(false);
        shotsFired = 0;
        homingLevel = 0;
        multiLevel = 0;
        multiFireAngles = new float[] {-40f, 40f, 20f, -20f };
    }

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3(-moveVertical, 0.0f, moveHorizontal);
        GetComponent<Rigidbody>().velocity = movement * speed;

        float xBounds = Mathf.Clamp(GetComponent<Rigidbody>().position.x, screenBoundary.xMin, screenBoundary.xMax);
        float zBounds = Mathf.Clamp(GetComponent<Rigidbody>().position.z, screenBoundary.zMin, screenBoundary.zMax);
        GetComponent<Rigidbody>().position = new Vector3(xBounds, 0.0f, zBounds);

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * tilt);

        if (optionSlot1.childCount != 0)
        {
            optionSlot1.GetChild(0).gameObject.GetComponent<Rigidbody>().position = optionSlot1.position;
            optionSlot1.GetChild(0).gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * tilt);
        }

        if (optionSlot2.childCount != 0)
        {
            optionSlot2.GetChild(0).gameObject.GetComponent<Rigidbody>().position = optionSlot2.position;
            optionSlot2.GetChild(0).gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * tilt);
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(multiLevel < MULTI_MAX)
                multiLevel++;

            if(homingLevel < HOMING_MAX)
                homingLevel++;

            if(optionLevel < OPTION_MAX)
                AddOption();

            shield.SetActive(!shield.activeSelf);
        }

        if (Input.GetButton("Fire1") && Time.time > timeUntilNextShot)
        {
            //StandardFire
            timeUntilNextShot = Time.time + fireRate;
            Instantiate(bolt, mainGun.position, mainGun.rotation);
            GetComponent<AudioSource>().Play();

            //MultiFire
            for (int i = 0; i < multiLevel; i++)
            {
                Instantiate(bolt, mainGun.position, Quaternion.Euler(0, multiFireAngles[i], 0));
            }

            //HomingFire
            if (homingLevel > 0)
            {
                shotsFired++;

                if (shotsFired >= (shotsUntilSpecial - (2 * homingLevel)))
                {
                    shotsFired = 0;
                    Instantiate(specialBolt, mainGun.position, mainGun.rotation);
                }
            }
   
            //OptionFire
            if (optionSlot1.childCount != 0)
            {
                Instantiate(bolt, optionSlot1.position, optionSlot1.rotation);
            }

            if (optionSlot2.childCount != 0)
            {
                Instantiate(bolt, optionSlot2.position, optionSlot2.rotation);
            }
        }
    }

    void AddOption()
    {
        optionLevel++;
        GameObject temp;

        if(optionSlot1.childCount == 0)
        {
            temp = (GameObject)Instantiate(option, optionSlot1.position, optionSlot1.rotation);
            temp.transform.parent = optionSlot1;
            temp = null;
            return;
        }

        if(optionSlot2.childCount == 0)
        {
            temp = (GameObject)Instantiate(option, optionSlot2.position, optionSlot2.rotation);
            temp.transform.parent = optionSlot2;
            temp = null;
            return;
        }
    }

    public void RemoveOption(GameObject o)
    {
        optionLevel--;
        Transform temp;

        if (optionSlot1.childCount != 0)
        {
            temp = optionSlot1.GetChild(0);
            if(temp == o.transform)
            {
                temp = null;
                Destroy(o);
                return;
            }
        }

        if (optionSlot2.childCount != 0)
        {
            temp = optionSlot2.GetChild(0);
            if (temp == o.transform)
            {
                temp = null;
                Destroy(o);
                return;
            }
        }
    }
}
