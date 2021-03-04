using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatusData)), CanEditMultipleObjects]
public class StatusDataEditor : Editor
{
    public SerializedProperty VelocidadeDeMovimento_Prop;
    public SerializedProperty MaxLife_Prop;
    public SerializedProperty TempoDeAceleracao_Prop;
    
    public SerializedProperty Gravidade_Prop;
    public SerializedProperty GravidadePredefinida_Prop;
    private void OnEnable()
    {
        VelocidadeDeMovimento_Prop = serializedObject.FindProperty("i_velocidadeDeMovimento");
        //MaxLife_Prop = serializedObject.FindProperty("maxLife");
        TempoDeAceleracao_Prop = serializedObject.FindProperty("i_tempoDeAceleracao");
        
        GravidadePredefinida_Prop = serializedObject.FindProperty("gravidadePreDefinida");
        Gravidade_Prop = serializedObject.FindProperty("i_gravidade");


}
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(VelocidadeDeMovimento_Prop, new GUIContent("Velocidade de Movimento"));
        // EditorGUILayout.PropertyField(MaxLife_Prop, new GUIContent("Max Life"));
        EditorGUILayout.PropertyField(TempoDeAceleracao_Prop, new GUIContent("Tempo de Aceleração"));
      
        EditorGUILayout.PropertyField(GravidadePredefinida_Prop, new GUIContent("Gravidade Pré Definida?"));
        if (!GravidadePredefinida_Prop.boolValue)
        {
            EditorGUILayout.PropertyField(Gravidade_Prop, new GUIContent("Gravidade"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
