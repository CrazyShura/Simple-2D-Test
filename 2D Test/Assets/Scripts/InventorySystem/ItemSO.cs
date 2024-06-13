using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item")]
public class ItemSO : ScriptableObject
{
	[SerializeField]
	int ID = -1;
	[SerializeField]
	string itemName = "name";
	[SerializeField]
	GameObject grfx;
	[SerializeField]
	float interactionRange = .5f;

	public int ID1 { get => ID; }
	public string ItemName { get => itemName; }
	public GameObject Grfx { get => grfx; }
	public float InteractionRange { get => interactionRange; }
}
