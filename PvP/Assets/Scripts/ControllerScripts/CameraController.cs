using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.2f;
    private const String horizontal = "Horizontal";
    private const String vertical = "Vertical";
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var vec = new Vector3(Input.GetAxis(horizontal), Input.GetAxis(vertical));
        this.transform.Translate(vec * speed);
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            transform.position = originalPos;
        }
    }
}
