/*
Copyright (c) 2013 Mitch Thompson

Standard MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;


public static class TexturePackerImport{
[MenuItem("Assets/TexturePacker/Process to Meshes")]		static Mesh[] ProcessToMeshesDef(){	return ProcessToMeshes( 0.0f );}
[MenuItem("Assets/TexturePacker/Process to Meshes 100%")]	static Mesh[] ProcessToMeshes100(){	return ProcessToMeshes( 1.0f );}
[MenuItem("Assets/TexturePacker/Process to Meshes 40%")]	static Mesh[] ProcessToMeshes40(){	return ProcessToMeshes( 0.4f );}
	
	
	static Mesh[] ProcessToMeshes( float set_scale )
	{
		TextAsset txt = (TextAsset)Selection.activeObject;
		
		Quaternion rotation = Quaternion.identity;
		string pref = EditorPrefs.GetString("TexturePackerImporterFacing", "back");
		
		switch(pref){
		case "back":
			rotation = Quaternion.identity;
			break;
		case "forward":
			rotation = Quaternion.LookRotation(Vector3.back);
			break;
		case "up":
			rotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);
			break;
		case "down":
			rotation = Quaternion.LookRotation(Vector3.up, Vector3.back);
			break;
		case "right":
			rotation = Quaternion.LookRotation(Vector3.left);
			break;
		case "left":
			rotation = Quaternion.LookRotation(Vector3.right);
			break;
		}
		
				
		Mesh[] meshes;
		string name_extra = string.Empty;
		if ( set_scale != 0 )
		{
			meshes = TexturePacker.ProcessToMeshes(txt.text, rotation, set_scale);
			name_extra = "_" + ((int)(set_scale * 100)).ToString();
		}
		else
		{
			meshes = TexturePacker.ProcessToMeshes(txt.text, rotation, 1f);
		}
		
		string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(txt));
		
		Directory.CreateDirectory(Application.dataPath + "/" + rootPath.Substring(7, rootPath.Length-7) + "/Meshes");
		
		Mesh[] returnMeshes = new Mesh[meshes.Length];
		
		int i = 0;
		foreach(Mesh m in meshes){
			string assetPath = rootPath + "/Meshes/" + Path.GetFileNameWithoutExtension(m.name) + name_extra + ".asset";
			Mesh existingMesh = (Mesh)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Mesh));
			if(existingMesh == null){
				AssetDatabase.CreateAsset(m, assetPath);
				existingMesh = (Mesh)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Mesh));
			}
			else{
				existingMesh.triangles = new int[0];
				existingMesh.colors32 = new Color32[0];
				existingMesh.uv = new Vector2[0];
				existingMesh.vertices = m.vertices;
				existingMesh.uv = m.uv;
				existingMesh.colors32 = m.colors32;
				existingMesh.triangles = m.triangles;
				existingMesh.RecalculateNormals();
				existingMesh.RecalculateBounds();
				EditorUtility.SetDirty(existingMesh);
				Mesh.DestroyImmediate(m);
			}
			
			returnMeshes[i] = existingMesh;
			i++;
		}	
		
		return returnMeshes;
	}
	
	static Mesh CreateQuad(float w, float h)
	{
		Mesh m = new Mesh();
		Vector3[] verts	= new Vector3[4];
		Vector2[] uvs	= new Vector2[4];
		
		float wh = w / 2.0f;
		float hh = h / 2.0f;
		
		verts[0] = new Vector3(-wh,-hh, 0);
		verts[1] = new Vector3(-wh, hh, 0);
		verts[2] = new Vector3( wh, hh, 0);
		verts[3] = new Vector3( wh,-hh, 0);
		
		uvs[0] = new Vector2( 0, 0 );
		uvs[1] = new Vector2( 0, 1 );
		uvs[2] = new Vector2( 1, 1 );
		uvs[3] = new Vector2( 1, 0 );
		
		m.vertices = verts;
		m.uv = uvs;
		//m.triangles = new int[6]{0,3,1,1,3,2};
		m.triangles = new int[6]{0,1,3,1,2,3};
		/*
		 * 0 3
		 * 1 2
		 * 
		 * 
		 */ 
		m.RecalculateNormals();
		m.RecalculateBounds();
		m.name = "quad_" + w.ToString() + "_" + h.ToString();
		
		return m;
	}
	
	[MenuItem("Assets/CreateMeshes/Create Quad 64*64")]		static Mesh CreateQuadMesh_64x64(){return CreateQuadMesh( 64, 64 );}
	[MenuItem("Assets/CreateMeshes/Create Quad 128*128")]	static Mesh CreateQuadMesh_128x128(){return CreateQuadMesh( 128, 128 );}
	[MenuItem("Assets/CreateMeshes/Create Quad 256*256")]	static Mesh CreateQuadMesh_256x256(){return CreateQuadMesh( 256, 256 );}
	[MenuItem("Assets/CreateMeshes/Create Quad 512*512")]	static Mesh CreateQuadMesh_512x512(){return CreateQuadMesh( 512, 512 );}
	[MenuItem("Assets/CreateMeshes/Create Quad 1024*1024")]	static Mesh CreateQuadMesh_1024x1024(){return CreateQuadMesh( 1024, 1024 );}
	[MenuItem("Assets/CreateMeshes/Create Quad 2048*2048")]	static Mesh CreateQuadMesh_2048x2048(){return CreateQuadMesh( 2048, 2048 );}
	
	static Mesh CreateQuadMesh( float w, float h )
	{
		//Application.dataPath
		string rootPath = "Assets/CreatedMeshes";
		Directory.CreateDirectory( rootPath );
		Mesh m = CreateQuad( w, h );
		string assetPath = rootPath + "/" + Path.GetFileNameWithoutExtension( m.name ) + ".asset";
		
		Mesh existingMesh = (Mesh)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Mesh));
		
		if(existingMesh == null)
		{
			Debug.Log( "assetPath: " + assetPath + ", m: " + m );
			AssetDatabase.CreateAsset(m, assetPath);
			existingMesh = (Mesh)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Mesh));
		}
		else
		{
			existingMesh.triangles = new int[0];
			existingMesh.colors32 = new Color32[0];
			existingMesh.uv = new Vector2[0];
			existingMesh.vertices = m.vertices;
			existingMesh.uv = m.uv;
			existingMesh.colors32 = m.colors32;
			existingMesh.triangles = m.triangles;
			existingMesh.RecalculateNormals();
			existingMesh.RecalculateBounds();
			EditorUtility.SetDirty(existingMesh);
			Mesh.DestroyImmediate(m);
		}
			
		return m;
	}

	[MenuItem("Assets/TexturePacker/Process to Prefabs")]
	static void ProcessToPrefabs(){
		Mesh[] meshes = ProcessToMeshesDef();
		
		
		TextAsset txt = (TextAsset)Selection.activeObject;
		string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(txt));
		
		string prefabPath = rootPath.Substring(7, rootPath.Length-7) + "/Prefabs";
		Directory.CreateDirectory(Application.dataPath + "/" + prefabPath);
		
		prefabPath = "Assets/" + prefabPath;
		
		
		//make material
		TexturePacker.MetaData meta = TexturePacker.GetMetaData(txt.text);
		
		string matPath = rootPath + "/" + (Path.GetFileNameWithoutExtension(meta.image) + ".mat");
		string texturePath = rootPath + "/" + meta.image;
		Material mat = (Material)AssetDatabase.LoadAssetAtPath(matPath, typeof(Material));
		if(mat == null){
			mat = new Material(Shader.Find("Sprites/Transparent Unlit"));
			Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
			if(tex == null){
				EditorUtility.DisplayDialog("Error!", "Texture " + meta.image + " not found!", "Ok");	
			}
			mat.mainTexture = tex;
			AssetDatabase.CreateAsset(mat, matPath);
		}
		
		
		AssetDatabase.Refresh();
		
		for(int i = 0; i < meshes.Length; i++){
			string prefabFilePath = prefabPath + "/" + meshes[i].name + ".prefab";
			
			bool createdNewPrefab = false;
			Object prefab = AssetDatabase.LoadAssetAtPath(prefabFilePath, typeof(Object));
			
			if(prefab == null){
				prefab = PrefabUtility.CreateEmptyPrefab(prefabFilePath);
				createdNewPrefab = true;
			}
			
			if(createdNewPrefab){
				GameObject go = new GameObject(meshes[i].name, typeof(MeshRenderer), typeof(MeshFilter));
				go.GetComponent<MeshFilter>().sharedMesh = meshes[i];
				go.renderer.sharedMaterial = mat;
				
				PrefabUtility.ReplacePrefab(go, prefab, ReplacePrefabOptions.ConnectToPrefab);
				
				GameObject.DestroyImmediate(go);
			}
			else{
				GameObject pgo = (GameObject)prefab;
				pgo.renderer.sharedMaterial = mat;
				pgo.GetComponent<MeshFilter>().sharedMesh = meshes[i];
				EditorUtility.SetDirty(pgo);
			}
		}
	}
	
	//Validators
	[MenuItem("Assets/TexturePacker/Process to Prefabs", true)]
	[MenuItem("Assets/TexturePacker/Process to Meshes", true)]
	static bool ValidateProcessTexturePacker(){
		Object o = Selection.activeObject;
		
		if(o == null)
			return false;
		
		if(o.GetType() == typeof(TextAsset)){
			return (((TextAsset)o).text.hashtableFromJson()).IsTexturePackerTable();
		}
		
		return false;
	}
	

	
	
	//Attach 90 degree "Shadow" meshes
	[MenuItem("Assets/TexturePacker/Attach Shadow Mesh")]
	static void AttachShadowMesh(){
		List<Mesh> meshes = new List<Mesh>();
		foreach(Object o in Selection.objects){
			if(o is Mesh) meshes.Add(o as Mesh);	
		}
		
		foreach(Mesh m in meshes){
			Vector3[] verts = new Vector3[m.vertexCount*2];
			Vector2[] uvs = new Vector2[m.vertexCount*2];
			Color32[] colors = new Color32[m.vertexCount*2];
			int[] triangles = new int[m.triangles.Length * 2];
			
			System.Array.Copy(m.vertices, 0, verts, m.vertexCount, m.vertexCount);
			System.Array.Copy(m.uv, 0, uvs, m.vertexCount, m.vertexCount);
			System.Array.Copy(m.colors32, 0, colors, m.vertexCount, m.vertexCount);
			System.Array.Copy(m.triangles, 0, triangles, m.triangles.Length, m.triangles.Length);
			
			for(int i = 0; i < m.vertexCount; i++){
				verts[i].x = verts[i+m.vertexCount].x;
				verts[i].y = verts[i+m.vertexCount].z;
				verts[i].z = verts[i+m.vertexCount].y;
				
				uvs[i] = uvs[i+m.vertexCount];
				colors[i] = new Color32(0,0,0,64);
				
					
			}
			
			for(int i = 0; i < m.triangles.Length; i++){
				triangles[i] = triangles[i + m.triangles.Length];
				triangles[i + m.triangles.Length] += m.vertexCount;
				
			}
						
			m.vertices = verts;
			m.uv = uvs;
			m.colors32 = colors;
			m.triangles = triangles;
			
			m.RecalculateNormals();
			m.RecalculateBounds();
			EditorUtility.SetDirty(m);
		}
	}
	
	
	//Validators
	[MenuItem("Assets/TexturePacker/Attach Shadow Mesh", true)]
	static bool ValidateAttachShadowMesh(){
		Object[] objs = Selection.objects;
		foreach(Object o in objs){
			if(!(o is Mesh)){
				return false;
			}
		}
		
		return true;
	}
	
	
	
	//Options
	[MenuItem("Assets/TexturePacker/Facing/Back")]
	static void SetFacingBack(){	EditorPrefs.SetString("TexturePackerImporterFacing", "back"); }
	
	[MenuItem("Assets/TexturePacker/Facing/Forward")]
	static void SetFacingForward(){	EditorPrefs.SetString("TexturePackerImporterFacing", "forward"); }
	
	[MenuItem("Assets/TexturePacker/Facing/Up")]
	static void SetFacingUp(){	EditorPrefs.SetString("TexturePackerImporterFacing", "up"); }
	
	[MenuItem("Assets/TexturePacker/Facing/Down")]
	static void SetFacingDown(){	EditorPrefs.SetString("TexturePackerImporterFacing", "down"); }
	
	[MenuItem("Assets/TexturePacker/Facing/Right")]
	static void SetFacingRight(){	EditorPrefs.SetString("TexturePackerImporterFacing", "right"); }
	
	[MenuItem("Assets/TexturePacker/Facing/Left")]
	static void SetFacingLeft(){	EditorPrefs.SetString("TexturePackerImporterFacing", "left"); }
}
