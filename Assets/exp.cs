using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exp : MonoBehaviour
{
    [SerializeField] LayerMask fl;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + new Vector3(0.0f,3.0f,0.0f), Vector3.down , out hit , 3.5f , fl ))
        {
            Face face = hit.transform.gameObject.GetComponent<Face>();
            print(face.GetFaceValue);
        }
    }
}
