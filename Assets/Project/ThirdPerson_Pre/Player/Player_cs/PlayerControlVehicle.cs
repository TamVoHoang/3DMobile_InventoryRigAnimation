using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlVehicle : MonoBehaviour
{
    [SerializeField] PlayerActiveOthers playerActive;
    [SerializeField] CarActiveOthers carActive;

    public bool isPlayerActived, isCarActived;

    // Start is called before the first frame update
    void Awake()
    {
        isPlayerActived = true;
        isCarActived = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerActived) playerActive.activePlayer();
        else playerActive.deActivePlayer();

        if (isCarActived) carActive.activeCar();
        else carActive.deActiveCar();

    }
}
