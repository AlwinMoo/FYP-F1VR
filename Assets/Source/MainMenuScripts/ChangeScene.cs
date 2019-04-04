using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public Animator Player;
    public bool changeScene;
    public bool animationFinish;

    // Start is called before the first frame update
    void Start()
    {
        animationFinish = false;
        changeScene = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown("space"))
        {
            changeScene = true;
        }
       if (changeScene == true)
        {
            Player.SetBool("KeySelected", true);
        }


        if (animationFinish == true)
        {
            SceneChanger.GetInstance().SceneMain();
        }
        
    }
}
