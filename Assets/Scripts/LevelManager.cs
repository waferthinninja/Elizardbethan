using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    private List<Pattern> EasyPatterns;
    private List<Pattern> MediumPatterns;
    private List<Pattern> HardPatterns;

    public TextAsset patternDataFile;

	void Start () 
    {
        // TODO load patterns from file

        string patternData = patternDataFile.text;

        

        EasyPatterns = new List<Pattern>();
        MediumPatterns = new List<Pattern>();
        HardPatterns = new List<Pattern>();

        // there must be at least one pattern in each set 

        // temp
        EasyPatterns.Add(new Pattern());
        MediumPatterns.Add(new Pattern());
        HardPatterns.Add(new Pattern());
		
	}
	
	public Level GenerateLevel(int levelNumber)
	{
        Level level = new Level();

	    int length = 4 + levelNumber / 2;
        int easyCount = Mathf.Max(4 - levelNumber / 2, 0);
        int hardCount = Mathf.Max((levelNumber - 6) / 2, 0);
        int mediumCount = length - (easyCount + hardCount);

        level.Patterns = new List<Pattern>(length);
        AddPatterns(level, easyCount, ref EasyPatterns);
        AddPatterns(level, mediumCount, ref MediumPatterns);
        AddPatterns(level, hardCount, ref HardPatterns);

	    return level;
	}

    private void AddPatterns(Level level, int count, ref List<Pattern> patterns)
    {
        for (int i = 0; i < count; i++)
        {
            int id = Random.Range(0, patterns.Count - 1);
            level.Patterns.Add(patterns[id]);
        }
    }
}

