using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{

    public bool spinEnabled;
    public bool strafeEnabled;
    public bool throttleEnabled;
    public bool grappleEnabled;
    public Grapple grapple;
    public bool oxygenEnabled;
    public Oxygen oxygen;
    public bool overheatEnabled;
    public TextMeshProUGUI objectiveDisplay;
    public int stage;


    
    void Awake()
    {
        // grapple = GetComponent<Grapple>();
        oxygen = GetComponent<Oxygen>();
        oxygen.enabled = false;
        stage = 0;
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        if (inputVec.y != 0 && stage == 1){
            stage = 2;
        }
        if (inputVec.x != 0 && stage == 3){
            stage = 4;
        }
        

    }

    public void OnLook(InputValue input)
    {
        if (stage == 0){
            stage = 1;
        }
        
    }

    public void OnThrottle(InputValue input)
    {
        if (stage == 6){
            stage = 7;
        }
    }

    public void OnSpin(InputValue input){
        if (stage == 5){
            stage = 6;
        }
    }

    public void OnBrake(InputValue input)
    {
        if (stage == 9){
            stage = 10;
        }

        
    }

    void OnFire() {
        if(stage == 10){
            stage = 11;
        }
    }

    void OnReel(InputValue input){
        if(stage == 11){
            stage = 12;
        }
    }

    void OnTab(){
        //Increase stage
        stage = stage+1;
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Oxygen Tank" && stage == 17)
        {
            stage = 18;
        }else if (col.collider.tag == "Objective Item" && stage == 18){
            //Switch to first level
            SceneManager.LoadScene("SampleScene");
        }
    }


    void FixedUpdate()
    {
        if (stage == 0){
            objectiveDisplay.text = "Turn/look by moving your mouse";
        }
        if (stage == 1){
            objectiveDisplay.text = "Strafe up or down by using W and S.";
            
        }
        if (stage == 2){
            objectiveDisplay.text = "This also changes the color of your vertical speedometer on the left side of your screen.\nHit tab to continue.";
        }
        if (stage == 3){
            objectiveDisplay.text = "Strafe left or right by using A and D.";
        }
        if (stage == 4){
            objectiveDisplay.text = "This also changes the color of your horizontal speedometer on the left side of your screen.\nHit tab to continue.";
        }
        if (stage == 5){
            objectiveDisplay.text = "Spin using Q and E";
        }
        if (stage == 6){
            objectiveDisplay.text = "The throttle is visible on the right. Set your throttle using scroll wheel.";
        }
        if (stage == 7){
            objectiveDisplay.text = "The throttle goes from -20 to 50. It turns blue when in reverse.\nHit tab to continue.";
        }
        if (stage == 8){
            objectiveDisplay.text = "Your forward speed is measured by the large number on the right of the dashboard\nHit tab to continue.";
        }
        if (stage == 9){
            objectiveDisplay.text = "Reset your throttle to 0 using space bar";
        }
        if (stage == 10){
            objectiveDisplay.text = "Launch and release your grapple using left click";
        }
        if (stage == 11){
            objectiveDisplay.text = "Reel in your grapple by holding right click. This changes the color of the grapple light on the side of this screen";
        }
        if (stage == 12){
            objectiveDisplay.text = "Your crosshair will change color based on how the grapple will interact with objects. \nHit tab to continue.";
        }
        if (stage == 13){
            objectiveDisplay.text = "Red crosshair = grapple is in use or out of range\nHit tab to continue."; 
        }
        if (stage == 14){
            objectiveDisplay.text = "Yellow crosshair = grapplable static object that the grapple pulls you towards\nHit tab to continue.";
        }
        if (stage == 15){
            objectiveDisplay.text = "Blue crosshair = grapplable object you can pull towards you \nHit tab to continue.";
        }
        if (stage == 16){
            objectiveDisplay.text = "Your ship is slowly leaking oxygen. Your oxygen is visible on the top of screen.\nHit tab to continue.";
            oxygen.enabled = true;
        }
        if (stage == 17){
            objectiveDisplay.text = "You lose additional oxygen by hitting objects. Refill your oxygen by colliding with blue oxygen canisters";
            oxygen.enabled = true;
        }
        if (stage == 18){
            //set the black box to be active
            objectiveDisplay.text = "Objective: \n Pick up a black box to finish the tutorial";
        }
    }
}
