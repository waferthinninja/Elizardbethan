using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTile : MonoBehaviour {

	private SpriteRenderer rend;

	private ParallaxTile prevTile;

	public float yEntryPoint;
	public float yExitPoint;

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

	public void SetPrevTile (ParallaxTile _prevTile){
		prevTile = _prevTile;
	}

	public void SetYFromPrevTile () {
		float lastY = transform.position.y;
		float moveBy = prevTile.transform.position.y + prevTile.yExitPoint + yEntryPoint;

		// continue extending this class, but reify the ramp tiles- instead of the parallax layer having an array of sprites, it has an array of prefabs. 
		// possibly it has one prefab with an array of sprites and runs sequentially
		// or more than one and follows direction / randomly
		// but it's midnight and that's all folks!
	}
}
