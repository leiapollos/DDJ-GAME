using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Text endGameScreen;
    public Text endGameScreenBackground;

    public AudioManager audio;

    List<GameObject> hiddingSpots = new List<GameObject>();
    List<GameObject> zombies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        score = 0;
        GameObject[] hiddingSpotsArray = GameObject.FindGameObjectsWithTag("Hidding Spot");
        for(int i = 0; i < hiddingSpotsArray.Length; i++)
        {
            hiddingSpots.Add(hiddingSpotsArray[i]);
        }
        GameObject[] zombiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < zombiesArray.Length; i++)
        {
            zombies.Add(zombiesArray[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateZombiesList();
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
        EndGameScreen();
    }

    void UpdateZombiesList()
    {
        zombies.Clear();
        GameObject[] zombiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < zombiesArray.Length; i++)
        {
                zombies.Add(zombiesArray[i]);
        }
    }


    public void UpdateScore(int val)
    {
        score = score + val;
    }

    public Transform ClosestHiddingSpot(Transform civilian)
    {
        Transform closest = null;
        float closestMagnitude = 10000;
        foreach (GameObject hiddingSpot in hiddingSpots)
        {
            //Debug.Log(closest);
            if((hiddingSpot.transform.position - civilian.position).magnitude < closestMagnitude)
            {
                closest = hiddingSpot.transform;
                closestMagnitude = (hiddingSpot.transform.position - civilian.position).magnitude;
            }
        }
        return closest;
    }

    public Transform ClosestZombie(Transform civilian)
    {
        Transform closest = null;
        float closestMagnitude = 10000;
        foreach (GameObject zombie in zombies)
        {
            if (zombie == null) continue;
            if (zombie.GetComponent<Enemy2>().dead) continue;
            if ((zombie.transform.position - civilian.position).magnitude < closestMagnitude)
            {
                closest = zombie.transform;
                closestMagnitude = (zombie.transform.position - civilian.position).magnitude;
            }
        }
        return closest;
    }

    public int CountCivilians()
    {
        var civilians = GameObject.FindGameObjectsWithTag("Civilian");
        int count = 0;
        foreach (GameObject c in civilians)
        {
            count++;
        }
        return count;
    }

    public void EndGameScreen()
    {
        var ded = player.GetComponent<Player>().dead;
        if (!ded && CountCivilians() > 0) return;

        if (Input.GetKey(KeyCode.R)) // Restart Scene
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        timerText.enabled = false;
        timerTextBackground.enabled = false;
        endGameScreen.enabled = true;
        endGameScreenBackground.enabled = true;

        if (audio.IsPlaying("MainTheme"))
        {
            audio.StopAll();
            if (!ded) audio.Play("Victory");
            else audio.Play("Loss");
        }

        player.GetComponent<Player>().enabled = false;

        foreach (GameObject zombie in zombies)
        {
            zombie.GetComponent<Enemy2>().dead = true;
        }

    }
}
