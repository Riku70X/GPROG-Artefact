using UnityEngine;

public class AirborneState : PlayerState
{
    public override void enter(MovementScript thisObject)
    {
        thisObject.spriteRenderer.color = Color.cyan;
    }
    public override void handleInput(MovementScript thisObject)
    {
        thisObject.handleMovement(thisObject);

        // give the player a short period of time after running of a ledge to jump
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && thisObject.coyoteTime > 0)
        {
            thisObject.rigidBody.AddForce(new Vector2(0, thisObject.jumpForce));
            thisObject.coyoteTime = 0;
        }
        thisObject.coyoteTime -= Time.deltaTime;

        // use coins to perform a double jump
        if ((Input.GetKeyDown(KeyCode.W) && CoinManagerScript.CoinCounter >= 20))
        {
            thisObject.rigidBody.velocity = new Vector2(thisObject.rigidBody.velocity.x, 0);
            thisObject.rigidBody.AddForce(new Vector2(0, thisObject.jumpForce));
            CoinManagerScript.CoinCounter -= 20;
        }

        // return to groundedstate if the player touches the ground
        if (thisObject.becameGrounded)
        {
            thisObject.changeStates(thisObject, new GroundedState());
        }

        // perform a ground pound if the player presses S
        if (Input.GetKeyDown(KeyCode.S))
        {
            thisObject.changeStates(thisObject, new StompState());
        }

        // begin wallsliding if the player is touching a wall while falling
        if (thisObject.isTouchingWall && thisObject.rigidBody.velocity.y < 0)
        {
            thisObject.changeStates(thisObject, new WallSlideState());
        }
    }
    public override void report(MovementScript thisObject)
    {
        Debug.Log("Player is currently airborne");
    }
}