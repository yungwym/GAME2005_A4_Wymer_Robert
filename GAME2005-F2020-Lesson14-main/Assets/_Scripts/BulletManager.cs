using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletManager : MonoBehaviour
{
    public GameObject bullet;

    private static BulletManager m_instance = null;

    private BulletManager()
    {

    }

    public static BulletManager Instance()
    {
        if (m_instance == null)
        {
            m_instance = new BulletManager();
        }
        return m_instance;
    }

    public int MaxBullets { get; set;  }

    private Queue<GameObject> m_bulletPool;



    public void Init(int maxBullets)
    {
        MaxBullets = maxBullets;
        BuildPool();
    }  

    private void BuildPool()
    {
        m_bulletPool = new Queue<GameObject>();

        for (int count = 0; count < MaxBullets; count++)
        {
            var tempBullet = Instantiate(bullet);
            m_bulletPool.Enqueue(tempBullet);
        }
    }

    public GameObject GetBullet(Vector3 position, Vector3 direction)
    {
        GameObject newBullet = null;

        newBullet = m_bulletPool.Dequeue();
        newBullet.SetActive(true);
        newBullet.transform.position = position;
        newBullet.GetComponent<BulletBehaviour>().direction = direction;
       // newBullet.transform.SetParent(self.gameObject.transform);

        return newBullet;
    }

    public bool HasBullets()
    {
        return m_bulletPool.Count > 0;
    }

    public void ReturnBullet(GameObject returnedBullet)
    {
        returnedBullet.SetActive(false);
        m_bulletPool.Enqueue(returnedBullet);
    }
}
