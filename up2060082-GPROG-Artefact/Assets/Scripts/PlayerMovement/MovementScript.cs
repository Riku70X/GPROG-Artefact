using UnityEngine;

public class PlayerState
{
    // base class which allows all states to have a function for input, a function for debugging, an enter function, and an exit function
    public virtual void handleInput(MovementScript thisObject) { }
    public virtual void report(MovementScript thisObject) { }
    public virtual void enter(MovementScript thisObject) { }
    public virtual void exit(MovementScript thisObject) { }
} // general state machine format taken from the gamestates moodle task

public class MovementScript : MonoBehaviour
{
    public PlayerState currentState;

    // Can be changed in the editor to adjust game feel
    public float speed = 2.5f;
    public float jumpForce = 700.0f;
    public float maxSpeed = 8.0f;
    public float stompSpeed = 25.0f;
    public float stompPauseTime = 0.3f;
    public float wallSlideSpeed = 3.0f;
    public float coyoteTime = 0.1f;
    public float diveSpeed = 1000.0f;

    // Should only be changed by the game itself
    public bool becameGrounded = false;
    public bool isAirborne = false;
    public bool isTouchingWall = false;
    public bool wallJumped = false;
    public float stompTimer = 0.0f;
    public Vector2 stompPosition;

    // Components for the player
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidBody;

    // Reference to coin manager object
    public GameObject CoinManager;

    // Functions
    public void changeStates(MovementScript thisObject, PlayerState newState)
    {
        thisObject.currentState.exit(thisObject);
        thisObject.currentState = newState;
        thisObject.currentState.enter(thisObject);
    }
    public void makeSmall(MovementScript thisObject) // Shrinks the hitbox and sprite, and turns the sprite red
    {
        thisObject.transform.localScale = new Vector3(1, 0.5f, 1);
        thisObject.transform.position = new Vector3(thisObject.transform.position.x, thisObject.transform.position.y - 0.5f, 0);
        thisObject.spriteRenderer.color = Color.red;
    }

    public void makeTall(MovementScript thisObject) // Enlarges the hitbox and sprite, and turns the sprite white
    {
        thisObject.transform.position = new Vector3(thisObject.transform.position.x, thisObject.transform.position.y + 0.5f, 0);
        thisObject.transform.localScale = Vector3.one;
        thisObject.spriteRenderer.color = Color.white;
    }

    public void handleMovement(MovementScript thisObject)
    {
        // "A" and "D" send the player to the left/right
        if (Input.GetKey(KeyCode.A))
        {
            thisObject.rigidBody.AddForce(new Vector2(-(thisObject.speed * Mathf.Cos((float)(thisObject.rigidBody.velocity.x * 0.196))), 0.0f));
        }
        if (Input.GetKey(KeyCode.D))
        {
            thisObject.rigidBody.AddForce(new Vector2(thisObject.speed * Mathf.Cos((float)(thisObject.rigidBody.velocity.x * 0.196)), 0.0f));
        }

        // Ensure the speed never exceeds the maxSpeed
        thisObject.rigidBody.velocity = new Vector2(Mathf.Clamp(thisObject.rigidBody.velocity.x, -(thisObject.maxSpeed), thisObject.maxSpeed), thisObject.rigidBody.velocity.y);
    }

    public bool canStand(MovementScript thisObject) // Check 1 unit above the player to see if there is an object
    {
        Vector2 playerTop1 = new Vector2(thisObject.transform.position.x - 0.501f, thisObject.transform.position.y + 0.501f);
        RaycastHit2D ray1 = Physics2D.Raycast(playerTop1, thisObject.transform.up, 1);
        Vector2 playerTop2 = new Vector2(thisObject.transform.position.x + 0.501f, thisObject.transform.position.y + 0.501f);
        RaycastHit2D ray2 = Physics2D.Raycast(playerTop2, thisObject.transform.up, 1);
        if (ray1.collider == null && ray2.collider == null) return true;
        else return false;
    }

    public bool isAgainstRightWall(MovementScript thisObject) // Check .1 units to the right of the player to see if there is an object
    {
        Vector2 playerRight = new Vector2(thisObject.transform.position.x + .501f, thisObject.transform.position.y);
        RaycastHit2D ray = Physics2D.Raycast(playerRight, thisObject.transform.right, .1f);
        if (ray.collider == null) return false;
        else return true;
    }

    // Player constantly checks to see if it can perform certain moves, like jumping or walljumping
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor") { becameGrounded = true; isAirborne = false; }
        if (collision.gameObject.tag == "Wall") isTouchingWall = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor") { becameGrounded = false; isAirborne = false;  }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor") { becameGrounded = false; isAirborne = true;  }
        if (collision.gameObject.tag == "Wall") isTouchingWall = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = new GroundedState();
        InvokeRepeating("Report", 0.0f, 3.0f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.handleInput(this);
    }

    // Report is called once every 3 seconds
    void Report()
    {
        currentState.report(this);
    }
}