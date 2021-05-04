using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for defining a room
public class MazeRoom : ScriptableObject
{

    public int settingsIndex;

    public MazeRoomSettings settings;

    private List<MazeCellv2> cells = new List<MazeCellv2>();

    public void Add(MazeCellv2 cell)
    {
        cell.room = this;
        cells.Add(cell);
    }

    public void Assimilate(MazeRoom room)
    {
        for (int i = 0; i < room.cells.Count; i++)
        {
            Add(room.cells[i]);
        }
    }
}
