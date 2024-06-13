using UnityEngine;

public class Item : MonoBehaviour
{
	#region Fields
	[SerializeField]
	ItemSO itemSO;
	CircleCollider2D interatcionCollider;

	#endregion

	#region Properties
	public ItemSO ItemSO { get => itemSO; } 
	#endregion

	#region Methods
	void Initialize()
	{
		if (TryGetComponent<CircleCollider2D>(out CircleCollider2D _collider))
		{
			interatcionCollider = _collider;
		}
		else
		{
			interatcionCollider = gameObject.AddComponent<CircleCollider2D>();
		}
		interatcionCollider.isTrigger = true;
		interatcionCollider.radius = itemSO.InteractionRange;
		gameObject.layer = 6;
		Instantiate(itemSO.Grfx, transform);
	}

	private void Awake()
	{
		Initialize();
	}
	#endregion
}
