using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Model[] models;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Transform create(Vector3 pos, Quaternion rot)
    {
        if (models == null) return null;
        if (models.Length == 0) return null;
        int index = (int)Random.Range(0, models.Length);
        Vector3 newPos = new Vector3(pos.x,0.0f,pos.z);
        return Instantiate(models[index].model, newPos, rot) as Transform;
    }

    [System.Serializable]
    public class Model
    {
        public Transform model;
    }
}
