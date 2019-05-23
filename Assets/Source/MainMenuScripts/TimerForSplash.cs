using System.Collections;
using UnityEngine.Video;
using System.Collections.Generic;
using UnityEngine;

public class TimerForSplash : MonoBehaviour
{

    public GameObject go_Logo;
    public GameObject go_Video;
    public GameObject RawImage;
    VideoPlayer Video;

    // Start is called before the first frame update
    void Start()
    {
        Video = go_Video.GetComponent<UnityEngine.Video.VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.realtimeSinceStartup <= 10)
        {
            go_Logo.gameObject.SetActive(true);
            RawImage.gameObject.SetActive(false);
        }
        if (Time.realtimeSinceStartup >= 10)
        {
            RawImage.gameObject.SetActive(true);
            go_Logo.gameObject.SetActive(false);
            Video.Play();
        }
    }
}
