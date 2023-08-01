using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObjective : MonoBehaviour
{
    public Transform objectiveObject;
    public Transform mothership;
    private GameObject[] objectiveObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (objectiveObject != null){
            transform.LookAt(objectiveObject);
        }else{
            // transform.rotation = Quaternion.identity;
            transform.LookAt(mothership);
            objectiveObjects = GameObject.FindGameObjectsWithTag("Objective Item");
            if (objectiveObjects.Length > 0){
                objectiveObject = objectiveObjects[0].transform;
            }
        }
    }
}
