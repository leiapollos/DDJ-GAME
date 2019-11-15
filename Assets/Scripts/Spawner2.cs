using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    public Enemy2 enemy2;
    public int maxNumberOfZombies = 3;
    List<Enemy2> currentZombies = new List<Enemy2>();

    float x = 0;
    float y = 0;
    float z = 0;
    Enemy2 spawnedEnemy = null;
    Vector3 pos;
    float lastTime = 0;

    public void Start()
    {
        lastTime = Time.time;
        for(int i = 0; i < maxNumberOfZombies; i++)
        {
            x = Random.Range(-10, 10);
            z = Random.Range(-10, 10);
            pos = new Vector3(x, y, z);
            spawnedEnemy = Instantiate(enemy2, pos, Quaternion.identity) as Enemy2;
            currentZombies.Add(spawnedEnemy);
        }
    }


    public void FixedUpdate()
    {

        if(Time.time - lastTime > 5)
        {
            while (currentZombies.Count < maxNumberOfZombies)
            {
                x = Random.Range(-10, 10);
                z = Random.Range(-10, 10);
                pos = new Vector3(x, y, z);
                spawnedEnemy = Instantiate(enemy2, pos, Quaternion.identity) as Enemy2;
                currentZombies.Add(spawnedEnemy);
            }
            lastTime = Time.time;
        }
        foreach (Enemy2 zombie in currentZombies)
        {
            if (zombie == null)
                currentZombies.Remove(zombie);
        }
            
    }

}