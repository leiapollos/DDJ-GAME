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
        keyCode = KeyCode.F;
        name = "Health_PowerUP";
    }

    protected override void Prepare()
    {
        if (!used)
        {
            used = true;
        }
    }

    protected override void ExecutePowerUp()
    {
        player.AddHealth(healthAmmount);
        Finish();
    }
}
