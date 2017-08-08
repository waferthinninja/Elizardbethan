using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnEvent {

    public float Distance;
    public string ObjectName;
    public float YPosition;
    public string Parent;

    public SpawnEvent(float distance, string objectName, float yPosition, string parent)
    {
        Distance = distance;
        ObjectName = objectName;
        YPosition = yPosition;
        Parent = parent;
    }
}
