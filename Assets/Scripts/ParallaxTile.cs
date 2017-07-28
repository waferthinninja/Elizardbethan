using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTile : MonoBehaviour {

	private SpriteRenderer rend;

	[SerializeField]
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

	public int GetSpritePointer() {
		return spritePointer;
	}

	public void SetYFromPrevTile () {
		float lastY = transform.position.y;
		float moveBy = prevTile.transform.position.y + prevTile.yExitPoint + yEntryPoint;
	}
		
	Sprite NextSprite () {
		spritePointer = (prevTile.GetSpritePointer () + 1) % tileSet.GetTileSetSize ();
		Sprite nextSprite = tileSet.GetSprite(spritePointer);
		return nextSprite;
	}

	public void SetTileSprite (int spritePointer) {
		rend.sprite = tileSet.GetSprite (spritePointer);
	}
}
