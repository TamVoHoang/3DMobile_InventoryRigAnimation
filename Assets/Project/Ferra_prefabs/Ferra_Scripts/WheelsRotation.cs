using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class WheelsRotation : MonoBehaviour
{
    Ferra_InputManager im;
    public Vector3 targetAngleRight = new Vector3(0f, 0, -70f);
    public Vector3 targetAngleLeft = new Vector3(0f, 0, 70f);

    public Vector3 currentAngle;
    [Range(0, 10)] public float smoothBoost = 1;


    //bool turnR = false, turnL = false;
    public float hr;

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponentInParent<Ferra_InputManager>();
        currentAngle = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //hr = Input.GetAxis("Horizontal");
        hr = im.hr;

        if (hr > 0) // qua phai
        {
            currentAngle = new Vector3(0, currentAngle.y, -(Mathf.LerpAngle(currentAngle.z, targetAngleRight.z, hr * smoothBoost)));
            transform.localEulerAngles = currentAngle;

        }

        if (hr < 0f) // qua trai
        {
            //Debug.Log("run");
            currentAngle = new Vector3(0, currentAngle.y, -(Mathf.LerpAngle(currentAngle.z, targetAngleLeft.z, -hr * smoothBoost)));
            transform.localEulerAngles = currentAngle;
        }
    }
}
