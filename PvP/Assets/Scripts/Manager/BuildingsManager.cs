using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildingsManager : MonoBehaviour
{
    public GameObject WaterMinePrefab;
    public GameObject FertilizerMinePrefab;
    public GameObject SpawnerPrefab;
    public GameObject highlightPrefab;
    public GameObject WaterLayer;
    public GameObject DirtLayer;

    private GameObject highlight;
    public GameObject GameManagerObject;
    private GameManager gameManager;

    private BuildingKind toBeBuild;

    // Start is called before the first frame update
    void Start()
    {
        highlight = Instantiate(highlightPrefab);
        gameManager = GameManagerObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (toBeBuild == BuildingKind.None) return;

        var cellPosition = gameManager.GetMouseToWorldPos();
        highlight.transform.position = cellPosition;

        if (Input.GetMouseButton(0)) MouseDown(cellPosition);
        if (Input.GetMouseButton(1)) Cancel();
    }

    public void EnterBuildMode(BuildingKind kind)
    {
        highlight.SetActive(true);
        toBeBuild = kind;
    }

    public Boolean IsInBuildMode() => toBeBuild != BuildingKind.None;

    public void Cancel()
    {
        highlight.SetActive(false);
        toBeBuild = BuildingKind.None;
    }

    private void MouseDown(Vector3Int cellPosition)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        var currentObject = gameManager.GetGameObjectFromPosition(cellPosition);
        if (currentObject != null)
        {
            return;
        }

        GameObject building = null;
        var pos = new Vector2(cellPosition.x, cellPosition.y);
        switch (toBeBuild)
        {
            case BuildingKind.Spawner:
                if(!(DirtLayer.GetComponent<TilemapCollider2D>().OverlapPoint(pos) 
                  || WaterLayer.GetComponent<TilemapCollider2D>().OverlapPoint(pos)))
                {
                    building = Instantiate(SpawnerPrefab, cellPosition, Quaternion.identity);
                    building.GetComponent<Spawner>().Id = gameManager.GetNextSpawnerId();
                }
                break;
            case BuildingKind.WaterMine:
                if(WaterLayer.GetComponent<TilemapCollider2D>().OverlapPoint(pos))
                {
                    Debug.Log("C");
                    building = Instantiate(WaterMinePrefab, cellPosition, Quaternion.identity);
                    Debug.Log("B");
                }
                Debug.Log("A");
                break;
            case BuildingKind.FertilizerMine:
                if(DirtLayer.GetComponent<TilemapCollider2D>().OverlapPoint(pos))  building = Instantiate(FertilizerMinePrefab, cellPosition, Quaternion.identity);
                break;
        }

        if (building != null)
        {
            var b = building.GetComponent<Building>();
            b.Manager = gameManager;
            b.TeamNumber = Team.Team1;
            b.TeamColor = Color.red;
            b.MaxHealth = b.CurrentHealth = 10;
            gameManager.CreateBuilding(building);
            toBeBuild = BuildingKind.None;
            highlight.SetActive(false);
        }
    }
}
