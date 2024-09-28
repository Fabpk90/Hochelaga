using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class UIInventory : MonoBehaviour
{
	public static UIInventory Instance;

	[SerializeField] private UIDocument uiMainDocument;

	private VisualElement steleMaker;
	private List<VisualElement> slots = new();
	private List<VisualElement> dropAreas = new();
	private bool isDragging = false;
	private Vector2 elementStartPosition;
	private Vector2 offset;

	private void Awake()
	{
		Instance = this;

		VisualElement bar = uiMainDocument.rootVisualElement.Q<VisualElement>("InventoryBar");
		steleMaker = uiMainDocument.rootVisualElement.Q<VisualElement>("SteleMaker");
		slots = bar.Query("BigSlot").ToList();
		dropAreas = steleMaker.Query("DropArea").ToList();

		//temp
		draggableElement = uiMainDocument.rootVisualElement.Q<VisualElement>("Item");

		draggableElement.RegisterCallback<MouseDownEvent>(MouseDown);
		draggableElement.RegisterCallback<MouseMoveEvent>(OnMouseMove);
		draggableElement.RegisterCallback<MouseUpEvent>(MouseUp);
	}


	private void Start()
	{
		foreach (var slot in slots)
		{
			RegisterMouseEvent(slot);
		}

		slots[0].RegisterCallback<ClickEvent>(evt => {  });
	}

	void RegisterMouseEvent(VisualElement elem)
	{
		elem.RegisterCallback<MouseEnterEvent>(_ => { });
		elem.RegisterCallback<MouseLeaveEvent>(_ => { });
	}

	/// ///////////// /

	private VisualElement draggableElement;


	private void MouseDown(MouseDownEvent evt)
	{
		elementStartPosition = new(draggableElement.style.left.value.value, draggableElement.style.top.value.value);
		newPosition = elementStartPosition;
		offset = evt.mousePosition;
		draggableElement.CaptureMouse();
		isDragging = true;
		evt.StopPropagation();
	}

	Vector2 newPosition;
	private void OnMouseMove(MouseMoveEvent evt)
	{
		if (!isDragging) return;

		newPosition += evt.mouseDelta;
		draggableElement.style.left = newPosition.x;
		draggableElement.style.top = newPosition.y;
		evt.StopPropagation();
	}

	private void MouseUp(MouseUpEvent evt)
	{
		if (!isDragging) return;

		isDragging = false;
		draggableElement.ReleaseMouse();
		offset = Vector2.zero;

		VisualElement currentDropArea = IsOverDropArea(draggableElement, dropAreas);
		if (currentDropArea != null)
		{
			//draggableElement.style.left = currentDropArea.style.left;
			//draggableElement.style.top = currentDropArea.style.top;
		}
		else
		{
			currentDropArea = IsOverDropArea(draggableElement, slots);
			if (currentDropArea != null)
			{
				currentDropArea.Add(draggableElement);
				draggableElement.style.left = currentDropArea.style.left;
				draggableElement.style.top = currentDropArea.style.top;
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
