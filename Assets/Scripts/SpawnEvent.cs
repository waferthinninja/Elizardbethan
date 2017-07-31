using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpawnEvent {

    float Distance;
    string ObjectName;
    float YPosition;
    string Parent;

    public SpawnEvent(float distance, string objectName, float yPosition, string parent)
    {
        Distance = distance;
        ObjectName = objectName;
        YPosition = yPosition;
        Parent = parent;
    }
}
