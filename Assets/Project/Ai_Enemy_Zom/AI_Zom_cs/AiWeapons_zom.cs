using System.Collections;
using UnityEngine;

public class AiWeapons_zom : MonoBehaviour
{
    private Animator animator;
    [SerializeField] GameObject rightHand_SpawnRaycast;
    [SerializeField] GameObject leftHand_SpawnRaycast;

    [SerializeField] float minSwordDisRaycast = 0.5f;
    PlayerHealth playerHealth;

    bool isEnemyTakeDamagePlayer;

    //play sound when trigger Attack in Animator
    AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    private void Awake() {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }
    private void Start() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isEnemyTakeDamagePlayer = false; // dang false = chua take => cho tru mau player
    }

    private void Update() {
        // kiem tra de sinh ra tia raycast o tay va dau

        CheckHandRaycast(rightHand_SpawnRaycast.transform, Vector3.right);
        CheckHandRaycast(leftHand_SpawnRaycast.transform, -Vector3.right);
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
        Debug.Log("co vao day truoc khi loi");
        
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
            if(hitBox && !hitPlayer.IsDead && !isEnemyTakeDamagePlayer)
            {
                Debug.Log("co vao hitbox player");
                isEnemyTakeDamagePlayer = true;
                hitBox.OnSwordRaycastHit(100f, hitBox.transform.position); //! OK but lay mau player lien tuc

                StartCoroutine(EnemyTakePlayerHealthCO(0.5f)); // sau float time - set isEnemyTakeDamagePlayer = false - cho lay mau player
            }
        } else {
            Debug.DrawRay(RHand.position, RHand.transform.TransformDirection(aimDirection) * minSwordDisRaycast, Color.red);
        }
    }

    IEnumerator EnemyTakePlayerHealthCO(float time) {

        yield return new WaitForSeconds(time);
        isEnemyTakeDamagePlayer = false;
    }

    // sound
    public void PlaySound() {
        //audioSource.PlayOneShot(audioClip, 1);
    }
    //todo
}
