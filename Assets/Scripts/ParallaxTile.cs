using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTile : MonoBehaviour {

	private SpriteRenderer rend;

	void Awake () {
		rend = GetComponent<SpriteRenderer> ();
	}

	public void SwapTileSprite(Sprite sprite) {
		rend.sprite = sprite;
	}
}
