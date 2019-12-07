using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPowerUP : PowerUp
{
    public Player player;
    protected float powerUpTime;
    public int healthAmmount = 5;
    // Start is called before the first frame update

    public HealthPowerUP() : base()
    {
        keyCode = KeyCode.E;
        name = "Health_PowerUP";
    }

    protected override void UsePowerUp()
    {
        if (!used)
        {
            player.AddHealth(healthAmmount);
            powerUpTime = Time.time;
            used = true;
        }
        if (Time.time - powerUpTime > 1)
        {
            aquired = false;
            keyImage.enabled = false;
        }
    }

    protected override void ExecutePowerUp()
    {
    }
}
