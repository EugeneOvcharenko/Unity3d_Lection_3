using UnityEngine;
using System.Collections;

public class FrameAmination : MonoBehaviour {

	public	bool			animate_textures;
	public	bool			animate_sprites;
	public	bool			animate_meshes;

	public	Texture2D[]		textures;
	public	Sprite[]		sprites;
	public	Mesh[]			meshes;

	public	int				index_textures;
	public	int				index_sprites;
	public	int				index_meshes;

	public	Material		rMaterial;
	public	SpriteRenderer	rSprite;
	public	MeshFilter		rMesh;

	public	float			animTime = 0.1f;
	public	CTimer			timer;

	public	AnimationCurve	curve_texture;
	public	AnimationCurve	curve_sprites;
	public	AnimationCurve	curve_meshes;
	
	void Start () {
		timer = new CTimer();

		timer.Start( animTime );
		timer.loop = true;

		index_textures = 0;
		index_sprites = 0;
		index_meshes = 0;
	}
	
	void Update () {
	
		if ( timer.Update ( Time.deltaTime ) )
		{
			int idx;
			if ( animate_textures )
			{
				index_textures = ++index_textures >= textures.Length ? 0 : index_textures;

				idx = Mathf.Min( Mathf.RoundToInt( (float)textures.Length * curve_texture.Evaluate( (float)index_textures / (float)textures.Length ) ), textures.Length - 1 );
				idx = Mathf.Max( 0, idx );
				if ( rMaterial != null )
				{
					rMaterial.mainTexture = textures[ idx ];
				}
			}

			if ( animate_meshes )
			{
				index_meshes = ++index_meshes >= meshes.Length ? 0 : index_meshes;
				idx = Mathf.Min( Mathf.RoundToInt( (float)meshes.Length * curve_meshes.Evaluate( (float)index_meshes / (float)meshes.Length ) ), meshes.Length - 1 );
				idx = Mathf.Max( 0, idx );
				if ( rMesh != null )
				{
					rMesh.mesh = meshes[ idx ];
				}
			}

			if ( animate_sprites )
			{
				index_sprites = ++index_sprites >= sprites.Length ? 0 : index_sprites;
				idx = Mathf.Min( Mathf.RoundToInt( (float)sprites.Length * curve_sprites.Evaluate( (float)index_sprites / (float)sprites.Length ) ), sprites.Length - 1 );
				idx = Mathf.Max( 0, idx );
				if ( rSprite != null )
				{
					rSprite.sprite = sprites[ idx ];
				}
			}
		}
	}
}
