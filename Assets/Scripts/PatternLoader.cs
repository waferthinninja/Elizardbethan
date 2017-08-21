using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;

public class PatternLoader : MonoBehaviour {

    public Pattern LoadPatternFromFile(string file)
    {
        Debug.Log("Loading pattern from file " + file);
        var pattern = new Pattern();
        var xml = new XmlDocument();
        xml.LoadXml(File.ReadAllText(file));

        var patternNode = xml.SelectSingleNode("pattern");
        pattern.Length = float.Parse(patternNode.SelectSingleNode("length").InnerText);

        var spawnEvents = new List<SpawnEvent>();
        var spawns = patternNode.SelectNodes("spawn");
        foreach (XmlNode spawnNode in spawns)
        {
            SpawnEvent spawn = new SpawnEvent(float.Parse(spawnNode.SelectSingleNode("distance").InnerText),
                spawnNode.SelectSingleNode("object").InnerText.Trim(),
                float.Parse(spawnNode.SelectSingleNode("ypos").InnerText),
                spawnNode.SelectSingleNode("parent").InnerText);
            spawnEvents.Add(spawn);
        }

        // sort the list before applying to the pattern as a queue 
        spawnEvents.Sort(CompareByDistance);
        pattern.SpawnEvents = new Queue<SpawnEvent>(spawnEvents);

        return pattern;
    }
    
    private static int CompareByDistance(SpawnEvent e1, SpawnEvent e2)
    {
        return e1.Distance.CompareTo(e2.Distance);
    }
}
