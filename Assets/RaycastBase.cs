using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBase : MonoBehaviour
{
	public LayerMask collisionMask;

	[SerializeField] protected const float skinWidth = .015f;
	[SerializeField] protected int horizontalRayCount = 4;//quantidade de rays q ficam a frente do personagem fazendo verificações
	[SerializeField] protected int verticalRayCount = 4;//quantidade de rays q ficam a baixo do personagem fazendo verificações

	[HideInInspector] protected float horizontalRaySpacing;//distancia entre cada ray
	[HideInInspector] protected float verticalRaySpacing;//distancia entre cada ray

	[HideInInspector] public BoxCollider2D boxcollider;//Collisor do personagem
	protected RaycastOrigins raycastOrigins;

	public virtual void Awake()
	{
		boxcollider = GetComponent<BoxCollider2D>();
	}

	public virtual void Start()
	{
		CalculateRaySpacing();
	}

	public void UpdateRaycastOrigins()
	{
		Bounds bounds = boxcollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	public void CalculateRaySpacing()
	{
		Bounds bounds = boxcollider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	public struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
