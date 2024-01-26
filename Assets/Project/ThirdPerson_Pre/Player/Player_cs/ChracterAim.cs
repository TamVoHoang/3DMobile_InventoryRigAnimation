using UnityEngine.EventSystems;
using UnityEngine;

public class ChracterAim : MonoBehaviour
{
    [SerializeField] public float mouseSentivity = 1f;
    [SerializeField] public float xAixs, yAxis;
    [SerializeField] private Transform camFollowPos;
    [SerializeField] float yAxisLimit = 45f;

    //RaycastWeapon weapon; // tai scrip nay trong folder player
    //public float aimDuration = 0.3f;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //weapon = GetComponentInChildren<RaycastWeapon>(); // class weaponManager nam o folder con gan trong cay sung
    }
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        
        // xAixs += Input.GetAxisRaw("Mouse X") * mouseSentivity;
        // yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSentivity;

        xAixs += InputManager.Instance.GetAim.x * mouseSentivity;
        yAxis -= InputManager.Instance.GetAim.y * mouseSentivity;

        yAxis = Mathf.Clamp(yAxis, -yAxisLimit, yAxisLimit);

    }
    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAixs, transform.eulerAngles.z);
    }
}
