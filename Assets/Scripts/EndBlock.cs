using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBlock : Block {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void CollideAction(Player player)
    {
        GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>().CompleteLevel(); 
    }
}
