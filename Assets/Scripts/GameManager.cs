using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//The main controller of the game with a lot of params that can drastically change the game
public class GameManager : MonoBehaviour
{
    public Maze mazePrefab;
    private Maze mazeInstance;
    public List<GameObject> poi = new List<GameObject>();
    public int numberOfEnemies;
    public int numberOfAgents;
    public int mazeSize;
    public GameObject agentPrefab;
    public GameObject itemPrefab;
    public int numberOfBatteries;
    public int numberOfFlares;
    public GameObject randomTrashPrefab;
    public int numberOfTrashes;
    public static int currentLevel = 1;
    public Text currentLevelText;

    private void Start()
    {
        BeginGame();
        currentLevelText.text = currentLevel.ToString();
        InvokeRepeating("ambientSound", Random.Range(5, 13), Random.Range(5, 15));
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void BeginGame()
    {
        //set difficulty level of maze
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeSize = mazeSize + currentLevel;
        mazeInstance.size = new IntVector2(Mathf.Clamp(mazeSize, 0, 25), Mathf.Clamp(mazeSize, 0, 25));
        mazeInstance.trapProb = Mathf.Clamp(currentLevel / 150f, 0f, 0.1f);
        GetComponentInChildren<GridForPath>().gridWorldSize = new Vector2(Mathf.Clamp(mazeSize, 0, 25), Mathf.Clamp(mazeSize, 0, 25));

        //agents,enemies,batteries,flares depending on level
        numberOfAgents = Mathf.Clamp((currentLevel - 1), 0, 30);
        numberOfEnemies = Mathf.Clamp(Mathf.CeilToInt((currentLevel - 4) / 1.5f), 0, 10);
        numberOfBatteries = Mathf.Clamp(currentLevel * 2, 0, 10);
        numberOfFlares = Mathf.Clamp(Mathf.CeilToInt((currentLevel) / 2f), 0, 5);
        numberOfTrashes = Mathf.Clamp(currentLevel * 25, 0, 500);


        StartCoroutine(mazeInstance.Generate());
    }

    public void RestartGame()
    {
        currentLevel = 1;
        PlayerInvetory.numOfFlares = 1;
        PlayerInvetory.flashlightEnergy = 30f;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        currentLevel += 1;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    //Spawn of objects
    public void mazeReady()
    {
        GameObject.Find("Player").gameObject.GetComponent<PlayerController>().placePlayer();
        GameObject.Find("GameManager/A*").gameObject.GetComponent<GridForPath>().CreateGrid();
        GameObject.Find("GameManager/A*").gameObject.GetComponent<Pathfinding>().awakePathfinding();
        GameObject.FindObjectOfType<PathRequestManager>().StartPathRequestManager();

        for (int i = 0; i < numberOfAgents; i++)
        {
            Transform randomPos = GameObject.FindObjectOfType<Maze>().freeCells[Random.Range(0, GameObject.FindObjectOfType<Maze>().freeCells.Count)].gameObject.transform;
            Instantiate(agentPrefab, randomPos.position, Quaternion.identity);
        }

        for (int i = 0; i < numberOfTrashes; i++)
        {
            Transform randomPos = GameObject.FindObjectOfType<Maze>().cells[Random.Range(0, GameObject.FindObjectOfType<Maze>().size.x), Random.Range(0, GameObject.FindObjectOfType<Maze>().size.z)].gameObject.transform;
            GameObject item = Instantiate(randomTrashPrefab, randomPos.position + new Vector3(Random.Range(-0.5f, 0.5f), 15, Random.Range(-0.5f, 0.5f)), Quaternion.Euler(Random.Range(0.0f, 360.0f), 0f, 0f), GameObject.Find("Trash").transform);
        }

        for (int i = 0; i < numberOfBatteries; i++)
        {
            Transform randomPos = GameObject.FindObjectOfType<Maze>().freeCells[Random.Range(0, GameObject.FindObjectOfType<Maze>().freeCells.Count)].gameObject.transform;
            GameObject.FindObjectOfType<Maze>().freeCells.Remove(randomPos.GetComponent<MazeCellv2>());
            GameObject item = Instantiate(itemPrefab, randomPos.position + new Vector3(Random.Range(-0.5f, 0.5f), 15, Random.Range(-0.5f, 0.5f)), Quaternion.Euler(Random.Range(0.0f, 360.0f), 0f, 0f));
            item.GetComponent<PickableObject>().item = PickableObject.PickableObjectType.Battery;
        }

        for (int i = 0; i < numberOfFlares; i++)
        {
            Transform randomPos = GameObject.FindObjectOfType<Maze>().freeCells[Random.Range(0, GameObject.FindObjectOfType<Maze>().freeCells.Count)].gameObject.transform;
            GameObject.FindObjectOfType<Maze>().freeCells.Remove(randomPos.GetComponent<MazeCellv2>());
            GameObject item = Instantiate(itemPrefab, randomPos.position + new Vector3(Random.Range(-0.5f, 0.5f), 15, Random.Range(-0.5f, 0.5f)), Quaternion.Euler(Random.Range(0.0f, 360.0f), 0f, 0f));
            item.GetComponent<PickableObject>().item = PickableObject.PickableObjectType.Flare;
        }
    }

    void ambientSound()
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
