using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIInventory : MonoBehaviour
{
	public static UIInventory Instance;

	[SerializeField] private UIDocument uiMainDocument;
	[SerializeField] private Item exemple;

	private VisualElement steleMaker;
	private List<VisualElement> slots = new();
	private List<VisualElement> dropAreas = new();
	private List<VisualElement> draggableObjects = new();

	private bool isDragging = false;
	private VisualElement draggableElement = null;
	private Dictionary<VisualElement, VisualElement> itemContainsBy = new();
	private Dictionary<VisualElement, Item> items = new();
	private Vector2 elementStartPosition;

	private void Awake()
	{
		Instance = this;

		VisualElement bar = uiMainDocument.rootVisualElement.Q<VisualElement>("InventoryBar");
		steleMaker = uiMainDocument.rootVisualElement.Q<VisualElement>("SteleMaker");
		slots = bar.Query("BigSlot").ToList();
		dropAreas = steleMaker.Query("DropArea").ToList();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E)){
			AddDraggableElement(exemple.visualAsset, exemple);
		}
	}

	public void AddDraggableElement(VisualTreeAsset asset, Item item)
	{
		VisualElement newElement = asset.Instantiate();
		newElement.RegisterCallback<MouseDownEvent>(MouseDown);
		newElement.RegisterCallback<MouseMoveEvent>(MouseMove);
		newElement.RegisterCallback<MouseUpEvent>(MouseUp);

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

	private void MouseDown(MouseDownEvent evt)
	{
		Debug.Log("mousedown");
		draggableElement = evt.target as VisualElement;
		elementStartPosition = new(draggableElement.style.left.value.value, draggableElement.style.top.value.value);
		newPosition = elementStartPosition;
		isDragging = true;
		draggableElement.CaptureMouse();
		evt.StopPropagation();
	}

	Vector2 newPosition;
	private void MouseMove(MouseMoveEvent evt)
	{
		if (!isDragging) return;
		Debug.Log("mousedrag");

		newPosition += evt.mouseDelta;
		draggableElement.style.left = newPosition.x;
		draggableElement.style.top = newPosition.y;
		evt.StopPropagation();
	}

	private void MouseUp(MouseUpEvent evt)
	{
		Debug.Log("mouse up");
		draggableElement.ReleaseMouse();

		if (!isDragging) return;

		isDragging = false;

		VisualElement currentDropArea = IsOverDropArea(draggableElement, dropAreas);
		if (currentDropArea != null && !itemContainsBy.ContainsValue(currentDropArea))
		{
			//draggableElement.style.left = currentDropArea.style.left;
			//draggableElement.style.top = currentDropArea.style.top;
			itemContainsBy[draggableElement] = currentDropArea;
		}
		else
		{
			currentDropArea = IsOverDropArea(draggableElement, slots);
			if (currentDropArea != null && !itemContainsBy.ContainsValue(currentDropArea))
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

		evt.StopPropagation();
	}

	private VisualElement IsOverDropArea(VisualElement draggable, List<VisualElement> areas)
	{
		Rect draggableRect = draggable.worldBound;
		Rect dropRect;
		foreach (var dropaera in areas)
		{
			dropRect = dropaera.worldBound;
			if (dropRect.Overlaps(draggableRect)) return dropaera;
		}
		return null;
	}

}
