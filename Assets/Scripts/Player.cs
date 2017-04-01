using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The main Player class
/// </summary>
public class Player : MonoBehaviour
{


    //Gravitational Acceleration
    public float Gravity;
    public float JumpYVelocity;

    //Speed to move and rotation
    public float MoveSpeed;
    public float RotationSpeed;

    //Clamping Angles
    public float MaxRotationAngle, MinRotationAngle;


    //List of available blocks that can be placed
    public Block[] AvailableBlocks;

    //Component references
    private CharacterController controller;
    private Camera camera;
    private Transform BlockPlacer;

    //Actual Vector used for movement
    public Vector3 velocity;
    [HideInInspector]
    public Vector3 rotation;
    [HideInInspector]
    public Vector3 acceleration;

    //Mouse stuff
    private float rotationX = 0, rotationY = 0;
    private Quaternion origRotation;

    //What block to place
    private int curBlockIndex = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        acceleration = Vector3.zero;
        origRotation = transform.localRotation;
        velocity = Vector3.zero;
        camera = transform.GetChild(0).GetComponent<Camera>();
        BlockPlacer = transform.GetChild(1);
        UpdatePlaceBlock();

    }

    /// <summary>
    /// Movement and Jumping
    /// </summary>
    private void Movement()
    {
        //For translation
        float horzMove = Input.GetAxis("Horizontal");
        float vertMove = Input.GetAxis("Vertical");

        //For rotation
        float camX = Input.GetAxis("Mouse X");
        float camY = Input.GetAxis("Mouse Y");


        //Movement
        Vector3 oldVel = velocity;

        velocity.x = horzMove;
        velocity.z = vertMove;
        velocity *= Time.deltaTime * MoveSpeed;
        velocity = transform.TransformDirection(velocity);

        velocity = new Vector3(velocity.x, oldVel.y, velocity.z);


        //Don't clamp X
        rotationX += camX * RotationSpeed * Time.deltaTime;
        Quaternion xQuat = Quaternion.AngleAxis(rotationX, Vector3.up);

        //Do clamp Y
        rotationY += camY * RotationSpeed * Time.deltaTime;
        rotationY = ClampAngle(rotationY, MinRotationAngle, MaxRotationAngle);
        Quaternion yQuat = Quaternion.AngleAxis(-rotationY, Vector3.right);

        //Construct new rotation
        transform.localRotation = origRotation * xQuat;
        camera.transform.localRotation = origRotation * yQuat;

        //Jumping
        if (controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = JumpYVelocity * Time.deltaTime;
            }
        }

        //Gravitational Acceleration
        velocity.y += Gravity * Time.deltaTime;

        //Actually move and stuff
        controller.Move(velocity);  
    }
    

    /// <summary>
    /// Logic for placing blocks
    /// </summary>
    private void BlockPlacement()
    {
        /*
         *Left click to drop block 
         Scroll wheel changes y position
         Switch block with Q and E
         */

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        //Switch Blocks
        if (Input.GetButtonDown("Next"))
            curBlockIndex++;
        if (Input.GetButtonDown("Prev"))
            curBlockIndex--;

        if (curBlockIndex >= AvailableBlocks.Length)
            curBlockIndex = 0;
        if (curBlockIndex < 0)
            curBlockIndex = AvailableBlocks.Length - 1;



        if (Input.GetButtonDown("Next") || Input.GetButtonDown("Prev"))
        {
            UpdatePlaceBlock();
        }

        //Modify BlockPlacer height
        BlockPlacer.transform.position = new Vector3(BlockPlacer.position.x, BlockPlacer.position.y + scroll, BlockPlacer.position.z);



        if (Input.GetButtonDown("Fire1"))
        {
            LevelManager l = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();

            if (l.BlockQuantities[curBlockIndex] > 0)
            {
                Block b = Instantiate<Block>(AvailableBlocks[curBlockIndex], BlockPlacer.position, BlockPlacer.rotation);
                l.BlockQuantities[curBlockIndex]--;

            }


        }



        }
    
    private void OnTriggerEnter(Collider other)
    {
        //Discard collisions with self
        if (other.name == name)
            return;
        if (other.GetComponent<Block>())
        {
            other.GetComponent<Block>().CollideAction(this);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        //Discard collisions with self
        if (other.name == name)
            return;
        if (other.GetComponent<Block>())
        {
            other.GetComponent<Block>().CollideExitAction(this);
        }
    }

    private void UpdatePlaceBlock()
    {
        //Assign appropriate material and mesh
        BlockPlacer.GetComponent<Renderer>().sharedMaterial = AvailableBlocks[curBlockIndex].GetComponent<Renderer>().sharedMaterial;
        BlockPlacer.GetComponent<MeshFilter>().sharedMesh = AvailableBlocks[curBlockIndex].GetComponent<MeshFilter>().sharedMesh;
    }

    private void Update()
    {
        Movement();
        BlockPlacement();


    }

    /// <summary>
    /// Clamps an angle. Thanks to http://answers.unity3d.com/questions/29741/mouse-look-script.html
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle <= -360F)
         angle += 360F;
        if (angle >= 360F)
         angle -= 360F;


        return Mathf.Clamp(angle, min, max);
    }
}

