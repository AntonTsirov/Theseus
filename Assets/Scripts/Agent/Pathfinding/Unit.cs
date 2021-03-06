using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for receiving the path from the manager and following it
//also detect when it sees the player
public class Unit : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float rotationSpeed;
    public Vector3[] path;
    public int targetIndex;
    public Transform target;
    public Vector3 endDest;
    public bool canSeePlayer = false;
    public float fieldOfViewDegrees = 90f;
    public float visibilityDistance = 5f;

    public void StartSearchOfPath(Transform targetPos)
    {
        target = targetPos;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        endDest = target.position;
    }

    void Update()
    {
        CanSeePlayer();
        if (speed < maxSpeed)
        {
            speed = Mathf.Clamp(speed + Time.deltaTime / 10f, 0, maxSpeed);
        }
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
        else
        {
            //if not path found go to rest beh and reset
            Debug.Log("no succ path found");
            target = null;
            targetIndex = 0;
            path = new Vector3[0];

            gameObject.GetComponent<Animator>().SetBool("isExploring", false);
            gameObject.GetComponent<Animator>().SetBool("isHunting", false);
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
                Gizmos.color = Color.red;
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
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(endDest, new Vector3(0.2f, 0.2f, 0.2f));
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
                if (hit.transform.tag == "Player")
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }

        }
    }

    public void hitByFlashlight()
    {
        speed = Mathf.Clamp(speed - Time.deltaTime / 2f, 0f, maxSpeed);
    }
}
