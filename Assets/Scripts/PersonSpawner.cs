using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{
    public Person person;
    public int maxNumberOfPersons;
    public int range;
    List<Enemy2> current = new List<Enemy2>();

    float x = 0;
    float y = 0;
    float z = 0;
    Person spawnedPerson = null;
    Vector3 pos;
    float lastTime = 0;

    public void Start()
    {
        lastTime = Time.time;
        for (int i = 0; i < maxNumberOfPersons; i++)
        {
            x = Random.Range(this.transform.position.x - range, this.transform.position.x + range);
            z = Random.Range(this.transform.position.z - range, this.transform.position.z + range);
            pos = new Vector3(x, y, z);
            spawnedPerson = Instantiate(person, pos, Quaternion.identity) as Person;
        }
    }

}
