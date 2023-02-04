using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PrefabLeft;
    public GameObject PrefabRight;
    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse click's position in 2d plane
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pz.z = 0;

        // convert mouse click's position to Grid position
        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3Int cellPosition = gridLayout.WorldToCell(pz);

        // set selectedUnit to clicked location on grid
        Debug.Log(cellPosition);

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            var vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec3.z = 0;
            var currentObject = GetGameObjectFromPosition(vec3);
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
