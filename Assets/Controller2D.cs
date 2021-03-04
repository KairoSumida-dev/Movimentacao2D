using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Eixo { x, y, z }
public class Controller2D : RaycastBase
{
	float anguloMaximoDeEscalada = 80;
	float anguloMaximoDeDescida = 80;
	float tamanhoDoRaycastDeVerificacao = 0.5f;//comprimento do raycast

	[SerializeField] private float offSetDeInicioDoRayDeVerificacao = 0.5f;// Imagine que o ponto de inicio é no centro do personagem, com isso movemos para frente ou para tras o ponto de inicio do raycast
	public CollisionInfo collisions;
	[HideInInspector] public Vector2 direcaoDeComando; // indica a direção que o personagem quer se mover
	public bool TemTratamentoDeFisica = true;
	[SerializeField] private GameObject malhaDoPersonagem; // GameObject que tem toda a parte visual ou de animação do personagem. Essa sofrerá rotação e o personagem deve acompanhar
	[SerializeField] private Eixo eixoDeRotacao; //Geralmente deve ser no eixo Y porem se o Y estiver rotacionando de forma errada, pode-se mudar para X ou Z
	[SerializeField] private float pesoDaInclinacao; // variavel que controla o quao dificil é escalar uma rampa ou descer ela(reduz a velocidade ou aumenta)
	public bool acompanhaAnguloRampa = false;

	//private Vector3 velocity; //Velocidade atual do personagem
	public Vector2 direcaoAtual { get; set; }//Direção que ele está indo
	private bool ok = false;

	public override void Start()
	{
		base.Start();

		collisions.faceDir = 1;//Aponta o personagem para a direita
		collisions.abaixo = false;
		StartCoroutine(IWait());
	}

	public void Mover(Vector3 velocidade, bool estaNaPlataforma)
	{
		Mover(velocidade, Vector2.zero, estaNaPlataforma);
	}
	IEnumerator IWait()
    {
		yield return new WaitWhile(() => collisions.abaixo);
		ok = true;
    }
	public void Mover(Vector3 velocidade, Vector2 direcao, bool estaNaPlataforma = false)
	{
		if (!ok)
			return;

		

			float valf = 0;
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxcollider.bounds.center.x, boxcollider.bounds.min.y), Vector2.down, 1, collisionMask);
			Vector3 temp = Vector3.Cross(hit.normal, Vector3.down);
			Vector3 groundSlopeDir = Vector3.Cross(temp, hit.normal);
			float valor;
			valor = Vector2.Angle(hit.normal, Vector2.up);

			if (malhaDoPersonagem != null)//Verifica se o personagem vai rotacionar ou não
			{

				if (groundSlopeDir.x < 0)//Rampa a nordeste
				{
					valor *= -1;
				}

				if (collisions.abaixo)
				{
					if (valor > 10)
					{
						if (pesoDaInclinacao < 0)
						{
							pesoDaInclinacao *= 1;
						}
					}
					else if ((valor < -10))
					{
						if (pesoDaInclinacao > 0)
						{
							pesoDaInclinacao *= -1;
						}
					}
					else
						pesoDaInclinacao = (int)valor;
				}

				//go.transform.localPosition = new Vector3(initialPoses.x, initialPoses.y- hit.distance, initialPoses.z);

				malhaDoPersonagem.transform.eulerAngles = new Vector3(malhaDoPersonagem.transform.eulerAngles.x, malhaDoPersonagem.transform.eulerAngles.y, -valor);

			}

			UpdateRaycastOrigins();
			collisions.Reset();
			collisions.velocidadeAntiga = velocidade;
			direcaoDeComando = direcao;

			if (velocidade.x != 0)
			{
				collisions.faceDir = (int)Mathf.Sign(velocidade.x);
			}


			if (velocidade.y < 0)
			{
				DescendSlope(ref velocidade);
			}

			if (TemTratamentoDeFisica)
			{
				HorizontalCollisions(ref velocidade);
				if (velocidade.y != 0)
				{
					VerticalCollisions(ref velocidade);
				}
			}

			CheckGroundedAhead();



			if (collisions.caindoDaPlataforma)
			{
				valf = valor;
				if (direcao.x > 0) //Andando>>
				{
					if (valf > 0) //Subida é >>
					{//Fica dificil
						valf = pesoDaInclinacao / 100 * valf / 100f; //vai dar um valor negativo
						valf = velocidade.x * valf;
						Debug.Log("valf" + valf + "       vel: " + velocidade.x);

					}
					else
					{//Fica facil
						valf = pesoDaInclinacao / 100 * valf * -1f / 100f;//vai dar um valor positivo
						valf = velocidade.x * valf;
						Debug.Log("valf" + valf + "       vel: " + velocidade.x);

					}
				}
				else if (direcao.x < 0)//ta andando <<
				{
					if (valf < 0)// Subida é <<
					{//Fica dificil <
						valf = pesoDaInclinacao / 100 * -1 * valf / 100f; //vai dar um valor positivo pq a velocidade é negativa
						valf = velocidade.x * valf;
						Debug.Log("valf" + valf + "       vel: " + velocidade.x);
					}
					else
					{
						valf = pesoDaInclinacao / 100 * valf / 100f; //vai dar um valor negativo pq a velocidade é positiva
						valf = velocidade.x * valf;
						Debug.Log("valf" + valf + "       vel: " + velocidade.x);
					}
				}
			}

			//ESTOU TENTANDO ZERAR A VELOCIDADE P QUANDO ELE NAO ESTA DANDO O COMANDO PARA O LADO


			velocidade = new Vector2(velocidade.x + valf, velocidade.y);
			Debug.Log(velocidade.x);
			transform.Translate(velocidade, Space.World);
	
		if (estaNaPlataforma)
		{
			collisions.abaixo = true;
		}
	}

	void HorizontalCollisions(ref Vector3 velocity)
	{
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;

		if (Mathf.Abs(velocity.x) < skinWidth)
		{
			rayLength = 5 * skinWidth;
		}

		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (hit)
			{

				if (hit.distance == 0)
				{
					continue;
				}

				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				if (i == 0 && slopeAngle <= anguloMaximoDeEscalada)
				{
					
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.anguloRampaAntiga)
					{
						distanceToSlopeStart = hit.distance - skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}

				if (slopeAngle > anguloMaximoDeEscalada)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

			

					collisions.esquerda = directionX == -1;
					collisions.direita = directionX == 1;
				}
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++)
		{

			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.blue);

			if (hit)
			{
				if (hit.collider.tag == "Through")
				{
					if (directionY == 1 || hit.distance == 0)
					{
						continue;
					}
					if (collisions.caindoDaPlataforma)
					{
						continue;
					}
					if (direcaoDeComando.y == -1)
					{
						collisions.caindoDaPlataforma = true;
						Invoke("ResetFallingThroughPlatform", .5f);
						continue;
					}
				}

				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

			

				collisions.abaixo = directionY == -1;
				collisions.acima = directionY == 1;
			}
		}

		
	}

	void CheckGroundedAhead()
	{
		float directionX = collisions.faceDir;

		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
		rayOrigin = new Vector2(transform.position.x + (offSetDeInicioDoRayDeVerificacao * directionX), rayOrigin.y);

		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, tamanhoDoRaycastDeVerificacao, collisionMask);

		if (hit)
			collisions.temChaoAFrente = true;

		Debug.DrawRay(rayOrigin, (Vector2.down * 2) * tamanhoDoRaycastDeVerificacao, Color.green);


	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle)
	{
		float moveDistance = Mathf.Abs(velocity.x);
		float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY)
		{
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
			collisions.abaixo = true;
			collisions.anguloRampa = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign(velocity.x);
		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

		if (hit)
		{
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= anguloMaximoDeDescida)
			{
				if (Mathf.Sign(hit.normal.x) == directionX)
				{
					if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
					{
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
						velocity.y -= descendVelocityY;

						collisions.anguloRampa = slopeAngle;
						collisions.abaixo = true;
					}
				}
			}
		}
	}


	public struct CollisionInfo
	{
		public bool acima,abaixo, esquerda, direita;

		public bool temChaoAFrente;

		
		public float anguloRampa, anguloRampaAntiga;
		public Vector3 velocidadeAntiga;
		public int faceDir;
		public bool caindoDaPlataforma;

		public void Reset()
		{
			acima = abaixo = false;
			esquerda = direita = false;
			temChaoAFrente = false;

			anguloRampa = 0;
		}
	}
}
