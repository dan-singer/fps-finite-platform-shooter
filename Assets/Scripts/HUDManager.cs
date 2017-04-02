using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private int score;

    public Text scoreText;

    public Text blockType;

    Player p;

	// Use this for initialization
	void Start () {

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        blockType = GameObject.Find("BlockType").GetComponent<Text>();

        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        score = 0;
	}
	
	// Update is called once per frame
	void Update () {

        scoreText.text = "Score: " + score;

        blockType.text = "Block type: " + p.CurrentBlock;
	}
}
