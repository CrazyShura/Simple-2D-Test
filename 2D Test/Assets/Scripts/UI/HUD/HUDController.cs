using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
	#region Fields
	static HUDController instance;

	[SerializeField]
	Transform inventoryMenue;
	[SerializeField]
	Transform inventoryCellParent;
	[SerializeField]
	Transform inventoryCell;
	#endregion

	#region Properties
	public static HUDController Instance { get => instance; }
	#endregion

	#region Methods
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("Trying to have more then one HUD controllers. Dont do that");
			Destroy(gameObject);
			return;
		}
		instance = this;
	}

	public void TogglePlayerInventory()
	{
		if (inventoryMenue.gameObject.activeSelf)
		{
			inventoryMenue.gameObject.SetActive(false);
		}
		else
		{
			while (inventoryCellParent.childCount > 0)
			{
				DestroyImmediate(inventoryCellParent.GetChild(0).gameObject);
			}
			foreach (ItemSO _item in PlayerControler.Instance.Backpack.Items)
			{
				if(_item != null)
				{
					Transform _cell = Instantiate(inventoryCell, inventoryCellParent);
					_cell.GetChild(0).GetComponent<TMP_Text>().text = _item.ItemName;
				}
			}
			inventoryMenue.gameObject.SetActive(true);
		}
	}
	#endregion
}
