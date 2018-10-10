using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {


    public List<WayPoint> neighbors;

    public WayPoint previous { get; set; }
    public float distance { get; set; }

    public void OnDrawGizmos()
    {
        if (neighbors == null)
            return;
        Gizmos.color = new Color(0f, 0f, 0f);
        foreach(var neighbor in neighbors)
        {
            if (neighbor != null)
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
