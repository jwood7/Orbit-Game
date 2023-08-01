using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Controller : MonoBehaviour
{

    // TO DO:
    /*
        - Strafe
            - Add fuel capacity to strafe controls (prevents high speed strafing by holding for too long)
        - Thrust
            - Change thrust increment at high levels
        - Turn
            - Rotation on mouse move
            - Turn speed depends on where mouse is on screen (farther = faster turn)
            - Turn speed/effectiveness also depends on thrust (this may need adjustment)
            - Figure out what is causing unexpected spin on diagonal mouse movement


    */

    public float strafeSpeed;
    public float thrustSpeed;
    public float turnSpeed;
    public float spinSpeed;
    public float brakeSpeed;
    public float inertia1;
    public float inertia2;
    public Rigidbody rb;
    public Vector3 strafeInput;
    public Vector3 turnInput;
    public float thrustInput;
    public float spinInput;
    public float brakeInput;
    public float minThrottle;
    public float maxThrottle;
    public float safeThrottle;
    public float turnDragConst;
    public float FOV;
    public TextMeshProUGUI speedometer;
    public TextMeshProUGUI upSpeedometer;
    public TextMeshProUGUI rightSpeedometer;
    public TextMeshProUGUI throttleDisplay;
    public float throttleIncrement;
    public float heat;
    public float overheat;
    public float cooldownIncrement;
    public bool overheated;
    public GameObject overheatBar;
    private MeshRenderer overheatBarRenderer;
    public Material green;
    public Material yellow;
    public MeshRenderer[] throttlePips;
    public Material red;
    public Material blue;
    public Material emptyRed;
    public Material emptyBlue;
    public Material emptyGreen;
    public Transform vIndicator;
    public Transform hIndicator;
    public AudioSource audioMainEngine;
    public AudioSource audioLThrust;
    public AudioSource audioRThrust;
    public AudioSource audioUThrust;
    public AudioSource audioDThrust;
    public AudioSource audioBrakeThrust;
    // public Transform crosshair;
    public Oxygen oxygen;
    public bool isPaused;
    public Grapple grapple;
    public GameObject pauseMenu;
    private Coroutine throttleCoroutine;
    private float throttleInput;
    
    void setThottlePips(){
        //Need to account for negative throttle
        for (int i = 0;  i< throttlePips.Length; i++){
            if ((thrustInput-minThrottle)/(maxThrottle-minThrottle) >= (i/(float)(throttlePips.Length))){
                if (i/(float)throttlePips.Length <= (-1*minThrottle/(maxThrottle-minThrottle)) ){
                    throttlePips[i].material = emptyBlue;
                // } else if (i-((maxThrottle-minThrottle)/throttlePips.Length) == 0){
                //     throttlePips[i].material = red;
                } else if ((i/(float)throttlePips.Length) > (safeThrottle-minThrottle)/(float)(maxThrottle-minThrottle)){
                    throttlePips[i].material = red;
                }else {
                    throttlePips[i].material = green;
                }
            
            }else{
                if (i/(float)throttlePips.Length <= (-1*minThrottle/(maxThrottle-minThrottle)) ){
                    throttlePips[i].material = blue;
                // } else if (i-((maxThrottle-minThrottle)/throttlePips.Length) == 0){
                //     throttlePips[i].material = emptyRed;
                } else if ((i/(float)throttlePips.Length) > (safeThrottle-minThrottle)/(float)(maxThrottle-minThrottle)){
                    throttlePips[i].material = emptyRed;
                }else {
                    throttlePips[i].material = emptyGreen;
                }
            }
        }
    }

    void setVelocityIndicator(Transform indicator, float speed){
        // +/- 0.35 transform on x axis
        // +/- 15 speed
        float relPos = speed; 
        if (speed > 15){
            relPos = 15;
        }
        if (speed < -15){
            relPos = -15;
        }
        indicator.localPosition = new Vector3 (relPos/15*0.35f, indicator.localPosition.y, indicator.localPosition.z);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null){
            Debug.LogError("Failed to get rigid body");
        }
        heat = 0;
        overheatBarRenderer = overheatBar.GetComponent<MeshRenderer>();
        setThottlePips();
        audioMainEngine.Play();
        audioMainEngine.loop = true;
        audioMainEngine.volume = 0;
        audioMainEngine.pitch = 0;
        throttleDisplay.text = "0";
        Cursor.lockState = CursorLockMode.Confined; //not sure if this works
        Cursor.visible = false; //Consider a directional cursor around the crosshair, if you change how mouse/camera movement works
        oxygen = GetComponent<Oxygen>();
        oxygen.enabled = false;
        isPaused = false;
        grapple = GetComponent<Grapple>();
        pauseMenu = GameObject.FindWithTag("Pause Menu");
        pauseMenu.SetActive(false);
        throttleInput = 0;
    }

    

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        // Debug.Log(inputVec.x + " " + inputVec.y);

        // strafeInput = new Vector2(inputVec.x, inputVec.y);
        if (!isPaused){
            if (inputVec.y > 0){
                upSpeedometer.color = new Color32(0,255,0,255);
            }else if (inputVec.y < 0){
                upSpeedometer.color = new Color32(255,0,0,255);
            }else{
                upSpeedometer.color = new Color32(0,0,255,255);
            }
            if (inputVec.x > 0){
                rightSpeedometer.color = new Color32(0,255,0,255);
            }else if (inputVec.x < 0){
                rightSpeedometer.color = new Color32(255,0,0,255);
            }else{
                rightSpeedometer.color = new Color32(0,0,255,255);
            }
        }
        strafeInput = new Vector3(inputVec.x, inputVec.y, 0);

    }

    public void OnLook(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        // Debug.Log(inputVec.x + " " + inputVec.y);

        // strafeInput = new Vector2(inputVec.x, inputVec.y);
        turnInput = new Vector3(inputVec.x, inputVec.y, 0);
        // Vector3 mousePos = Mouse.current.position.ReadValue();
        // mousePos.z=Camera.main.nearClipPlane;
        // // THIS IS WRONG
        // // Ship does not fly based on mouse position, but mouse movement
        // // (May want to consider changing that, actually)
        // // so the arrow should not point point at cursor, but should be... difference between inputs??? idk
        // float angleToMouse = Mathf.Atan2((Screen.height/2f)-mousePos.y, (Screen.width/2f)-mousePos.x) * (180/Mathf.PI);
        // // float angleToMouse =  Vector2.Angle(new Vector2(0,0), Mouse.current.position.ReadValue());
        // // Debug.Log(inputVec);
        // // Debug.Log(angleToMouse);
        // // crosshair.right = new Vector2(Worldpos.x,Worldpos.y);
        // crosshair.right = inputVec;
        // crosshair.localRotation = Quaternion.Euler(0, 0, angleToMouse);
        // Debug.Log(Mouse.current.position.ReadValue());
        
    }


    public void OnThrottle(InputValue input)
    {
        if (!isPaused){
            Vector2 inputVec = input.Get<Vector2>();
            
            float relMaxThrottle = maxThrottle;
            if (overheated){
                relMaxThrottle = safeThrottle;
            }
            // Debug.Log("scroll input: " + inputVec.y);
            if (inputVec.y > 0 && thrustInput < relMaxThrottle)
            {
                thrustInput = thrustInput+throttleIncrement;
            }else if (inputVec.y < 0 && thrustInput > minThrottle)
            {
                thrustInput = thrustInput-throttleIncrement;
            }
            // Debug.Log("Thrust Input " + thrustInput);
            throttleDisplay.text = thrustInput.ToString();
            // Will have to tune thrust a bit, might want to make finer adjustments at low throttle
            // Or less fine adjustmets at high throttle
            setThottlePips();
            float thrustVol = Mathf.Abs(thrustInput/10f);
            if (thrustVol <= 1f && thrustVol > 0){
                thrustVol = 1.5f;
            }
            audioMainEngine.pitch = thrustVol;
            audioMainEngine.volume = thrustVol/maxThrottle;
        }
    }

    public void OnHoldThrottle(InputValue input)
    {
        if (!isPaused){
            Vector2 inputVec = input.Get<Vector2>();
            
            float relMaxThrottle = maxThrottle;
            if (overheated){
                relMaxThrottle = safeThrottle;
            }
            // Debug.Log("scroll input: " + inputVec.y);
            if (inputVec.y > 0 && thrustInput < relMaxThrottle)
            {
                throttleInput = throttleIncrement;
                if (throttleCoroutine == null){
                    throttleCoroutine = StartCoroutine(ThrottleHeld());
                }
            }else if (inputVec.y < 0 && thrustInput > minThrottle)
            {
                throttleInput = -1*throttleIncrement;
                if (throttleCoroutine == null){
                    throttleCoroutine = StartCoroutine(ThrottleHeld());
                }
            }else{
                if (throttleCoroutine != null){
                    StopCoroutine(throttleCoroutine);
                    throttleCoroutine = null;
                    throttleInput = 0;
                }
            }
            // Debug.Log("Thrust Input " + thrustInput);
            
        }
    }

    IEnumerator ThrottleHeld()
    {
        while (throttleInput != 0 && thrustInput+throttleInput<=maxThrottle && thrustInput+throttleInput >=minThrottle){
            thrustInput = thrustInput+throttleInput;
            throttleDisplay.text = thrustInput.ToString();
            // Will have to tune thrust a bit, might want to make finer adjustments at low throttle
            // Or less fine adjustmets at high throttle
            setThottlePips();
            float thrustVol = Mathf.Abs(thrustInput/10f);
            if (thrustVol <= 1f && thrustVol > 0){
                thrustVol = 1.5f;
            }
            audioMainEngine.pitch = thrustVol;
            audioMainEngine.volume = thrustVol/maxThrottle;
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    public void OnSpin(InputValue input){
        Vector2 inputVec = input.Get<Vector2>();
        spinInput = inputVec.x;
    }

    public void OnBrake(InputValue input)
    {
        if (!isPaused){
            brakeInput = input.Get<float>();
            
            if (thrustInput > 0){
                // speedometer.color = new Color32(0,255,0,255);
                brakeInput = brakeInput*-1;
            }else if (thrustInput < 0){
                // speedometer.color = new Color32(255,0,0,255);
            }else{
                // speedometer.color = new Color32(0,0,255,255);
                brakeInput = 0;
            }
        }
        

        
    }

    public void OnZeroThrottle(InputValue input)
    {
        if (!isPaused){
            float inputVec = input.Get<float>();
            // Debug.Log("Brake " + inputVec);
            if (inputVec > 0){
                // braking = true;
                thrustInput = 0;
                // Debug.Log("Thrust Input " + thrustInput);
                throttleDisplay.text = thrustInput.ToString();
                setThottlePips();
                audioMainEngine.volume = 0;
            }
        }

        
    }

    public void OnExit (InputValue input)
    {
        // Debug.Log("Game exiting");
        // Application.Quit();
        if (isPaused){
            Resume();
        }else{
            Pause();
        }
    }

    public void Resume(){
        Time.timeScale = 1;
        isPaused = false;
        Cursor.visible = false;
        if(grapple){
            grapple.enabled = true;
        }
        pauseMenu.SetActive(false);
    }

    public void Pause(){
        Time.timeScale = 0;
        isPaused = true;
        Cursor.visible = true;
        if(grapple){
            grapple.enabled = false;
        }
        pauseMenu.SetActive(true);
    }

    // public void OnControllerCursor(InputValue input){
    //     Vector2 inputVec = input.Get<Vector2>();
    //     // float vecX = 0, vecY = 0;
    //     Debug.Log("inputVec: " + inputVec.x + ", " + inputVec.y);
    //     // if (inputVec.x > 0){
    //     //     vecX = 1;
    //     // }else if (inputVec.x < 0){
    //     //     vecX = -1;
    //     // }
    //     // if (inputVec.y > 0){
    //     //     vecY = 1;
    //     // }else if (inputVec.y < 0){
    //     //     vecY = -1;
    //     // }
    //     Vector2 currentPosition = Mouse.current.position.ReadValue();
    //     Mouse.current.WarpCursorPosition( currentPosition + inputVec*2);

    // }

    public void OnNavigate(){
        Debug.Log("NAVIGATING");
    }
    
    public void OnClick(){
        Debug.Log("Clicking!");
    }

    // private float Decrease (float val, float amount)
    // {
    //     if (val < 0){
    //         return val - amount;
    //     }
    //     if (val > 0){
    //         return amount - val;
    //     }
    //     return 0;
    // }

    

    void FixedUpdate()
    {
        
        //Actual forward speed, should be roughly thrust input when moving straight
        float forwardSpeed = Vector3.Dot(rb.velocity, transform.forward);
        float upSpeed = Vector3.Dot(rb.velocity, transform.up);
        float rightSpeed = Vector3.Dot(rb.velocity, transform.right);
        if (forwardSpeed < 0){
            speedometer.color = new Color32(0,0,255,255);
        }else{
            speedometer.color = new Color32(255,0,0,255);
        }
        speedometer.text = forwardSpeed.ToString("F1");
        upSpeedometer.text = upSpeed.ToString("F1");
        rightSpeedometer.text = rightSpeed.ToString("F1");
        Camera.main.fieldOfView = FOV + forwardSpeed*0.2f;
        setVelocityIndicator(vIndicator, upSpeed);
        setVelocityIndicator(hIndicator, rightSpeed);
        // Debug.Log(forwardSpeed);
        if (oxygen.enabled == false){
            if (forwardSpeed != 0 || upSpeed != 0 || rightSpeed != 0){
                oxygen.enabled = true;
            }
        }
        //Adding thrust
        rb.AddRelativeForce(new Vector3 (0,0,thrustInput) * thrustSpeed);
        //Overheat mechanic
        if (thrustInput > safeThrottle){
            heat = heat + ((thrustInput-safeThrottle)/10);
            overheatBar.transform.localScale = new Vector3 (0.8f - 0.8f*(heat/overheat), overheatBar.transform.localScale.y, overheatBar.transform.localScale.z);
            throttleDisplay.color = new Color32((byte)(255*(thrustInput/maxThrottle)),(byte)(255-255*(thrustInput/maxThrottle)),0,255);
        }else{
            if(heat > 0){
                heat = heat - cooldownIncrement;
                overheatBar.transform.localScale = new Vector3 (0.8f - 0.8f*(heat/overheat), overheatBar.transform.localScale.y, overheatBar.transform.localScale.z);
            }
            if(overheated){
                throttleDisplay.color = new Color32(255,255,0,255);
            }else{
                throttleDisplay.color = new Color32(0,255,0,255);
            }
            if (thrustInput < 0){
                throttleDisplay.color = new Color32(0,0,255,255);
            }
        }
        if (heat <= 0){
            overheated = false;
            heat = 0;
            // overheatBarRenderer.material.SetColor("_Color", Color.green);
            overheatBarRenderer.material = green;
            brakeSpeed = inertia1;
        }else{
            brakeSpeed = inertia2;
        }
        
        if (heat > overheat || overheated){
            throttleDisplay.text = thrustInput.ToString();
            // need a timer to reset heat
            if (overheated == false){
                thrustInput = 50;
                // overheatBarRenderer.material.SetColor("_Color", Color.red);
                
            overheatBarRenderer.material = yellow;
            setThottlePips();
            float thrustVol = Mathf.Abs(thrustInput/10f);
            if (thrustVol <= 1f && thrustVol > 0){
                thrustVol = 1.5f;
            }
            audioMainEngine.pitch = thrustVol;
            audioMainEngine.volume = thrustVol/maxThrottle;
            }
            overheated = true; 
        }
        
        //Adding FOV with speed. Could be smoothed out later
        
        //Strafing
        rb.AddRelativeForce(strafeInput * strafeSpeed);
        rb.AddRelativeForce(new Vector3 (0,0,brakeInput) * strafeSpeed);
        
        //Current solution for turning. Will eventually change to make turn speed change with mouse movement/position and speed.
        float turnDrag;
        if(thrustInput <= 15){
            turnDrag = turnDragConst;
        }else{
            if(thrustInput > safeThrottle){
                turnDrag = 1/(Mathf.Abs(thrustInput) - thrustInput*0.85f);
            }else{
                turnDrag = 1/(Mathf.Abs(thrustInput) - thrustInput*0.95f);
            }
            
            //  Debug.Log("turnDrag");
            //  Debug.Log(turnDrag);
        }
        //BASIC TURNING
        // rb.transform.Rotate(Vector3.up*turnInput.x*(turnSpeed*turnDrag));
        // rb.transform.Rotate(Vector3.left*turnInput.y*(turnSpeed*turnDrag));
        //PHYSICS BASED TURNING
        rb.AddRelativeTorque(Vector3.up*turnInput.x*(turnSpeed*turnDrag));
        rb.AddRelativeTorque(Vector3.left*turnInput.y*(turnSpeed*turnDrag));
        //Spin controls
        rb.AddRelativeTorque(Vector3.forward*spinInput*spinSpeed); 
        
        //Constant drag in all directions to make flying controllable
        rb.AddForce(rb.velocity*-brakeSpeed);
        rb.AddTorque(rb.angularVelocity * -1f);

        //handle sound
        if (strafeInput.x > 0 || spinInput > 0){
            if(!audioLThrust.isPlaying){
                audioLThrust.Play();
                audioRThrust.Stop();
                audioLThrust.loop = true;
            }
        }else if (strafeInput.x < 0 || spinInput < 0){
            if(!audioRThrust.isPlaying){
                audioRThrust.Play();
                audioLThrust.Stop();
                audioRThrust.loop = true;
            }
        }else{
            audioLThrust.Stop();
            audioRThrust.Stop();
        }
        if (strafeInput.y > 0 ){
            if(!audioUThrust.isPlaying){
                audioUThrust.Play();
                audioDThrust.Stop();
                audioUThrust.loop = true;
            }
        }else if (strafeInput.y < 0){
            if(!audioDThrust.isPlaying){
                audioDThrust.Play();
                audioUThrust.Stop();
                audioDThrust.loop = true;
            }
        }else{
            audioUThrust.Stop();
            audioDThrust.Stop();
        }
        if (brakeInput != 0){
            if(!audioBrakeThrust.isPlaying){
                audioBrakeThrust.Play();
                audioBrakeThrust.loop = true;
            }
        }else{
            audioBrakeThrust.Stop();
        }
    }
}
