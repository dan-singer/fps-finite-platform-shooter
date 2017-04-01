using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the level....what else do you want from me
/// </summary>
public class LevelManager : MonoBehaviour {



    /// <summary>
    /// List of quantites to be applied to all Blocks in player's inventory. 
    /// The Length MUST equal the length of the Player's inventory.
    /// </summary>
    public int[] BlockQuantities;

	// Use this for initialization
	void Start () {
        Player p = GameObject.FindWithTag("Player").GetComponent<Player>();

        //Exit if they don't match
#if UNITY_EDITOR
        if (BlockQuantities.Length != p.AvailableBlocks.Length)
        {
            Debug.LogError("Player's Available Block Length and LevelManager's BlockQuantites don't match in size!");
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
