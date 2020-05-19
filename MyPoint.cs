using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPoint : MonoBehaviour
{
    [SerializeField] Color _color = Color.yellow;
    [SerializeField] float _radius = 0.2f;

    void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
