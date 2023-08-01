using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed;
    public float reloadTime;
    public float burstGap;
    public float burstLength;
    private bool reloaded;
    public int ammo;
    public Image crosshair;
    public AudioClip gunshot;
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void Awake()
    {
        
        crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
        reloaded = true;
        
    }

    // Update is called once per frame
    void OnFire()
    {
        if (reloaded && ammo > 0 && this.enabled){
            StartCoroutine(Fire());
        }
        
    }

    void Update(){
        crosshair.color = new Color(255, 255, 0, crosshair.color.a);
    }

    IEnumerator Fire()
    {
        reloaded = false;
        for (int i = 0; i< burstLength && ammo > 0; i++){
            GameObject bullet = Instantiate (projectile, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward*projectileSpeed, ForceMode.Impulse);
            ammo = ammo - 1;
            audioSource.PlayOneShot(gunshot, 0.2F);
            yield return new WaitForSeconds(burstGap);
        }
        yield return new WaitForSeconds(reloadTime);
        reloaded = true;
        
    }

    
}