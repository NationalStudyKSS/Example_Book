using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� Bullet �±׸� ������ �ִٸ�
        if (collision.collider.CompareTag("Bullet"))
        {
            // �ش� ������Ʈ�� �ı�
            Destroy(collision.gameObject);
        }
    }
}
