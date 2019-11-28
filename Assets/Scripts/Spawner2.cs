﻿using System.Collections.Generic;
using UnityEngine;

public class Spawner2 : MonoBehaviour
{
    public Enemy2 enemy2;
    public int maxNumberOfZombies;
    public int range;
    int timeInterval = 10;
    int newWaveTimeInterval = 30;
    List<Enemy2> currentZombies = new List<Enemy2>();

    float x = 0;
    float y = 0;
    float z = 0;
    Enemy2 spawnedEnemy = null;
    Vector3 pos;
    float lastTime = 0;
    float lastWaveTime = 0;

    public void Start()
    {
        lastWaveTime = Time.time;
        lastTime = Time.time;
        for (int i = 0; i < maxNumberOfZombies; i++)
        {
            x = Random.Range(this.transform.position.x - range, this.transform.position.x + range);
            z = Random.Range(this.transform.position.z - range, this.transform.position.z + range);
            pos = new Vector3(x, y, z);
            spawnedEnemy = Instantiate(enemy2, pos, Quaternion.identity) as Enemy2;
            currentZombies.Add(spawnedEnemy);
        }
    }


    public void FixedUpdate()
    {
        if (Time.time - lastWaveTime > newWaveTimeInterval)
        {
           // Debug.Log(Time.time);
            maxNumberOfZombies = maxNumberOfZombies + Mathf.RoundToInt(maxNumberOfZombies / 2);
            lastWaveTime = Time.time;
        }

        if (Time.time - lastTime > timeInterval && currentZombies.Count < maxNumberOfZombies)
        {
            //while (currentZombies.Count < maxNumberOfZombies)
            //{
            x = Random.Range(this.transform.position.x -range, this.transform.position.x + range);
            z = Random.Range(this.transform.position.z - range, this.transform.position.z + range);
            pos = new Vector3(x, y, z);
            spawnedEnemy = Instantiate(enemy2, pos, Quaternion.identity) as Enemy2;
            currentZombies.Add(spawnedEnemy);
            //}
            lastTime = Time.time;
        }
        for (int i = 0; i < currentZombies.Count; i++)
        {
            if (currentZombies[i] == null)
                currentZombies.RemoveAt(i);
        }
    }

}