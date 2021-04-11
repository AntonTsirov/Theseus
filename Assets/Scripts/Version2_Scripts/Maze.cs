using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public IntVector2 size;
    public MazeCellv2 cellPrefab;
    public MazeCellv2 cellPrefabWithTrap;
    public float trapProb;
    public MazeCellv2[,] cells;
    public MazePassage passagePrefab;
    public MazeWall[] wallPrefabs;
    public float generationStepDelay;
    public MazeDoor doorPrefab;
    [HideInInspector]
    public float doorProbability;
    public MazeRoomSettings[] roomSettings;
    private List<MazeRoom> rooms = new List<MazeRoom>();
    private int randomIndex;

    public MazeCellv2 GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCellv2[size.x, size.z];
        List<MazeCellv2> activeCells = new List<MazeCellv2>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            //yield return delay;
            DoNextGenerationStep(activeCells);
        }

        //call generation of pathfinding grid 
        yield return new WaitForSeconds(1);
        GameObject.FindObjectOfType<GameManager>().mazeReady();
    }

    private void DoFirstGenerationStep(List<MazeCellv2> activeCells)
    {
        MazeCellv2 newCell = CreateCell(RandomCoordinates);
        newCell.Initialize(CreateRoom(-1));
        activeCells.Add(newCell);
        float[] doorProbOption = new float[3] { 0f, 0.05f, 0.1f };
        doorProbability = doorProbOption[Random.Range(0, doorProbOption.Length)];
        randomIndex = Random.Range(0, 5);
    }

    private void DoNextGenerationStep(List<MazeCellv2> activeCells)
    {
        int[] randomIndexArr = new int[5] { 0, Mathf.FloorToInt((activeCells.Count - 1) / 2), activeCells.Count - 1, activeCells.Count - 1, activeCells.Count - 1 };
        int currentIndex = randomIndexArr[randomIndex]; ;//if you pick first it very different behaviour of generation
        MazeCellv2 currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            MazeCellv2 neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else if (currentCell.room != null && (currentCell.room.settingsIndex == neighbor.room.settingsIndex))
            {
                CreatePassageInSameRoom(currentCell, neighbor, direction);
            }
            else
            {
                CreateWall(currentCell, neighbor, direction);
                // No longer remove the cell here.
            }
        }
        else
        {
            CreateWall(currentCell, null, direction);
            // No longer remove the cell here.
        }
    }

    private MazeCellv2 CreateCell(IntVector2 coordinates)
    {
        MazeCellv2 newCell = Instantiate(Random.value <= trapProb ? cellPrefabWithTrap : cellPrefab) as MazeCellv2;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition =
            new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
        return newCell;
    }

    private void CreatePassage(MazeCellv2 cell, MazeCellv2 otherCell, MazeDirection direction)
    {
        MazePassage prefab = Random.value <= doorProbability ? doorPrefab : passagePrefab;
        MazePassage passage = Instantiate(prefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(prefab) as MazePassage;
        if (passage is MazeDoor)
        {
            otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
        }
        else
        {
            otherCell.Initialize(cell.room);
        }
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(MazeCellv2 cell, MazeCellv2 otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        //wall.gameObject.transform.GetChild(0).transform.localScale = new Vector3(wall.gameObject.transform.GetChild(0).transform.localScale.x, wall.gameObject.transform.GetChild(0).transform.localScale.y, wall.gameObject.transform.GetChild(0).transform.localScale.z + renderOffSet);
        if (direction == MazeDirection.North || direction == MazeDirection.South)
        {
            //wall.gameObject.transform.GetChild(0).transform.localScale = new Vector3(0.8f, wall.gameObject.transform.GetChild(0).transform.localScale.y, wall.gameObject.transform.GetChild(0).transform.localScale.z);
        }
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefabs[1]) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
            wall.transform.name = direction.ToString() + " its opposite";
            wall.gameObject.SetActive(false);// TODO change
            if (direction == MazeDirection.North || direction == MazeDirection.South)
            {
                //wall.gameObject.transform.GetChild(0).transform.localScale = new Vector3(0.9f, wall.gameObject.transform.GetChild(0).transform.localScale.y, wall.gameObject.transform.GetChild(0).transform.localScale.z);
            }
        }
    }

    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }

    private MazeRoom CreateRoom(int indexToExclude)
    {
        MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
        newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
        if (newRoom.settingsIndex == indexToExclude)
        {
            newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
        }
        newRoom.settings = roomSettings[newRoom.settingsIndex];
        rooms.Add(newRoom);
        return newRoom;
    }

    private void CreatePassageInSameRoom(MazeCellv2 cell, MazeCellv2 otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
        if (cell.room != otherCell.room)
        {
            //MazeRoom roomToAssimilate = otherCell.room;
            //cell.room.Assimilate(roomToAssimilate);
            //rooms.Remove(roomToAssimilate);
            //Destroy(roomToAssimilate);
        }
    }
}
