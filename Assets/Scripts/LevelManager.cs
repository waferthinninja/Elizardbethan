using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using Assets.Scripts.Util;
using System;

[RequireComponent(typeof(PatternLoader))]
public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<LevelManager>();
            return instance;
        }
    }

    private List<Pattern> EasyPatterns;
    private List<Pattern> MediumPatterns;
    private List<Pattern> HardPatterns;
    
    public int PoolSize = 50; // prob want to make this large enough that the player is unlikely to ever need new levels
                            // note this is not really effectively pooling since we are going to "new" patterns etc even if we reuse the level
                            // but moot if getting to this sort of level is virtually impossible
    public Level[] LevelPool;
    //private int _levelIndex = 0;

    private PatternLoader _patternLoader;

	void Start () 
    {
        _patternLoader = GetComponent<PatternLoader>();

        LoadPatterns("easy", ref EasyPatterns);
        LoadPatterns("medium", ref MediumPatterns);
        LoadPatterns("hard", ref HardPatterns);

        Debug.Log(EasyPatterns.Count + " easy patterns loaded");
        Debug.Log(MediumPatterns.Count + " medium patterns loaded");
        Debug.Log(HardPatterns.Count + " hard patterns loaded");

        PopulateLevelPool();
    }

    public Level GetLevel(int levelNumber)
    {
        int i = (levelNumber - 1) % PoolSize;
        if (LevelPool[i].LevelNumber != levelNumber)
        {
            LevelPool[i] = GenerateLevel(levelNumber);
        }
        return LevelPool[i];
    }

    private void PopulateLevelPool()
    {

        Debug.Log("Pregenerating " + PoolSize + " levels ");
        LevelPool = new Level[PoolSize];
        for (int i = 1; i <= PoolSize; i++)
        {
            LevelPool[i-1] = GenerateLevel(i);
        }
    }

    void LoadPatterns(string sectionName, ref List<Pattern> list)
    {
        list = new List<Pattern>();

        string[] files = Directory.GetFiles("Assets/Data/Patterns/" + sectionName);

        foreach (string file in files)
        {
            if (file.EndsWith(".xml"))
                list.Add(_patternLoader.LoadPatternFromFile(file));
        }        
    }


	public Level GenerateLevel(int levelNumber)
    {
        Level level = new Level();
        level.LevelNumber = levelNumber;
        AddPatterns(levelNumber, level);

        return level;
    }
    
    private void AddPatterns(int levelNumber, Level level)
    {
        int length = 4 + levelNumber / 2;
        int easyCount = Mathf.Max(4 - levelNumber / 2, 0);
        int hardCount = Mathf.Max((levelNumber - 6) / 2, 0);
        int mediumCount = length - (easyCount + hardCount);

        level.Patterns = new Queue<Pattern>(length);
        AddPatterns(level, easyCount, ref EasyPatterns);
        AddPatterns(level, mediumCount, ref MediumPatterns);
        AddPatterns(level, hardCount, ref HardPatterns);
    }

    private void AddPatterns(Level level, int count, ref List<Pattern> patterns)
    {
        for (int i = 0; i < count; i++)
        {
            int id = UnityEngine.Random.Range(0, patterns.Count - 1);
            Pattern pattern = Util.DeepCopy<Pattern>(patterns[id]);                        
            level.Patterns.Enqueue(pattern);
        }
    }
}

