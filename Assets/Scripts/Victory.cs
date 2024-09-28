using UnityEngine;

public class Victory : MonoBehaviour
{
	[Header("Politique")]
	[SerializeField] private string idItemHeadPolitique = "ID1";
	[SerializeField] private string idItemBodyPolitique = "ID3";
	[SerializeField] private string idItemFeetPolitique = "ID5";
	[Header("Militaire")]
	[SerializeField] private string idItemHeadMilitaire = "ID7";
	[SerializeField] private string idItemBodyMilitaire = "ID9";
	[SerializeField] private string idItemFeetMilitaire = "ID11";
	[Header("Religieux")]
	[SerializeField] private string idItemHeadReligieux = "ID13";
	[SerializeField] private string idItemBodyReligieux = "ID15";
	[SerializeField] private string idItemFeetReligieux = "ID17";

	public static Victory Instance;

	private void Awake()
	{
		Instance = this;
	}

	public void Verify()
	{
		(Item, Item, Item) items = UIInventory.Instance.GetOrder();
		int result = AreAllVariablesInTuple(items, idItemHeadPolitique, idItemBodyPolitique, idItemFeetPolitique);
		switch (result) {
			case 1: 
				Debug.Log("GAGNE");
				break;

			case 2:
				Debug.Log("Wrong order :(");
				break;

			case 0:
				Debug.Log("PERDU");
				break;
		}
	}
	public int AreAllVariablesInTuple((Item, Item, Item) tuple, string var1, string var2, string var3)
	{
		if (tuple.Item1.idName == var1 && tuple.Item2.idName == var2 && tuple.Item3.idName == var3)
		{
			return 1;
		}

		bool allPresent = (tuple.Item1.idName == var1 || tuple.Item1.idName == var2 || tuple.Item1.idName == var3) &&
						  (tuple.Item2.idName == var1 || tuple.Item2.idName == var2 || tuple.Item2.idName == var3) &&
						  (tuple.Item3.idName == var1 || tuple.Item3.idName == var2 || tuple.Item3.idName == var3);
		if (allPresent)
			return 2;

		return 0;
	}
}
