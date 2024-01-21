using System.Collections;
using System.Collections.Generic;
//using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Controller_gearSystem : MonoBehaviour
{
    //public GameObject FerraGM; // phai co script nay thi ko bi loi do thieu data khi vo so
    //[SerializeField] WheelsRotation wheelsRotation;
    public CameraCar camCar;
    public GameManager_gearSystem manager;
    Ferra_InputManager IM;
    Rigidbody rb;
    WheelsRotation wheelRotation;
    enum driveType // dieu kien dau vao de lua hcon ben unity
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
    [SerializeField] driveType typeOfDrive; // kieu lai 2 ban hturoc, sau, ca 4 banh
    enum gearBox
    {
        automatic,
        manual
    }

    //public float handBrakeFriction = 0;
    public AnimationCurve enginePower;
    public float maxRPM, minRPM;
    public float engineRPM;
    [SerializeField] float totalPower, wheelsRPM;

    [SerializeField] private gearBox gearChange;
    public float[] gears;
    public float[] gearChangeSpeed;
    [SerializeField] float maxKPH =300f;

    public int gearNum = 1;
    public float KPH;
    public bool reverse = false;
    public bool test; //engine sound boolean

    public bool nitrusFlag = false;
    public float nitrusValue;
    public float activeNitrusForce;

    [SerializeField] float brakePower = 0;
    public bool isDangLui = false;
    [SerializeField] int breakForceManual =1000;
    [SerializeField] int breakForceAuto = 20;

    [SerializeField] GameObject centerMass;
    [SerializeField] GameObject WheelMeshes, WheelColliders;
    WheelCollider[] wheel = new WheelCollider[4];
    GameObject[] wheelMesh = new GameObject[4];
    [SerializeField] float[] slip = new float[4]; // GIA TRI TRA VE KHI BANH XE SLIP TREN GROUND


    public float handBrakeFrictionMultiplier = 2f;
    private WheelFrictionCurve forwardFriction, sidewaysFriction;
    [SerializeField] float forceDown, vertical, horizontal, driftFactor, lastValue, radius = 6f;
    float smoothTime = 0.09f;
    private bool flag = false;

    private void Awake()
    {
        //FerraGM.SetActive(true); // phai on de goi ham vo so
        //wheelsRotation = GetComponentInChildren<WheelsRotation>();
        //wheelsRotation.enabled = true;
    }
    private void Start()
    {
        //if (SceneManager.GetActiveScene().name == "awakeScene") return;
        GetObject();
        StartCoroutine(timedLoop());
    }

    void GetObject()
    {
        IM = GetComponent<Ferra_InputManager>();
        rb = GetComponent<Rigidbody>();
        //camCar = GetComponentInChildren<CameraCar>();

        // tim folder de gan vao tham so
        WheelColliders = GameObject.Find("WheelColliders");// tim foler co ten "WheelColliders" => bien gamobject o tren
        wheel[0] = WheelColliders.transform.Find("FL_col").GetComponent<WheelCollider>();
        wheel[1] = WheelColliders.transform.Find("FR_col").GetComponent<WheelCollider>();
        wheel[2] = WheelColliders.transform.Find("BL_col").GetComponent<WheelCollider>();
        wheel[3] = WheelColliders.transform.Find("BR_col").GetComponent<WheelCollider>();

        WheelMeshes = GameObject.Find("WheelMeshes");
        wheelMesh[0] = WheelMeshes.transform.Find("FL_mesh").gameObject;
        wheelMesh[1] = WheelMeshes.transform.Find("FR_mesh").gameObject;
        wheelMesh[2] = WheelMeshes.transform.Find("BL_mesh").gameObject;
        wheelMesh[3] = WheelMeshes.transform.Find("BR_mesh").gameObject;

        // goi mass trong RB de lam xe nang hon
        centerMass = GameObject.Find("centerMass");
        rb.centerOfMass = centerMass.transform.localPosition;
        //wheelRotation = GetComponentInChildren<WheelsRotation>();
    }
    void Update()
    {
        horizontal = IM.hr;
        vertical = IM.vr;
        //SteerFront();
        lastValue = engineRPM;

        addDownForce();
        AnimatedWheels();
        if(gearChange == gearBox.automatic) calculateEnginePower();// chay autoGear
        if(gearChange == gearBox.manual) calculateEnginePowerManualGear(); // chay ManualGear
        if (gameObject.tag == "AI") return;
        adjustTraction();
        activateNitrus();
    }
    private void FixedUpdate()
    {
        SteerFront();
    }
    void addDownForce() // PUT FORCE ON CAR
    {
        rb.AddForce(-transform.up * forceDown * rb.velocity.magnitude);
    }
    void AnimatedWheels() // ANIMATION WHEELS
    {
        Vector3 wheelPositon = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheel[i].GetWorldPose(out wheelPositon, out wheelRotation);
            wheelMesh[i].transform.position = wheelPositon;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }
    void SteerFront() // TURN LEFT RIGHT
    {
        if (horizontal > 0f) // QUEO PHAI
        {
            wheel[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheel[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;

        }
        else if (horizontal < 0f) // QUEO TRAI
        {
            wheel[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheel[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;

        }
        else // CHAY THANG
        {
            wheel[0].steerAngle = 0;
            wheel[1].steerAngle = 0;

        }
    }
    private void wheelRPM()
    {
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheel[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;

            // khi chay manual thi ko chay dong nay. NEU CHAY DOAN NAY,KHI VAO GAEM LUI SE HIEN R VA TIEN TOI THI LAI CONG THEM 1 DU THUC TE CHI LA 1
            if (wheelsRPM < 0 && !reverse)
            {
                reverse = true;
                manager.changeGear();
            }
            else if (wheelsRPM > 0 && reverse)
            {
                reverse = false;
                manager.changeGear();
            }
    }
    private void wheelRPMManual()
    {
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheel[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;

            // khi chay manual thi ko chay dong nay. NEU CHAY DOAN NAY,KHI VAO GAEM LUI SE HIEN R VA TIEN TOI THI LAI CONG THEM 1 DU THUC TE CHI LA 1
    }
    private void brakeVehicle()
    {
        if (vertical < 0) // khi dap thang, neu toc do >= 20 thi luc thang 1000
        {
            brakePower = (KPH >= 20) ? breakForceManual : 0;
        }
        else if (vertical == 0 && (KPH <= 10 || KPH >= -10)) // neu buong thang toc do <=10 luc thang =10
        {
            brakePower = breakForceAuto;
        }
        else
        {
            brakePower = 0; // nu toc do dang 20, cho du dang toi hoac lui, co dap thang cung van =0
        }
    }
    void Move() // MOVE CAR - ADD TORQUE
    {
        brakeVehicle();
            if (typeOfDrive == driveType.allWheelDrive) // lai 4 banh
            {
                for (int i = 0; i < wheel.Length; i++)
                {
                    wheel[i].motorTorque = (totalPower / 4); // tin hieu dau vao se ben input goi Nhan W
                    wheel[i].brakeTorque = brakePower;
                }
            }
            else if (typeOfDrive == driveType.frontWheelDrive)// lai 2 banh truoc
            {
                wheel[0].motorTorque = totalPower / 2;
                wheel[1].motorTorque = totalPower / 2;

                for (int i = 0; i < wheel.Length; i++)
                {
                    wheel[i].brakeTorque = brakePower;
                }
            }
            else //REAR WHEEL
            {
                wheel[2].motorTorque = totalPower / 2;
                wheel[3].motorTorque = totalPower / 2;
                for (int i = 0; i < wheel.Length; i++)
                {
                    wheel[i].brakeTorque = brakePower;
                }
            }
        //DO TOC DO KM/H
        KPH = rb.velocity.magnitude * 3.6f;
    }
    private bool isGrounded()
    {
        if (wheel[0].isGrounded && wheel[1].isGrounded && wheel[2].isGrounded && wheel[3].isGrounded)
            return true;
        else
            return false;
    }
    private bool checkGears()
    {
        if (KPH >= gearChangeSpeed[gearNum]) return true;
        else return false;
    }
    void shifter() // TANG GIAM GEAR VA THAY DOI GEARTEXT
    {
        if (!isGrounded()) return;
        // AUTOGEAR
        if (gearChange == gearBox.automatic)
        {
            Debug.Log(gears.Length-1);
            if (engineRPM > maxRPM && gearNum < gears.Length - 1 && !reverse && checkGears())
            {
                gearNum++;
                camCar.FOVGearChanges();
                
                //manager.changeGear();
                if (gameObject.tag != "AI") manager.changeGear(); // hien thi ben UI
                return;
            }
            if (engineRPM < minRPM && gearNum > 0)
            {
                gearNum--;
                camCar.FOVGearChanges();
                //manager.changeGear();
                if (gameObject.tag != "AI") manager.changeGear(); // hien thi ben UI ngay khi vao game la 2, sau do check thay giam lai
            }
        }
    }
    private void calculateEnginePower() // GOI HAM MOVE(), WHEELRPM, SHIFTER - TANG GIAM GEAR Auto
    {
        wheelRPM(); // => wheelsRPM
        if (vertical != 0) rb.drag = 0.005f;
        if (vertical == 0) rb.drag = 0.1f;
        //totalPower = 3.6f * enginePower.Evaluate(engineRPM) * (vertical);//


        if(KPH <=maxKPH) totalPower = 3.6f * enginePower.Evaluate(engineRPM) * (vertical);//
        else totalPower = 0; // QUYET DINH Xe CHAY HAY DUNG LAI


        float velocity = 0.0f;
        if (engineRPM >= maxRPM || flag)
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, maxRPM - 500, ref velocity, 0.05f);
            flag = (engineRPM >= maxRPM - 450) ? true : false;
            test = (lastValue > engineRPM) ? true : false;
        }
        else
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (gears[gearNum])), ref velocity, smoothTime);
            test = false;
        }
        if (engineRPM >= maxRPM + 1000) engineRPM = maxRPM + 1000; // clamp at max
        Move();
        shifter();
    }
    private void adjustTraction()
    {
        //tine it takes to go from normal drive to drift 
        float driftSmothFactor = .9f * Time.deltaTime;

        if (IM.handbrake)
        {
            sidewaysFriction = wheel[0].sidewaysFriction;
            forwardFriction = wheel[0].forwardFriction;

            float velocity = 0;
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = forwardFriction.extremumValue = forwardFriction.asymptoteValue =
                Mathf.SmoothDamp(forwardFriction.asymptoteValue, driftFactor * handBrakeFrictionMultiplier, ref velocity, driftSmothFactor);

            for (int i = 0; i < 4; i++)
            {
                wheel[i].sidewaysFriction = sidewaysFriction;
                wheel[i].forwardFriction = forwardFriction;
            }

            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = forwardFriction.extremumValue = forwardFriction.asymptoteValue = 1.1f;
            //extra grip for the front wheels
            for (int i = 0; i < 2; i++)
            {
                wheel[i].sidewaysFriction = sidewaysFriction;
                wheel[i].forwardFriction = forwardFriction;
            }
            rb.AddForce(transform.forward * (KPH / 400) * 10000);
        }
        //executed when handbrake is being held
        else
        {
            forwardFriction = wheel[0].forwardFriction;
            sidewaysFriction = wheel[0].sidewaysFriction;

            forwardFriction.extremumValue = forwardFriction.asymptoteValue = sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue =
                ((KPH * handBrakeFrictionMultiplier) / 300) + 1;

            for (int i = 0; i < 4; i++)
            {
                wheel[i].forwardFriction = forwardFriction;
                wheel[i].sidewaysFriction = sidewaysFriction;
            }
        }
        //checks the amount of slip to control the drift
        for (int i = 2; i < 4; i++)
        {
            WheelHit wheelHit;

            wheel[i].GetGroundHit(out wheelHit);
            ////smoke
            //if (wheelHit.sidewaysSlip >= 0.3f || wheelHit.sidewaysSlip <= -0.3f || wheelHit.forwardSlip >= .3f || wheelHit.forwardSlip <= -0.3f)
            //    //playPauseSmoke = true;
            //else
            //    //playPauseSmoke = false;


            if (wheelHit.sidewaysSlip < 0) driftFactor = (1 + -IM.hr) * Mathf.Abs(wheelHit.sidewaysSlip);

            if (wheelHit.sidewaysSlip > 0) driftFactor = (1 + IM.hr) * Mathf.Abs(wheelHit.sidewaysSlip);
        }
    }
    public void activateNitrus()
    {
        if (!IM.boosting && nitrusValue <=3) // ko boost && <=5
        {
            nitrusValue += Time.deltaTime / 2;
        }
        else
        {
            // nhan giu boost -
            // >5 -
            nitrusValue -= (nitrusValue <= 0) ? 0 : Time.deltaTime;
        }
        if (IM.boosting && KPH <= maxKPH)
        {
            if (nitrusValue > 0)
            {
                //CarEffects.startNitrusEmitter();
                rb.AddForce(transform.forward * activeNitrusForce);
            }
            //else CarEffects.stopNitrusEmitter();
        }
        //else CarEffects.stopNitrusEmitter();

    }
    private IEnumerator timedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);
            radius = 6 + KPH / 20;

        }
    }
    void GetFriction() // CREATE FRICTION WHEELS AND GROUND
    {
        for (int i = 0; i < wheel.Length; i++) //FL-FR
        {
            WheelHit wheelHit;
            wheel[i].GetGroundHit(out wheelHit); // moi banh xe se getgrounhit tra lai ket qua wheelHit
            slip[i] = wheelHit.forwardSlip; // lay wheelhit.sidewayslip va gan cho slip[] de kiem tra
        }
    }
    private bool checkGearsManual()
        {
            if (KPH >= gearChangeSpeed[gearNum-1]) return true;
            else return false;
        }
    private void calculateEnginePowerManualGear() // GOI HAM MOVE(), WHEELRPM, SHIFTER - TANG GIAM GEAR Auto
        {
            wheelRPMManual();
            if (vertical != 0) rb.drag = 0.005f;
            if (vertical == 0) rb.drag = 0.1f;

        if(KPH <=maxKPH) // van toc nho hon gioi han  && KPH <= gearChangeSpeed[gearNum-1]
        {
            totalPower = 3.6f * enginePower.Evaluate(engineRPM) * (vertical);
            if(checkGearsManual()) totalPower = 0;

        }else totalPower = 0; // QUYET DINH Xe CHAY HAY DUNG LAI


        float velocity = 0.0f;
        if (engineRPM >= maxRPM || flag)
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, maxRPM - 500, ref velocity, 0.05f);
            flag = (engineRPM >= maxRPM - 450) ? true : false;
            test = (lastValue > engineRPM) ? true : false;
        }
        else
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (gears[gearNum-1])), ref velocity, smoothTime);
            test = false;
        }
        if (engineRPM >= maxRPM + 1000) engineRPM = maxRPM + 1000; // clamp at max


        Move();

        //shifter Manual
        if (!isGrounded()) return;
        if(gearChange == gearBox.manual)
        {
            //Debug.Log(gears.Length-1);
            if (gearNum < gears.Length  && checkGearsManual()) //&& engineRPM > maxRPM
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gearNum ++;
                    manager.changeGearUpManual(); // hien thi ben UI
                    camCar.FOVGearChanges();
                    return;
                }
                else totalPower =0;
            }
            if (engineRPM < minRPM && gearNum > 1 && Input.GetKeyDown(KeyCode.Q))
            {
                gearNum--;
                camCar.FOVGearChanges();
                manager.changeGearDownManual();
            }

            if (engineRPM < minRPM && gearNum > 1) // dieu kien tra so tu dong ko can Q
            {
                gearNum--;
                camCar.FOVGearChanges();
                manager.changeGearDownManual(); // hien thi ben UI ngay khi vao game la 2, sau do check thay giam lai
            }

            // XET DIEU KIEN CHAY LUI VA HIEN R TREN MAN HINH
            if (vertical <0 && gearNum ==1 && wheelsRPM <0)
            {
                //Debug.Log("dang chay lui");
                isDangLui = true;
                manager.changeGearDownManual();
            }else if(vertical > 0 && gearNum == 1)
            {
                isDangLui=false;
                manager.changeGearDownManual();
            }

        }

    }
}




