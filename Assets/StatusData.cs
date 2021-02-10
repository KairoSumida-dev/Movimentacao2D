using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class StatusData : MonoBehaviour
{
    
    [Range(0.1f, 100)] public float velocidadeDeMovimento = 1;//Controla quanto o personagem consegue andar, seria mais ou menos como a força para andar
    [Range(0.1f, 1)] public float tempoDeAceleracao = 0.1f;
    [Range(1, 3)] public float alturaDoPulo = 3;
    public Vector2 velocidadeReal;//Considerando todo o ambiente e forças que podem atrapalhar ou ajudar a velocidade
    //[Range(1, 10000000)] public int maxLife = 0;

    

}
