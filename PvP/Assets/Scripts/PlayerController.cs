using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Prefab;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec3.x = (Int32)vec3.x;
            vec3.y = (Int32)vec3.y;
            vec3.z = 0;
            Instantiate(Prefab, vec3, Quaternion.identity);
        }   
    }
}
