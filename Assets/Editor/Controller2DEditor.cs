using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Controller2D)), CanEditMultipleObjects]
public class Controller2DEditor : Editor
{
    public SerializedProperty CollisionMask_Prop;
    public SerializedProperty AcompanhaAnguloRampa_Prop;
    public SerializedProperty MalhaDoPersonagem_Prop;
    public SerializedProperty EixoDaRotacao_Prop;
    public SerializedProperty PesoDaInclinacao_Prop;
    private void OnEnable()
    {
        CollisionMask_Prop = serializedObject.FindProperty("collisionMask");
        AcompanhaAnguloRampa_Prop = serializedObject.FindProperty("acompanhaAnguloRampa");
        //MaxLife_Prop = serializedObject.FindProperty("maxLife");
        MalhaDoPersonagem_Prop = serializedObject.FindProperty("malhaDoPersonagem");
        EixoDaRotacao_Prop = serializedObject.FindProperty("eixoDeRotacao");
        PesoDaInclinacao_Prop = serializedObject.FindProperty("pesoDaInclinacao");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(CollisionMask_Prop, new GUIContent("Mascara de colisão"));
        EditorGUILayout.PropertyField(AcompanhaAnguloRampa_Prop, new GUIContent("Acompanha Angulo da rampa?"));
        if (AcompanhaAnguloRampa_Prop.boolValue)
        {
            // EditorGUILayout.PropertyField(MaxLife_Prop, new GUIContent("Max Life"));
            EditorGUILayout.PropertyField(MalhaDoPersonagem_Prop, new GUIContent("Malha do Personagem"));
            EditorGUILayout.PropertyField(EixoDaRotacao_Prop, new GUIContent("Eixo da Rotação"));
            EditorGUILayout.PropertyField(PesoDaInclinacao_Prop, new GUIContent("Peso da inclinação"));
        }
        serializedObject.ApplyModifiedProperties();
    }

}
