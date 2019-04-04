using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Valve.VR.InteractionSystem
{
    public class HandAnimation : MonoBehaviour
    {

        private Animator _anim;
        public SteamVR_Action_Boolean grabGripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

        // Use this for initialization
        void Start()
        {
            _anim = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_anim != null)
            {
                //if we are pressing grab, set animator bool IsGrabbing to true
                if (grabGripAction.GetState(SteamVR_Input_Sources.LeftHand))
                {
                    if (!_anim.GetBool("IsGrabbing"))
                    {
                        _anim.SetBool("IsGrabbing", true);
                    }
                }
                else
                {
                    //if we let go of grab, set IsGrabbing to false
                    if (_anim.GetBool("IsGrabbing"))
                    {
                        _anim.SetBool("IsGrabbing", false);
                    }
                }
                if (grabGripAction.GetState(SteamVR_Input_Sources.RightHand))
                {
                    if (!_anim.GetBool("IsGrabbing2"))
                    {
                        _anim.SetBool("IsGrabbing2", true);
                    }
                }
                else
                {
                    //if we let go of grab, set IsGrabbing to false
                    if (_anim.GetBool("IsGrabbing2"))
                    {
                        _anim.SetBool("IsGrabbing2", false);
                    }
                }

            }
        }
    }
}