using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {


    private static AudioManager instance;
    /// <summary>
    /// Singleton design pattern
    /// </summary>
    public static AudioManager Instance
    {
        get
        {
            //If instance hasn't been assigned to anything 
            if (instance == null)
            {
                //If we found too many, it's bad
                if (FindObjectsOfType<AudioManager>().Length > 1)
                {
                    Debug.LogError("Too many singletons!");
                    return instance;
                }
                //Assign instance to the AudioManager we found
                instance = FindObjectOfType<AudioManager>();

                //If we didn't find one, make it
                if (instance == null)
                {
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<AudioManager>();
                    singleton.AddComponent<AudioSource>();
                    singleton.name = "AudioManager (Singleton)";
                    DontDestroyOnLoad(singleton);
                }

            }
            return instance;
        }
    }


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
