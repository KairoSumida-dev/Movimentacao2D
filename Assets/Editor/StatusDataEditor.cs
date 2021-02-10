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
    public SerializedProperty VelocidadeReal_Prop;
    private void OnEnable()
    {
        VelocidadeDeMovimento_Prop = serializedObject.FindProperty("velocidadeDeMovimento");
        //MaxLife_Prop = serializedObject.FindProperty("maxLife");
        TempoDeAceleracao_Prop = serializedObject.FindProperty("tempoDeAceleracao");
        VelocidadeReal_Prop = serializedObject.FindProperty("velocidadeReal");

}
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(VelocidadeDeMovimento_Prop, new GUIContent("Velocidade de Movimento"));
        // EditorGUILayout.PropertyField(MaxLife_Prop, new GUIContent("Max Life"));
        EditorGUILayout.PropertyField(TempoDeAceleracao_Prop, new GUIContent("Tempo de Aceleração"));
        EditorGUILayout.PropertyField(VelocidadeReal_Prop, new GUIContent("Velocidade Real"));
        serializedObject.ApplyModifiedProperties();
    }
}
