using System.Security.Cryptography;
using UnityEngine;

public class DuckJumpState : PlayerState
{
    public override void enter(MovementScript thisObject)
    {
        thisObject.makeSmall(thisObject);
        thisObject.rigidBody.AddForce(new Vector2(0, thisObject.jumpForce));
        thisObject.becameGrounded = false;
    }
    public override void handleInput(MovementScript thisObject)
    {
        thisObject.handleMovement(thisObject);

        // return to ducking state if the player touches the ground. Player cannot release duck while airborne.
        if (thisObject.becameGrounded)
        {
            thisObject.changeStates(thisObject, new DuckingState());
        }
    }
    public override void exit(MovementScript thisObject)
    {
        thisObject.becameGrounded = false;
        thisObject.makeTall(thisObject);
    }
    public override void report(MovementScript thisObject)
    {
        Debug.Log("Player is currently DuckJumping");
    }
}