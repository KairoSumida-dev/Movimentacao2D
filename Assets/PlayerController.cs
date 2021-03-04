using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Jump")]
    public float apiceDoTempoDePulo = .4f;
    public int numeroMaximoDePulos = 1;
	int numeroDePulosRestantes;

	private StatusData statusData;
    float velocidadeMaximaDoPulo;
    float velocidadeMinimaDoPulo;
    public Controller2D controller;
	public bool estaJogando { get; private set; }
	public bool podePular = true;
	public bool podeAndarParaEsquerda = true;
	private Vector2 input;
	private Vector2 velocidadeReal;
	private Animacao animacao;
	bool estaOlhandoParaDireita;
	
	//[HideInInspector] public Vector3 velocity;
	float suavisacaoNoMovimentoX;
	void Awake()
    {
        controller = GetComponent<Controller2D>();
        statusData = GetComponent<StatusData>();
		animacao = GetComponent<Animacao>();
    }
    private void Start()
    {
		if (statusData.i_gravidade == 0)
		{
			statusData.i_gravidade = -(2 * statusData.i_alturaDoPulo) / Mathf.Pow(apiceDoTempoDePulo, 2);
		}
		velocidadeMaximaDoPulo = Mathf.Abs(statusData.i_gravidade) * apiceDoTempoDePulo;
		velocidadeMinimaDoPulo = Mathf.Sqrt(2 * Mathf.Abs(statusData.i_gravidade));
		estaOlhandoParaDireita = transform.localScale.x > 0;
		numeroDePulosRestantes = numeroMaximoDePulos;
		estaJogando = true;
	}
	
	public void MoverEsquerda()
	{
		if (!podeAndarParaEsquerda)
			return;
		if (estaJogando)
		{
			input = new Vector2(-1, 0);
			if (!estaOlhandoParaDireita)
				Flip();
			animacao.Flip(true);
			animacao.MudarAnimacao("Walk");
		}
	}
  
    public void MoverDireita()
	{
		if (estaJogando)
		{
			input = new Vector2(1, 0);
			if (estaOlhandoParaDireita)
				Flip();
			animacao.Flip(false);
			animacao.MudarAnimacao("Walk");
		}
	}
	private void Flip()
	{

		if (estaOlhandoParaDireita)
		{
			estaOlhandoParaDireita = false;
		}
		else
		{
			estaOlhandoParaDireita = true;
		}
	}
	public void Pular()
	{
		if (!estaJogando)
			return;
		if (!podePular)
			return;
		if (controller.collisions.abaixo)//está no chao
		{
			velocidadeReal.y = velocidadeMaximaDoPulo;
			numeroDePulosRestantes = numeroMaximoDePulos;//seta o maximo de pulos que pode dar no ar
		}
        else // já está no ar
        {
			numeroDePulosRestantes--;
			if (numeroDePulosRestantes > 0)
			{
				velocidadeReal.y = velocidadeMinimaDoPulo;

			}
		}
	}
	private void HandleInput()
	{
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			MoverEsquerda();
		else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			MoverDireita();
		//		else if((Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.D)))
		else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
			PararDeMover();

		//if (Input.GetKeyDown(KeyCode.S))
			//Cair();
		else if (Input.GetKeyUp(KeyCode.S))
			PararDeMover();


		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			Pular();
		}

		if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
		{
			JumpOff();
		}

		
	}
	

	public void PararDeMover()
	{
		input = Vector2.zero;
	}
	public void JumpOff()
	{
		if (velocidadeReal.y > velocidadeMinimaDoPulo)
		{
			velocidadeReal.y = velocidadeMinimaDoPulo;
		}
	}
	public void Cair()
	{
		input = new Vector2(0, -1);
	}
    private void Update()
    {
		HandleInput();

		float velocidadeAlvo = input.x * statusData.t_velocidadeDeMovimento;
		velocidadeReal.x = Mathf.SmoothDamp(velocidadeReal.x, velocidadeAlvo, ref suavisacaoNoMovimentoX, (controller.collisions.abaixo) ? 0.2f : statusData.t_tempoDeAceleracao);

		velocidadeReal.y += statusData.t_gravidade * Time.deltaTime;

	}
    private void LateUpdate()
    {
		controller.Mover(velocidadeReal * Time.deltaTime, input);

		if(controller.collisions.abaixo|| controller.collisions.acima)
        {
			velocidadeReal.y = 0;
        }
		if(velocidadeReal.x <=0.1f && velocidadeReal.x >=-0.1f)
        {
			animacao.MudarAnimacao("Idle");
		}
    }
}
