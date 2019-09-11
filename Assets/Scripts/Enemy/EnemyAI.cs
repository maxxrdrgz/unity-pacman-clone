using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 0.3f;
    private Vector2 destination;
    private Vector2 nextDirection;
    private Rigidbody2D rigidbody;
    private Animator animator;
    private enum CurrentDirection {
        left,
        right,
        down,
        up
    }
    private CurrentDirection direction;
    private CurrentDirection[] directions = {
        CurrentDirection.left, 
        CurrentDirection.right, 
        CurrentDirection.up, 
        CurrentDirection.down
    };
    private bool canMove;
    
    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    /** 
        Upon start, this sets the initial position and direction for the ghost
    */
    void Start()
    {
        destination = GetFirstDestination();
        if(Random.Range(0,2) > 0){
            direction = CurrentDirection.left;
            DetermineNextDirection();
        }else{
            direction = CurrentDirection.right;
            DetermineNextDirection();
        }
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Animate();
    }

    /** 
        When this script is applied to a ghost gameobject, depending on the name
        this function will return the initial position.

        @return {Vector3} Returns a vector 3 containing the initial position
        for a particular ghost
    */
    Vector3 GetFirstDestination(){
        switch(gameObject.name){
            case "Blu":
                return new Vector3(13 ,20,0);
                break;
            case "Greeny":
                return new Vector3(15,20,0);
                break;
            case "Pinky":
                return new Vector3(14,20,0);
                break;
            default:
                return new Vector3(0,0,0);
                break;
        }
    }

    /** 
        This function will return a random value from the CurrentDirection enum

        @return {CurrentDirection} returns enum value for CurrentDirection
    */
    CurrentDirection GetRandDirection(){
        return directions[Random.Range(0, directions.Length)];
    }

    /** 
        Depending on the direction, this function will set the nextDirection
    */
    void DetermineNextDirection(){
        switch(direction){
            case CurrentDirection.left:
                nextDirection = Vector2.left;
                break;
            case CurrentDirection.right:
                nextDirection = Vector2.right;
                break;
            case CurrentDirection.up:
                nextDirection = Vector2.up;
                break;
            case CurrentDirection.down:
                nextDirection = Vector2.down;
                break;
        }
    }

    /** 
        Sets direction to a new direction. This changes the path of the ghosts.
    */
    void SetCurrentDirection(){
        canMove = false;
        CurrentDirection newDir = GetRandDirection();
        while(newDir == direction){
            newDir = GetRandDirection();
        }
        direction = newDir;
        DetermineNextDirection();
        canMove = true;
    }

    /** 
        Sets the destination by summing the gameobjects current position and
        the nextDirection
    */
    void SetDestination(){
        destination = (Vector2)transform.position + nextDirection;
    }

    /** 
        This function will use the current gameobjects position plus the given
        direction to detect if the ghost's nextdirection will hit a wall in the
        maze, returns true if so.

        @params {Vector 2} coordinates containing a 
        @return {bool} States if provided direction doesn't run into a wall
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
        By calculating the current direction, using the x and y, the animator
        params are set.
    */
    void Animate(){
        Vector2 dir = destination - (Vector2) transform.position;
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);
    }

    /** 
        Moves the gameobject from it's current position to the destination,
        otherwise a new destination is generated
    */
    void Move(){
        if((Vector2)transform.position != destination){
            Vector2 pos = Vector2.MoveTowards(transform.position, destination, speed);
            rigidbody.MovePosition(pos);
        }else{
            if(isValidDirection(nextDirection)){
                SetDestination();
            }else{
                SetCurrentDirection();
            }
        }
    }

    /** 
        Detects collision with the player. Destroys the player upon collision.

        @params {Collider2D} The other Collider2D involved in this collision.
    */
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            Destroy(other.gameObject);
        }
    }
}
