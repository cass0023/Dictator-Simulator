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

    //interact variables
    public bool tvInteract;
    [SerializeField]private KeyCode interact;
    
    void Start(){
        canMove = true;
        tvInteract = false;
    }
    void Update()
    {
        //Gets movement input
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        //Gets mouse input
        cameraAxisX = Input.GetAxis("Mouse X") * mouseSensitivity;
        cameraAxisY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        CheckMovement();
        if(canMove){
            Move();
            CameraRotate();
        }
        CheckInput();
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
    
    //Triggers and Interact
    void CheckInput(){
        if(Input.GetKeyDown(interact) && tvInteract)
        {
             InteractionManager.Instance.SwitchCamera("StatCamera");
            canMove = false;
        }
		if (Input.GetKeyDown(KeyCode.Escape) && tvInteract)
		{
			InteractionManager.Instance.SwitchCamera("PlayerCam");
            canMove = true;
		}
	}
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.name == "StatScreenZone"){
            tvInteract = true;
        }
    }
    void OnTriggerExit(Collider collider){
		if (collider.gameObject.name == "StatScreenZone")
		{
			tvInteract = false;
		}
	}
}
