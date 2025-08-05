using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // 따라가야 할 대상을 연결할 변수
    [SerializeField] Transform _targetTr;

    // Main Camera 자신의 Transform 컴포넌트
    [SerializeField] Transform _camTr;

    // 따라갈 대상으로부터 떨어질 거리
    [Range(2.0f, 20.0f)]
    [SerializeField] float _distance = 10.0f;

    // Y축으로 이동할 높이
    [Range(0.0f, 10.0f)]
    [SerializeField] float _height = 2.0f;

    // 반응 속도
    [SerializeField] float _damping = 10.0f;

    // 카메라 LookAt의 Offset 값
    [SerializeField] float _targetOffset = 2.0f;

    // SmoothDamp 함수에서 사용할 속도 변수
    [SerializeField] Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        // Main Camera 자신의 Transform 컴포넌트를 추출
        _camTr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        // 추적해야 할 대상의 뒤쪽으로 distance만큼 이동
        // 높이를 height만큼 이동
        // _camTr.position = _targetTr.position - (_targetTr.forward * _distance) + (Vector3.up * _height);
        Vector3 pos = _targetTr.position - (_targetTr.forward * _distance) + (Vector3.up * _height);

        // 구면 선형 보간 함수를 사용해 부드럽게 위치를 변경
        //_camTr.position = Vector3.Slerp(_camTr.position, pos, Time.deltaTime * _damping);

        _camTr.position = Vector3.SmoothDamp(_camTr.position, pos, ref _velocity, _damping);

        // Camera를 피벗 좌표를 향해 회전
        _camTr.LookAt(_targetTr.position + (_targetTr.up * _targetOffset));
    }
}
