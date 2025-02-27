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
    private bool canMoveMouse = true;
    public float mouseSensitivity;
    float VertCameraRotate;
    float cameraAxisX, cameraAxisY;
    public Transform cameraTransform;

    //interact variables
    private bool tvInteract, computerInteract, doorInteract;
    [SerializeField]private KeyCode interact;
    public GameObject newWeekPopUp;
    
    //switch scene after delay so camera switching works.
    bool openCom;
    float temp;
    float delay = 2;
    void Start(){
        rb = GetComponent<Rigidbody>();
        canMove = true;
        tvInteract = false;
        computerInteract = false;
        doorInteract = false;
        openCom = false;
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
        CheckInput();
        //adds delay to switching scene so camera transition works
        if(openCom){
            canMoveMouse = false;
            openCom = false;
            
        }
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
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            rb.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        if(Input.GetKeyDown(interact) && tvInteract)
        {
            InteractionManager.Instance.SwitchCamera("StatCamera");
            canMove = false;
        }
		if (Input.GetKeyDown(KeyCode.Escape) && tvInteract || Input.GetKeyDown(KeyCode.Escape) && computerInteract)
		{
			InteractionManager.Instance.SwitchCamera("PlayerCam");
            canMove = true;
		}
        if(Input.GetKeyDown(interact) && computerInteract){
            InteractionManager.Instance.SwitchCamera("ComCamera");
            openCom = true;
        }
        if(Input.GetKeyDown(interact) && doorInteract){
            canMoveMouse = false;
            Debug.Log("Door interacted");
            newWeekPopUp.SetActive(true);
            //enable ui that lets the player know they are about to end the week
        }
	}
    void StopMouseMovement(){
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
    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.name == "StatScreenZone"){
            tvInteract = true;
        }
        if(collider.gameObject.name == "ComputerScreenZone"){
            computerInteract = true;
        }
        if(collider.gameObject.name == "DoorInteractZone"){
            doorInteract = true;
        }
    }
    void OnTriggerExit(Collider collider){
		tvInteract = false;
        computerInteract = false;
        doorInteract = false;
        canMoveMouse = true;
	}
}
