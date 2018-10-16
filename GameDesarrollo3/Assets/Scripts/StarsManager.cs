﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    [SerializeField] public GameObject[] star;
    private Level levelInfo;
    [SerializeField]  public bool[] isStarTaken;
    public static StarsManager instance;
    public int levelNumb;


    public static StarsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StarsManager>();
            }
            return instance;
        }
    }

    public void Awake()
    {
        levelInfo = LevelManager.Instance.GetLevel(levelNumb);
        isStarTaken[0] = levelInfo.stars[0];
        isStarTaken[1] = levelInfo.stars[1];
        isStarTaken[2] = levelInfo.stars[2];
        if (isStarTaken[0])
            star[0].gameObject.SetActive(false);
        if (isStarTaken[1])
            star[1].gameObject.SetActive(false);
        if (isStarTaken[2])
            star[2].gameObject.SetActive(false);

        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        
    }

    public void SetStarsTaken(GameObject starN)
    {
        if (starN == star[0])
            isStarTaken[0] = true;
        if (starN == star[1])
            isStarTaken[1] = true;
        if (starN == star[2])
            isStarTaken[2] = true;
    }

    public bool GetStarsTaken(int star)
    {
        return isStarTaken[star];
    }
}
