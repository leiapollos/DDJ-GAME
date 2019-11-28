using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject player;

    public Text timerTextBackground;
    public Text timerText;
    private float startTime;

    float t;
    string minutes;
    string seconds;

    public Text scoreTextBackground;
    public Text scoreText;
    private int score;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if(player.GetComponent<LivingEntity>().dead == false)
        {
            //Change time
            t = Time.time - startTime;
            minutes = ((int)t / 60).ToString();
            seconds = (t % 60).ToString("f2");
            timerText.text = minutes + ":" + seconds;
            timerTextBackground.text = minutes + ":" + seconds;

            //Change score
            scoreText.text = score.ToString();
            scoreTextBackground.text = score.ToString();
        }
    }

    public void UpdateScore(int val)
    {
        score = score + val;
    }
}
