using UnityEngine;
using UnityEngine.SceneManagement;

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

	private int currentMission = 0;
	private int erreurs = 0;

	public static Victory Instance;

	private void Awake()
	{
		Instance = this;
	}

	public void Verify()
	{
		(Item, Item, Item) items = UIInventory.Instance.GetOrder();
		(int, int, int) result = (-1, -1, -1);
		switch (currentMission)
		{
			case 0:
				result = AreAllVariablesInTuple(items, idItemHeadPolitique, idItemBodyPolitique, idItemFeetPolitique);
				break;
			case 1:
				result = AreAllVariablesInTuple(items, idItemHeadMilitaire, idItemBodyMilitaire, idItemFeetMilitaire);
				break;
			case 2:
				result = AreAllVariablesInTuple(items, idItemHeadReligieux, idItemBodyReligieux, idItemFeetReligieux);
				break;
		}

		if (result == (2, 2, 2))
		{
			currentMission++;
			erreurs = 0;
			UIInventory.Instance.StartNewMission(currentMission);
		}
		else
		{
			erreurs++;
			UIInventory.Instance.AddError(erreurs);
			UIInventory.Instance.ShowResults(result);
		}

		if (erreurs >= 3)
		{
			SceneManager.LoadSceneAsync(3);
		}
		if (currentMission == 3)
		{
			SceneManager.LoadSceneAsync(2);
		}
	}
	public (int, int, int) AreAllVariablesInTuple((Item, Item, Item) tuple, string var1, string var2, string var3)
	{
		//0 = wrong, 1 = not the right place  but this item should be on the stele, 2=right item
		(int, int, int) results = new(0,0,0);

		if (tuple.Item1.idName == var1)
		{
			results.Item1 = 2;
		}
		else if (tuple.Item1.idName == var2 || tuple.Item1.idName == var3)
		{
			results.Item1 = 1;
		}

		if (tuple.Item2.idName == var2)
		{
			results.Item2 = 2;
		}
		else if (tuple.Item2.idName == var1 || tuple.Item2.idName == var3)
		{
			results.Item2 = 1;
		}

		if (tuple.Item3.idName == var3)
		{
			results.Item3 = 2;
		}
		else if (tuple.Item3.idName == var1 || tuple.Item3.idName == var2)
		{
			results.Item3 = 1;
		}

		return results;
	}
}
