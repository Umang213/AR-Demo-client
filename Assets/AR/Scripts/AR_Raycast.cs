using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Raycast : MonoBehaviour
{
    public Camera arCamera; 
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ProcessHit(hit.collider.gameObject);
        }
    }

    private void ProcessHit(GameObject hitObject)
    {
        
    }
}
