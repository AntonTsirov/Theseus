using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;
    public List<GameObject> poi = new List<GameObject>();
    public int numberOfEnemies;
    public int numberOfAgents;
    public GameObject agentPrefab;

    private void Start()
    {
        BeginGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //RestartGame();
        }

    }

    private void BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        StartCoroutine(mazeInstance.Generate());
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        foreach (Unit agentObj in GameObject.FindObjectsOfType<Unit>())
        {
            Destroy(agentObj.gameObject);
        }
        GameObject.FindObjectOfType<GameManager>().poi.Clear();
        BeginGame();
    }

    public void mazeReady()
    {
        GameObject.Find("Player").gameObject.GetComponent<PlayerController>().placePlayer();
        GameObject.Find("GameManager/A*").gameObject.GetComponent<GridForPath>().CreateGrid();
        GameObject.Find("GameManager/A*").gameObject.GetComponent<Pathfinding>().awakePathfinding();
        GameObject.FindObjectOfType<PathRequestManager>().StartPathRequestManager();

        for (int i = 0; i < numberOfAgents; i++)
        {
            Transform randomPos = GameObject.FindObjectOfType<Maze>().cells[Random.Range(0, GameObject.FindObjectOfType<Maze>().size.x), Random.Range(0, GameObject.FindObjectOfType<Maze>().size.z)].gameObject.transform;
            Instantiate(agentPrefab, randomPos.position, Quaternion.identity);
        }
    }
}
