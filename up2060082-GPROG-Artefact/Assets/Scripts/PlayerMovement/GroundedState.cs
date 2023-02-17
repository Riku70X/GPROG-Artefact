using UnityEngine;

public class GroundedState : PlayerState
{
    public override void enter(MovementScript thisObject)
    {
        thisObject.spriteRenderer.color = Color.white;
        thisObject.coyoteTime = 0.1f;
        thisObject.becameGrounded = false;
    }
    public override void handleInput(MovementScript thisObject)
    {
        thisObject.handleMovement(thisObject);

        // "W" and "space" propel the player upwards, if they are grounded
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            thisObject.rigidBody.AddForce(new Vector2(0, thisObject.jumpForce));
            thisObject.coyoteTime = 0;
        }

        // if the player ever jumps or walks of a ledge, mark them as airborne
        if (thisObject.isAirborne)
        {
            thisObject.changeStates(thisObject, new AirborneState());
        }

        // "S" makes the player duck
        if (Input.GetKey(KeyCode.S))
        {
            thisObject.changeStates(thisObject, new DuckingState());
        }
    }
    public override void report(MovementScript thisObject)
    {
        Debug.Log("Player is currently grounded");
    }
}