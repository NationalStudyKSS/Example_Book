using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletCtrl : MonoBehaviour
{
    // �Ѿ��� �ı���
    [SerializeField] float _damage = 20.0f;

    // �Ѿ� �߻� ��
    [SerializeField] float _force = 1500.0f;

    [SerializeField] Rigidbody _rb;

    private void Start()
    {
        // Rigidbody ������Ʈ�� ����
        _rb = GetComponent<Rigidbody>();

        // Rigidbody�� ���� ���� �Ѿ��� �߻�
        _rb.AddForce(transform.forward * _force);
    }
}
