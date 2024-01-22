using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class SwitchCamPosition : MonoBehaviour
{
    public GameObject camPlayerFollow; // doi tuong ma third camera cua player follow
    [SerializeField] float speed;
    Vector3 target;
    [SerializeField] int index = 0;

    #region manualDrag
    // keo tha toa do pov de vao game camPlayerFollow chay den khi TAB va thirdcamera follow vao
    //[SerializeField] Transform[] povs;
    #endregion manualDrag

    [SerializeField] GameObject povPlayerParent; // folder cha chua cac pov con
    [SerializeField] Transform[] pov = new Transform[3];
    
    private void Start()
    {
        getObjects();
    }
    void getObjects()
    {
        camPlayerFollow = GameObject.Find("CamPlayerFollow");
        povPlayerParent = GameObject.Find("POVPlayerCam");
        pov[0] = povPlayerParent.transform.Find("POV0");
        pov[1] = povPlayerParent.transform.Find("POV1");
        pov[2] = povPlayerParent.transform.Find("POV2");
    }
    void Update()
    {
        target = pov[index].position;
    }
    private void FixedUpdate()
    {
        changePos();
        camPlayerFollow.transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        camPlayerFollow.transform.forward = pov[index].forward;
    }

    void changePos()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (index == 0) index = 1;
            else if (index == 1) index = 2;
            else if (index == 2) index = 0;
        }
    }

}
