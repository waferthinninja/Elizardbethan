using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour {

	[SerializeField]
	private ParallaxTile[] tiles;
	[SerializeField]
	private GameObject tileFab;
	[SerializeField]
	private ParallaxTileset tileSet;

	[SerializeField]
	private TestSpawner spawner;

	[SerializeField]
	private float parallaxFactor;
	private int layerUnitSize;
	private float tileSize;
	private int numTiles;
    
	public void SetUpLayer (float xSize) {
		
		parallaxFactor = transform.localScale.x;
		layerUnitSize = (int)(xSize / parallaxFactor) + 1;
		tileSize = tileSet.GetTileSize ();
		numTiles = Mathf.RoundToInt (layerUnitSize / tileSize + 0.5f);
		tiles = new ParallaxTile[numTiles];
		float leftEdge = 0 - (xSize / 2);

		for (int i = 0; i < numTiles; i++) {

			GameObject newTile = (GameObject)Instantiate (tileFab, this.transform);
			tiles [i] = newTile.GetComponent<ParallaxTile> ();
			tiles [i].transform.Translate (Vector2.right * (leftEdge + i * tileSize * parallaxFactor));
            
			if (i > 0) {
				tiles [i].SetPrevTile (tiles [i - 1]);
			} 
		}

		tiles [0].SetPrevTile (tiles [numTiles - 1]);

		for (int i = 0; i < numTiles; i++) {
			tiles [i].SetTileset (tileSet);
			if (i == 0) {
				tiles [i].SetTileSprite (0);
			} else {
				tiles [i].SwapTileSprite ();
			}
		}
	}

	public void XParallax (float xMove) {
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].transform.Translate (Vector2.left * xMove * parallaxFactor);

			if (tiles [i].transform.position.x < ParallaxManager.Instance.GetLeftEdge ()) {
				tiles [i].transform.Translate (Vector2.right * numTiles * tileSize * parallaxFactor);
				tiles [i].DespawnRespawn ();
				//if (spawner != null) {
				//	spawner.SpawnOnTile (tiles [i]);
				//}
			}
		}
	}

	public void YParallax (float yMove) {
		transform.Translate (Vector2.up * yMove * parallaxFactor);
	}
		
    public GameObject GetLastTile()
    {
        return tiles[tiles.Length - 1].gameObject;
    }

}
