using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class PatternEditor : MonoBehaviour {

    public Transform Bounds;
    public List<Transform> Parents;
    public List<Transform> Ignored;

    private XmlDocument xml;

    public void Save()
    {
        string filename = string.Format("Pattern_{0}.xml", DateTime.Now.ToString("yyyyMMdd_hhmmss")); 
        Debug.Log(string.Format("Saving pattern to {0}", filename));
        FileStream f = File.Open(filename, FileMode.Create);
        StreamWriter sw = new StreamWriter(f);
        
        xml = new XmlDocument();
        XmlElement patternNode = (XmlElement)xml.AppendChild(xml.CreateElement("pattern"));
        patternNode.AppendChild(xml.CreateElement("length")).InnerText = Bounds.localScale.x.ToString();

        foreach(Transform parent in Parents)
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
                spawnNode.AppendChild(xml.CreateElement("distance")).InnerText = AdjustForBounds(t.position.x).ToString();
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

    private float AdjustForBounds(float pos)
    {
        return pos + (Bounds.localScale.x / 2f);
    }

    private string StripDuplicatePart(string name)
    {
        if (name.IndexOf('(') < 1)
            return name;

        return name.Substring(0, name.IndexOf('('));
    }
}
