using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : Block {

    /// <summary>
    /// Value to multiply friction by to make everything wobbly
    /// </summary>
    public float frictionMultiplier;
    private float newFriction, oldFriction;

	// Use this for initialization
	void Start () {
        oldFriction = GameObject.FindWithTag("Player").GetComponent<Player>().friction;
        newFriction = oldFriction * frictionMultiplier;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void CollideAction(Player player)
    {
        player.friction = newFriction;
    }

}
