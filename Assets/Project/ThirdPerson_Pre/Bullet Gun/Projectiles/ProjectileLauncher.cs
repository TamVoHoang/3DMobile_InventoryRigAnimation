using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject serverProjectilePrefab;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private GameObject flashHeadon;


    // [SerializeField] ParticleSystem[] muzzleFlash;
    // [SerializeField] private Collider playerCollider;


    [Header("Settings")]
    [SerializeField] float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float muzzleFlashDuration;


    private bool shouldFire;
    public bool shouldSpwan;
    private float previousFireTime;
    private float muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;
        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner) return;
        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    [ServerRpc]
    private void PrimaryFireServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        GameObject projectileInstance = Instantiate(serverProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.forward = direction;

        
        if (projectileInstance.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = rb.transform.forward * projectileSpeed;
            
        }

        SpawnDummyProjectileClientRpc(spawnPos, direction);
    }

    [ClientRpc]
    private void SpawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if(IsOwner) return;
        SpawnDummyProjectile(spawnPos, direction);
    }

    void Update()
    {
        if (muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;

            if (muzzleFlashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
                flashHeadon.SetActive(false);
            }
        }


        if(!IsOwner) return; // neu la owner thi ban
        //if (!shouldFire) { return; } // inputsystem co phat hien mouse 0
        if(shouldSpwan)
        {
            if(Time.time < (1/fireRate) + previousFireTime) return;


            PrimaryFireServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.forward);

            SpawnDummyProjectile(projectileSpawnPoint.position, projectileSpawnPoint.forward); // firing local owner


            previousFireTime = Time.time;
        }

    }
    private void HandlePrimaryFire(bool shouldFire)
        {
            this.shouldFire = shouldFire;
        }

    private void SpawnDummyProjectile(Vector3 spawnPos, Vector3 direction) // local foring
    {

        muzzleFlash.SetActive(true);
        flashHeadon.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        GameObject projectileInstance = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.forward = direction;

        if (projectileInstance.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = rb.transform.forward * projectileSpeed;
            if(rb) Debug.Log("co rb");
        }
    }


}
