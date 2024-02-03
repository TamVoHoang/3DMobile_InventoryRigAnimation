using UnityEngine;

public class ChracterAim : MonoBehaviour
{
    [SerializeField] public float mouseSentivity = 1.2f;
    [SerializeField] public float xAixs, yAxis;
    [SerializeField] private Transform camFollowPos; // maincamera se move den day, vi trong main camera co virtul camera
    [SerializeField] private Vector2 yAxixLimit = new Vector2(20, 20);
    //RaycastWeapon weapon; // tai scrip nay trong folder player
    //public float aimDuration = 0.3f;
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //weapon = GetComponentInChildren<RaycastWeapon>(); // class weaponManager nam o folder con gan trong cay sung
    }
    void Update()
    {
        //?using mouse to aim player => muon dung thi phai active InputManager.cs coll 2 dong 54 va 84
        // xAixs += Input.GetAxisRaw("Mouse X") * mouseSentivity;
        // yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSentivity;

        //?using virtual UI to aim player. gia tri lay tu khi SetAIm trong UICanvasControllerInput.cs coll 33
        
    }
    private void FixedUpdate() {
        xAixs += InputManager.Instance.GetAim.x * mouseSentivity;
        yAxis -= InputManager.Instance.GetAim.y * mouseSentivity;
        yAxis = Mathf.Clamp(yAxis, -yAxixLimit.x, yAxixLimit.y);
    }

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAixs, transform.eulerAngles.z);
    }
}
