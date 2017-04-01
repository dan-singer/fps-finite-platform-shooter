using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBlock : Block {

    public float SpeedMultiplier = 1.2f;
    private float fasterSpeed, normalSpeed;

	// Use this for initialization
	void Start () {
        normalSpeed = GameObject.FindWithTag("Player").GetComponent<Player>().MoveSpeed;
        fasterSpeed = normalSpeed * SpeedMultiplier;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void CollideAction(Player player)
    {
        player.MoveSpeed = fasterSpeed;
        player.IsBoosting = true;
    }

    public override void CollideExitAction(Player player)
    {
    }
}
