using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SucklerGraphics : MonoBehaviour
{
    SucklerController controller;
    public AIPath aiPath;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<SucklerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.boss)
        {
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                transform.localScale = new Vector3(-1.75f, 1.75f, 1.75f);
            }
            else if (aiPath.desiredVelocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
            }
        }
        else
        {
            if (aiPath.desiredVelocity.x >= 0.01f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (aiPath.desiredVelocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
            
    }
}
