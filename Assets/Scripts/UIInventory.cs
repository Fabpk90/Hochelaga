using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIInventory : MonoBehaviour
{
	public static UIInventory Instance;

	public UIDocument uiMainDocument;
	public EventReference sfxStele;
	public EventReference sfxInventaire;

	private VisualElement steleMaker, atelierPanel;
	private Button submitButton, closeButton, closeAtelierButton;
	private Button tutoButton, homeButton;
	private Label labelCacao;
	private Label labelQuest;
	private List<VisualElement> slots = new();
	private List<VisualElement> dropAreas = new();
	private List<(Label, Label)> textsAreas = new();
	private List<VisualElement> draggableObjects = new();
	private VisualElement error1, error2, error3;

	private VisualElement mission2, mission3;
	private Button closemission1, closemission2;


	private bool isDragging = false;
	private VisualElement draggableElement = null;
	private VisualElement dragContener;
	private Dictionary<VisualElement, VisualElement> itemContainsBy = new();
	public Dictionary<VisualElement, Item> items = new();
	private Vector2 elementStartPosition;
	private Vector2 offset = Vector2.zero;
	private bool slot1ShowResults, slot2ShowResults, slot3ShowResults = false;

	public System.EventHandler<Item> OnItemInInventoryAdded;
	public System.EventHandler<Item> OnItemInInventoryRemoved;
	
	private void Awake()
	{
		Instance = this;

		steleMaker = uiMainDocument.rootVisualElement.Q<VisualElement>("SteleMaker");
		atelierPanel = uiMainDocument.rootVisualElement.Q<VisualElement>("Atelier");
		dragContener = uiMainDocument.rootVisualElement.Q<VisualElement>("DragContener");
		submitButton = uiMainDocument.rootVisualElement.Q<Button>("SubmitButton");
		closeButton = uiMainDocument.rootVisualElement.Q<Button>("CloseStele");
		closeAtelierButton = uiMainDocument.rootVisualElement.Q<Button>("CloseAtelier");
		labelCacao = uiMainDocument.rootVisualElement.Q<Label>("CounterCacao");
		error1 = steleMaker.Q<VisualElement>("error1");
		error2 = steleMaker.Q<VisualElement>("error2");
		error3 = steleMaker.Q<VisualElement>("error3");

		mission2 = uiMainDocument.rootVisualElement.Q<VisualElement>("Objectif2");
		mission3 = uiMainDocument.rootVisualElement.Q<VisualElement>("Objectif3");
		closemission1 = mission2.Q<Button>("CloseMission2");
		closemission2 = mission3.Q<Button>("CloseMission3");

		VisualElement bar = uiMainDocument.rootVisualElement.Q<VisualElement>("InventoryBar");
		slots = bar.Query("BigSlot").ToList();
		tutoButton = bar.Q<Button>("TutoButton");
		homeButton = bar.Q<Button>("HomeButton");
		labelQuest = bar.Q<Label>("MissionLabel");

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

		submitButton.SetEnabled(false);
		submitButton.clicked += Victory.Instance.Verify;
		closeButton.clicked += ()=>OpenStele(false);
		closeAtelierButton.clicked += ()=>OpenAtelier(false);
		uiMainDocument.rootVisualElement.RegisterCallback<MouseMoveEvent>(OnMouseMove);
		homeButton.clicked += ClickHome;
		tutoButton.clicked += ClickTuto;
		closemission1.clicked += () => mission2.style.display = DisplayStyle.None;
		closemission2.clicked += () => mission3.style.display = DisplayStyle.None;

		foreach (var item in AtelierInteractable.Instance.recipes)
		{
			Button buttonAtelier = uiMainDocument.rootVisualElement.Q<Button>(item.nameButton);
			buttonAtelier.clicked += ()=> AtelierInteractable.Instance.CreateObject(item.nameButton);
		}

		OpenAtelier(false);
		StartNewMission(0);
	}

	public void StartNewMission(int numMission)
	{
		OpenStele(false);
		UnlockSubmitVerify();

		switch (numMission)
		{
			case 0:
				labelQuest.text = TextManager.GetTextByID("ID23");
				break;
			case 1:
				labelQuest.text = TextManager.GetTextByID("ID24");
				mission2.style.display = DisplayStyle.Flex;
				RemoveDraggable(items[itemContainsBy.FirstOrDefault(x => x.Value == dropAreas[0]).Key]);
				RemoveDraggable(items[itemContainsBy.FirstOrDefault(x => x.Value == dropAreas[1]).Key]);
				RemoveDraggable(items[itemContainsBy.FirstOrDefault(x => x.Value == dropAreas[2]).Key]);
				break;
			case 2:
				labelQuest.text = TextManager.GetTextByID("ID25");
				mission3.style.display = DisplayStyle.Flex;
				RemoveDraggable(items[itemContainsBy.FirstOrDefault(x => x.Value == dropAreas[0]).Key]);
				RemoveDraggable(items[itemContainsBy.FirstOrDefault(x => x.Value == dropAreas[1]).Key]);
				RemoveDraggable(items[itemContainsBy.FirstOrDefault(x => x.Value == dropAreas[2]).Key]);
				break;
		}

		ShowResults((-1,-1,-1));
		foreach (var label in textsAreas)
		{
			label.Item1.text = "";
			label.Item2.text = "";
		}
	}

	public void ClickHome()
	{
		SceneManager.LoadSceneAsync("MainMenu");
	}

	public void ClickTuto()
	{
		Tutorial.Instance.OpenTutorial();
	}

	public void AddDraggableElement(Item item)
	{
		if (items.ContainsValue(item)) return;

		VisualElement newElement = item.visualAsset.Instantiate();
		newElement.RegisterCallback<MouseDownEvent>(evt=>StartCoroutine(MouseDown(evt)));

		foreach (var slot in slots)
		{
			if (!itemContainsBy.ContainsValue(slot))
			{
				itemContainsBy.Add(newElement, slot);
				items.Add(newElement, item);
				slot.Add(newElement);
				OnItemInInventoryAdded?.Invoke(this, item);
				break;
			}
		}
	}

	public void RemoveDraggable(Item item)
	{
		if (!items.ContainsValue(item)) return;

		OnItemInInventoryRemoved?.Invoke(this, item);
		VisualElement elem = items.FirstOrDefault(x => x.Value == item).Key;
		itemContainsBy.Remove(elem);
		items.Remove(elem);
		elem.RemoveFromHierarchy();
	}

	public void OpenStele(bool open = true)
	{
		steleMaker.style.display = open ? DisplayStyle.Flex : DisplayStyle.None;
		foreach (var slot in dropAreas)
		{
			if(itemContainsBy.ContainsValue(slot))
				itemContainsBy.FirstOrDefault(x => x.Value == slot).Key.style.display = open ? DisplayStyle.Flex : DisplayStyle.None;
		}

		if (open == false)
		{
			PlayerController.instance?.controls.Move.Enable();
		}
	}

	public void OpenAtelier(bool open = true)
	{
		atelierPanel.style.display = open ? DisplayStyle.Flex : DisplayStyle.None;

		if (open == false) 
		{
			PlayerController.instance?.controls.Move.Enable();
		}
	}

	private WaitUntil waitForButtonUp = new WaitUntil(()=>Input.GetMouseButtonUp(0));
	private IEnumerator MouseDown(MouseDownEvent evt)
	{
		draggableElement = evt.currentTarget as VisualElement;

		elementStartPosition = new(draggableElement.style.left.value.value, draggableElement.style.top.value.value);

		offset = evt.mousePosition - draggableElement.worldBound.position;
		draggableElement.style.left = evt.mousePosition.x - offset.x;
		draggableElement.style.top = evt.mousePosition.y - offset.y;
		evt.StopPropagation();

		dragContener.Add(draggableElement);

		if (dropAreas.Contains(itemContainsBy[draggableElement]))
		{
			ApplyResult(itemContainsBy[draggableElement], -1);
			submitButton.SetEnabled(false);
		}

		isDragging = true;
		yield return waitForButtonUp;
		isDragging = false;

		if (itemContainsBy[draggableElement] != null && dropAreas.Contains(itemContainsBy[draggableElement]))
			FillDescr(null, itemContainsBy[draggableElement]);

		VisualElement currentDropArea = IsOverDropArea(draggableElement.Q("Item").worldBound.center, dropAreas);
		if (currentDropArea != null && (!itemContainsBy.ContainsValue(currentDropArea) || itemContainsBy[draggableElement] == currentDropArea))
		{
			itemContainsBy[draggableElement] = currentDropArea;
			FillDescr(items[draggableElement], currentDropArea);
			UnlockSubmitVerify();
			if (!sfxStele.IsNull)
			{
				FMODUnity.RuntimeManager.PlayOneShot(sfxStele);
			}

		}
		else
		{
			currentDropArea = IsOverDropArea(draggableElement.Q("Item").worldBound.center, slots);
			if (currentDropArea != null && (!itemContainsBy.ContainsValue(currentDropArea)))
			{
				draggableElement.style.left = currentDropArea.style.left;
				draggableElement.style.top = currentDropArea.style.top;
				itemContainsBy[draggableElement] = currentDropArea;
				if (!sfxInventaire.IsNull)
				{
					FMODUnity.RuntimeManager.PlayOneShot(sfxInventaire);
				}
			}
			else
			{
				draggableElement.style.left = elementStartPosition.x;
				draggableElement.style.top = elementStartPosition.y;
			}

			if (slots.Contains(itemContainsBy[draggableElement]))
				itemContainsBy[draggableElement].Add(draggableElement);
		}
	}

	private void OnMouseMove(MouseMoveEvent evt){
		if (!isDragging) return;

		draggableElement.style.left = evt.mousePosition.x - offset.x;
		draggableElement.style.top = evt.mousePosition.y - offset.y;
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

	public void AddError(int error)
	{
		switch (error)
		{
			case 1:
				error1.style.display = DisplayStyle.Flex;
				break;
			case 2:
				error2.style.display = DisplayStyle.Flex;
				break;
			case 3:
				error3.style.display = DisplayStyle.Flex;
				break;
		}
	}

	public void ShowResults((int, int, int) results)
	{
		ApplyResult(dropAreas[0], results.Item1);
		ApplyResult(dropAreas[1], results.Item2);
		ApplyResult(dropAreas[2], results.Item3);
	}

	private void ApplyResult(VisualElement slot, int result)
	{
		Color color = Color.clear;
		switch (result)
		{
			case 0:
				color = Color.red;
				break;

			case 1:
				color = Color.yellow;
				break;

			case 2:
				//all good, stay transparent
				break;
		}

		slot.style.borderLeftColor = color;
		slot.style.borderRightColor = color;
		slot.style.borderBottomColor = color;
		slot.style.borderTopColor = color;
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

	public bool HasObjectInInventory(Item item)
	{
		if (!items.ContainsValue(item))
			return false;

		VisualElement elem = items.FirstOrDefault(x => x.Value == item).Key;

		if(slots.Contains(itemContainsBy[elem])) 
			return true;

		return false;
	}

	public List<Item> GetInventory()
	{
		List<Item> inventory = new();
		foreach (var item in items)
		{
			if (slots.Contains(itemContainsBy[item.Key]))
				inventory.Add(item.Value);
		}
		return inventory;
	}
}
