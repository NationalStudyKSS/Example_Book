using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    [SerializeField] Color _color = Color.yellow;
    [SerializeField] float _radius = 0.1f;

    private void OnDrawGizmos()
    {
        // ����� ���� ����
        Gizmos.color = _color;
        // ��ü ����� ����� ����. ���ڴ� (���� ��ġ, ������)
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
