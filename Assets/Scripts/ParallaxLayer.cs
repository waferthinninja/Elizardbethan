using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour {

	[SerializeField]
	private ParallaxTile[] tiles;
	[SerializeField]
	private GameObject tileFab;
	[SerializeField]
	private Sprite[] tileSprites;
	private int spritePointer;


	[SerializeField]
	private float parallaxFactor; // affects size of tile and speed of movement
	private int tileCount;

	private ParallaxManager mgr;

	void Awake () {
		mgr = GetComponentInParent<ParallaxManager> ();
		parallaxFactor = transform.localScale.x; // tiles are assumed to be 1u wide.
		// for now deal with oversize tiles by leaving blanks in the tileset, but not perfect!
	}

	public void SetUpLayer (float xSize) {

		tileCount = (int)(xSize / parallaxFactor) + 1;
		tiles = new ParallaxTile[tileCount];
		float leftEdge = 0 - (xSize / 2);
		for (int i = 0; i < tileCount; i++) {
			GameObject newTile = (GameObject)Instantiate (tileFab, this.transform);
			tiles [i] = newTile.GetComponent<ParallaxTile> ();
			tiles [i].transform.Translate (Vector2.right * (leftEdge + i * parallaxFactor));
			tiles [i].SwapTileSprite (NextSprite ());
		}
	}

	public void XParallax (float xMove) {
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].transform.Translate (Vector2.left * xMove * parallaxFactor);
			if (tiles [i].transform.position.x < mgr.GetLeftEdge ()) {
				tiles [i].transform.Translate (Vector2.right * tileCount * parallaxFactor);
				tiles [i].SwapTileSprite (NextSprite ());
			}
		}
	}

	public void YParallax (float yMove) {
		transform.Translate (Vector2.up * yMove * parallaxFactor);
	}
		
	Sprite NextSprite () {
		Sprite nextSprite = tileSprites [spritePointer];
		spritePointer = (spritePointer + 1) % tileSprites.Length;
		return nextSprite;
	}
}
