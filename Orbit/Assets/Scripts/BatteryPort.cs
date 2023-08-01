using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPort : MonoBehaviour
{
    public Door door;
    private bool powered;
    public MeshRenderer wire;
    public Material green;
    public Material clear;
    public Transform batteryTransform;
    // Start is called before the first frame update
    void Awake()
    {
        wire.material = clear;
    }

    void OnTriggerEnter(Collider col){
        if (col.tag == "Battery" && !powered){
            door.power++;
            powered = true;
            wire.material = green;
            col.transform.position = batteryTransform.position;
            col.transform.rotation = batteryTransform.rotation;
            col.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        }
    }
    void OnTriggerExit(Collider col){
        if (col.tag == "Battery" && powered){
            door.power--;
            powered = false;
            wire.material = clear;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
