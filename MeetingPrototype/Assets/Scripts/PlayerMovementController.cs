using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region Serialized Private Variables

    [SerializeField] private int MovementSpeed;
    [SerializeField] Camera PlayerCamera;

    #endregion

    #region MonoBehaviour Callbacks
    private void Update()
    {
        Rotate();
        Move();
    }
    #endregion


    #region Private methods
    void Move()
    {
        float HorizontalAxis = Input.GetAxis("Horizontal");
        float VerticalAxis = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(HorizontalAxis,0, VerticalAxis) * Time.deltaTime * MovementSpeed);
    }


    void Rotate()
    {
        float CameraAxis = PlayerCamera.transform.eulerAngles.y ;

        transform.rotation = Quaternion.Euler(new Vector3(0, CameraAxis, 0));
    }
    #endregion
}
