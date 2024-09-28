using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 10;
    
    
    private PlayerControls controls;

    private Vector3 movementDirection;
    
    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
        
        controls.Move.MoveXAxis.performed += MoveXAxisOnperformed;
        controls.Move.MoveXAxis.canceled += MoveXAxisOncanceled;
        
        controls.Move.MoveYAxis.performed += MoveYAxisOnperformed;
        controls.Move.MoveYAxis.canceled += MoveYAxisOncanceled;
    }

    private void MoveYAxisOncanceled(InputAction.CallbackContext obj)
    {
        movementDirection.y = 0;
    }
    
    private void MoveXAxisOncanceled(InputAction.CallbackContext obj)
    {
        movementDirection.x = 0;
    }

    private void MoveYAxisOnperformed(InputAction.CallbackContext obj)
    {
        //print("YAxis " + obj.ReadValue<float>());
        movementDirection.y = obj.ReadValue<float>();
    }

    private void MoveXAxisOnperformed(InputAction.CallbackContext obj)
    {
        //print("XAxis " + obj.ReadValue<float>());
        movementDirection.x = obj.ReadValue<float>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = movementDirection * (Time.deltaTime * movementSpeed);
        //print(move);
        transform.Translate(move);
    }
}
