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
        var originalPos = this.transform.position;
        this.transform.Translate(vec * speed);
        if (this.transform.position.x > 113
            || this.transform.position.x < -97
            || this.transform.position.y > 76
            || this.transform.position.y < -88)
        {
            this.transform.position = originalPos;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            transform.position = originalPos;
        }
    }
}
