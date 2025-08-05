using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    [SerializeField] Color _color = Color.yellow;
    [SerializeField] float _radius = 0.1f;

    private void OnDrawGizmos()
    {
        // 기즈모 색상 설정
        Gizmos.color = _color;
        // 구체 모양의 기즈모 생성. 인자는 (생성 위치, 반지름)
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
