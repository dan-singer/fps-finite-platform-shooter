using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


class Player : MonoBehaviour
{
    //Gravitational Acceleration
    public Vector3 Gravity;

    //Speed to move and rotation
    public float MoveSpeed;
    public float RotationSpeed;


    private CharacterController controller;
    private Camera camera;

    //Actual Vector used for movement
    private Vector3 movement;
    private Vector3 rotation;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        //For translation
        float horzMove = Input.GetAxis("Horizontal");
        float vertMove = Input.GetAxis("Vertical");

        //For rotation
        float camX = Input.GetAxis("Mouse X");
        float camY = Input.GetAxis("Mouse Y");

        //Movement
        movement = new Vector3(horzMove, 0, vertMove) * Time.deltaTime * MoveSpeed;
        movement = transform.TransformDirection(movement);

        //Camera rotation
        rotation = new Vector3(-camY, camX, 0) * Time.deltaTime * RotationSpeed;

        controller.Move(movement);
        transform.eulerAngles += rotation;
        
        
    }

}

