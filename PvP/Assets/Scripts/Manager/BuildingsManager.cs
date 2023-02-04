using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildingsManager : MonoBehaviour
{
    public GameObject WaterMinePrefab;
    public GameObject FertilizerMinePrefab;
    public GameObject SpawnerPrefab;
    public GameObject highlightPrefab;

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
        
    }

    public void EnterBuildMode(BuildingKind kind)
    {
        toBeBuild = kind;
    }

    private void OnMouseDown()
    {
        Debug.Log("Hallo");
        if (toBeBuild == BuildingKind.None || EventSystem.current.IsPointerOverGameObject()) return;

        var cellPosition = gameManager.GetMouseToWorldPos();

        var currentObject = gameManager.GetGameObjectFromPosition(cellPosition);
        if (currentObject != null)
        {
            return;
        }

        GameObject building = null;

        switch(toBeBuild)
        {
            case BuildingKind.Spawner:
                building = Instantiate(SpawnerPrefab, cellPosition, Quaternion.identity);
                break;
            case BuildingKind.WaterMine:
                building = Instantiate(WaterMinePrefab, cellPosition, Quaternion.identity);
                break;
            case BuildingKind.FertilizerMine:
                building = Instantiate(FertilizerMinePrefab, cellPosition, Quaternion.identity);
                break;
        }

        if(building != null)
        {
            gameManager.CreateBuilding(building);
            toBeBuild = BuildingKind.None;
        }
    }
}