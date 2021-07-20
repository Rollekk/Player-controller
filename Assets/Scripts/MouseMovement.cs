using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;
    public PlayerController playerController;

    float lookForward = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        lookForward -= mouseY;
        lookForward = Mathf.Clamp(lookForward, -90f, 90f);

        //Move camera up & down, rotates just camera
        if(playerController.wallSide == 1f) transform.localRotation = Quaternion.Euler(lookForward, 0f, 5f);
        else if(playerController.wallSide == -1f) transform.localRotation = Quaternion.Euler(lookForward, 0f, -5f);
        else if(playerController.wallSide == 0f) transform.localRotation = Quaternion.Euler(lookForward, 0f, 0f);
        //Move camera sideways, rotates whole player
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
