using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public Animator Player;
    public bool m_bChangeScene;
    public bool m_bAnimationFinish;

    // Start is called before the first frame update
    void Start()
    {
        m_bAnimationFinish = false;
        m_bChangeScene = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown("space"))
        {
            m_bChangeScene = true;
        }
       if (m_bChangeScene == true)
        {
            Player.SetBool("KeySelected", true);
        }


        if (m_bAnimationFinish == true)
        {
            SceneChanger.GetInstance().SceneMain();
        }
        
    }
}
