/*
	Mesh Tools - Mesh Tweaker
    Copyright (C) 2013 Mitch Thompson

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MeshTweaker{
	[MenuItem("Assets/MeshTweaker/Rotate/X45")]
	static void RotateX45(){BatchRotate(45,0,0);}
	[MenuItem("Assets/MeshTweaker/Rotate/X90")]
	static void RotateX90(){BatchRotate(90,0,0);}
	
	[MenuItem("Assets/MeshTweaker/Rotate/Y45")]
	static void RotateY45(){BatchRotate(0,45,0);}
	[MenuItem("Assets/MeshTweaker/Rotate/Y90")]
	static void RotateY90(){BatchRotate(0,90,0);}
	
	[MenuItem("Assets/MeshTweaker/Rotate/Z45")]
	static void RotateZ45(){BatchRotate(0,0,45);}
	[MenuItem("Assets/MeshTweaker/Rotate/Z90")]
	static void RotateZ90(){BatchRotate(0,0,90);}
	
	static void BatchRotate(float x, float y, float z){
		Quaternion quat = Quaternion.Euler(x,y,z);
		foreach(Object o in Selection.objects){
			if(o is Mesh){
				RotateMesh(o as Mesh, quat);
			}
		}	
	}
	
	
	static void RotateMesh(Mesh mesh, Quaternion quat){
		Vector3[] verts = mesh.vertices;
		for(int i = 0; i < verts.Length; i++){
			verts[i] = quat * verts[i];	
		}
		
		mesh.vertices = verts;
		
		mesh.RecalculateNormals();
		
		EditorUtility.SetDirty(mesh);
	}	
	
	#region Scale tweak
	[MenuItem("Assets/MeshTweaker/Scale/10%")] static void ScaleMesh10(){BatchScale( 0.1f );}
	[MenuItem("Assets/MeshTweaker/Scale/20%")] static void ScaleMesh20(){BatchScale( 0.2f );}
	[MenuItem("Assets/MeshTweaker/Scale/30%")] static void ScaleMesh30(){BatchScale( 0.3f );}
	[MenuItem("Assets/MeshTweaker/Scale/40%")] static void ScaleMesh40(){BatchScale( 0.4f );}
	[MenuItem("Assets/MeshTweaker/Scale/50%")] static void ScaleMesh50(){BatchScale( 0.5f );}
	[MenuItem("Assets/MeshTweaker/Scale/60%")] static void ScaleMesh60(){BatchScale( 0.6f );}
	[MenuItem("Assets/MeshTweaker/Scale/70%")] static void ScaleMesh70(){BatchScale( 0.7f );}
	[MenuItem("Assets/MeshTweaker/Scale/80%")] static void ScaleMesh80(){BatchScale( 0.8f );}
	[MenuItem("Assets/MeshTweaker/Scale/90%")] static void ScaleMesh90(){BatchScale( 0.9f );}
	
	[MenuItem("Assets/MeshTweaker/Scale/75%")] static void ScaleMesh75(){BatchScale( 0.75f );}
	[MenuItem("Assets/MeshTweaker/Scale/25%")] static void ScaleMesh25(){BatchScale( 0.25f );}
	
	[MenuItem("Assets/MeshTweaker/Scale/125%")] static void ScaleMesh125(){BatchScale( 1.25f );}
	[MenuItem("Assets/MeshTweaker/Scale/150%")] static void ScaleMesh150(){BatchScale( 1.50f );}
	[MenuItem("Assets/MeshTweaker/Scale/200%")] static void ScaleMesh200(){BatchScale( 2.00f );}
	
	[MenuItem("Assets/MeshTweaker/Scale/x10")] static void ScaleMeshx10(){BatchScale( 10.0f );}
	[MenuItem("Assets/MeshTweaker/Scale/x100")] static void ScaleMeshx100(){BatchScale( 100.0f );}
	
	static void BatchScale(float s){		
		foreach(Object o in Selection.objects){
			if(o is Mesh){
				ScaleMesh(o as Mesh, s);
			}
		}	
	}
	
	static void ScaleMesh(Mesh mesh, float scale){
		Vector3[] sverts = mesh.vertices;
		for(int i = 0; i < sverts.Length; i++)
		{
			sverts[i] = Vector3.Scale( sverts[i], new Vector3( scale, scale, scale ) );
		}
		
		mesh.vertices = sverts;		
		mesh.RecalculateNormals();
		
		mesh.name += "_" + (int)( scale*100f );
		
		EditorUtility.SetDirty(mesh);
	}
	
	#endregion
	
	#region Color Tweak
	[MenuItem("Assets/MeshTweaker/Color/(1,1,1,0)")] static void ColorMesh_1110(){BatchColor( new Color( 1, 1, 1, 0 ) );}
	[MenuItem("Assets/MeshTweaker/Color/(1,1,1,1)")] static void ColorMesh_1111(){BatchColor( new Color( 1, 1, 1, 1 ) );}
	[MenuItem("Assets/MeshTweaker/Color/(1,0,0,1)")] static void ColorMesh_1001(){BatchColor( new Color( 1, 0, 0, 1 ) );}
	[MenuItem("Assets/MeshTweaker/Color/(0,1,0,1)")] static void ColorMesh_0101(){BatchColor( new Color( 0, 1, 0, 1 ) );}
	[MenuItem("Assets/MeshTweaker/Color/(0,0,1,1)")] static void ColorMesh_0011(){BatchColor( new Color( 0, 0, 1, 1 ) );}
	
	
	static void BatchColor(Color c){		
		foreach(Object o in Selection.objects){
			if(o is Mesh){
				ColorMesh(o as Mesh, c);
			}
		}	
	}
	
	static void ColorMesh(Mesh mesh, Color color){
		mesh.SetColor( color );
		EditorUtility.SetDirty(mesh);
	}
	#endregion
	
	#region Alpha Tweak
	[MenuItem("Assets/MeshTweaker/Alpha/(0.0)")] static void AlphaMesh_00(){BatchColorAlpha( 0.0f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(0.1)")] static void AlphaMesh_01(){BatchColorAlpha( 0.1f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(0.2)")] static void AlphaMesh_02(){BatchColorAlpha( 0.2f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(0.3)")] static void AlphaMesh_03(){BatchColorAlpha( 0.3f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(0.4)")] static void AlphaMesh_04(){BatchColorAlpha( 0.4f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(0.5)")] static void AlphaMesh_05(){BatchColorAlpha( 0.5f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(0.6)")] static void AlphaMesh_06(){BatchColorAlpha( 0.6f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(0.7)")] static void AlphaMesh_07(){BatchColorAlpha( 0.7f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(0.8)")] static void AlphaMesh_08(){BatchColorAlpha( 0.8f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(0.9)")] static void AlphaMesh_09(){BatchColorAlpha( 0.9f );}
	[MenuItem("Assets/MeshTweaker/Alpha/(1.0)")] static void AlphaMesh_10(){BatchColorAlpha( 1.0f );}
	
	
	static void BatchColorAlpha(float a){		
		foreach(Object o in Selection.objects){
			if(o is Mesh){
				ColorAlphaMesh(o as Mesh, a);
			}
		}	
	}
	
	static void ColorAlphaMesh(Mesh mesh, float a){
		mesh.SetAlpha( a );
		EditorUtility.SetDirty(mesh);
	}
	#endregion
	
	[MenuItem("Assets/MeshTweaker/Align/Center")]
	static void BatchCenter(){
		foreach(Object o in Selection.objects){
			if(o is Mesh){
				CenterMesh(o as Mesh);
			}
		}
	}
	
	static void CenterMesh(Mesh mesh){
		Vector3[] verts = mesh.vertices;
		
		Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		
		foreach(Vector3 v in verts){
			if(v.x < min.x) min.x = v.x;
			else if(v.x > max.x) max.x = v.x;
			
			if(v.y < min.y) min.y = v.y;
			else if(v.y > max.y) max.y = v.y;
			
			if(v.z < min.z) min.z = v.z;
			else if(v.z > max.z) max.z = v.z;
		}
		
		Vector3 average = (min + max) * 0.5f;
		
		for(int i = 0; i < verts.Length; i++){
			verts[i] = verts[i] + ( Vector3.zero - average );
		}
		
		mesh.vertices = verts;
		EditorUtility.SetDirty(mesh);
	}
}

public static class MeshHelper
{
	/// <summary>
	/// Sets random color as vertex color for the mesh.
	/// </summary>
	public static Mesh SetRandomColor (this Mesh mesh)
	{
		Color c = new Color (Random.value, Random.value, Random.value, Random.value);
		return mesh.SetColor (c);
	}
	
	/// <summary>
	/// Sets given color as vertex color for the mesh.
	/// </summary>
	public static Mesh SetColor (this Mesh mesh, Color color)
	{
		if (mesh != null) {
			Color[] cols = new Color[ mesh.vertexCount ];
			int i;
			for (i = 0; i < mesh.vertexCount; i++) {
				cols [i] = color;
			}
			mesh.colors = cols;
			
			//Debug.Log( "Mesh: " + mesh.name + ".Color[0] = " + cols[ 0 ].ToString() );
			return mesh;
		}
		return null;
	}
	
	/// <summary>
	/// Moves given mesh by specified amount.
	/// </summary>
	public static Mesh Move (this Mesh mesh, Vector3 amount)
	{
		if (mesh != null) {
			Vector3[] v = mesh.vertices;
			int i;
			for (i = 0; i < v.Length; i++) {
				v [i] += amount;
			}
			mesh.vertices = v;
			
			//Debug.Log( "Mesh: " + mesh.name + ".Color[0] = " + cols[ 0 ].ToString() );
			return mesh;
		}
		return null;
	}
	
	/// <summary>
	/// Sets given alpha as vertex alpha for the mesh.
	/// </summary>
	public static void SetAlpha (this Mesh mesh, float a)
	{
		if (mesh != null) {
			Color[] cols = new Color[ mesh.vertexCount ];
			int i;
			for (i = 0; i < mesh.vertexCount; i++) {
				cols [i].a = a;
			}
			mesh.colors = cols;
		}
	}
}