using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    //OVERLAP PARAMETERS
    [SerializeField]
    private Transform detectorOG;
    public Vector2 detectorSize= Vector2.one;
    public Vector2 detectorOGOffSet = Vector2.zero;

    public float detectionDelay=0.3f;
    public LayerMask detectorLayerMask;


    //GIZMO
    public Color noDetectColor = Color.green;
    public Color detectColor = Color.red;
    public bool showGizmos=true;

    private GameObject target=null;
    public bool detected = false;

    private void Start()
    {
        StartCoroutine(DetectionCourutine());
    }

    IEnumerator  DetectionCourutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        PerformDetection();
        StartCoroutine(DetectionCourutine());
    }

    private void PerformDetection()
    {
        Collider2D coll = Physics2D.OverlapBox((Vector2)detectorOG.position + detectorOGOffSet, detectorSize, 0, detectorLayerMask);
        if (coll != null)
        {
            target = coll.gameObject;
            detected = true;
        }
        else
        {
            detected = false;
            target=null;
        }
    }

    private void OnDrawGizmos()
    {
        if(showGizmos && detectorOG != null)
        {
            Gizmos.color = noDetectColor;
            Gizmos.DrawCube((Vector2)detectorOG.position + detectorOGOffSet, detectorSize);

            if (detected)
            {
                Gizmos.color = detectColor;
                Gizmos.DrawCube((Vector2)detectorOG.position + detectorOGOffSet, detectorSize);

            }
        }
        
    }

}
