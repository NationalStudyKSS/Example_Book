using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // ���󰡾� �� ����� ������ ����
    [SerializeField] Transform _targetTr;

    // Main Camera �ڽ��� Transform ������Ʈ
    [SerializeField] Transform _camTr;

    // ���� ������κ��� ������ �Ÿ�
    [Range(2.0f, 20.0f)]
    [SerializeField] float _distance = 10.0f;

    // Y������ �̵��� ����
    [Range(0.0f, 10.0f)]
    [SerializeField] float _height = 2.0f;

    // ���� �ӵ�
    [SerializeField] float _damping = 10.0f;

    // ī�޶� LookAt�� Offset ��
    [SerializeField] float _targetOffset = 2.0f;

    // SmoothDamp �Լ����� ����� �ӵ� ����
    [SerializeField] Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        // Main Camera �ڽ��� Transform ������Ʈ�� ����
        _camTr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        // �����ؾ� �� ����� �������� distance��ŭ �̵�
        // ���̸� height��ŭ �̵�
        // _camTr.position = _targetTr.position - (_targetTr.forward * _distance) + (Vector3.up * _height);
        Vector3 pos = _targetTr.position - (_targetTr.forward * _distance) + (Vector3.up * _height);

        // ���� ���� ���� �Լ��� ����� �ε巴�� ��ġ�� ����
        //_camTr.position = Vector3.Slerp(_camTr.position, pos, Time.deltaTime * _damping);

        _camTr.position = Vector3.SmoothDamp(_camTr.position, pos, ref _velocity, _damping);

        // Camera�� �ǹ� ��ǥ�� ���� ȸ��
        _camTr.LookAt(_targetTr.position + (_targetTr.up * _targetOffset));
    }
}
