using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private int score;

    Text text;
    
    /* Schrodinger's Thumbs Method
     public private int void THUMBS(Thumbs t)
     {
        if schrodinger's finger : 
            printLine(Console.Write); 
        for
            if i then p
        quit

        TryCatch(int)" ";

        return (int)" ";
     }
    */
	// Use this for initialization
	void Start () {
        
        score = 0;	
	}
	
	// Update is called once per frame
	void Update () {

        text.text = ("Score: " + score);
	}
}
