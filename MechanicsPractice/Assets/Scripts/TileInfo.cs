using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TileInfo
{
    //General Purpose Related Data
    public Vector3Int GridPos;
    public bool isWall;
    public bool isActive;

    //Pathfinding Related Data
    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;
    public TileInfo parent;
}
