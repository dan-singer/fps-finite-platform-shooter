using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBlock : Block {

    /// <summary>
    /// Velocity to Launch player
    /// </summary>
    public Vector3 LaunchVelocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void CollideAction(Player player)
    {
        player.velocity += LaunchVelocity * Time.deltaTime; 
    }
}
