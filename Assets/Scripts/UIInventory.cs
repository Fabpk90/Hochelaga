using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIInventory : MonoBehaviour
{
	public static UIInventory Instance;

	[SerializeField] private UIDocument uiMainDocument;
	[SerializeField] private Item exemple;

	private VisualElement steleMaker;
	private Button submitButton, closeButton;
	private Label labelCacao;
	private List<VisualElement> slots = new();
	private List<VisualElement> dropAreas = new();
	private List<(Label, Label)> textsAreas = new();
	private List<VisualElement> draggableObjects = new();

	private bool isDragging = false;
	private VisualElement draggableElement = null;
	private Dictionary<VisualElement, VisualElement> itemContainsBy = new();
	private Dictionary<VisualElement, Item> items = new();
	private Vector2 elementStartPosition;
	private Vector2 newPosition;

	private bool firstTime = true; //gamejam stuff aka ugly ducktaped solution
	
	private void Awake()
	{
		Instance = this;

		VisualElement bar = uiMainDocument.rootVisualElement.Q<VisualElement>("InventoryBar");
		steleMaker = uiMainDocument.rootVisualElement.Q<VisualElement>("SteleMaker");
		submitButton = uiMainDocument.rootVisualElement.Q<Button>("SubmitButton");
		closeButton = uiMainDocument.rootVisualElement.Q<Button>("CloseStele");
		labelCacao = uiMainDocument.rootVisualElement.Q<Label>("CounterCacao");
		slots = bar.Query("BigSlot").ToList();
		dropAreas.Add(steleMaker.Q("DropArea1"));
		dropAreas.Add(steleMaker.Q("DropArea2"));
		dropAreas.Add(steleMaker.Q("DropArea3"));
		textsAreas.Add((steleMaker.Q("InfosHead").Q<Label>("Title"), steleMaker.Q("InfosHead").Q<Label>("Descr")));
		textsAreas.Add((steleMaker.Q("InfosBody").Q<Label>("Title"), steleMaker.Q("InfosBody").Q<Label>("Descr")));
		textsAreas.Add((steleMaker.Q("InfosFoot").Q<Label>("Title"), steleMaker.Q("InfosFoot").Q<Label>("Descr")));
	}

	private void Start()
	{
		TextManager.LoadCSV();
		submitButton.clicked += Victory.Instance.Verify;
		closeButton.clicked += ()=>OpenStele(false);
		OpenStele(false);
		UnlockSubmitVerify();
	}

	private void Update()
	{
		if (!isDragging) return;

		newPosition += Mouse.current.delta.ReadValue() * (Vector2.right + Vector2.down);
		draggableElement.style.left = newPosition.x;
		draggableElement.style.top = newPosition.y;
	}

	public void AddDraggableElement(Item item)
	{
		VisualElement newElement = item.visualAsset.Instantiate();
		newElement.style.height = 140;
		newElement.style.width = 140;
		newElement.RegisterCallback<MouseDownEvent>(evt=>StartCoroutine(MouseDown(evt)));

		foreach (var slot in slots)
		{
			if (!itemContainsBy.ContainsValue(slot))
			{
				itemContainsBy.Add(newElement, slot);
				items.Add(newElement, item);
				slot.Add(newElement);
				break;
			}
		}
	}

	public void OpenStele(bool open = true)
	{
		steleMaker.style.display = open ? DisplayStyle.Flex : DisplayStyle.None;
		foreach (var slot in dropAreas)
		{
			if(itemContainsBy.ContainsValue(slot))
				itemContainsBy.FirstOrDefault(x => x.Value == slot).Key.style.display = open ? DisplayStyle.Flex : DisplayStyle.None;
		}

		if (open == false && !firstTime) // ugly hax
		{
			PlayerController.instance.controls.Move.Enable();
		}

		firstTime = false;
	}

	private WaitUntil waitForButtonUp = new WaitUntil(()=>Input.GetMouseButtonUp(0));
	private IEnumerator MouseDown(MouseDownEvent evt)
	{
		draggableElement = evt.currentTarget as VisualElement;
		evt.StopPropagation();

		elementStartPosition = new(draggableElement.style.left.value.value, draggableElement.style.top.value.value);
		newPosition = elementStartPosition;

		isDragging = true;
		yield return waitForButtonUp;
		isDragging = false;

		if (itemContainsBy[draggableElement] != null && dropAreas.Contains(itemContainsBy[draggableElement]))
			FillDescr(null, itemContainsBy[draggableElement]);

		VisualElement currentDropArea = IsOverDropArea(draggableElement.worldBound.center, dropAreas);
		if (currentDropArea != null && (!itemContainsBy.ContainsValue(currentDropArea) || itemContainsBy[draggableElement] == currentDropArea))
		{
			itemContainsBy[draggableElement] = currentDropArea;
			FillDescr(items[draggableElement], currentDropArea);
			UnlockSubmitVerify();
		}
		else
		{
			currentDropArea = IsOverDropArea(draggableElement.worldBound.center, slots);
			if (currentDropArea != null && (!itemContainsBy.ContainsValue(currentDropArea)))
			{
				currentDropArea.Add(draggableElement);
				draggableElement.style.left = currentDropArea.style.left;
				draggableElement.style.top = currentDropArea.style.top;
				itemContainsBy[draggableElement] = currentDropArea;
			}
			else
			{
				draggableElement.style.left = elementStartPosition.x;
				draggableElement.style.top = elementStartPosition.y;
			}
		}
	}

	private VisualElement IsOverDropArea(Vector2 position, List<VisualElement> areas)
	{
		foreach (var dropaera in areas)
		{
			if (dropaera.worldBound.Contains(position))
				return dropaera;
		}
		return null;
	}

	private void FillDescr(Item item, VisualElement currentDropArea)
	{
		string title = item==null ? "" : TextManager.GetTextByID(item.idName);
		string descr = item == null ? "" : TextManager.GetTextByID(item.idDescr);
		textsAreas[dropAreas.IndexOf(itemContainsBy[draggableElement])].Item1.text = title;
		textsAreas[dropAreas.IndexOf(itemContainsBy[draggableElement])].Item2.text = descr;
	}

	public void UpdateLabelCacao(string text)
		=> labelCacao.text = text;

	private void UnlockSubmitVerify()
	{
		bool unlock = true;
		foreach (var dropArea in dropAreas)
		{
			if (!itemContainsBy.ContainsValue(dropArea))
			{
				unlock = false;
				break;
			}
		}
		submitButton.SetEnabled(unlock);
	}

	public (Item, Item, Item) GetOrder()
	{
		return new()
		{
			Item1 = this.items[itemContainsBy.FirstOrDefault(x => x.Value == dropAreas[0]).Key],
			Item2 = this.items[itemContainsBy.FirstOrDefault(x => x.Value == dropAreas[1]).Key],
			Item3 = this.items[itemContainsBy.FirstOrDefault(x => x.Value == dropAreas[2]).Key]
		};
	}
}
