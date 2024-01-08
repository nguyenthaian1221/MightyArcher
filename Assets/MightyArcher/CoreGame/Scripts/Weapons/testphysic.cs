using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testphysic : MonoBehaviour
{

    public float fieldOfImpact;
    public float explosionForce;
    public LayerMask layer;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Explosion();
    //    }
    //}


    public void Explosion()
    {

        //GameObject _explosion = Instantiate(explosion, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, fieldOfImpact,layer);

        foreach (Collider target in colliders)
        {
            string oTag = target.gameObject.tag;
            if (oTag == "Player" || oTag == "player2")
            {
                Vector2 direction = target.transform.position - transform.position;
                target.GetComponent<Rigidbody>().AddForce(direction * explosionForce);
            }

            //       
        }
    }


}
