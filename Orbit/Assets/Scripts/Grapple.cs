using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Grapple : MonoBehaviour
{
    public Transform player;
    public float maxGrappleDistance;
    public int grappleable; //Layer mask number
    public Vector3 grapplePoint;
    public Transform grappleStart;
    public LineRenderer line; 
    public GameObject hitObject;
    public Rigidbody hitRb;
    public int reelSpeed;
    public bool reeling;
    public Rigidbody playerRb;
    
    public Material reelingColor;
    public Material inactiveColor;
    public Material attachedColor;
    public Material reelableColor;
    public Material disabledColor;

    public MeshRenderer indicator1;
    public MeshRenderer indicator2;
    public Image crosshair;
    public AudioSource audioGrappleReel;
    AudioSource audioSource;
    public AudioClip grappleHit;
    public AudioClip grappleHit2;
    public AudioClip grappleLaunch;
    public AudioClip grappleRelease;
    public GameObject grapplePointObject;
    // Start is called before the first frame update
    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        grappleable = 1 << 8;
        grappleable = ~grappleable; //everything but layer 8;
        line.enabled = false;
        indicator1.material = inactiveColor;
        audioSource = GetComponent<AudioSource>();
    }

    void DelayedUpdate()
    {
        //In order to briefly display grapple before you miss
    }

    // Update is called once per frame
    void OnFire() {
        RaycastHit hit;
        reeling = false;
        indicator2.material = disabledColor;
        if (!line.enabled && this.enabled){
            audioSource.PlayOneShot(grappleLaunch, 0.8F);
            if (Physics.Raycast(player.position, player.forward, out hit, maxGrappleDistance, grappleable, QueryTriggerInteraction.Ignore))
            {
                //hit
                Debug.Log("HIT!");
                grapplePoint = hit.point;
                grapplePointObject =  new GameObject("grapplePointObject");
                grapplePointObject.transform.position = grapplePoint;
                hitObject = hit.transform.gameObject;
                grapplePointObject.transform.parent = hitObject.transform;
                if(hitObject.GetComponent<Rigidbody>() != null)
                {
                    hitRb = hitObject.GetComponent<Rigidbody>();
                    // grapplePoint = hitObject.transform.position;
                    indicator1.material = reelableColor;
                    audioSource.PlayOneShot(grappleHit, 0.4F);
                }else{
                    hitRb = null;
                    indicator1.material = attachedColor;
                    audioSource.PlayOneShot(grappleHit2, 0.4F);
                }
                
                indicator2.material = inactiveColor;
                //get object that is struck
                line.enabled = true;
                line.SetPosition(1, grapplePoint);

            }
            else{
                //miss
                grapplePoint = player.position + player.forward * maxGrappleDistance;
                line.enabled = false;
                reeling = false;
                indicator1.material = inactiveColor;
            }
        }else{
            if (this.enabled){
                line.enabled = false;
                reeling = false;
                indicator1.material = inactiveColor;
                audioSource.PlayOneShot(grappleRelease, 0.2F);
                if(grapplePointObject){
                    Destroy(grapplePointObject);
                    grapplePointObject = null;
                }
            }
        }
        
    }

    void OnReel(InputValue input){
        if (this.enabled){
            Debug.Log("reeled");
            float inputVec = input.Get<float>();
            // Debug.Log(inputVec);
            if (inputVec == 0){
                reeling = false;
                if (line.enabled){
                    indicator1.material =  (hitRb != null) ? reelableColor : attachedColor ;
                }
                // line.enabled = false;
            }else{
                if (line.enabled){
                    if (hitRb != null){
                        grapplePoint = grapplePointObject.transform.position;
                    }
                    reeling = true;
                    // indicator2.material = reelingColor;
                }
            }
            // hitObject.transform.LookAt(grappleStart);
            // hitRb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
    }

    void OnCollisionEnter(){
        // Debug.Log("Crashed");
        audioGrappleReel.Stop();
        reeling = false;
        line.enabled = false;
        indicator1.material = inactiveColor;
    }

    void FixedUpdate()
    {
        //BASIC FUNCTIONALITY
        // ray cast from point to object
        // on hit, addForce to object towards the space ship??
        
        
        if (reeling){
            if (hitRb != null){
                hitRb.AddForce(hitRb.velocity * -0.5f);
                hitRb.AddForceAtPosition((grappleStart.position - hitObject.transform.position) * reelSpeed, grapplePoint);
                playerRb.AddForce(( grapplePoint - grappleStart.position) * reelSpeed*hitRb.mass/playerRb.mass*0.5f * 0.5f);
                grapplePoint = grapplePointObject.transform.position;
                indicator2.material = reelableColor;
                if(!audioGrappleReel.isPlaying){
                    audioGrappleReel.Play();
                    audioGrappleReel.loop = true;
                }
            }else{
                playerRb.AddForce(( grapplePoint - grappleStart.position) * reelSpeed * 0.5f);
                //addForce to player RB in direction of grapple point
                indicator2.material = attachedColor;
                if(!audioGrappleReel.isPlaying){
                    audioGrappleReel.Play();
                    audioGrappleReel.loop = true;
                }
                
            }
        }else{
            indicator2.material = inactiveColor;
                audioGrappleReel.Stop();
        }
        if (line.enabled){
            line.SetPosition(0, grappleStart.position);
            if (hitRb != null){
                grapplePoint = grapplePointObject.transform.position;
            }
            line.SetPosition(1, grapplePoint);
            if(hitRb != null && Vector3.Distance (grappleStart.position, hitObject.transform.position) > 10){
                hitRb.AddForceAtPosition((grappleStart.position - hitObject.transform.position) * 0.1f * reelSpeed, grapplePoint);
                hitRb.AddForce(hitRb.velocity * -0.5f);
            }

            if (Vector3.Distance (grappleStart.position, grapplePoint) > maxGrappleDistance){
                reeling = false;
                line.enabled = false;
                indicator1.material = inactiveColor;      
            }

            
            
        }else{
            indicator2.material = disabledColor;
        }
        //crosshair color 
        RaycastHit seen;
        if (!line.enabled){
            if (Physics.Raycast(player.position, player.forward, out seen, maxGrappleDistance, grappleable, QueryTriggerInteraction.Ignore))
            {
                GameObject seenObject = seen.transform.gameObject;
                if(seenObject.GetComponent<Rigidbody>() != null)
                {
                    crosshair.color = new Color(0, 0, 255, crosshair.color.a);
                }else{
                    crosshair.color = new Color(255, 255, 0, crosshair.color.a);
                }

            }
            else{
                crosshair.color = new Color(255, 0, 0, crosshair.color.a);
            }
        }else{
            crosshair.color = new Color(255, 0, 0, crosshair.color.a);
        }
        
        
        //MORE ADVANCED
        // on left click, ray cast from point to object
        // on hit, addForce to keep object x distance from space ship
        // on right click, decrease distance from space ship. Distances decreases as right click held

        //MORE ADVANCED
        // if object smaller/lighter than player -> do same as before
        // if object larger/heavier than player -> 
        // Add force to keep space ship from exceeding x distance from object
        // right click decreases that. Adds force towards object
        // if object simmiliar size to player, force is applied  to both object and space ship
        // to keep them x distance apart or less
    }
}
