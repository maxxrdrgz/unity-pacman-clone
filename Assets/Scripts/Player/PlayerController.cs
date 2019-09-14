using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.2f;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private Vector2 destination = Vector2.zero;
    private Vector2 direction = Vector2.zero;
    private Vector2 nextDirection = Vector2.zero;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Animate();
    }

    /** 
        Given the direction, using the players position, this function will
        cast a line to detect if the next direction will hit a wall of the maze,
        if so returns false.

        @params {Vector2} direction the player wants to head in
        @return {bool} boolean stating if the player can move in that direction
    */
    bool isValidDirection(Vector2 dir){
        Vector2 pos = transform.position;

        dir += new Vector2(dir.x * 0.45f, dir.y * 0.45f);
        RaycastHit2D hit = Physics2D.Linecast(pos+dir, pos);
        if(hit.collider.tag == "Maze"){
            return false;
        }else{
            return true;
        }
    }

    /** 
        This function will change the animator parameters based on the direction
        the player is heading in. By doing so, this will change the animations
        of the player sprite.
    */
    void Animate(){
        Vector2 dir = destination - (Vector2) transform.position;
        print(dir);
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);
    }

    /** 
        This function moves the players rigidbody, and get's input from the user
        This function also checks to see if the next deriection is valid as it
        get's closer to original distance.
    */
    void Move(){
        Vector2 p = Vector2.MoveTowards(transform.position, destination, speed);

        rigidbody.MovePosition(p);
        if(Input.GetAxis("Horizontal") > 0){
            nextDirection = Vector2.right;
        }
        if(Input.GetAxis("Horizontal") < 0){
            nextDirection = Vector2.left;
        }
        if(Input.GetAxis("Vertical") < 0){
            nextDirection = Vector2.down;
        }
        if(Input.GetAxis("Vertical") > 0){
            nextDirection = Vector2.up;
        }
        //destination = (Vector2)transform.position + nextDirection;
        //direction = nextDirection;
        if(Vector2.Distance(destination, transform.position) < 0.0001f){
            if(isValidDirection(nextDirection)){
                destination = (Vector2)transform.position + nextDirection;
                direction = nextDirection;
            } else if(isValidDirection(direction)) {
                destination = (Vector2)transform.position + direction;
            }
        }
    }
}
