using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animacao : MonoBehaviour
{
    private AnimationData animacaoExecutando;
    private List<AnimationData> possiveisAnimacoes = new List<AnimationData>();
    float proximoTempoFrame = 0;
    [SerializeField] SpriteRenderer render;
    int frame = 0;
    float tempoTemp;
    public bool ok { get; private set; } = false;
    public void Flip(bool valor)
    {
        render.flipX = valor;
    }
    IEnumerator IWait()//Esperar as animações serem adicionadas
    {
        yield return new WaitWhile(() => possiveisAnimacoes == null);
        ok = true;
    }
    public void SetData(AnimationData animacoes)//A classe G_AnimationGroupData vai se encarregar de passar suas informações
    {
        possiveisAnimacoes.Add(animacoes);
    }
    void Start()
    {
        if (render == null)//caso o spriterender n seja especificado, ele irá buscar no proprio GameObject
            render = GetComponent<SpriteRenderer>();
        StartCoroutine(IWait());//Starta a verificação se a classe está funcionando corretamente
    }

     public void MudarAnimacao(string nomeAnimacao)//Muda a animação conforme a necessidade
    {
        if (!ok)//A classe não está pronta
            return;

        if (possiveisAnimacoes.Exists(x => x.NomeAnimacao == nomeAnimacao))//Verifica se a animação existe
        {

            if (animacaoExecutando != null &&  nomeAnimacao == animacaoExecutando.NomeAnimacao)//Tem uma animação sendo executada e o nome da animação é identico ao que foi pedido
            {
                //A animacao é a mesma
            }
            else
            {
                animacaoExecutando = possiveisAnimacoes.Find(x => x.NomeAnimacao == nomeAnimacao);//Troca para a nova animação
                proximoTempoFrame = 1f / animacaoExecutando.FrameRate;//troca o tempo de execução 
                frame = 0;//Reinicia o ciclo
                tempoTemp = Time.time;//Tempo atual
            }
        }
    }
    void Update()
    {
        if (!ok)
            return;
        
        if (animacaoExecutando == null)
            return;

       
        //Se o tempo for o tempo do proximo frame
        if (Time.time - tempoTemp - proximoTempoFrame > (1f / animacaoExecutando.FrameRate))
        {
            render.sprite = animacaoExecutando.SpritesDaAnimacao[frame];//Executa o proximo frame
            frame = (frame + 1) % animacaoExecutando.SpritesDaAnimacao.Count;//Seta o proximo frame
            //calculate the time of the next frame.
            proximoTempoFrame += 1f / animacaoExecutando.FrameRate;//Seta o proximo tempo de frame
        }
       
        
    }
}
