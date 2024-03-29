﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlock : Block {


    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void CollideAction(Player player)
    {
        if (animator)
            animator.SetTrigger("Activate");
        GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>().Respawn(); 

    }
}
