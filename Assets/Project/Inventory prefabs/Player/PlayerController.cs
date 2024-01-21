using UnityEngine.EventSystems;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerControllers : MonoBehaviour
{
    [SerializeField] private Interactable focusInteracCurrent;
    private Camera cam;
    [SerializeField] private LayerMask movementMask;
    private PlayerMotor playerMotor;

    void Start()
    {
        cam = Camera.main;
        playerMotor = GetComponent<PlayerMotor>();
    }

    void Update()
    {
        //todo no move when clicking on InventoryUI
        if(EventSystem.current.IsPointerOverGameObject()) return;
        
        if(Input.GetMouseButton(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, movementMask)) {
                //? draw Line from cam to hit.point
                Debug.DrawLine(cam.transform.position, hit.point, Color.red, 0.1f);
                //Debug.Log(hit.collider.name + " " + hit.point);

                //? move player to hit
                playerMotor.MoveToPoint(hit.point); // => move
                RemoveFocus(); // xet bien focusInteract = null, targetTrans (playermotor.cs) = null
            }
        }

        if(Input.GetMouseButtonDown(1)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100)) {

                //? draw Line from cam to hit.point
                Debug.DrawLine(cam.transform.position, hit.point, Color.green, 0.1f);
                Interactable interactable =  hit.collider.GetComponent<Interactable>();
                if(interactable != null) {
                    SetFocus(interactable);
                }
            }
        }
    }

    //todo tham so: hit.collider Items
    private void SetFocus(Interactable newFocusInterac) {
        if(newFocusInterac != focusInteracCurrent) {
            if(focusInteracCurrent != null) focusInteracCurrent.OnDeFocused();
            focusInteracCurrent = newFocusInterac;

            playerMotor.FollowTarget(newFocusInterac);// gan gia tri targetTrans
        }
        
        newFocusInterac.OnFocused(transform);
    }

    //todo bien target = null
    private void RemoveFocus() {
        //todo dat ham update() trong interactable.cs
        if(focusInteracCurrent != null)
            focusInteracCurrent.OnDeFocused();// isFocus = false

        focusInteracCurrent = null; // bien interactable.s = null
        playerMotor.StopFollowingTarget(); // null gia tri targetTrans

    }
}