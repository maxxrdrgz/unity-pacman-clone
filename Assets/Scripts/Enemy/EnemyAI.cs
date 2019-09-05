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

    CurrentDirection GetRandDirection(){
        return directions[Random.Range(0, directions.Length)];
    }

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

    void SetDestination(){
        destination = (Vector2)transform.position + nextDirection;
    }

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

    void Animate(){
        Vector2 dir = destination - (Vector2) transform.position;
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);
    }

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

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            Destroy(other.gameObject);
        }
    }
}
