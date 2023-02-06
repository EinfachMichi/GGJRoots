using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager instance;
    
    [SerializeField] private Transform rightPos, leftPos;
    [SerializeField] private Transform spiderRightPos, spiderLeftPos;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Vector2 spawnCycleRange;
    [SerializeField] private float spawnChanceAnt;
    [SerializeField] private float spawnChanceSnail;
    [SerializeField] private float spawnChanceSpider;
    [SerializeField] private float spawnChanceTank;
    [SerializeField] private TextMeshProUGUI timerDisplay;
    [SerializeField] private GameObject victory;
    [SerializeField] private Animator transAnim;

    private int currentWaveIndex;
    [HideInInspector] public bool leftFree = true, rightFree = true;
    private bool spiderLeft;
    private bool spiderRight;
    private float timer;
    private float seconds;
    private int minutes;
    public bool gameOver;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
        minutes = 10;
        seconds = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        seconds -= Time.deltaTime;
        UpdateDisplay();
        if (seconds <= 0)
        {
            seconds = 59;
            minutes--;
        }
        
        if (timer <= 180)
        {
            spawnCycleRange.y = 3.5f;
        }
        else if (timer <= 270)
        {
            spawnCycleRange.y = 3f;
        }
        else if (timer <= 360)
        {
            spawnCycleRange.y = 2.5f;
        }
        else if (timer <= 450)
        {
            spawnCycleRange.y = 2f;
        }
        else if (timer <= 540)
        {
            spawnCycleRange.y = 1.5f;
        }
    }

    private void UpdateDisplay()
    {
        if (minutes < 10)
        {
            timerDisplay.text = "0" + minutes + ":" + seconds.ToString("0");
            if (seconds <= 9.5f)
            {
                timerDisplay.text = "0" + minutes + ":0" + seconds.ToString("0");
            }
        }
        else
        {
            timerDisplay.text = minutes + ":" + seconds.ToString("0");
            if (seconds <= 9.5f)
            {
                timerDisplay.text = minutes + ":0" + seconds.ToString("0");
            }
        }

        if (minutes == 0 && seconds <= 0)
        {
            timerDisplay.text = "TIME UP!";
            victory.SetActive(true);
            transAnim.SetTrigger("FadeOut");
            AudioManager.instance.Play("Victory", AudioManager.instance.effectSounds);
            gameOver = true;
        }
    }
    
    private IEnumerator SpawnRoutine()
    {
        float spawnCycle = Random.Range(spawnCycleRange.x, spawnCycleRange.y);
        GameObject enemy = enemies[0];
        Transform pos = rightPos;
        float rng = Random.Range(0f, 1f);
        
        if (rng <= spawnChanceTank)
        {
            enemy = enemies[3];
            float dir = Random.Range(0f, 1f);
            if (dir <= 0.5f)
            {
                pos = leftPos;
            }
            else
            {
                pos = rightPos;
            }
        }
        else if (rng <= spawnChanceSpider)
        {
            if (leftFree || rightFree)
            {
                float dir = Random.Range(0f, 1f);
                enemy = enemies[2];
                if (dir <= 0.5f)
                {
                    if (leftFree)
                    {
                        pos = spiderLeftPos;
                        leftFree = false;
                        spiderLeft = true;
                    }
                    else
                    {
                        pos = spiderRightPos;
                        rightFree = false;
                        spiderRight = true;
                    }
                }
                else
                {
                    if (rightFree)
                    {
                        pos = spiderRightPos;
                        rightFree = false;
                        spiderRight = true;
                    }
                    else
                    {
                        pos = leftPos;
                        leftFree = false;
                        spiderLeft = true;
                    }
                }
            }
        }
        else if (rng <= spawnChanceSnail)
        {
            enemy = enemies[1];
            float dir = Random.Range(0f, 1f);
            if (dir <= 0.5f)
            {
                pos = leftPos;
            }
            else
            {
                pos = rightPos;
            }
        }
        else
        {
            enemy = enemies[0];
            float dir = Random.Range(0f, 1f);
            print(dir);
            if (dir >= 0.5f)
            {
                pos = leftPos;
            }
            else
            {
                pos = rightPos;
            }
        }

        GameObject spawn = Instantiate(enemy, pos.position, Quaternion.identity);
        if (spawn.tag == "Spider")
        {
            if (spiderRight) spawn.GetComponent<Spider>().right = true;
            else if (spiderLeft) spawn.GetComponent<Spider>().left = true;
        }

        yield return new WaitForSeconds(spawnCycle);
        StartCoroutine(SpawnRoutine());
    }
}
