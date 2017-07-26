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

	public void DePopTile () {
		if (transform.childCount >= 1) {
			for (int i = 0; i < transform.childCount; i++) {
				Transform toDestroy = transform.GetChild (i);
				Destroy (toDestroy.gameObject, 0.1f);
			}
		}
	} 
}
