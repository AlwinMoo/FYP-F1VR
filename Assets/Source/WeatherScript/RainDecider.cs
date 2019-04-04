using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDecider : MonoBehaviour {

    public GameObject Rain;
    public GameObject RainDrop;
    public Light Sun;
    public Light Moon;

    private Transform _player;
    public float _weatherHeight = 90f;

    bool RainOrNot;
    float myTimer;
    int rainchance;
    float Time2;
    bool once = true;

	// Use this for initialization
	void Start () {
        RainOrNot = false;
        myTimer = 20;
        Time2 = 0;
        GameObject _playerGameObject = GameObject.FindGameObjectWithTag("Car");
        _player = _playerGameObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time2 >= 10)
        {
            rainchance = Random.Range(1, 5);
            if (rainchance == 3)
            {
                RainOrNot = true;
            }
            Time2 = 0;
        }

        Time2 += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.O))
        {
            RainOrNot = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            RainOrNot = false;
        }

        if (RainOrNot == true)
        {
            if (Sun.intensity > 0.006f)
                Sun.intensity -= 0.003f;
            if (Moon.intensity > 0.006f)
                Moon.intensity -= 0.003f;

            if (once == true)
            {
                once = false;
            }
            Rain.SetActive(true);
            RainDrop.SetActive(true);
            if (myTimer > 0)
            {
                myTimer -= Time.deltaTime;
            }
            if (myTimer <= 0)
            {
                myTimer = 20;
                RainOrNot = false;
            }
        }
        else
        {
            Rain.SetActive(false);
            RainDrop.SetActive(false);
            once = true;
            if (Sun.intensity < 0.49f)
                Sun.intensity += 0.005f;
            if (Sun.intensity > 0.49f)
                Sun.intensity = 0.5f;
            if (Moon.intensity < 0.49f)
            Moon.intensity += 0.005f;
            if (Moon.intensity > 0.49f)
                Moon.intensity = 0.5f;
        }

        if(Rain.active == true)
        Rain.transform.position = new Vector3(_player.position.x + 62, _player.position.y + _weatherHeight, _player.position.z + 47);
    }
}
