using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : MonoBehaviour
{
    public Tilemap tilemap;       // Drag the Tilemap object here
    public TileBase tileToPlace;  // Drag your created tile asset here

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = tilemap.WorldToCell(worldPos);
            tilemap.SetTile(cellPos, tileToPlace);
        }
    }
}
