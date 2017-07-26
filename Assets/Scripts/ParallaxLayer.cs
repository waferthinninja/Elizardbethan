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
	private TestSpawner spawner;

	[SerializeField]
	private float parallaxFactor; // affects size of tile and speed of movement
	private int layerUnitSize;
	private float tileSize;
	private int numTiles;

	private ParallaxManager mgr;

	void Awake () {
	}

	public void SetUpLayer (float xSize) {
		mgr = GetComponentInParent<ParallaxManager> ();
		parallaxFactor = transform.localScale.x;
		layerUnitSize = (int)(xSize / parallaxFactor) + 1;
		tileSize = tileSprites [0].bounds.max.x * 2;
		numTiles = Mathf.RoundToInt (layerUnitSize / tileSize + 0.5f);
		//Debug.Log (numTiles.ToString ());
		tiles = new ParallaxTile[numTiles];
		float leftEdge = 0 - (xSize / 2);
		for (int i = 0; i < numTiles; i++) {
			GameObject newTile = (GameObject)Instantiate (tileFab, this.transform);
			tiles [i] = newTile.GetComponent<ParallaxTile> ();
			tiles [i].transform.Translate (Vector2.right * (leftEdge + i * tileSize * parallaxFactor));
			tiles [i].SwapTileSprite (NextSprite ());
		}
	}

	public void XParallax (float xMove) {
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].transform.Translate (Vector2.left * xMove * parallaxFactor);
			if (tiles [i].transform.position.x < mgr.GetLeftEdge ()) {
				tiles [i].transform.Translate (Vector2.right * numTiles * tileSize * parallaxFactor);
				tiles [i].SwapTileSprite (NextSprite ());
				tiles [i].DePopTile ();
				if (spawner != null) {
					spawner.SpawnOnTile (tiles [i]);
				}
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
