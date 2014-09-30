/*
Copyright (c) 2013 Mitch Thompson

Standard MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

// TODO: Add support for SimpleJson parser for usage on WP8
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TexturePackerExtensions{
	public static Rect TPHashtableToRect(this Hashtable table){
		return new Rect((float)table["x"], (float)table["y"], (float)table["w"], (float)table["h"]);
	}
	
	public static Vector2 TPHashtableToVector2(this Hashtable table){
		if(table.ContainsKey("x") && table.ContainsKey("y")){
			return new Vector2((float)table["x"], (float)table["y"]);
		}
		else{
			return new Vector2((float)table["w"], (float)table["h"]);
		}
	}
	
	public static Vector2 TPVector3toVector2(this Vector3 vec){
		return new Vector2(vec.x, vec.y);
	}
	
	public static bool IsTexturePackerTable(this Hashtable table){
		if(table == null) return false;
		
		if(table.ContainsKey("meta")){
			Hashtable metaTable = (Hashtable)table["meta"];
			if(metaTable.ContainsKey("app")){
				return true;
//				if((string)metaTable["app"] == "http://www.texturepacker.com"){
//					return true;	
//				}
			}
		}
		
		return false;
	}
}

public class TexturePacker{
	public class PackedFrame{
		public string name;
		public Rect frame;
		public Rect spriteSourceSize;
		public Vector2 sourceSize;
		public bool rotated;
		public bool trimmed;
		Vector2 atlasSize;
		
		public PackedFrame(string name, Vector2 atlasSize, Hashtable table){
			this.name = name;
			this.atlasSize = atlasSize;
			
			frame = ((Hashtable)table["frame"]).TPHashtableToRect();
			spriteSourceSize = ((Hashtable)table["spriteSourceSize"]).TPHashtableToRect();
			sourceSize = ((Hashtable)table["sourceSize"]).TPHashtableToVector2();
			rotated = (bool)table["rotated"];
			trimmed = (bool)table["trimmed"];
		}
		
		public Mesh BuildBasicMesh(float scale, Color32 defaultColor){
			return BuildBasicMesh(scale, defaultColor, Quaternion.identity);
		}
		
		public Mesh BuildBasicMesh(float scale, Color32 defaultColor, Quaternion rotation){
			Mesh m = new Mesh();
			Vector3[] verts = new Vector3[4];
			Vector2[] uvs = new Vector2[4];
			Color32[] colors = new Color32[4];
		
			
			if(!rotated){
				verts[0] = new Vector3(frame.x,frame.y,0);
				verts[1] = new Vector3(frame.x,frame.y+frame.height,0);
				verts[2] = new Vector3(frame.x+frame.width,frame.y+frame.height,0);
				verts[3] = new Vector3(frame.x+frame.width,frame.y,0);
			}
			else{
				verts[0] = new Vector3(frame.x,frame.y,0);
				verts[1] = new Vector3(frame.x,frame.y+frame.width,0);
				verts[2] = new Vector3(frame.x+frame.height,frame.y+frame.width,0);
				verts[3] = new Vector3(frame.x+frame.height,frame.y,0);
			}
			
			

			
			uvs[0] = verts[0].TPVector3toVector2();
			uvs[1] = verts[1].TPVector3toVector2();
			uvs[2] = verts[2].TPVector3toVector2();
			uvs[3] = verts[3].TPVector3toVector2();
			
			for(int i = 0; i < uvs.Length; i++){
				uvs[i].x /= atlasSize.x;
				uvs[i].y /= atlasSize.y;
				uvs[i].y = 1.0f - uvs[i].y;
			}
			
			if(rotated){
				verts[3] = new Vector3(frame.x,frame.y,0);
				verts[0] = new Vector3(frame.x,frame.y+frame.height,0);
				verts[1] = new Vector3(frame.x+frame.width,frame.y+frame.height,0);
				verts[2] = new Vector3(frame.x+frame.width,frame.y,0);
			}
			
			
			//v-flip
			for(int i = 0; i < verts.Length; i++){
				verts[i].y = atlasSize.y - verts[i].y;
			}
			
			//original origin
			for(int i = 0; i < verts.Length; i++){
				verts[i].x -= frame.x - spriteSourceSize.x + (sourceSize.x/2.0f);
				verts[i].y -= (atlasSize.y - frame.y) - (sourceSize.y - spriteSourceSize.y) + (sourceSize.y/2.0f);
			}
			
			//scaler
			for(int i = 0; i < verts.Length; i++){
				verts[i] *= scale;
			}
			
			//rotator
			if(rotation != Quaternion.identity){
				for(int i = 0; i < verts.Length; i++){
					verts[i] = rotation * verts[i];
				}
			}
		
			for(int i = 0; i < colors.Length; i++){
				colors[i] = defaultColor;
			}
			
			
			m.vertices = verts;
			m.uv = uvs;
			m.colors32 = colors;
			m.triangles = new int[6]{0,3,1,1,3,2};
			
			m.RecalculateNormals();
			m.RecalculateBounds();
			m.name = name;
			
			return m;
		}
	}
	
	public class MetaData{
		public string image;
		public string format;
		public Vector2 size;
		public float scale;
		public string smartUpdate;
		
		public MetaData(Hashtable table){
			image = (string)table["image"];
			format = (string)table["format"];
			size = ((Hashtable)table["size"]).TPHashtableToVector2();
			scale = float.Parse(table["scale"].ToString());
			smartUpdate = (string)table["smartUpdate"];
		}
	}
	
	
	public static Mesh[] ProcessToMeshes(string text){
		return ProcessToMeshes(text, Quaternion.identity, 1f);
	}
	
	public static Mesh[] ProcessToMeshes(string text, Quaternion rotation, float extra_scale )
	{
		if ( text == null )
		{
			return null;
		}
		
		Hashtable table = text.hashtableFromJson();
		
		if ( table == null )
		{
			return null;
		}
		
		MetaData meta = new MetaData((Hashtable)table["meta"]);
		
		List<PackedFrame> frames = new List<PackedFrame>();
		Hashtable frameTable = (Hashtable)table["frames"];
		
		foreach(DictionaryEntry entry in frameTable){
			frames.Add(new PackedFrame((string)entry.Key, meta.size, (Hashtable)entry.Value));
		}
		
		List<Mesh> meshes = new List<Mesh>();
		for(int i = 0; i < frames.Count; i++){
			meshes.Add(frames[i].BuildBasicMesh(0.01f * extra_scale, new Color32(128,128,128,128), rotation));
		}
		
		return meshes.ToArray();
	}
	
	public static MetaData GetMetaData(string text){
		Hashtable table = text.hashtableFromJson();
		MetaData meta = new MetaData((Hashtable)table["meta"]);
		
		return meta;
	}
}
