using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 Bullet 태그를 가지고 있다면
        if (collision.collider.CompareTag("Bullet"))
        {
            // 해당 오브젝트를 파괴
            Destroy(collision.gameObject);
        }
    }
}
