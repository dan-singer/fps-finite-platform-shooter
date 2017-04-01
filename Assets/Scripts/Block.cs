using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// What should happen when steps on this block?
    /// </summary>
    public virtual void CollideAction(Player player)
    {

    }

    /// <summary>
    /// What should happen when player leaves this block?
    /// </summary>
    /// <param name="player"></param>
    public virtual void CollideExitAction(Player player)
    {

    }


}
