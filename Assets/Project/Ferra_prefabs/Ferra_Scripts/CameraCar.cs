using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCar : MonoBehaviour
{
    public Camera cam; // camera gan tren xe

    Ferra_InputManager im;
    Controller_gearSystem cc;
    [SerializeField]
    Transform[] povs; // vi tri
    [SerializeField] float povsSpeedTranform = 100f;
    int index;
    Vector3 target;

    #region FOV
    public float defaltFOV = 0; // fov mac dinh cua cam
    public float FOVBoost = 70f;
    [Range(5,40)] public float timeBoost = 10;

    public float FOVForward = 65f;
    [Range(5, 40)] public float timeForWard = 30;

    public float FOVBackward =40f;
    [Range(5, 20)] public float timeBackWard = 8;
    
    public float FOVGear = 40f;
    [Range(5, 20)] public float TimeGear = 10f;
    #endregion FOV

    private void Awake()
    {
        //cam.enabled = true; // NEU CO NGUOI NGOI LEN THI TAT CAMERA NAY
        defaltFOV = cam.fieldOfView; // gan gia tri hien tai fov cua cam vao bien defaltFOV
        im = GetComponentInParent<Ferra_InputManager>();
        cc = GetComponentInParent<Controller_gearSystem>();
    }
    void Update()
    {

        changePos();
        target = povs[index].position;
    }
    private void LateUpdate()
    {
        camMoveToPovs();
        fovFB();
        boostFovCam();
        
    }

    void camMoveToPovs()
    {
        //cam di chuyen den vi tri index
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * povsSpeedTranform);
        transform.forward = povs[index].forward; // cam huong theo vi tri povs[] theo huong toa do cua povs
    }
    void fovFB()
    {
        if (im.vr > 0f) camMoveForwad();
        if (im.vr < 0f) camMoveBackwad();
    }
     public void camMoveForwad() // toi hoac vo so
    {
        // increasing FOV after putting im.hr
        if ((index == 0 || index==1)) 
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, FOVForward, Time.deltaTime * timeForWard);
        else cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaltFOV, Time.deltaTime * timeForWard);
    }
     public void camMoveBackwad() // chay lui hoc tra so
    {
        // descresing FOV after putting im.hr
        if ((index == 0 || index==1))
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, FOVBackward, Time.deltaTime * timeBackWard);
        else cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaltFOV, Time.deltaTime * timeBackWard);
    }
    public void boostFovCam()
    {
        if ((index == 0 || index==1)){

        if (im.boosting)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, FOVBoost, Time.deltaTime * timeBoost);
        else
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaltFOV, Time.deltaTime * timeBoost);
        }
    }
    public void FOVGearChanges()
    {
        if ((index == 0 || index==1)) 
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, FOVGear,Time.deltaTime * TimeGear);
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
