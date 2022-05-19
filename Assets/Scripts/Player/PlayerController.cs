using Baldr;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Invisible Variables
    Rigidbody player_rb;
    [HideInInspector]
    public bool movementLock;
    [HideInInspector]
    public bool cameraLock;
    float vertMouse = 0f;
    float horizMouse = 0f;
    //public bool lockMouse;

    [Group("Parameters")]
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float cameraSensitivityMultiplier = 1f;

    [Group("Assignables")]
    [SerializeField] private Camera fpsCam;

    
    // Start is called before the first frame update
    void Start()
    {
        player_rb = GetComponent<Rigidbody>();
        //lockMouse = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movementLock) {
            Move();
        }

        if (!cameraLock) {
            LookAround();
        }

        //if (lockMouse) {
        //    Cursor.lockState = CursorLockMode.Locked;
        //} else {
        //    Cursor.lockState = CursorLockMode.None;
        //}

    }

    void Move() {

        transform.Translate(Vector3.right * Input.GetAxisRaw("Horizontal") * playerSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * Input.GetAxisRaw("Vertical") * playerSpeed * Time.deltaTime);

    }

    void LookAround() {
        horizMouse += Input.GetAxis("Mouse X")*cameraSensitivityMultiplier;
        
        vertMouse = vertMouse - Input.GetAxis("Mouse Y")*cameraSensitivityMultiplier;
        
        vertMouse = Mathf.Clamp(vertMouse, -80f, 85f);
        // Debug.Log(vertMouse.ToString());
        gameObject.transform.eulerAngles = new Vector3(0, horizMouse, 0);
        fpsCam.transform.localEulerAngles = new Vector3(vertMouse, 0, 0);
    }

    [Button("Parameters")]
    void PrintPosition()
    {
        Debug.Log(transform.position);
    }
}
