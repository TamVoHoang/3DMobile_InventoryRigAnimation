using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap_FerraCamera : MonoBehaviour
{
    //public Camera miniMapCam_Car; // caemra chieu xuong ban do mini UI
    public Transform ferraCar; // VI TRI XE

    //[SerializeField] GameObject ferraIcon; // BIEU TUONG MUI TEN CUA XE TREN MINI MAP
    private void Awake()
    {
        //miniMapCam_Car.enabled = true;
        //ferraIcon.SetActive(true);

    }
    private void LateUpdate()
    {
        Vector3 newPosition = ferraCar.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, ferraCar.eulerAngles.y, 0f);
    }

}
