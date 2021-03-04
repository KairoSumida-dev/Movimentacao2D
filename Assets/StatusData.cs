using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class StatusData : MonoBehaviour
{
    
    [Range(0.1f, 100)] public float i_velocidadeDeMovimento = 1;//Controla quanto o personagem consegue andar, seria mais ou menos como a força para andar
    [Range(0.1f, 1)] public float i_tempoDeAceleracao = 0.1f;
    [Range(1, 3)] public float i_alturaDoPulo = 3;
    
    public float i_gravidade;
    public bool gravidadePreDefinida;
    //[Range(1, 10000000)] public int maxLife = 0;

    public float t_velocidadeDeMovimento;//Considerando todo o ambiente e forças que podem atrapalhar ou ajudar a velocidade
    public float t_gravidade;
    public float t_alturaDoPulo;
    public float t_tempoDeAceleracao;

    private void Start()
    {
        t_velocidadeDeMovimento = i_velocidadeDeMovimento;
        t_gravidade = i_gravidade;
        t_alturaDoPulo = i_alturaDoPulo;
        t_tempoDeAceleracao = i_tempoDeAceleracao;
    }
}
