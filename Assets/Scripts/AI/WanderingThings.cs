using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingThings : MonoBehaviour
{
    public bool repeat = true;
    public float speed = 10;
    public List<Transform> waypoints;
    public bool stop = false;
    public Vector3 size;
    
    private int waypointIndex;
    
    private float minDistance = 0.25f;

	private void Start()
	{
        size = transform.localScale;
	}
	void Update()
    {
        if(stop || waypoints.Count == 0)return;
        
        Transform tr = waypoints[waypointIndex];

        transform.position = Vector3.MoveTowards(transform.position, tr.position, (minDistance - 0.1f) * Time.deltaTime * speed);
        
        if ((transform.position - tr.position).sqrMagnitude < minDistance * minDistance)
        {
            int oldwaypoint = waypointIndex;

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

            //déplacement gauche/droite
            if (waypoints[waypointIndex].position.x > waypoints[oldwaypoint].position.x)
            {
				transform.localScale = size; // droite
            }
            else
            {
				transform.localScale = new Vector3(size.x*-1, size.y, 1); // gauche
            }
        }
    }
}
