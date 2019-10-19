using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlock : Block {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void CollideAction(Player player)
    {
        player.velocity *= 0.5f;
        player.Gravity = Mathf.Abs(player.Gravity); 
    }
}
