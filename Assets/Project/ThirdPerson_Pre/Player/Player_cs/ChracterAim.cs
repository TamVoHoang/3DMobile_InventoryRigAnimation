using UnityEngine;

public class ChracterAim : Singleton<ChracterAim> // co the bo singleton tai day
{

    [Range(1f, 5f)]
    [SerializeField] float currentMouseSensitivity = 1f;
    [SerializeField] public float xAixs, yAxis;
    [SerializeField] private Transform camFollowPos; // maincamera se move den day, vi trong main camera co virtul camera
    [SerializeField] private Vector2 yAxixLimit = new Vector2(30, 30);
    //[SerializeField] public RigBuilder rigBuilder;
    //RaycastWeapon weapon; // tai scrip nay trong folder player
    //public float aimDuration = 0.3f;

    public float MouseSensitivity {get { return currentMouseSensitivity; } set { currentMouseSensitivity = value; }}
    float minSensitivity = 1f;
    float maxSensitivity = 5;
    public float MaxSensitivity { get { return maxSensitivity;}}
    public float MinSensitivity { get { return minSensitivity;}}

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        /* rigBuilder = GetComponent<RigBuilder>();
        Cursor.lockState = CursorLockMode.Locked;
        weapon = GetComponentInChildren<RaycastWeapon>(); */ // class weaponManager nam o folder con gan trong cay sung
    }
    void Update()
    {
        //?using mouse to aim player => muon dung thi phai active InputManager.cs coll 2 dong 54 va 84
        // xAixs += Input.GetAxisRaw("Mouse X") * mouseSentivity;
        // yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSentivity;

        //?using virtual UI to aim player. gia tri lay tu khi SetAIm trong UICanvasControllerInput.cs coll 33
        
        xAixs += InputManager.Instance.GetAim.x * currentMouseSensitivity/10;
        yAxis -= InputManager.Instance.GetAim.y * currentMouseSensitivity/10;
        
        yAxis = Mathf.Clamp(yAxis, -yAxixLimit.x, yAxixLimit.y);
    }

    /* private void FixedUpdate() {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAixs, transform.eulerAngles.z);
    } */

    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAixs, transform.eulerAngles.z);
    }
}
