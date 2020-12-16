using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollisionManager : MonoBehaviour
{
    public CubeBehaviour[] cubes;
    public BulletBehaviour[] bullets;

    // Start is called before the first frame update
    void Start()
    {
        cubes = FindObjectsOfType<CubeBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

        bullets = FindObjectsOfType<BulletBehaviour>();

        foreach (var b in bullets)
        {
            foreach(var c in cubes)
            {
                SphereAABB(b, c);
            }
        }




        for (int i = 0; i < cubes.Length; i++)
        {
            for (int j = 0; j < cubes.Length; j++)
            {
                if (i != j)
                {
                    CheckAABBs(cubes[i], cubes[j]);
                }
            }
        }
    }

    public static void CheckAABBs(CubeBehaviour a, CubeBehaviour b)
    {
        if ((a.min.x <= b.max.x && a.max.x >= b.min.x) &&
            (a.min.y <= b.max.y && a.max.y >= b.min.y) &&
            (a.min.z <= b.max.z && a.max.z >= b.min.z))
        {
            if (!a.contacts.Contains(b))
            {
                a.contacts.Add(b);
                a.isColliding = true;
            }
        }
        else
        {
            if (a.contacts.Contains(b))  
            {
                a.contacts.Remove(b);
                a.isColliding = false;
            }
        }
    }

    public static void SphereAABB(BulletBehaviour bullet, CubeBehaviour cube)
    {
        //Get box closet point to sphere center
        var x = Mathf.Max(cube.min.x, Mathf.Min(bullet.transform.position.x, cube.max.x));
        var y = Mathf.Max(cube.min.y, Mathf.Min(bullet.transform.position.y, cube.max.y));
        var z = Mathf.Max(cube.min.z, Mathf.Min(bullet.transform.position.z, cube.max.z));

        var distance = Mathf.Sqrt((x - bullet.transform.position.x) * (x - bullet.transform.position.x) +
                                 (y - bullet.transform.position.y) * (y - bullet.transform.position.y) +
                                 (z - bullet.transform.position.z) * (z - bullet.transform.position.z));

        if (distance < bullet.radius)
        {
            //Colliding 
            Bounce(bullet);
        }
    }

      
    private static void Bounce(BulletBehaviour bullet)
    {
        bullet.direction = new Vector3(bullet.direction.x, bullet.direction.y, -bullet.direction.z); 
    }
}
