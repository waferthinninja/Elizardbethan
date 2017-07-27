using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTile : MonoBehaviour {

	private SpriteRenderer rend;

	private ParallaxTile prevTile;
	[SerializeField]
	public float yEntryPoint;
	[SerializeField]
	public float yExitPoint;

	private ParallaxTileset tileSet;

	private int spritePointer;

	void Awake () {
		rend = GetComponent<SpriteRenderer> ();
	}

	public void SwapTileSprite() {
		rend.sprite = NextSprite();
	}

	public void DePopTile () {
		if (transform.childCount >= 1) {
			for (int i = 0; i < transform.childCount; i++) {
				Transform toDestroy = transform.GetChild (i);
				Destroy (toDestroy.gameObject, 0.1f);
			}
		}
	} 

	public void SetTileset (ParallaxTileset _tileSet) {
		tileSet = _tileSet;
	}

	public void SetPrevTile (ParallaxTile _prevTile){
		prevTile = _prevTile;
	}

	public void SetYFromPrevTile () {
		float lastY = transform.position.y;
		float moveBy = prevTile.transform.position.y + prevTile.yExitPoint + yEntryPoint;
	}
		
	Sprite NextSprite () {
		Sprite nextSprite = tileSet.GetSprite(spritePointer);
		spritePointer = (spritePointer + 1) % tileSet.GetTileSetSize(); // if we are a sequential tileset, we need to look at prevTile for our sprite pointer!
		return nextSprite;
	}
}
