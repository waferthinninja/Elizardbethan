using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pattern {

    public float Length; // length of this pattern in game units
    public Queue<SpawnEvent> SpawnEvents;

}
