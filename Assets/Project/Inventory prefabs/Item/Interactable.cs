using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float radius = 3f;
    public float Radius{get{return radius;} private set{radius = value;}}

    [SerializeField] private Transform interactionTransform;
    public Transform InteractionTransform{get{return interactionTransform;} private set{interactionTransform = value;}}

    private bool isFocus = false;
    [SerializeField] private Transform player;

    private bool hasInteracted = false;

    protected virtual void Interact() {
        Debug.Log("RUN FROM LOP CHA " + "interating with Transform: " + transform.name);
    }

    private void Awake() {
        interactionTransform = this.transform;
    }

    private void Update() {
        if(isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            
            if(distance <= radius)
            {
                Debug.Log("co chay den day");
                Interact();
                Debug.Log("interact at interactable.object");
                hasInteracted = true;
            }

            Debug.Log("dang chay update class cha");
        }
    }

    //todo item duoc player focused ( playertransform, isFocus, hasInteracted)
    public void OnFocused(Transform playerTransform) {
        isFocus = true;
        player = playerTransform;

        hasInteracted = false;
    }
    public void OnDeFocused() {
        isFocus = false;
        player = null;

        hasInteracted = false;
    }
    private void OnDrawGizmos() {
        if(interactionTransform == null) interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}