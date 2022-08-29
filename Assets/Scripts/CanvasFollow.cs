using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFollow : MonoBehaviour
{
    [SerializeField] private Transform target = null;

    private void LateUpdate()
    {
        transform.position = target.position + new Vector3(0, -1, 0);
        transform.rotation = Quaternion.identity;
    }
}
