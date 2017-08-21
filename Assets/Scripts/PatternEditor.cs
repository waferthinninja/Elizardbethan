using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

[RequireComponent(typeof(PatternLoader))]
public class PatternEditor : MonoBehaviour {

    public Transform Bounds;
    public List<Transform> Parents;
    public List<Transform> Ignored;

    public string FilePath;

    private XmlDocument xml;

    private PatternLoader _patternLoader;
    

    public void LoadFromSpecifiedFilePath()
    {
        _patternLoader = GetComponent<PatternLoader>();
        try
        {
            Pattern pattern = _patternLoader.LoadPatternFromFile(FilePath);

            ClearExistingItems();

            Bounds.localScale = new Vector3(pattern.Length, Bounds.localScale.y, Bounds.localScale.z);

            foreach (SpawnEvent spawnEvent in pattern.SpawnEvents)
            {
                Transform t = ObjectFactory.Instance.Instantiate(spawnEvent.ObjectName);
                ParallaxLayer pl = GameObject.Find(spawnEvent.Parent).GetComponent<ParallaxLayer>();
                t.SetParent(pl.transform);
                t.position = new Vector3(AdjustForBounds(spawnEvent.Distance, true), spawnEvent.YPosition + t.parent.position.y);
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load pattern");
            Debug.Log(e.Message);
        }

    }

    private void ClearExistingItems()
    {
        foreach (Transform parent in Parents)
        {
            Debug.Log("Clearing objects from " + parent.name);
            for (int i = parent.childCount; i > 0; i--) // Transform obj in parent)
            {
                Transform obj = parent.GetChild(i-1);
                if (!Ignored.Contains(obj))
                {
                    Debug.Log("Destroying " + obj.name);
                    DestroyImmediate(obj.gameObject);
                }
                else
                {
                    Debug.Log("Ignoring " + obj.name);
                }
            }
        }
    }

    public void SaveAsEasy()
    {
        SaveNew("Easy");
    }

    public void SaveAsMedium()
    {
        SaveNew("Medium");
    }

    public void SaveAsHard()
    {
        SaveNew("Hard");
    }

    public void SaveToSpecifiedFilePath()
    {
        SaveAs(FilePath);
    }

    private void SaveNew(string difficulty)
    {
        int suffix = GetNewSuffix(difficulty);
        string filename = string.Format("Assets/Data/Patterns/{0}/{0}{1}.xml", difficulty, suffix); 
        Debug.Log(string.Format("Saving pattern to {0}", filename));
        FilePath = filename;
        SaveAs(filename);
    }

    private int GetNewSuffix(string difficulty)
    {
        int i = 1;
        while (File.Exists(string.Format("Assets/Data/Patterns/{0}/{0}{1}.xml", difficulty, i)))
        {
            i++;
        }
        return i;
    }

    private void SaveAs(string filename)
    {
        FileStream f = File.Open(filename, FileMode.Create);
        StreamWriter sw = new StreamWriter(f);

        xml = new XmlDocument();
        XmlElement patternNode = (XmlElement)xml.AppendChild(xml.CreateElement("pattern"));
        patternNode.AppendChild(xml.CreateElement("length")).InnerText = Bounds.localScale.x.ToString();

        foreach (Transform parent in Parents)
        {
            Debug.Log("Adding children of " + parent.name);
            AddChildObjects(parent, patternNode);
        }
        sw.WriteLine(xml.OuterXml);
        sw.Close();
        f.Close();
    }

    private void AddChildObjects(Transform parent, XmlElement patternNode)
    {
        foreach (Transform t in parent)
        {
            if (!Ignored.Contains(t))
            {
                Debug.Log("Adding object " + t.name);
                XmlElement spawnNode = (XmlElement)patternNode.AppendChild(xml.CreateElement("spawn"));
                spawnNode.AppendChild(xml.CreateElement("distance")).InnerText = AdjustForBounds(t.position.x, false).ToString();
                spawnNode.AppendChild(xml.CreateElement("object")).InnerText = StripDuplicatePart(t.name);
                spawnNode.AppendChild(xml.CreateElement("ypos")).InnerText = t.position.y.ToString();
                spawnNode.AppendChild(xml.CreateElement("parent")).InnerText = parent.name;
            }
            else
            {
                Debug.Log("Ignoring object " + t.name);
            }
        }
    }

    private float AdjustForBounds(float pos, bool loading)
    {
        return pos + ((loading ? -1 : 1) * Bounds.localScale.x / 2f);
    }

    private string StripDuplicatePart(string name)
    {
        if (name.IndexOf('(') < 1)
            return name;

        return name.Substring(0, name.IndexOf('('));
    }
}
