using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject player;

    public GameObject gameUI;

    //public GameObject minimapUI;

    //public GameObject timerUI;
    public Text timerTextBackground;
    public Text timerText;
    private float startTime;

    float t;
    string minutes;
    string seconds;

    //public GameObject scoreUI;
    public Text scoreTextBackground;
    public Text scoreText;
    private int score;


    public AudioManager audio;


    List<GameObject> hiddingSpots = new List<GameObject>();
    List<GameObject> zombies = new List<GameObject>();

    public Transform[] target;
    public float speed;

    public GameObject subway;

    Vector3 dir;

    bool first = true;

    public Text text1;
    public Text text2;

    public Light playerLight;

    public GameObject loadingScreen;
    public Slider slider;
    public Text LoadingTextBackground;
    public Text LoadingText;

    public GameObject endGameScreenUI;
    public Text endGameScoreTextBackground;
    public Text endgameScoreText;

    public GameObject deathScreenUI;
    public Text deathScoreTextBackground;
    public Text deathScoreText;

    // Start is called before the first frame update
    void Start()
    {
        playerLight.intensity = 1;
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

        dir = (target[1].transform.position - subway.transform.position).normalized;
        
        timerText.text = "0:0,00";
        timerTextBackground.text = "0:0,00";

        scoreText.text = "0";
        scoreTextBackground.text = "0";
        freezeZombies();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateZombiesList();
        if(player.GetComponent<LivingEntity>().dead == false)
        {
            // GO TO STATION
            if ((target[1].transform.position - subway.transform.position).magnitude > 0.5)
            {
                moveObject(dir);
                freezeZombies();
            }
            else
            {
                while (first == true)
                {
                    startTime = Time.time;
                    unfreezeZombies();
                    playerLight.intensity = 6;
                    player.GetComponent<Player>().canMove = true;
                    gameUI.SetActive(true);
                    //timerUI.SetActive(true);
                    //minimapUI.SetActive(true);
                    //scoreUI.SetActive(true);
                    first = false;
                }


                //RUN THE GAME NORMALLY


                //Change time
                t = Time.time - startTime;
                minutes = ((int)t / 60).ToString();
                seconds = (t % 60).ToString("f2");
                timerText.text = minutes + ":" + seconds;
                timerTextBackground.text = minutes + ":" + seconds;

                //Change score
                scoreText.text = score.ToString();
                scoreTextBackground.text = score.ToString();

                if ((player.transform.position - target[1].transform.position).magnitude < 1)
                {
                    //Destroy(this.gameObject);

                    text1.enabled = true;
                    text2.enabled = true;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                    {
                        playerLight.intensity = 1;
                        gameUI.SetActive(false);
                        //timerUI.SetActive(false);
                        //minimapUI.SetActive(false);
                        //scoreUI.SetActive(false);
                        dir = (target[0].transform.position - subway.transform.position).normalized;
                        player.GetComponent<Player>().canMove = false;
                        Quaternion rotation = Quaternion.LookRotation(target[0].transform.position - subway.transform.position, Vector3.up);
                        player.transform.rotation = rotation;
                        if ((target[0].transform.position - subway.transform.position).magnitude > 0.1)
                        {
                            moveObject(dir);
                            freezeZombies();
                        }
                        text1.enabled = false;
                        text2.enabled = false;

                        StartCoroutine(WaitEndGameScreen());
                       
                    }

                    //GetComponent<LivingEntity>().gameController.GetComponent<GameController>().UpdateScore(20);

                }
                else
                {
                    text1.enabled = false;
                    text2.enabled = false;
                }


            }
        }
        else
        {
            StartCoroutine(WaitDeathScreen());
        }
    }


    void moveObject(Vector3 direction)
    {
        subway.transform.Translate(direction * speed * Time.deltaTime);
        if(first==false)
            player.transform.Translate(-direction * speed * Time.deltaTime);
        else
            player.transform.Translate(direction * speed * Time.deltaTime);
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

    void freezeZombies()
    {
        foreach(GameObject z in zombies)
        {
            z.GetComponent<Enemy2>().pathfinder.speed = 0f;
        }
    }

    void unfreezeZombies()
    {
        foreach (GameObject z in zombies)
        {
            z.GetComponent<Enemy2>().pathfinder.speed = 3.5f;
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

    public void DeathScreen()
    {
        var ded = player.GetComponent<Player>().dead;
        if (!ded && CountCivilians() > 0) return;

        //if (Input.GetKey(KeyCode.R)) // Restart Scene
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}

        //timerText.enabled = false;
        //timerTextBackground.enabled = false;
        //endGameScreen.enabled = true;
        //endGameScreenBackground.enabled = true;


        //audio.StopAll();
        //audio.Play("Loss");


        //player.GetComponent<Player>().enabled = false;

        //foreach (GameObject zombie in zombies)
        //{
        //    zombie.GetComponent<Enemy2>().dead = true;
        //}



        deathScoreText.text = score.ToString();
        deathScoreTextBackground.text = score.ToString();
        deathScreenUI.SetActive(true);

    }

    IEnumerator WaitDeathScreen()
    {
        audio.StopAll();
        yield return new WaitForSeconds(0.5f);
        audio.Play("Loss");
        DeathScreen();
    }

    IEnumerator WaitEndGameScreen()
    {
        yield return new WaitForSeconds(3f);
        EndGameScreen();
    }

    void EndGameScreen()
    {
        audio.StopAll();
        audio.Play("Victory");
        endgameScoreText.text = score.ToString();
        endGameScoreTextBackground.text = score.ToString();
        endGameScreenUI.SetActive(true);
    }

    public void TryAgain()
    {
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().name));
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().name.Equals("Level1"))
        {
            StartCoroutine(LoadAsynchronously("Level2"));
        }
        else if (SceneManager.GetActiveScene().name.Equals("Level2"))
        {
            StartCoroutine(LoadAsynchronously("StartMenu"));
        }
    }

    public void Menu()
    {
        StartCoroutine(LoadAsynchronously("StartMenu"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
       // yield return new WaitForSeconds(3f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;
            LoadingText.text = ((int)(progress * 100)).ToString() + "%";
            LoadingTextBackground.text = ((int)(progress * 100)).ToString() + "%";

            yield return null;
        }
    }


}
