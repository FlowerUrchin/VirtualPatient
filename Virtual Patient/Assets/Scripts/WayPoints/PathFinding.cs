using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    public float walkSpeed = 5.0f;

    public Stack<Vector3> currentPath;
    public Player.position movingTo;
    public Vector3 currentWayPointPosition;
    public float moveTimeTotal;
    public float moveTimeCurrent;

    public void NagivateTo(Vector3 destination)
    {

        currentPath = new Stack<Vector3>();

        var currentNode = FindClosestWayPoint(transform.position);
        var endNode = FindClosestWayPoint(destination);

        if(currentNode == null || endNode == null || currentNode == endNode)
        {
            currentPath = null;
            return;
        }

        var closedList = new List<WayPoint>();
        var openList = new SortedList<float, WayPoint>();

        openList.Add(0, currentNode);
        currentNode.previous = null;
        currentNode.distance = 0;

        while(openList.Count > 0)
        {

            currentNode = openList.Values[0];
            openList.RemoveAt(0);

            var dist = currentNode.distance;
            closedList.Add(currentNode);
            if (currentNode == endNode)
                break;

            foreach(var neighbor in currentNode.neighbors)
            {

                if (closedList.Contains(neighbor) || openList.ContainsValue(neighbor))
                    continue;

                neighbor.previous = currentNode;
                neighbor.distance = dist + (neighbor.transform.position - currentNode.transform.position).magnitude;

                var DistanceToTarget = (neighbor.transform.position - endNode.transform.position).magnitude;
                openList.Add(neighbor.distance + DistanceToTarget, neighbor);

            }

        }

        if (currentNode == endNode)
        {
            while (currentNode.previous != null)
            {
                currentPath.Push(currentNode.transform.position);
                currentNode = currentNode.previous;
            }
            currentPath.Push(transform.position);
        }

    }

    public void Stop()
    {
        currentPath = null;
        moveTimeTotal = 0;
        moveTimeCurrent = 0;
        GameManager.instance.player.GetComponent<Player>().toggleWalkAnimation(false);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(currentPath != null && currentPath.Count > 0)
        {

            GameManager.instance.player.GetComponent<Player>().toggleWalkAnimation(true);

            if(moveTimeCurrent < moveTimeTotal)
            {
                moveTimeCurrent += Time.deltaTime;
                if (moveTimeCurrent > moveTimeTotal)
                    moveTimeCurrent = moveTimeTotal;
                transform.position = Vector3.Lerp(currentWayPointPosition, currentPath.Peek(), moveTimeCurrent / moveTimeTotal);
            }else
            {
                currentWayPointPosition = currentPath.Pop();
                if (currentPath.Count == 0)
                    Stop();
                else
                {
                    moveTimeCurrent = 0;
                    moveTimeTotal = (currentWayPointPosition - currentPath.Peek()).magnitude / walkSpeed;
                }
            }
        }
    }

    private WayPoint FindClosestWayPoint(Vector3 target)
    {

        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach(var waypoint in GameObject.FindGameObjectsWithTag("WayPoint"))
        {
            var distance = (waypoint.transform.position - target).magnitude;
            if (distance < closestDist)
            {
                closest = waypoint;
                closestDist = distance;
            }
        }
        if(closest != null)
        {
            return closest.GetComponent<WayPoint>();
        }
        return null;

    }
}
