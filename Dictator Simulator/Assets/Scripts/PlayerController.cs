using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //does not implement the new input system, will be updating later -cc
    public bool isMoving, canMove;
    private float xAxis, yAxis;
    public float moveSpeed;

    //Camera variables
    public float mouseSensitivity;
    float VertCameraRotate;
    float cameraAxisX, cameraAxisY;
    public Transform cameraTransform;
    void Start(){
        canMove = true;
    }
    void Update()
    {
        //Gets movement input
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        CheckMovement();
        if(canMove){
            Move();
        }
        //Gets mouse input
        cameraAxisX = Input.GetAxis("Mouse X") * mouseSensitivity;
        cameraAxisY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        CameraRotate();
    }
    void CheckMovement(){
        // Checks if player is moving
        if (xAxis != 0 || yAxis != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }
    public void Move(){
        Vector3 movement = new Vector3(xAxis, 0, yAxis);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
    void CameraRotate(){
        //Vertical rotation
        VertCameraRotate -= cameraAxisY;
        VertCameraRotate = Mathf.Clamp(VertCameraRotate, -90f, 90f);
        cameraTransform.localEulerAngles = Vector3.right * VertCameraRotate;
        //Horizontal rotation
        this.transform.Rotate(Vector3.up * cameraAxisX);
    }
}
