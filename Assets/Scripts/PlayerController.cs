using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float movementSpeed = 10;
    public Transform spriteObject;
    
    [HideInInspector]
    public PlayerControls controls;

    private Vector3 movementDirection;

    public bool dindonPickedUp = false;
    public EventHandler OnDindonPickedUp;

    [SerializeField] private GameObject footstepSound;
    [SerializeField] private GameObject EIndicator;
    private Vector3 Esize;
    
    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
        
        controls.Move.MoveXAxis.performed += MoveXAxisOnperformed;
        controls.Move.MoveXAxis.canceled += MoveXAxisOncanceled;
        
        controls.Move.MoveYAxis.performed += MoveYAxisOnperformed;
        controls.Move.MoveYAxis.canceled += MoveYAxisOncanceled;

        footstepSound.SetActive(movementDirection != Vector3.zero);

        Esize = EIndicator.transform.localScale;

		instance = this;
    }

    public void PickupDindon()
    {
        OnDindonPickedUp?.Invoke(this, null);
        dindonPickedUp = true;
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
        if (movementDirection.x > 0)
			spriteObject.localScale = new Vector3(-1f, 1f, 1);
        else spriteObject.localScale = Vector3.one;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = movementDirection * (Time.deltaTime * movementSpeed);
        Vector3 finalPos = move + transform.position;

        //[0;1] screenspace
        Vector3 screenPos = Camera.main.WorldToScreenPoint(finalPos);
        screenPos.x /= Screen.width;
        screenPos.y /= Screen.height;
        
        if(screenPos.x > 1 || screenPos.x < 0 || screenPos.y > 1 || screenPos.y < 0) 
            return;

        transform.position = finalPos;

    }

    public void ShowE(bool show = true)
    {
        EIndicator.SetActive(show);
    }

    private IEnumerator twinckleAnimation = null;
    public void TwinkleE()
    {
        if(twinckleAnimation != null)
            StopCoroutine(twinckleAnimation);
        StartCoroutine(TwinkleRoutine());  
    }
    private IEnumerator TwinkleRoutine()
    {
        Vector3 targetSize = Esize * 1.2f;
        while (EIndicator.transform.localScale.x <= targetSize.x)
        {
            EIndicator.transform.localScale += Vector3.one * 4f * Time.deltaTime;
            yield return null;
		}
		while (EIndicator.transform.localScale.x >= Esize.x)
		{
			EIndicator.transform.localScale -= Vector3.one * 4f * Time.deltaTime;
			yield return null;
		}
        twinckleAnimation = null;
	}
}
