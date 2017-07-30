using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParallaxTileset : MonoBehaviour {

	[SerializeField]
	private Sprite[] tileSprite;
	[SerializeField]
	private float[] yEntryPoint;
	[SerializeField]
	private float[] yExitPoint;
	[SerializeField]
	public bool yChanges = false;


	public Sprite GetTileSprite(int i) {
		return tileSprite [i];
	}

	public float[] GetEntryExit (int i) {
		float[] entryExit = new float[2];
		entryExit[0] = yEntryPoint[i];
		entryExit[1] = yExitPoint[i];
		return entryExit;
	}

	public float GetTileSize () {
		return tileSprite[0].bounds.max.x * 2;
	}

	public int GetTileSetSize() {
		return tileSprite.Length;
	}
}
