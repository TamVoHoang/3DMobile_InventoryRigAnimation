using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//todo game object = Canvas_PlayerInfo in AccountDataOverview scene
public class UI_Setting : MonoBehaviour
{
    private const string SENSITIVITY_SLIDER =  "Sensitivity_Slider";
    private const string SENSITIVITY_TEXT =  "Sensitivity_Text";
    [SerializeField] Slider mouseSensitivity_Slider;
    [SerializeField] TMPro.TMP_Text mouseSensitivity_Text;
    [SerializeField] Button PlusButton;
    [SerializeField] Button MinusButton;


    ChracterAim chracterAim;

    private void Awake() {
        chracterAim = FindObjectOfType<ChracterAim>();

        /* mouseSensitivity_Slider = GameObject.Find(SENSITIVITY_SLIDER).GetComponent<Slider>();
        mouseSensitivity_Text = GameObject.Find(SENSITIVITY_TEXT).GetComponent<TMPro.TMP_Text>(); */

    }
    private void Start() {
        // gan gia tri mac dinh cho slider theo character aim
        UpdateSensitivitySlider();

        PlusButton.onClick.AddListener(PlusButton_OnClicked);
        MinusButton.onClick.AddListener(MinusButton_OnClicked);

    }
    private void Update() {
        // dun ggia tri slider.value -> chracterAim.MouseSensitivity
        //chracterAim.MouseSensitivity =  mouseSensitivity_Slider.value;

        UpdateSensitivitySlider();
    }

    private void MinusButton_OnClicked()
    {
        mouseSensitivity_Slider.value -= 0.2f;
        SetMouseSensitivity();
    }

    private void PlusButton_OnClicked()
    {
        mouseSensitivity_Slider.value += 0.2f;
        SetMouseSensitivity();
    }

    void UpdateSensitivitySlider() {
        if (mouseSensitivity_Slider != null && mouseSensitivity_Text != null) {
            mouseSensitivity_Slider.maxValue = chracterAim.MaxSensitivity;
            mouseSensitivity_Slider.minValue = chracterAim.MinSensitivity;
            mouseSensitivity_Slider.value = chracterAim.MouseSensitivity;

            mouseSensitivity_Text.text = Math.Round(chracterAim.MouseSensitivity, 1).ToString();
        }
    }

    void SetMouseSensitivity() {
        chracterAim.MouseSensitivity =  mouseSensitivity_Slider.value;
        mouseSensitivity_Text.text = chracterAim.MouseSensitivity.ToString();
    }

    //todo
}
