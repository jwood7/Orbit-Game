using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int power;
    public int maxPower;
    private Animator doorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (power == maxPower){
            //openDoor
            doorAnimator.SetBool("IsPowered", true);
        }else{
            //closeDoor
            doorAnimator.SetBool("IsPowered", false);
        }
    }
}
