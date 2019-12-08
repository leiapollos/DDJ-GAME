using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VomitPowerUP : PowerUp
{
    public Gun gun;
    protected float powerUpTime;

    public VomitPowerUP() : base()
    {
        keyCode = KeyCode.E;
        name = "Vommit_PowerUP";
    }

    protected override void Prepare()
    {
        if (!used)
        {
            powerUpTime = Time.time;
            used = true;
        }
        if (Time.time - powerUpTime > 5)
        {
            Finish();
        }
    }

    protected override void ExecutePowerUp()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameObject.GetComponent<Player>().audioManager.Stop("Blargh");
            gameObject.GetComponent<Player>().audioManager.Play("Blargh");
        }

        if (Time.time - powerUpTime < 5)
        {
            gun.Shoot();
        }
            

    }
}
