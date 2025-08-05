using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    // �Ѿ� ������
    [SerializeField] GameObject _bullet;
    // �Ѿ� �߻� ��ǥ
    [SerializeField] Transform _firePos;

    private void Start()
    {
    }

    private void Update()
    {
        // ���콺 ���� ��ư Ŭ�� �� �Ѿ� �߻�
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        // Bullet �������� �������� ����(������ ��ü, ��ġ, ȸ��)
        Instantiate(_bullet, _firePos.position, _firePos.rotation);
    }
}
