using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingThings : MonoBehaviour
{
    public bool repeat = true;
    public float speed = 10;
    public List<Transform> waypoints;
    public bool stop = false;
    
    private int waypointIndex;
    

    private float minDistance = 0.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stop)return;
        
        Transform tr = waypoints[waypointIndex];

        transform.position = Vector3.MoveTowards(transform.position, tr.position, (minDistance - 0.1f) * Time.deltaTime * speed);
        
        if ((transform.position - tr.position).sqrMagnitude < minDistance * minDistance)
        {
            if (repeat)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Count;
            }
            else
            {
                waypointIndex++;
                if (waypointIndex > waypoints.Count - 1)
                {
                    stop = true;
                }
            }
        }
    }
}
