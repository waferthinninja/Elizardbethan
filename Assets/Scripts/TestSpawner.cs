using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    private int _levelNumber;
    private Level _level;
    private Pattern _pattern;

    private float _distance; // distance through the current pattern
    

    private void StartNextLevel()
    {        
        _levelNumber++;
        //Debug.Log("Starting level " + _levelNumber);
        _level = LevelManager.Instance.GetLevel(_levelNumber);
        StartNextPattern();
    }

    private void StartNextPattern()
    {
        _pattern = _level.Patterns.Dequeue();

        //Debug.Log("Starting Pattern of length " + _pattern.Length + " with " + _pattern.SpawnEvents.Count + " events");
    }

    
    void Update()
    {
        _distance += ParallaxManager.Instance.GetXSpeed();
        if ( _level == null //_level.Equals(default(Level)) 
            || (_distance > _pattern.Length && _level.Patterns.Count == 0))
        {
            if (_pattern != null)
                _distance -= _pattern.Length;
            StartNextLevel();
        }
        else if (_distance > _pattern.Length)
        {
            _distance -= _pattern.Length;
            StartNextPattern();
        }

        // see if the next event is due
        if (_pattern.SpawnEvents.Count > 0)
        {
            SpawnEvent e = _pattern.SpawnEvents.Peek();

            while (e.Distance <= _distance)
            {
                DoSpawnEvent(_pattern.SpawnEvents.Dequeue());
                if (_pattern.SpawnEvents.Count > 0)
                {
                    e = _pattern.SpawnEvents.Peek();
                }
                else break;
            }
        }     
    }

    private void DoSpawnEvent(SpawnEvent spawnEvent)
    {
      //  Debug.Log("Spawning " + spawnEvent.ObjectName);
        Transform t = ObjectFactory.Instance.Instantiate(spawnEvent.ObjectName);
        ParallaxLayer pl = GameObject.Find(spawnEvent.Parent).GetComponent<ParallaxLayer>();
        t.SetParent(pl.GetLastTile().transform);
        t.position = new Vector3(ParallaxManager.Instance.GetRightEdge(), spawnEvent.YPosition + t.parent.position.y);
    }

    
}
