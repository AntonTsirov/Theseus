using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    [HideInInspector]
    public bool canMove;
    private bool dropFromSky = true;

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
        }
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning) GetComponent<Animator>().SetBool("isRunning", true);
        else GetComponent<Animator>().SetBool("isRunning", false);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        if (curSpeedX == 0 && curSpeedY == 0) GetComponent<Animator>().SetBool("isWalking", false);
        else GetComponent<Animator>().SetBool("isWalking", true);
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (characterController.isGrounded) moveDirection.y = 0;

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
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
        Transform randomPos = GameObject.FindObjectOfType<Maze>().cells[Random.Range(0, GameObject.FindObjectOfType<Maze>().size.x), Random.Range(0, GameObject.FindObjectOfType<Maze>().size.z)].gameObject.transform;
        Vector3 temp = new Vector3(randomPos.position.x, randomPos.position.y + 100, randomPos.position.z);
        gameObject.transform.position = temp;//TODO: MAYBE CHANGE POSITION of code or way that is execited
        canMove = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Spike") && hit.transform.parent.parent.parent.GetComponent<Animator>().GetBool("canHurt"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("isDead");
            canMove = false;
        }
    }
}
