using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ferra_InputManager : MonoBehaviour
{
    public float vr, hr;
    public bool handbrake;
    public bool boosting;

    private void Update()
    {
        hr = Input.GetAxis("Horizontal");
        vr = Input.GetAxis("Vertical");


        handbrake = (Input.GetAxis("Jump") != 0) ? true : false; // co nhan space hay ko

        if(Input.GetKey(KeyCode.LeftShift)) boosting = true;
        else boosting = false;
    }
}
