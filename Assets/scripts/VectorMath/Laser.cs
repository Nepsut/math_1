using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private int reflectionNumber = 3;

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Gizmos.color = Color.red;

        Vector3 prevPoint = transform.position;
        Vector3 lastRefDir = transform.right;

        for (int i = 0; i <= reflectionNumber; i++) 
        {
            if (!Physics.Raycast(prevPoint, lastRefDir, out RaycastHit hit)) break;
            Vector3 reflection = Vector3.Reflect(lastRefDir, hit.normal);

            Handles.DrawLine(prevPoint, hit.point, 3.0f);
            Gizmos.DrawSphere(hit.point, 0.1f);

            prevPoint = hit.point;
            lastRefDir = reflection;
        }

    }
}
