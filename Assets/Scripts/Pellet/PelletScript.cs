using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /** 
        Detects if this gameobject (pellet) has collided with the player. If so
        this gameobject gets destroyed.

        @params {Collider2D} The other Collider2D involved in this collision.
    */
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            Destroy(gameObject);
        }
    }
}
