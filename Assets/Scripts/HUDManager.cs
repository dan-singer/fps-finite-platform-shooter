using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour {

    private int score;

    public Text scoreText;

    public Text blockType;

    public Text blockNum;

    public Text controls;

    Player p;

	// Use this for initialization
	void Start () {

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        blockType = GameObject.Find("BlockType").GetComponent<Text>();

        GameObject c = GameObject.Find("ControlsText");
        if (c)
            controls = GameObject.Find("ControlsText").GetComponent<Text>();

        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        score = 0;
	}
	
	// Update is called once per frame
	void Update () {

        scoreText.text = "Score: " + score;

        blockType.text = p.BlocksRemaining+ " " + p.CurrentBlock;

        if (controls)
        {
            if (SceneManager.GetActiveScene().name == "Level1")
                controls.text = "WASD to move. Space to Jump. Left click to place Block.";
            else
                controls.text = "Press Q or E to switch between blocks.\nUse the scroll wheel to change elevation of blocks.";
        }

    }
}
