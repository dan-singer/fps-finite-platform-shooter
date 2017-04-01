using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlockUpper : Block
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void CollideAction(Player player)
    {
        //player.velocity = Vector3.zero;
        player.Gravity = -Mathf.Abs(player.Gravity);
    }

}
