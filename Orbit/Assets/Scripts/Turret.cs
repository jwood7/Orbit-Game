using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject player;
    public GameObject projectile;
    public GameObject brokenTurret;
    public Transform firePoint;
    public float projectileSpeed;
    public float range;
    public float reloadTime;
    public float burstGap;
    public float burstLength;
    private bool reloaded;
    private bool notInstantiated;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        reloaded = true;
        notInstantiated = true;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.LookAt(player.transform);
        if (Vector3.Distance (player.transform.position, this.transform.position) < range && reloaded){
            StartCoroutine(Fire());
        }
        
    }

    void OnCollisionEnter(Collision col){
        Debug.Log("TURRET BROKEN");
        if (notInstantiated){
            GameObject newTurret = Instantiate (brokenTurret, this.transform.position, this.transform.rotation);
            notInstantiated = false;
            var rbs = newTurret.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs) {
                // rb.AddExplosionForce(col.relativeVelocity.magnitude * 100,col.contacts[0].point,2);
                rb.AddExplosionForce(300, col.transform.position, 5,1);
            }
        }
        Destroy(this.gameObject);
    }

    IEnumerator Fire()
    {
        reloaded = false;
        for (int i = 0; i< burstLength; i++){
            GameObject bullet = Instantiate (projectile, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*projectileSpeed, ForceMode.Impulse);
            yield return new WaitForSeconds(burstGap);
        }
        yield return new WaitForSeconds(reloadTime);
        reloaded = true;
        
    }
}
