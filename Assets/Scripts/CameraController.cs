using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed = 0.125f;
    [SerializeField] float cameraYOffset;

    private float targetY;

    private void LateUpdate()
    {
        if(Block.currentBlock != null) targetY = Block.currentBlock != null 
        ? Block.currentBlock.transform.position.y + cameraYOffset : 1.3f;
        Vector3 position = new(transform.position.x, targetY, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, position, speed);
        
        transform.position = smoothedPosition;
    }
}
