using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class LevelManager : MonoBehaviour
{
    private List<Pattern> EasyPatterns;
    private List<Pattern> MediumPatterns;
    private List<Pattern> HardPatterns;

    public TextAsset patternDataFile;

	void Start () 
    {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(patternDataFile.text);
        LoadPatternsFromXml(xml, "easy", ref EasyPatterns);
        LoadPatternsFromXml(xml, "medium", ref MediumPatterns);
        LoadPatternsFromXml(xml, "hard", ref HardPatterns);

        Debug.Log(EasyPatterns.Count);
        Debug.Log(MediumPatterns.Count);
        Debug.Log(HardPatterns.Count);
    }

    void LoadPatternsFromXml(XmlDocument xml, string sectionName, ref List<Pattern> list)
    {
        list = new List<Pattern>();
        var section = xml.SelectNodes(string.Format("//{0}/patterns/pattern", sectionName));
        foreach (XmlNode patternNode in section)
        {
            Pattern pattern = new Pattern();
            pattern.SpawnEvents = new List<SpawnEvent>();
            var spawns = patternNode.SelectNodes("spawn");
            foreach (XmlNode spawnNode in spawns)
            {
                SpawnEvent spawn = new SpawnEvent(float.Parse(spawnNode.SelectSingleNode("distance").InnerText),
                    spawnNode.SelectSingleNode("object").InnerText,
                    float.Parse(spawnNode.SelectSingleNode("ypos").InnerText),
                    spawnNode.SelectSingleNode("parent").InnerText);
                pattern.SpawnEvents.Add(spawn);
            }
            list.Add(pattern);
        }
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

