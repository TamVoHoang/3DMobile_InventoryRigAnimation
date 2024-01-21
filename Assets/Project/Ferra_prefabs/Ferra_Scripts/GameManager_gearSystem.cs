using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager_gearSystem : MonoBehaviour
{
    //public GameObject carSpeedometer; // PHAI BAT UI DE CO CHO FILL DATA VAO
    public Controller_gearSystem carCC;
    [SerializeField]  GameObject speedometer;
    public Text kph;
    public Text gearNum;
    public GameObject needle;
    public Slider nitrusSlider;

    public float vehicleSpeed;
    float startNeedelePos = -157f, endNeedelePos = -385f;
    float desiredNeedlePos;
    public int nitrusDive =20;


    private void Awake()
    {
        //carSpeedometer.SetActive(true); // bat dong ho xe bat buoc de co the chay
        GetComponents();
    }
    void GetComponents()
    {
        carCC = GameObject.FindGameObjectWithTag("Car").GetComponentInChildren<Controller_gearSystem>();

        speedometer = GameObject.Find("Car_Speedometer");
        kph = speedometer.transform.Find("kphText").GetComponent<Text>();
        gearNum = speedometer.transform.Find("currentGearText").GetComponent<Text>();
        needle = speedometer.transform.Find("needele").gameObject;
        nitrusSlider = speedometer.transform.Find("Slider").gameObject.GetComponent<Slider>();
    }

    void FixedUpdate()
    {
        //vehicleSpeed = carCC.KPH;
        kph.text = carCC.KPH.ToString("0");
        updateNeedele();
        nitrusUI();
        //changeGear();

    }

    public void updateNeedele()
    {
        desiredNeedlePos = startNeedelePos - endNeedelePos;
        float temp = carCC.engineRPM / 10000;
        // chuyen doi tu vi tri sang goc quay needle
        needle.transform.eulerAngles = new Vector3(0, 0, (startNeedelePos - temp * desiredNeedlePos));
    }
    public void changeGear() // so gear auto
    {
        gearNum.text = (!carCC.reverse) ? (carCC.gearNum + 1).ToString() : "R";
        //gearNum.text = carCC.gearNum.ToString();
    }

    public void changeGearUpManual() // thay doi so hien thi khi chay manual
    {
        gearNum.text = (carCC.gearNum ).ToString(); // day la dieu kien dung

    }

    public void changeGearDownManual()
    {
        //gearNum.text = (carCC.gearNum + 0).ToString(); // day la dieu kien dung

        if (!carCC.isDangLui) // neu ko lui isDangLui = false
        {
            gearNum.text = carCC.gearNum.ToString();
        }else if(carCC.isDangLui && carCC.gearNum == 1)
        {
            gearNum.text = "R";
        }
    }

    public void nitrusUI(){
        nitrusSlider.value = carCC.nitrusValue/nitrusDive;
    }
}
