using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour {

    private float score;
    public string Score
    {
        get
        {
            return string.Format("{0:0}%", score);
        }
    }

    public Text scoreText;

    public Text blockType;

    public Text blockNum;

    public Text controls;

    private Player player;
    private LevelManager levelManager;
    private int totalBlocks;

    /// <summary>
    /// Scores for each percentage
    /// </summary>
    public static List<float> Scores = new List<float>();

    /// <summary>
    /// Get the last saved score. 0 if none
    /// </summary>
    private float PreviousScore {
        get
        {
            if (Scores.Count == 0)
                return 0;
            else
                return Scores[Scores.Count - 1];
        }
    }
   

	// Use this for initialization
	void Start () {

        if (GameObject.Find("ScoreText"))
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        if (GameObject.Find("BlockType"))
            blockType = GameObject.Find("BlockType").GetComponent<Text>();

        if (GameObject.Find("ControlsText"))
            controls = GameObject.Find("ControlsText").GetComponent<Text>();

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();

        totalBlocks = levelManager.TotalBlocks;
        UpdateScore();
        levelManager.CompleteLevelEvent += OnClearLevel;
	}

    // Update is called once per frame
    void Update() {

        UpdateScore();

        if (blockType)
            blockType.text = player.BlocksRemaining + " " + player.CurrentBlock;

        if (controls)
        {
            if (SceneManager.GetActiveScene().name == "Level1")
                controls.text = "WASD to move. Space to Jump. Left click to place Block.";
            else
                controls.text = "Press Q or E to switch between blocks.\nUse the scroll wheel to change elevation of blocks.";
        }

        if (scoreText)
        {
            if (SceneManager.GetActiveScene().name == "Credits")
            {
                scoreText.text = "Final Respect: " + Score;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                scoreText.text = "Respect: " + Score;
            }
        }



    }

    private void UpdateScore()
    {
        score = (((float)levelManager.BlocksLeft / totalBlocks) * 100.0f) + PreviousScore;
    }

    private void OnClearLevel(int level)
    {
        //If there's no score for this level
        if (Scores.Count == level)
        {
            Scores.Add(score);
        }
        else
        {
            float oldScore = Scores[level];
            if (score > oldScore)
                Scores[level] = score;
        }

    }

    public void Quit()
    {
        Application.Quit();
    }

    

    
}
