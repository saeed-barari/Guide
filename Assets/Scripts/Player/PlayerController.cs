using Baldr;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Invisible Variables
    Rigidbody player_rb;
    public bool movementLock;
    public bool cameraLock;
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
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        transform.Translate(playerSpeed * Vector3.forward * Time.deltaTime * zMove);
        transform.Translate(playerSpeed * Vector3.right * Time.deltaTime * xMove);
    }

    void LookAround() {
        float vertMouse = -Input.GetAxis("Mouse Y")*cameraSensitivityMultiplier;
        float horizMouse = Input.GetAxis("Mouse X")*cameraSensitivityMultiplier;
        // TODO: Clamp the Vertical Rotation
        gameObject.transform.Rotate(0, horizMouse, 0);
        fpsCam.transform.Rotate(vertMouse, 0, 0);
    }

    [Button("Parameters")]
    void PrintPosition()
    {
        Debug.Log(transform.position);
    }
}
