using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0,1)]
    public float smoothTime;
    public int worldSize;
    public Transform playerTransform;

    public void moveTo(Vector2 newPos) {
        Vector3 pos = GetComponent<Transform>().position;
        pos.x = newPos.x;
        pos.y = newPos.y;
        GetComponent<Transform>().position = pos;
    }

    public void FixedUpdate() {
        Vector3 pos = GetComponent<Transform>().position;
        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);
        pos.x = Mathf.Clamp(pos.x, 0 + GetComponent<Camera>().orthographicSize * 2 + 5, worldSize - GetComponent<Camera>().orthographicSize * 2 - 5);
        GetComponent<Transform>().position = pos;
    }

}

