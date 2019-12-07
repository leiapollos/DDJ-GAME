using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    protected bool aquired, used;
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
        ImageToggle(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (aquired)
        {
            if (Input.GetKey(keyCode))
            {
                Prepare();
                ExecutePowerUp();
            }
        }
    }

    public void Aquire()
    {
        keyImage.enabled = true;
        aquired = true;
    }

    protected virtual void Prepare()
    {
        Debug.Log("PoweUP was not initialized.");
    }

    protected virtual void ExecutePowerUp()
    {
        Debug.Log("PoweUP was not initialized.");
    }

    public virtual void Finish()
    {
        aquired = false;
        ImageToggle(false);
    }

    protected void ImageToggle(bool state)
    {
        keyImage.enabled = state;
    }
}
