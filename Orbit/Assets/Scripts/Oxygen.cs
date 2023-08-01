using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Oxygen : MonoBehaviour
{
    
    public GameObject oxygenBar;
    public float oxygen;
    public float maxOxygen;
    public float rechargeRate;
    public float depletionRate;
    public bool recharging;
    public Image blackout;
    public float blackoutOpacity;
    public float blackoutRate;
    public GameObject oxygenTank;
    public GameObject usedTank;
    public GameObject blackBox;
    public GameObject usedBox;
    public GameObject gun;
    public GameObject usedGun;
    public Gun gunScript;
    public TextMeshProUGUI objectiveDisplay;
    public bool holdingItem;
    public Rigidbody rb;
    public float o2Amt;
    public float o2Max;
    public MeshRenderer[] indicatorsT;
    public MeshRenderer[] indicatorsS;
    public MeshRenderer indicatorEmpty;
    public Material fullColor;
    public Material emptyColor;
    public Transform holdPoint;
    public GameObject heldObject;
    public float throwForce; 
    public AudioClip impact;
    public AudioClip dock;
    public AudioClip release;
    AudioSource audioSource;
    public float savedDRate;
    public float savedRRate;
    public Grapple grapple;
    public Transform firePoint;

    // Start is called before the first frame update
    void Awake()
    {
        oxygen = maxOxygen;
        blackoutOpacity = 0;
        blackBox.SetActive(false);
        oxygenTank.SetActive(false);
        holdingItem = false;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        gunScript = this.gameObject.GetComponent<Gun>();
        gun.SetActive(false);
        if (gunScript){
            gunScript.enabled = false;
        }
        grapple = this.gameObject.GetComponent<Grapple>();
        if (grapple){
            grapple.enabled = true;
        }

    }

    void setO2Indicators(){
        if (o2Amt >= o2Max*(5/6f) ){
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;

            }else if (o2Amt < o2Max*(5/6f) && o2Amt > o2Max*(5/6f)){
                for ( int i = 0; i < 5; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 5; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                indicatorsT[5].material = emptyColor;
                indicatorsS[5].material = emptyColor;

            }else if (o2Amt < o2Max*(4/6f) && o2Amt > o2Max*(3/6f)){
                for ( int i = 0; i < 4; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 4; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                for ( int i = 4; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 4; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }

            }else if (o2Amt < o2Max*(3/6f) && o2Amt > o2Max*(2/6f)){
                for ( int i = 0; i < 3; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 3; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                for ( int i = 3; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 3; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }

            }else if (o2Amt < o2Max*(2/6f) && o2Amt > o2Max*(1/6f)){
                for ( int i = 0; i < 2; i++)
                {
                    indicatorsT[i].material = fullColor;
                }
                for ( int i = 0; i < 2; i++)
                {
                    indicatorsS[i].material = fullColor;
                }
                indicatorEmpty.material = fullColor;
                for ( int i = 2; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 2; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }

            }else if (o2Amt < o2Max*(1/6f) && o2Amt > 0){
                indicatorsS[0].material = fullColor;
                indicatorsS[0].material = fullColor;
                indicatorEmpty.material = fullColor;
                for ( int i = 1; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 1; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }
            }else if (o2Amt <= 0){
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsT[i].material = emptyColor;
                }
                for ( int i = 0; i < 6f; i++)
                {
                    indicatorsS[i].material = emptyColor;
                }
                indicatorEmpty.material = emptyColor;

            }
    }

    public void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Oxygen Tank" && !holdingItem)
        {   
            O2Storage O2Storage = col.gameObject.GetComponent<O2Storage>();
            if (oxygen < maxOxygen*.99 && O2Storage.amt > 0){
                recharging = true;
                o2Amt = O2Storage.amt;
                o2Max = O2Storage.maxAmt;
                Destroy(col.gameObject);
                holdingItem = true;
                rb.angularVelocity = new Vector3(0,0,0);
                setO2Indicators();
                audioSource.PlayOneShot(dock, 0.7F);
            }
        }else if(col.collider.tag == "Gun" && !holdingItem)
        {
            Ammo ammo = col.gameObject.GetComponent<Ammo>();
            if (ammo.amt > 0){
                gun.SetActive(true);
                gunScript.enabled = true;
                
                gunScript.ammo = ammo.amt;
                Destroy(col.gameObject);
                holdingItem = true;
                rb.angularVelocity = new Vector3(0,0,0);
                audioSource.PlayOneShot(dock, 0.7F);
                grapple.enabled = false;
            }
        }else if (col.collider.tag == "Objective Item" && !holdingItem){
            Destroy(col.gameObject);
            blackBox.SetActive(true);
            // objectiveDisplay.text = "Objective: \n - Deliver Black Box to Start Zone";
            holdingItem = true;
            rb.angularVelocity = new Vector3(0,0,0);
            audioSource.PlayOneShot(dock, 0.7F);
        }else if ((col.collider.tag == "Pick Up" || col.collider.tag == "Battery") && !holdingItem){
            col.transform.SetParent(gameObject.transform);
            holdingItem = true;
            Rigidbody colRb = col.gameObject.GetComponent<Rigidbody>();
            colRb.velocity = new Vector3(0,0,0);
            colRb.isKinematic = true;
            colRb.detectCollisions = false;
            col.transform.position = holdPoint.position;
            col.transform.rotation = holdPoint.rotation;
            heldObject = col.gameObject;
            audioSource.PlayOneShot(dock, 0.7F);
        }else{
            if ((!holdingItem || col.gameObject != heldObject)  && (col.collider.tag != "Objective Item" && col.collider.tag != "Oxygen Tank")){
                
                
                Debug.Log("Crash");
                oxygen = oxygen - (depletionRate * 500); // May want to multiply this by velocity later
                if (oxygen <= 0){
                    oxygen = 0;
                }
                audioSource.PlayOneShot(impact, 0.7F);
                oxygenBar.transform.localScale = new Vector3 (0.8f*(oxygen/maxOxygen), oxygenBar.transform.localScale.y, oxygenBar.transform.localScale.z);
            }else{
                Debug.Log("BAD COLLISION");
            }
        }
    }

    public void OnTriggerEnter(Collider col){
        if(col.tag == "Retrieval Zone" && holdingItem && blackBox.activeSelf)
        {
            blackBox.SetActive(false);
            objectiveDisplay.text = "Objective: \n - Delivered Black Box\n - All Objectives Complete!";
            holdingItem = false;
            Cursor.visible = true; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void OnRelease(){
        if(holdingItem){
            //instantiate object, or unparent. Based on what is active 
            if (blackBox.activeSelf){
                // release the black box
                blackBox.SetActive(false);
                holdingItem = false;
                Vector3 bbPos = blackBox.transform.position;
                Quaternion bbRot = blackBox.transform.rotation;
                GameObject ejectedBox = Instantiate (usedBox, bbPos, bbRot);
                ejectedBox.GetComponent<Rigidbody>().velocity = rb.velocity;
                ejectedBox.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up*-1.5f, ForceMode.Impulse);
                audioSource.PlayOneShot(release, 0.7F);
            }else if (oxygenTank.activeSelf){
                recharging = false;
                oxygenTank.SetActive(false);
                holdingItem = false;
                Vector3 O2Pos = oxygenTank.transform.position;
                Quaternion O2Rot = oxygenTank.transform.rotation;
                GameObject ejectedTank = Instantiate (usedTank, O2Pos, O2Rot);
                ejectedTank.GetComponent<Rigidbody>().velocity = rb.velocity;
                ejectedTank.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up*throwForce, ForceMode.Impulse);
                ejectedTank.GetComponent<O2Storage>().amt = o2Amt;
                audioSource.PlayOneShot(release, 0.7F);
            }else if (gun.activeSelf){
                gun.SetActive(false);
                holdingItem = false;
                Vector3 gunPos = firePoint.transform.position;
                Quaternion gunRot = firePoint.transform.rotation;
                GameObject ejectedGun = Instantiate (usedGun, gunPos, gunRot);
                ejectedGun.GetComponent<Rigidbody>().velocity = rb.velocity;
                ejectedGun.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*throwForce, ForceMode.Impulse);
                ejectedGun.GetComponent<Ammo>().amt = gunScript.ammo;
                grapple.enabled = true;
                gunScript.enabled = false;
                gunScript.ammo = 0;
            }else{
                heldObject.transform.SetParent(null);
                holdingItem = false;
                Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
                heldRb.isKinematic = false;
                heldRb.detectCollisions = true;
                heldRb.velocity = new Vector3(0,0,0);
                heldRb.velocity = rb.velocity;
                heldRb.AddRelativeForce(Vector3.forward*throwForce, ForceMode.Impulse);
                heldObject = null;
                audioSource.PlayOneShot(release, 0.7F);
            }
        }
    }

    public void OnExit ()
    {
        if (rechargeRate == 0 && depletionRate == 0){
            rechargeRate = savedRRate;
            depletionRate = savedDRate;
            blackout.color = new Color(blackout.color.r, blackout.color.g, blackout.color.b, 0);
        }else{
            savedDRate = depletionRate;
            savedRRate = rechargeRate;
            depletionRate = 0;
            rechargeRate = 0;
            blackout.color = new Color(blackout.color.r, blackout.color.g, blackout.color.b, 0.93f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (oxygen > 0){
            oxygen = oxygen - depletionRate;
            oxygenBar.transform.localScale = new Vector3 (0.8f*(oxygen/maxOxygen), oxygenBar.transform.localScale.y, oxygenBar.transform.localScale.z);
        }
        if(recharging){
            oxygenTank.SetActive(true);
            blackoutOpacity = 0;
            blackout.color = new Color(blackout.color.r, blackout.color.g, blackout.color.b, blackoutOpacity);
            oxygen = oxygen + rechargeRate;
            o2Amt = o2Amt - rechargeRate;
            oxygenBar.transform.localScale = new Vector3 (0.8f*(oxygen/maxOxygen), oxygenBar.transform.localScale.y, oxygenBar.transform.localScale.z);
            setO2Indicators();
            if (oxygen >= maxOxygen || o2Amt <= 0){
                recharging = false;
                if (oxygen >= maxOxygen) oxygen = maxOxygen;
                //hide oxygen tank
                oxygenTank.SetActive(false);
                holdingItem = false;
                //instantiate used oxygen tank
                Vector3 O2Pos = oxygenTank.transform.position;
                Quaternion O2Rot = oxygenTank.transform.rotation;
                GameObject ejectedTank = Instantiate (usedTank, O2Pos, O2Rot);
                ejectedTank.GetComponent<Rigidbody>().velocity = rb.velocity;
                ejectedTank.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up*throwForce, ForceMode.Impulse);
                ejectedTank.GetComponent<O2Storage>().amt = o2Amt;
                if (o2Amt == 0){
                    ejectedTank.tag = "Untagged";
                }
            }
        }
        if (gun.activeSelf){
            if(gunScript.ammo <= 0){
                gun.SetActive(false);
                holdingItem = false;
                Vector3 gunPos = firePoint.transform.position;
                Quaternion gunRot = firePoint.transform.rotation;
                GameObject ejectedGun = Instantiate (usedGun, gunPos, gunRot);
                ejectedGun.GetComponent<Rigidbody>().velocity = rb.velocity;
                ejectedGun.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*throwForce, ForceMode.Impulse);
                ejectedGun.GetComponent<Ammo>().amt = gunScript.ammo;
                grapple.enabled = true;
                gunScript.enabled = false;

            }
        }

        

        if(oxygen <= 0){
            blackoutOpacity = blackoutOpacity + blackoutRate;
            // Debug.Log(blackoutOpacity);
            blackout.color = new Color(blackout.color.r, blackout.color.g, blackout.color.b, blackoutOpacity);
            if (blackoutOpacity >= 1){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        
    }
}
