using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationData : MonoBehaviour
{
    public float FrameRate;
    public string NomeAnimacao;
    public List<Sprite> SpritesDaAnimacao = new List<Sprite>();

    private void Awake()
    {
        GetComponent<Animacao>().SetData(this);//Armazena os dados da classe
    }
}
