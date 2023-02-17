using UnityEngine;

public class DuckingState : PlayerState
{
    public override void enter(MovementScript thisObject)
    {
        thisObject.makeSmall(thisObject);
    }
    public override void handleInput(MovementScript thisObject)
    {
        // If the "S" key isn't being held, then attempt to stand up. If a ceiling is in the way, player will not stand.
        if (!(Input.GetKey(KeyCode.S)))
        {
            if (thisObject.canStand(thisObject))
            {
                thisObject.changeStates(thisObject, new GroundedState());
            }
        }

        // Player can jump while ducking, placing them in the duck-jump state. Compared to regular jump, they have a smaller hitbox and cannot stomp.
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && !(thisObject.isAirborne))
        {
            thisObject.changeStates(thisObject, new DuckJumpState());
        }
    }
    public override void exit(MovementScript thisObject)
    {
        thisObject.makeTall(thisObject);
    }
    public override void report(MovementScript thisObject)
    {
        Debug.Log("Player is currently Ducking");
    }
}