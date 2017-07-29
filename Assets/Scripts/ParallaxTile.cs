using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxTile : MonoBehaviour {

	private SpriteRenderer rend;

	[SerializeField]
	private ParallaxTile prevTile;
	[SerializeField]
	private float yEntryPoint; 
	[SerializeField]
	private float yExitPoint;

	private ParallaxTileset tileSet;

	private int spritePointer;

	void Awake () {
		rend = GetComponent<SpriteRenderer> ();
	}

	public void SwapTileSprite() {
		rend.sprite = NextSprite();
	}

	public void DespawnRespawn() {
		DePopTile ();
		SwapTileSprite ();
		float yDiff = prevTile.transform.position.y + prevTile.yExitPoint - transform.position.y - yEntryPoint;
		Debug.Log (this.ToString () + " " + yDiff);
		transform.Translate (Vector2.up * yDiff);
	}

	private void DePopTile () {
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
		
	Sprite NextSprite () {
		spritePointer = (prevTile.GetSpritePointer () + 1) % tileSet.GetTileSetSize ();
		Sprite nextSprite = tileSet.GetTileSprite(spritePointer);
		float[] entryExit = tileSet.GetEntryExit (spritePointer);
		yEntryPoint = entryExit [0];
		yExitPoint = entryExit [1];
		return nextSprite;
	}

	public void SetTileSprite (int spritePointer) {
		rend.sprite = tileSet.GetTileSprite (spritePointer);
	}

}
