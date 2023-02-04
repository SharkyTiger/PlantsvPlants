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
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            var vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec3.x = (Int32)vec3.x;
            vec3.y = (Int32)vec3.y;
            vec3.z = 1;
            var deadObject = GetGameObjectFromPosition(vec3);
            if (deadObject != null)
            {
                Destroy(deadObject);
            }
            if (Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                Instantiate(PrefabLeft, vec3, Quaternion.identity);
                return;
            }

            if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                Instantiate(PrefabRight, vec3, Quaternion.identity);
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
