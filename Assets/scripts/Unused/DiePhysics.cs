using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePhysics : MonoBehaviour
{
    private BoxCollider box = null;
    private Rigidbody rb = null;
    private bool inside_Cup = true;

    private Vector3[] fourDirections = { Vector3.up , Vector3.down , Vector3.right ,Vector3.left};

    [SerializeField] private Vector3 minimumVelocity = Vector3.zero;
    [SerializeField] Transform[] repulsionPoints = new Transform[9];
    [SerializeField] LayerMask dieLayer = new LayerMask();
    [SerializeField] float rayLength = 2.0f;
    [SerializeField] float pushForce = 10.0f;
    [SerializeField] float torgueAmount = 1.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        if(!inside_Cup && rb.velocity.magnitude > minimumVelocity.magnitude)
        RepelDice();
    }

    void RepelDice()
    {
        print("repeling " + gameObject.name);

        foreach(Transform cornerP in repulsionPoints)
        {
            RaycastHit hit;
            if(Physics.Raycast(cornerP.position , Vector3.down , out hit , rayLength , dieLayer))
            {
                if (hit.transform != cornerP.transform.parent)
                {
                    rb.AddForceAtPosition(Vector3.up * pushForce * Time.deltaTime, cornerP.position);
                }
            }
        }

        foreach(Vector3 direction in fourDirections)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position , direction , out hit , rayLength , dieLayer))
            {
                rb.AddForceAtPosition(hit.normal * pushForce * Time.deltaTime, hit.point);
            }
        }

    }

    

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Cup")
        {
            inside_Cup = false;

            rb.AddTorque(Random.Range(10f, torgueAmount), Random.Range(10f, torgueAmount), Random.Range(10f, torgueAmount));
        }
    }
}
