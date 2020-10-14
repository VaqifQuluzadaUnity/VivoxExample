using Cinemachine;
using UnityEngine;
using Photon.Pun;
public class PlayerMovementController : MonoBehaviourPunCallbacks//we inherit this class from MBPunCallbacks as we need to use IsMine variable in current script.
{
    [SerializeField] int MovementSpeed;         //MovementSpeed of player
    [SerializeField] CinemachineVirtualCamera CVR;      //For setting local player camera features(enable,disable cinemachine) we need this ref
    [SerializeField] Camera PlayerCamera;               //Rotation and local player camera features
    [SerializeField] GameObject PlayerGlass;            //The glass of player(we need to control layer for local players to see without seeing glass)
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)//If isnt local player
        {
            CVR.enabled = false;//we set cvr as it cant control our local player's camera.
            PlayerCamera.enabled = false;//same as cvr
            return;
        }
        PlayerGlass.layer = LayerMask.NameToLayer("Glass");     //Glass layer isnt visible by Camera.
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)//If local player
        {
            Move();
            Rotate();
        }
    }

    void Move()
    {
        float HorizontalAxis = Input.GetAxis("Horizontal");
        float VerticalAxis = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(HorizontalAxis, 0, VerticalAxis) * Time.deltaTime * MovementSpeed);
    }

    void Rotate()
    {
        float CameraYAngle = PlayerCamera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(new Vector3(0, CameraYAngle, 0));
    }

}
