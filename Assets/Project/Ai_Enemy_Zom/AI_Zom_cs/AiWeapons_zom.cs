using UnityEngine;

public class AiWeapons_zom : MonoBehaviour
{
    private Animator animator;
    [SerializeField] GameObject rightHand_SpawnRaycast;
    [SerializeField] GameObject leftHand_SpawnRaycast;

    [SerializeField] float minSwordDisRaycast = 0.5f;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        // kiem tra de sinh ra tia raycast o tay va dau
        CheckHandRaycast(rightHand_SpawnRaycast.transform, Vector3.right);
    }

    public void StartAttacking() {
        //thu hien animation tan cong
        //tao ra raycast tren tay de lay health player
        Debug.Log("zombie START attack animation raycast");
        animator.SetTrigger("Attack");
    }

    public void StopAttacking() {
        //thu hien animation tan cong
        //tao ra raycast tren tay de lay health player
        Debug.Log("zombie STOP attack animation raycast");
    }

    private void CheckHandRaycast(Transform RHand, Vector3 aimDirection) {
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Character"); //! player and enemy have same LayerMask

        if(Physics.Raycast(RHand.position, RHand.transform.TransformDirection(aimDirection), out hit, minSwordDisRaycast, layerMask)) {
            Debug.DrawRay(RHand.position, RHand.transform.TransformDirection(aimDirection) * minSwordDisRaycast, Color.yellow);

            if(hit.collider.gameObject.CompareTag("Agent")) {
                Debug.Log("RaycastCommand tu va cham voi agent");
                return; 
            } 

            var hitBox = hit.collider.GetComponentInChildren<HitBox>();
            var hitPlayer = hit.collider.GetComponent<PlayerHealth>();
            if(hitBox && !hitPlayer.IsDead) {
                Debug.Log("co vao hitbox");
                hitBox.OnSwordRaycastHit(10f, hitBox.transform.position); //ray.direction
            }
        } else {
            Debug.DrawRay(RHand.position, RHand.transform.TransformDirection(aimDirection) * minSwordDisRaycast, Color.red);

        }
    }

}
