using UnityEngine;

public class WallSlideState : PlayerState
{
    public override void enter(MovementScript thisObject)
    {
        thisObject.wallJumped = false;
        thisObject.spriteRenderer.color = Color.magenta;
    }
    public override void handleInput(MovementScript thisObject)
    {
        // While sliding, prevent downward velocity from spiralling
        thisObject.rigidBody.velocity = new Vector2(thisObject.rigidBody.velocity.x, Mathf.Clamp(thisObject.rigidBody.velocity.y, -(thisObject.wallSlideSpeed), .0f));

        // "W" and "space" propel the player away from the wall
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            thisObject.wallJumped = true;
            if (thisObject.isAgainstRightWall(thisObject))
            {
                thisObject.rigidBody.AddForce(new Vector2(-(thisObject.jumpForce), thisObject.jumpForce));
            }
            else
            {
                thisObject.rigidBody.AddForce(new Vector2(thisObject.jumpForce, thisObject.jumpForce));
            }
        }

        // return to groundedstate if the player touches the ground
        if (thisObject.becameGrounded)
        {
            thisObject.changeStates(thisObject, new GroundedState());
        }

        // return to airbornestate if the player moves away from the wall
        if (!(thisObject.isTouchingWall))
        {
            if (thisObject.wallJumped)
            {
                thisObject.rigidBody.AddForce(new Vector2(0, thisObject.jumpForce));
            }
            thisObject.changeStates(thisObject, new AirborneState());
        }
    }
    public override void exit(MovementScript thisObject)
    {
        thisObject.wallJumped = false;
    }
    public override void report(MovementScript thisObject)
    {
        Debug.Log("Player is currently wall sliding");
    }
}