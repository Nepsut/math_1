using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawLine(transform.position, transform.position + transform.right * 5f, 3f);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit))
        {
            //if we hit something
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);

            //normal
            MyDraw.DrawVectorAt(hit.point, 3f * hit.normal, Color.green, 3f);

            //reflection
            Vector3 reflection = transform.right - 2 * (Vector3.Dot(transform.right, hit.normal)) * hit.normal;

            MyDraw.DrawVectorAt(hit.point, 2.5f * reflection, Color.red, 3f); //or Vector3.Reflect()

            //Vector3 reflection2 = Vector3.Reflect(transform.right, hit.point); //??????
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
