using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
	#region Fields
	static PlayerControler instance;

	DefaultControls inputActions;
	Rigidbody2D rgbd;
	Inventory backpack;

	[SerializeField, Min(.1f)]
	float speed;
	[SerializeField]
	AnimationCurve dodgeCurve;
	[SerializeField, Min(.1f)]
	float dodgeDuration = 1f;
	[SerializeField, Min(.5f)]
	float dodgeDistance = 2f;

	Vector2 moveDirection;
	bool dodging = false;
	Vector2 dodgeStart, dodgeTarget;
	float dodgeTimer;
	SpriteRenderer grfx;

	List<Item> itemsInInteractionRange = new List<Item>();
	#endregion

	#region Properties
	public static PlayerControler Instance { get => instance; }

	public bool Dodging
	{
		get => dodging;
		protected set
		{
			dodging = value;
			if (dodging)
			{
				grfx.color = Color.red;
			}
			else
			{
				grfx.color = Color.gray;
			}

		}
	}

	public Inventory Backpack { get => backpack;}
	#endregion

	#region Methods
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Trying to have more then one player... why?");
			Destroy(gameObject);
			return;
		}
		instance = this;

		inputActions = new DefaultControls();
		inputActions.Enable();
		inputActions.PlayerControls.Movement.performed += cntx => ReadMovement(cntx.ReadValue<Vector2>());
		inputActions.PlayerControls.Movement.canceled += cntx => ReadMovement(cntx.ReadValue<Vector2>());
		inputActions.PlayerControls.Dodge.performed += cntx => Dodge();
		inputActions.PlayerControls.PickUp.performed += cntx => PickUp();
		inputActions.PlayerControls.Inventory.performed += cntx => HUDController.Instance.TogglePlayerInventory();

		rgbd = GetComponent<Rigidbody2D>();
		backpack = GetComponent<Inventory>();
		backpack.Initialize(20);

		grfx = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	void ReadMovement(Vector2 direction)
	{
		if (direction.magnitude != 0)
		{
			moveDirection = direction;
		}
		else
		{
			moveDirection = Vector2.zero;
			rgbd.velocity = Vector2.zero;
		}
	}

	void Dodge()
	{
		if (moveDirection.magnitude == 0 || dodging)
		{
			return;
		}
		Dodging = true;
		rgbd.velocity = Vector2.zero;
		dodgeTimer = dodgeDuration;
		dodgeStart = transform.position;
		dodgeTarget = dodgeStart + moveDirection * dodgeDistance;
	}

	void PickUp()
	{
		if (itemsInInteractionRange.Count == 0)
		{
			Debug.Log("No Items to pick up");
			return;
		}
		if (!backpack.HasSpace)
		{
			Debug.Log("Player inventory is out of space");
			return;
		}
		Item[] _itemsToPcikUp = itemsInInteractionRange.ToArray();
		for (int _i = 0; _i < _itemsToPcikUp.Length; _i++)
		{
			if (backpack.AddItemToInventory(_itemsToPcikUp[_i].ItemSO) < 0)
			{
				Debug.Log("Inventory is out of space");
				break;
			}
			itemsInInteractionRange.Remove(_itemsToPcikUp[_i]);
			Destroy(_itemsToPcikUp[_i].gameObject);
		}
	}

	private void Update()
	{
		if (Dodging)
		{
			dodgeTimer -= Time.deltaTime;
			if (dodgeTimer > 0)
			{
				float _travelPhase = dodgeCurve.Evaluate(1 - dodgeTimer / dodgeDuration);
				transform.position = Vector2.Lerp(dodgeStart, dodgeTarget, _travelPhase);
			}
			else
			{
				Dodging = false;
				transform.position = dodgeTarget;
			}
		}
	}

	private void FixedUpdate()
	{
		if (moveDirection.magnitude != 0 && !Dodging)
		{
			rgbd.velocity = moveDirection * speed;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 6)
		{
			itemsInInteractionRange.Add(collision.GetComponent<Item>());
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 6)
		{
			itemsInInteractionRange.Remove(collision.GetComponent<Item>());
		}
	}
	#endregion
}
