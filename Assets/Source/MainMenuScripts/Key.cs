using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class Key : MonoBehaviour
    {
        ChangeScene change;
        public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
        private SteamVR_Input_Sources hand;
        private Interactable interactable;
       
        // Start is called before the first frame update
        void Start()
        {
            interactable = GetComponent<Interactable>();
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            change = Player.GetComponent<ChangeScene>();
        }

        // Update is called once per frame
        void Update()
        {
            if (interactable.attachedToHand)
            {
                if (grabPinchAction.GetState(SteamVR_Input_Sources.RightHand))
                {
                    change.changeScene = true;
                }
            }
        }
    }
}