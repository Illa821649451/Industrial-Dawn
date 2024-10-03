using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 10f;
    public float gravity = 20f;
    public float rotationSpeed = 3f;
    public Transform cameraTransform;
    public Camera playerCamera;
    public float fovSpeed = 5f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;
    private float rotationY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        RotateChar();
        Stamina();
        MoveChar();
    }

    private void RotateChar()
    {
        rotationX += Input.GetAxis("Mouse X") * rotationSpeed;
        rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationY = Mathf.Clamp(rotationY, -90f, 80f);
        transform.localRotation = Quaternion.Euler(0f, rotationX, 0f);
        cameraTransform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);
    }
    private void Stamina()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 15f;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 65, fovSpeed * Time.deltaTime);
        }
        else
        {
            speed = 8f;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, 60, fovSpeed * Time.deltaTime);
        }
    }
    private void MoveChar()
    {
        if (controller.isGrounded)
        {
            // Стрибок
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Рух по горизонталі
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 horizontalMovement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        horizontalMovement = transform.TransformDirection(horizontalMovement);
        horizontalMovement *= speed;

        moveDirection.x = horizontalMovement.x;
        moveDirection.z = horizontalMovement.z;

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }

}

