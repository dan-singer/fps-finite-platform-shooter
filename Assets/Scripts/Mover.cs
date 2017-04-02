using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves, rotates, or scales objects 
/// </summary>
public class Mover : MonoBehaviour {

    public Vector3 Velocity;
    public Vector3 RotationalVelocity;
    public Vector3 MaxScale;

    public float MinX, MaxX, MinY, MaxY;

    public bool generateRandomValues = true;

    private Vector3 startScale;
	// Use this for initialization
	void Start () {
        startScale = transform.localScale;

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

        //Keep in view
        Vector3 viewport = Camera.main.WorldToViewportPoint(transform.position);
        if (viewport.x > 1 || viewport.x < 0)
            Velocity.x *= -1;
        if (viewport.y < 0 || viewport.y > 1)
            Velocity.y *= -1;


        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //viewport.y = Mathf.Clamp01(viewport.y);
    }

    void OnTriggerEnter(Collider other)
    {
        Velocity *= -1;
    }
}
