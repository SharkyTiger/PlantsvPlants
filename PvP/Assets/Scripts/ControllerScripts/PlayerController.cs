using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PrefabLeft;
    public GameObject PrefabRight;
    public GameObject highlightPrefab;
    private GameObject highlight;
    public Tilemap map;
    public Tilemap interActiveMap;
    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        highlight = Instantiate(highlightPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec3.x += 0.5f;
        vec3.y += 0.5f;
        var cellPosition = map.LocalToCell(vec3);
        highlight.transform.position = cellPosition;
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Debug.Log(cellPosition);

            var currentObject = GetGameObjectFromPosition(cellPosition);
            if (currentObject != null)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                Instantiate(PrefabLeft, cellPosition, Quaternion.identity);
                return;
            }

            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                Instantiate(PrefabRight, cellPosition, Quaternion.identity);
                return;
            }
        }   
    }

    GameObject GetGameObjectFromPosition(Vector3 position)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            float dist = Vector3.Distance(position, obj.transform.position);
            if (dist < 1 && obj != highlight)
            {
                return obj;
            }
        }
        return null;
    }
}
