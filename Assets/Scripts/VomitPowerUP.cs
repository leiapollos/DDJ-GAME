using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VomitPowerUP : PowerUp
{
    public Gun gun;
    protected float powerUpTime;
    // Start is called before the first frame update

    public VomitPowerUP() : base()
    {
        keyCode = KeyCode.E;
    }

    protected override void UsePowerUp()
    {
        if (!used)
        {
            powerUpTime = Time.time;
            used = true;
        }
        if (Time.time - powerUpTime > 5)
        {
            aquired = false;
            keyImage.enabled = false;
        }
    }

    protected override void ExecutePowerUp()
    {
        if (Time.time - powerUpTime < 5)
            gun.Shoot();
    }
}
