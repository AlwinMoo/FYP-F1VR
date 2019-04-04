using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        Physics.IgnoreCollision(collision.collider, this.GetComponent<Collider>());
    }
}
