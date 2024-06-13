using UnityEngine;

public class Inventory : MonoBehaviour
{
	#region Fields
	ItemSO[] items;

	bool initilized = false;

	#endregion

	#region Properties
	public bool Initilized { get => initilized; }
	public bool HasSpace
	{
		get
		{
			if (!initilized)
			{
				Debug.LogError("Trying to check inventory before it was initilized");
				return false;
			}
			for (int i = 0; i < items.Length; i++)
			{
				if (items[i] == null)
				{
					return true;
				}
			}
			return false;
		}
	}

	public ItemSO[] Items { get => items;}
	#endregion

	#region Methods
	public void Initialize(int length)
	{
		if (initilized)
		{
			Debug.LogError("Trying to initilize inventory more then once. No change has been made");
		}
		items = new ItemSO[length];
		initilized = true;
	}

	/// <summary>
	/// Adds item to the inventory.
	/// </summary>
	/// <param name="item">Item to add.</param>
	/// <returns>Returns index of where item was put, if there isnt enough space will return -1.</returns>
	public int AddItemToInventory(ItemSO item)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i] == null)
			{
				items[i] = item;
				return i;
			}
		}
		return -1;
	}
	#endregion
}