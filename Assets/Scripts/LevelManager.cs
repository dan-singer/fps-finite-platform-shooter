using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Manages the level....what else do you want from me
/// </summary>
public class LevelManager : MonoBehaviour {

    public Color DeathLightColor;
    public Color DeathCamColor;
    public Color VictoryLightColor;
    public Color VictoryCamColor;

    private Color normalLightColor;
    private Color normalCamColor;



    /// <summary>
    /// List of quantites to be applied to all Blocks in player's inventory. 
    /// The Length MUST equal the length of the Player's inventory.
    /// </summary>
    public int[] BlockQuantities;
    //quantities at the start
    private int[] originalQuantities; 


    private Player player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        normalLightColor = RenderSettings.ambientSkyColor;
        normalCamColor = Camera.main.backgroundColor;

        originalQuantities = new int[BlockQuantities.Length];
        //Store original quantity values so they can be reset later
        BlockQuantities.CopyTo(originalQuantities, 0);
        

        //Exit if they don't match
#if UNITY_EDITOR
        if (BlockQuantities.Length != player.AvailableBlocks.Length)
        {
            Debug.LogError("Player's Available Block Length and LevelManager's BlockQuantites don't match in size!");
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Respawns player in the level, clears all blocks created by Player
    /// </summary>
    public void Respawn()
    {
        RenderSettings.ambientSkyColor = DeathLightColor;
        Camera.main.backgroundColor = DeathCamColor;
        player.State = PlayerState.Frozen;
        StartCoroutine(ERespawn());
    }
    /// <summary>
    /// Completes the level. Cool color effect, then load next level.
    /// </summary>
    public void CompleteLevel()
    {
        RenderSettings.ambientSkyColor = VictoryLightColor;
        Camera.main.backgroundColor = VictoryCamColor;
        player.State = PlayerState.Frozen;
        StartCoroutine(ECompleteLevel());
    }

    private IEnumerator ERespawn()
    {
        yield return new WaitForSecondsRealtime(1f);
        //Reset Quantities array
        originalQuantities.CopyTo(BlockQuantities, 0);

        //Reset colors
        RenderSettings.ambientSkyColor = normalLightColor;
        Camera.main.backgroundColor = normalCamColor;
        //Reset Player
        player.RemovePlacedObjects();
        player.transform.position = player.StartPosition;
        player.transform.rotation = player.StartRotation;
        player.State = PlayerState.Controllable;

    }

    private IEnumerator ECompleteLevel()
    {
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
