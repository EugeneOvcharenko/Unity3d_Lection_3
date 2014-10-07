using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(CObjectPool))]
public class CObjectPoolDrawer : PropertyDrawer
{
	const int prefabH	= 48;
	const int boolW		= 16;
	const int floatsW	= 64;
	const int floatsH	= 48;
	
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{		
		SerializedProperty prefab		= property.FindPropertyRelative( "prefab" );
		SerializedProperty forceY		= property.FindPropertyRelative( "forceY" );
		SerializedProperty forveYMin	= property.FindPropertyRelative( "forveYMin" );
		SerializedProperty forveYMax	= property.FindPropertyRelative( "forveYMax" );
				
		EditorGUI.BeginProperty( position, label, property );
		
		position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), label );
		
		float w = position.width / 2.0f - 1;
		float w2 = ( w - boolW ) / 2.0f - 1;
		float h = position.height;
		float x = position.x;
		
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		prefab.objectReferenceValue	= EditorGUI.ObjectField(	new Rect( x, position.y, w,		h ), prefab.objectReferenceValue, typeof(GameObject), false ); x += w + 1;
		if ( forceY.boolValue )
		{
			forceY.boolValue		= EditorGUI.Toggle(			new Rect( x, position.y, boolW,	h ), forceY.boolValue );		x += boolW;//	w2 += boolW;
			forveYMin.floatValue	= EditorGUI.FloatField(		new Rect( x, position.y, w2,	h ), forveYMin.floatValue );	x += w2;	//w += floatsW;
			forveYMax.floatValue	= EditorGUI.FloatField(		new Rect( x, position.y, w2,	h ), forveYMax.floatValue );
		}
		else
		{
			forceY.boolValue		= EditorGUI.Toggle(			new Rect( x, position.y, boolW,	h ), forceY.boolValue ); x += boolW; w -= boolW;
									  EditorGUI.LabelField(		new Rect( x, position.y, w,		h ), "use force Y" );
		}
		
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}
