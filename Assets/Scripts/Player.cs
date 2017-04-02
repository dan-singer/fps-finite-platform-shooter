using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PlayerState
{
    Controllable,
    NotControllable,
    Frozen
}

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

    //What block to place
    private int curBlockIndex = 0;

    //Speed on Start of level
    private float originalSpeed;
    //Are we boosting right now? 
    public bool IsBoosting = false;

    public float friction;
    private float originalFriction;

    public Vector3 StartPosition;
    public Quaternion StartRotation;

    public PlayerState State = PlayerState.Controllable;

    private List<GameObject> playerPlacedObjects;

    private LevelManager levelManager;
    public Color transparentColor;

    //Audio
    private AudioSource aud;
    public AudioClip audNext, audPrev, audDrop, audBad;

    /// <summary>
    /// Access the current block
    /// </summary>
    public string CurrentBlock
    {
        get
        {
            string suffix = "s";
            if (BlocksRemaining == 1)
                suffix = "";
            Block b = AvailableBlocks[curBlockIndex];
            if (b is BoostBlock)
                return "Boost Block" + suffix;
            else if (b is JumpBlock)
                return "Jump Block" + suffix;
            else if (b is Block)
                if (b.transform.childCount == 0)
                    return "Block" + suffix;
                else if (b.transform.childCount == 2)
                    if (b.transform.GetChild(1).GetComponent<Block>() is FanBlock)
                        return "Fan Block" + suffix;
                    else
                        return "Block" + suffix;
                else
                    return "Block" + suffix;
            else
                return "???";
        }
    }

    /// <summary>
    /// Get the amount of blocks remaining for the current block type
    /// </summary>
    public int BlocksRemaining
    {
        get
        {
            return levelManager.BlockQuantities[curBlockIndex];
        }
    }
    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        controller = GetComponent<CharacterController>();
        aud = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        camera = transform.GetChild(0).GetComponent<Camera>();
        BlockPlacer = transform.GetChild(1);
        originalSpeed = MoveSpeed;
        originalFriction = friction;
        UpdatePlaceBlock();
        playerPlacedObjects = new List<GameObject>();
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();

        //Set Start Position and rotation
        if (StartPosition == Vector3.zero)
        {
            StartPosition = transform.position;
        }
        StartRotation = transform.localRotation;

    }

    /// <summary>
    /// Remove all objects that this Player placed.
    /// </summary>
    public void RemovePlacedObjects()
    {
        foreach (GameObject g in playerPlacedObjects)
        {
            Destroy(g);
        }
    }

    /// <summary>
    /// Movement and Jumping
    /// </summary>
    /// <param name="withInput">Consider player input</param>
    private void Movement(bool withInput)
    {
        //For translation
        float horzMove = Input.GetAxis("Horizontal");
        float vertMove = Input.GetAxis("Vertical");

        //For rotation
        float camX = Input.GetAxis("Mouse X");
        float camY = Input.GetAxis("Mouse Y");


        //Lock controls if necessary
        if (State == PlayerState.NotControllable)
        {
            horzMove = 0;
            vertMove = 0;
        }

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
        transform.localRotation = StartRotation * xQuat;
        camera.transform.localRotation = StartRotation * yQuat;

        //Jumping
        if (controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = JumpYVelocity * Time.deltaTime;
                
            }
        }
        else
        {
            //Gravitational Acceleration
            velocity.y += Gravity * Time.deltaTime;
        }


        //For ice movement, lower friction
        float tempY = velocity.y;
        velocity = Vector3.Lerp(oldVel, velocity, Time.deltaTime * friction);
        velocity.y = tempY;
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
        {
            curBlockIndex++;
            aud.PlayOneShot(audNext);
        }
        if (Input.GetButtonDown("Prev"))
        {
            curBlockIndex--;
            aud.PlayOneShot(audPrev);
        }

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

        //Switch between material color if can't use block
        if (levelManager.BlockQuantities[curBlockIndex] <= 0)
        {
            BlockPlacer.GetComponent<Renderer>().sharedMaterial.color = Color.black;
        }
        else
        {
            BlockPlacer.GetComponent<Renderer>().sharedMaterial.color = transparentColor;

        }

        if (Input.GetButtonDown("Fire1"))
        {

            if (levelManager.BlockQuantities[curBlockIndex] > 0)
            {
                Block b = Instantiate<Block>(AvailableBlocks[curBlockIndex], BlockPlacer.position, BlockPlacer.rotation);
                playerPlacedObjects.Add(b.gameObject); //Keep track of placed objects
                levelManager.BlockQuantities[curBlockIndex]--;
                aud.PlayOneShot(audDrop);
            }
            else
            {
                aud.PlayOneShot(audBad);
            }
        }



        }
    
    private void OnTriggerEnter(Collider other)
    {
        //Discard collisions with self
        if (other.name == name)
            return;

        if (!other.isTrigger)
            velocity.y = 0;
        //Polymorphism to the rescue for CollideActions
        if (other.GetComponent<Block>())
        {
            other.GetComponent<Block>().CollideAction(this);
        }

        //When we step on anything that's not a boost block, reset speed to original value
        if (other.GetComponent<BoostBlock>() == null && other.GetComponent<JumpBlock>() == null && !other.isTrigger)
        {
            MoveSpeed = originalSpeed;
        }
        //When we step on anything that's not an ice block, reset friction to original value
        if (other.GetComponent<IceBlock>() == null)
        {
            friction = originalFriction;
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

    /// <summary>
    /// Update the placed blocks matierla, mesh, scale, and rotation
    /// </summary>
    private void UpdatePlaceBlock()
    {
        //Copy over necessary material properties
        BlockPlacer.GetComponent<Renderer>().sharedMaterial.mainTexture = AvailableBlocks[curBlockIndex].GetComponent<Renderer>().sharedMaterial.mainTexture;
        BlockPlacer.GetComponent<Renderer>().sharedMaterial.mainTextureScale = AvailableBlocks[curBlockIndex].GetComponent<Renderer>().sharedMaterial.mainTextureScale;
        //Change mesh
        BlockPlacer.GetComponent<MeshFilter>().sharedMesh = AvailableBlocks[curBlockIndex].GetComponent<MeshFilter>().sharedMesh;
        //Change scale
        BlockPlacer.transform.localScale = AvailableBlocks[curBlockIndex].transform.localScale;

    }

    private void Update()
    {
        switch (State)
        {
            case PlayerState.Controllable:
                Movement(true);
                BlockPlacement();
                break;
            case PlayerState.NotControllable:
                Movement(false); 
                break;
            case PlayerState.Frozen:
                break;
        }
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

