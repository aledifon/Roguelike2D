using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingCharacters : MonoBehaviour
{
    public float speed;                 // Used to move characters with MoveTowards
    public LayerMask blockingLayer;     // Layer to check during the collision

    private BoxCollider2D boxCollider;
    private Rigidbody rb2D;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        
    }

    // Returns true --> the character can move
    // Returns false --> the caracter can't move
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        // To avoid the raycast hits with the character's collider
        boxCollider.enabled = false;

        // Launch a selective LineCast
        hit = Physics2D.Linecast(start, end, blockingLayer);

        // Re enable the character's collider
        boxCollider.enabled = true;

        // As long as the character does not detect any collider then we 
        // start the player movement
        if(hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        else
            return false;
    }

    // Coroutine used for character's movement
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        // Distance calculation to reach the target point
        // sqrMagnitude is more efficient than Magnitude as it does not perform the square root
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while(sqrRemainingDistance > float.Epsilon)
        {
            // Update Character's new position
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position,
                                                    end,
                                                    speed * Time.deltaTime);
            rb2D.MovePosition(newPosition);

            // Update the Remaining Distance to reach the target pos.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        // Assures the character reaches the target position
        rb2D.MovePosition(end);
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T: Component
    {
        RaycastHit2D hit;

        bool canMove = Move(xDir, yDir, out hit);

        // If the character can move we finish the method
        if (hit.transform == null)
            return;

        // Otherwise we get the component of the GO which the RayCast has hitted
        T hitComponent = hit.transform.GetComponent<T>();

        if(!canMove && hit.transform != null)        
            OnCantMove(hitComponent);        
    }

    // Abstract Method (must be implemented on the class that inherits this class)    
    // <T> tells the compiler that we'll pass generic type 'T' as parameter
    protected abstract void OnCantMove<T>(T component)
        where T : Component;                                // Stablish a restriction to the generic type (T)
                                                            // The T type must be a class that drives from UnityEngine.Component
}
