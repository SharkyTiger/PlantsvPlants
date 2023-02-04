using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PrefabLeft;
    public GameObject PrefabRight;
    public Tilemap map;
    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector3 vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec3.z = 1;
            var cellPosition = map.LocalToCell(vec3);
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

    GameObject? GetGameObjectFromPosition(Vector3 position)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            float dist = Vector3.Distance(position, obj.transform.position);
            if (dist < 1)
            {
                return obj;
            }
        }
        return null;
    }
}
