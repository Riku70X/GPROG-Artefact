using UnityEngine;

public class StompState : PlayerState
{
    public override void enter(MovementScript thisObject)
    {
        thisObject.makeSmall(thisObject);
        thisObject.rigidBody.velocity = Vector2.zero;
        thisObject.stompTimer = thisObject.stompPauseTime;
        thisObject.stompPosition = thisObject.transform.position;
    }
    public override void handleInput(MovementScript thisObject)
    {
        if (thisObject.stompTimer <= 0)
        {
            // After pausing, sets the y velocity to a large negative value
            thisObject.rigidBody.velocity = new Vector2(0, -(thisObject.stompSpeed));

            // reaching the ground reverts to grounded state
            if (thisObject.becameGrounded)
            {
                thisObject.changeStates(thisObject, new GroundedState());
            }
        }
        else
        {
            // Player pauses in the air for a short while
            thisObject.transform.position = thisObject.stompPosition;
            // Player can spend coins to dive out of a stomp
            if (Input.GetKey(KeyCode.A) && CoinManagerScript.CoinCounter > 3)
            {
                CoinManagerScript.CoinCounter -= 3;
                thisObject.rigidBody.velocity = Vector2.zero;
                thisObject.rigidBody.AddForce(new Vector2(-thisObject.diveSpeed, thisObject.jumpForce*.65f));
                thisObject.changeStates(thisObject, new AirborneState());
            } 
            else if (Input.GetKey(KeyCode.D) && CoinManagerScript.CoinCounter > 3)
            {
                CoinManagerScript.CoinCounter -= 3;
                thisObject.rigidBody.velocity = Vector2.zero;
                thisObject.rigidBody.AddForce(new Vector2(thisObject.diveSpeed, thisObject.jumpForce * .65f));
                thisObject.changeStates(thisObject, new AirborneState());
            }
            thisObject.stompTimer -= Time.deltaTime;
        }
    }
    public override void exit(MovementScript thisObject)
    {
        thisObject.makeTall(thisObject);
    }
    public override void report(MovementScript thisObject)
    {
        Debug.Log("Player is currently Stomping");
    }
}