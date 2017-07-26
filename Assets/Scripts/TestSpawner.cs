using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour {


	[SerializeField]
	private GameObject[] spawningPrefabs;
	private int spawnIndex;
	[SerializeField]
	private int spawnEveryXTiles;
	private int tileCount = 0;

	public void SpawnOnTile(ParallaxTile tile) {
		tileCount++;
		if (tileCount >= spawnEveryXTiles) {
			tileCount = 0;
			Instantiate (spawningPrefabs [spawnIndex], tile.transform); // the real spawner should use pooling.
			spawnIndex = (spawnIndex + 1) % spawningPrefabs.Length;
		}
	}
}
