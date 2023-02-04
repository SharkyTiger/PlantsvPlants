using UnityEngine;
using UnityEngine.Tilemaps;

public class GridHighlighter : MonoBehaviour
{
    public Tilemap map;
    public Sprite hoverSprite;
    private Vector3Int previousCellPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec3.z = 1;
        var cellPosition = map.LocalToCell(vec3);
        Debug.Log("A");
        if (!cellPosition.Equals(previousCellPos))
        {
            Debug.Log("B");
            map.GetTile<Tile>(previousCellPos).sprite = null;
            Debug.Log("C");
            map.GetTile<Tile>(cellPosition).sprite = hoverSprite;
            Debug.Log("D");
            previousCellPos = cellPosition;
            map.RefreshTile(previousCellPos);
            map.RefreshTile(cellPosition);
            Debug.Log("E");
        }
    }
}