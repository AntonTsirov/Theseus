using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    Vector3[] path;
    int targetIndex;
    [HideInInspector]
    public Transform target;
    public bool canSeePlayer = false;
    public float fieldOfViewDegrees = 90f;
    public float visibilityDistance = 5f;

    public void StartSearchOfPath(Transform targetPos)
    {
        target = targetPos;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    void Update()
    {
        CanSeePlayer();
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public void StopPathing()
    {
        StopCoroutine("FollowPath");
        target = null;
        targetIndex = 0;
        path = new Vector3[0];
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            Vector3 targetDir;
            Quaternion lookRotation;

            if (transform.position == new Vector3(currentWaypoint.x, transform.position.y, currentWaypoint.z))
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector3[0];

                    //targetDir = (target.position - this.transform.position).normalized;
                    targetDir = (new Vector3(target.position.x, this.transform.position.y, target.position.z) - this.transform.position).normalized;

                    lookRotation = Quaternion.LookRotation(targetDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * this.rotationSpeed);


                    gameObject.GetComponent<Animator>().SetBool("isExploring", false);
                    gameObject.GetComponent<Animator>().SetBool("isHunting", false);
                    target = null;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            //Rotate Towards Next Waypoint
            targetDir = (new Vector3(currentWaypoint.x, this.transform.position.y, currentWaypoint.z) - this.transform.position).normalized;
            lookRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * this.rotationSpeed);

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentWaypoint.x, transform.position.y, currentWaypoint.z), speed * Time.deltaTime);
            yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.2f, 0.2f, 0.2f));

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    void CanSeePlayer()
    {
        Vector3 rayDirection = GameObject.FindGameObjectWithTag("Player").transform.Find("Center").transform.position - transform.position;
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.Find("Center").transform.position;

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewDegrees * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, playerPos, out hit))
            {
                Debug.Log(hit.transform.tag);
                if (hit.transform.tag == "Player")
                {
                    Debug.DrawLine(transform.position, hit.transform.position, Color.green, 0.3f);
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                    Debug.DrawLine(transform.position, hit.transform.position, Color.red, 0.3f);
                }
            }

        }
    }
}
