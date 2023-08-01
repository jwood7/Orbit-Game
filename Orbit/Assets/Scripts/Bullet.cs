using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifetime = 30f;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Despawn());
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
