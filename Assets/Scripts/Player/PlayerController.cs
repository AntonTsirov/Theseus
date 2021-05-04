using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: split into smaller classes
//Responsible for the movement look-around and running of the player

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 10.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    [HideInInspector]
    public bool isRunning;
    CharacterController characterController;
    public Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    [HideInInspector]
    public bool canMove = false;
    private bool dropFromSky = true;
    private static bool isTipShown0 = false;
    private static bool isTipShown1 = false;
    private static bool isTipShown5 = false;
    private bool once = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //lock player while falling from sky
        if (!canMove && dropFromSky && characterController.isGrounded)
        {
            canMove = true;
            dropFromSky = false;
            moveDirection.y = 0;
            if (!isTipShown0)
            {
                FindObjectOfType<TextBubble>().showTutorial(0);
                isTipShown0 = true;
            }
        }
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        if (GetComponent<RuninngStamina>().currentRunningStamina > 0 && Input.GetKey(KeyCode.LeftShift) && !FindObjectOfType<TextBubble>().isUIonFocus && canMove && GetComponent<Animator>().GetBool("isWalking"))
        {
            isRunning = true;
            if (!isTipShown1)
            {
                FindObjectOfType<TextBubble>().showTutorial(1);
                isTipShown1 = true;
            }
        }
        else isRunning = false;
        if (isRunning) GetComponent<Animator>().SetBool("isRunning", true);
        else GetComponent<Animator>().SetBool("isRunning", false);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        if (curSpeedX == 0 && curSpeedY == 0)
        {
            GetComponent<Animator>().SetBool("isWalking", false);
            if (canMove) StartCoroutine("hearExit");
        }
        else
        {
            GetComponent<Animator>().SetBool("isWalking", true);
            if (canMove)
            {
                StopCoroutine("hearExit");
                if (FindObjectOfType<Exit>().GetComponent<AudioSource>().isPlaying)
                {
                    if (!once) StartCoroutine("stopExitMusic");
                }

            }
        }
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetKeyDown(KeyCode.Space) && canMove && characterController.isGrounded && !FindObjectOfType<TextBubble>().isUIonFocus)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

    }

    public void placePlayer()
    {
        canMove = false;
        Transform randomPos = GameObject.FindObjectOfType<Maze>().freeCells[Random.Range(0, GameObject.FindObjectOfType<Maze>().freeCells.Count)].gameObject.transform;
        GameObject.FindObjectOfType<Maze>().freeCells.Remove(randomPos.GetComponent<MazeCellv2>());
        Vector3 temp = new Vector3(randomPos.position.x, randomPos.position.y + 100, randomPos.position.z);
        gameObject.transform.position = temp;//TODO: MAYBE CHANGE POSITION of code or way that is execited
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.transform.CompareTag("Spike") && hit.transform.parent.parent.parent.GetComponent<Animator>().GetBool("canHurt") && canMove)
        {
            GameObject.FindGameObjectWithTag("Player").transform.Find("EyesCanvas").GetComponent<Animator>().speed = 0.5f;
            GameObject.FindGameObjectWithTag("Player").transform.Find("EyesCanvas").GetComponent<Animator>().SetTrigger("goSleep");
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("isDead");
            canMove = false;
            if (!isTipShown5)
            {
                FindObjectOfType<TextBubble>().showTutorial(5);
                isTipShown5 = true;
            }
        }

    }

    IEnumerator hearExit()
    {
        if (!FindObjectOfType<Exit>().GetComponent<AudioSource>().isPlaying)
        {
            yield return new WaitForSeconds(2f);
            FindObjectOfType<Exit>().GetComponent<AudioSource>().Play();
        }

    }

    IEnumerator stopExitMusic()
    {
        once = true;
        if (FindObjectOfType<Exit>().GetComponent<AudioSource>().isPlaying)
        {
            yield return new WaitForSeconds(4f);
            FindObjectOfType<Exit>().GetComponent<AudioSource>().Stop();
            once = false;
        }

    }
}
