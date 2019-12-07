using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public bool aquired;
    public bool used;
    protected KeyCode keyCode;
    public Image keyImage;
    public string name;

    public PowerUp()
    {
        keyCode = KeyCode.Space;
        aquired = false;
        used = false;
        name = "default";
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (aquired)
        {
            if (Input.GetKey(keyCode))
            {
                UsePowerUp();
                ExecutePowerUp();
            }
        }
    }
    public void Aquire()
    {
        Debug.Log("Aquired!!");
        keyImage.enabled = true;
        aquired = true;
    }

    protected virtual void UsePowerUp()
    {
        Debug.Log("PoweUP was not initialized.");
    }

    protected virtual void ExecutePowerUp()
    {
        Debug.Log("PoweUP was not initialized.");
    }
}
