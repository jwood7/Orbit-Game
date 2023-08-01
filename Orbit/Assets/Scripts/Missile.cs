using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public float lifetime;
    private Rigidbody rb;
    public float missileSpeed;
    public string targetTag;
    public float rotateSpeed;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target){
            // this.transform.LookAt(target.transform);
            // var rotation = Quaternion.LookRotation(heading);
            var heading = target.transform.position - transform.position;

            var rotation = Quaternion.LookRotation(heading);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));

            rb.AddRelativeForce(new Vector3 (0,0,1) * missileSpeed);
            rb.AddForce(rb.velocity*-0.9f);
        }else{
            target = GameObject.FindGameObjectWithTag(targetTag);
        }
        
    }

    void OnCollisionEnter(Collision col){
        Destroy(this.gameObject);
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
    }
}
