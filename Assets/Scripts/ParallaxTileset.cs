using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTileset : MonoBehaviour {

	[SerializeField]
	private Sprite[] tileSprites;

	public Sprite GetSprite(int i) {
		if (i >= 0 && i < tileSprites.Length) {
			return tileSprites [i];
		} else {
			return null;
		}
	}

	public float GetTileSize () {
		return tileSprites [0].bounds.max.x * 2;
	}

	public int GetTileSetSize() {
		return tileSprites.Length;
	}
}
