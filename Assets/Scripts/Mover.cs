using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Moves, rotates, or scales objects 
/// Plus some other stuff
/// </summary>
public class Mover : MonoBehaviour {

    public Color startColor = Color.white;
    public Vector3 Velocity;
    public Vector3 RotationalVelocity;
    public Vector3 MaxScale;
    public AudioClip audClick;


    public bool generateRandomValues = true;
    public bool loadlevelOnPressed = true;
    private bool pressed = false;
    private float t = 0;
    private AudioSource aud;
	// Use this for initialization
	void Start () {
        aud = GetComponent<AudioSource>();
        GetComponent<Renderer>().sharedMaterial.color = startColor;
        if (generateRandomValues)
        {
            System.Random r = new System.Random();
            float mult = 10;

            Velocity = new Vector3((float)r.NextDouble() * mult, (float)r.NextDouble() * mult, 0);
            RotationalVelocity = new Vector3((float)r.NextDouble() * mult, (float)r.NextDouble() * mult, (float)r.NextDouble() * mult);
            MaxScale = new Vector3((float)r.NextDouble() * mult, (float)r.NextDouble() * mult, (float)r.NextDouble() * mult);

        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Velocity * Time.deltaTime);
        transform.Rotate(RotationalVelocity * Time.deltaTime);
        transform.localScale = new Vector3(Mathf.PingPong(Time.time, MaxScale.x), Mathf.PingPong(Time.time, MaxScale.y), Mathf.PingPong(Time.time, MaxScale.z));

        //Keep in viewport -- sort of works
        Vector3 viewport = Camera.main.WorldToViewportPoint(transform.position);
        if (viewport.x > 1 || viewport.x < 0)
            Velocity.x *= -1;
        if (viewport.y < 0 || viewport.y > 1)
            Velocity.y *= -1;

        //Lock Z position
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (pressed) //Start fading out
        {
            t += Time.deltaTime;

            Material m = GetComponent<Renderer>().sharedMaterial;
            m.color = Color.Lerp(Color.white, new Color(0, 0, 0, 0), t);

            if (t >= 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Velocity *= -1;
    }

    void OnMouseDown()
    {
        if (!pressed && loadlevelOnPressed)
        {
            pressed = true;
            aud.PlayOneShot(audClick);
            t = 0;
        }
    }
}
