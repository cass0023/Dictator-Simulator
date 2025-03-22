using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //player move variables
    public bool isMoving, canMove, isGrounded;
    private float xAxis, yAxis;
    public float moveSpeed, jumpForce;
    private Rigidbody rb;

    //Camera variables
    public bool canMoveMouse = true;
    public float mouseSensitivity;
    float VertCameraRotate;
    float cameraAxisX, cameraAxisY;
    public Transform cameraTransform;
    public AudioSource presidentWalk;

    void Start(){
        rb = GetComponent<Rigidbody>();
        canMove = true;
    }
    void Update()
    {
        //Gets movement input
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        //Gets mouse input
        if(canMoveMouse){
            cameraAxisX = Input.GetAxis("Mouse X") * mouseSensitivity;
            cameraAxisY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        }
        else if(!canMoveMouse){
            StopMouseMovement();
        }
        CheckMovement();
        if(canMove){
            Move();
            CameraRotate();
        }
        JumpInput();
    }

    //Movement and Camera
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
    public void JumpInput(){
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            rb.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
	}
    public void AllowMouseMovement(){
        canMoveMouse = true;
    }
    public void StopMouseMovement(){
        canMoveMouse = false;
        cameraAxisX = 0;
        cameraAxisY = 0;
    }
    void OnCollisionEnter(Collision collision){
        //checks if player can jump (booo i just dont wanna raycast)
        if(collision.gameObject.name == "Floor"){
            isGrounded = true;
        }
    }
    public void PlayerMovingSounds()
    {
        if (isMoving == true) //&& !presidentWalk.isPlaying) 
        { 
            presidentWalk.Play();
        }
        //else if (isMoving == false && presidentWalk.isPlaying)
        //{
        //    presidentWalk.Stop();
        //}

    }
}
