using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class Positioning : MonoBehaviour
{

    [SerializeField] private Transform car;

    private void OnDrawGizmos()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit))
        {
            //if we hit something
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.1f);

            //normal
            MyDraw.DrawVectorAt(hit.point, 1f * hit.normal, Color.green, 4f);

            //"look direction"
            MyDraw.DrawVectorAt(hit.point, 1f * transform.right, Color.magenta, 4f);

            //use cross-product to get "right"-vector and draw it
            Vector3 right = Vector3.Cross(hit.normal, transform.right).normalized;
            MyDraw.DrawVectorAt(hit.point, 1f * right, Color.red, 4f);

            //use cross-product to get "forward"-vector and draw it
            Vector3 forward = Vector3.Cross(right, hit.normal);
            MyDraw.DrawVectorAt(hit.point, 1f * forward, Color.blue, 4f);


            car.SetLocalPositionAndRotation(hit.point, quaternion.LookRotation(right, hit.normal));

        }
    }
}
