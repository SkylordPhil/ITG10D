using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    private float time;
    private float score = 0;
    private GameObject player;

    public GameObject userInterface;
    public GameObject timeDisplay;
    public GameObject scoreDisplay;
    public GameObject levelDisplay;
    public GameObject levelProgress;
    public GameObject HealthPoint;

    TextMeshProUGUI time_text;
    TextMeshProUGUI score_text;
    TextMeshProUGUI level_text;

    private int number = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        time_text = timeDisplay.GetComponent<TextMeshProUGUI>();
        score_text = scoreDisplay.GetComponent<TextMeshProUGUI>();
        level_text = levelDisplay.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        int maxHP = player.GetComponent<PlayerController>().currentMaxHealth;
        int currentHP = player.GetComponent<PlayerController>().currentHealth;
        int currentLvl = player.GetComponent<PlayerController>().currentLevel;
        int currentXP = player.GetComponent<PlayerController>().currentXP;
        int neededXP = player.GetComponent<PlayerController>().neededXP;

        UpdateLevel(currentLvl);
        UpdateLevelProgress(currentXP, neededXP);
        UpdateHP(currentHP, maxHP);
        Timer();
        ScoreCounter();
    }

    private void UpdateLevel(int lvl)
    {
        level_text.text = lvl.ToString();
    }

    private void UpdateLevelProgress(int current, int needed)
    {
        if (needed > 0)
        {
            float percent = (float)current / (float)needed;
            levelProgress.GetComponent<Slider>().value = percent;
        }
    }

    public void UpdateHP(int current, int max)
    {
        if (number < max)
        {
            AddMaxHP(max);
        }

        DamageHP(current, max);
    }

    private void AddMaxHP(int max)
    {
        float x = 20;
        float y = -30;
        Transform UIParent = this.transform;

        if (HealthPoint == null)
        {
            HealthPoint = GameObject.FindGameObjectWithTag("HP");
        }

        for (int i = number; i < max; i++)
        {
            GameObject thisHP = Instantiate(HealthPoint);
            thisHP.GetComponent<RectTransform>().position = new Vector2(x + (x * i), y);

            if (i > 0)
            {
               thisHP.GetComponent<RectTransform>().position += new Vector3(5 * i, 0, 0);
            }

            thisHP.transform.SetParent(UIParent, false);
            number++;
        }
    }

    private void DamageHP(int current, int max)
    {
        GameObject[] hpArray = GameObject.FindGameObjectsWithTag("HP");

        for (int i = 0; i <= max; i++)
        {
            if (i <= current)
            {
                hpArray[i].GetComponent<RawImage>().color = Color.red;
            }
            else
            {
                hpArray[i].GetComponent<RawImage>().color = Color.white;
            }
        }
    }

    private void Timer()
    {
        time += Time.deltaTime;

        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");

        time_text.text = string.Format("{0}:{1}", minutes, seconds);
    }

    private void ScoreCounter()
    {
       score_text.text = score.ToString();
    }
}
