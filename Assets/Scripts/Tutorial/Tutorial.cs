using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.MessageBox;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;
	[SerializeField] private UIDocument uiDocument;
	private VisualElement panelTuto;
	private VisualElement panel2, panel3, panel4, panel5, panel6;
	private Button close;
	private bool isOpen = true;
	private int numberShow = 1;

	private void Awake()
	{
		Instance = this;
		panelTuto = uiDocument.rootVisualElement.Q<VisualElement>("TutorialWindow");
		close = panelTuto.Q<Button>("Close");
		panel2 = panelTuto.Q<VisualElement>("Tuto2");
		panel3 = panelTuto.Q<VisualElement>("Tuto3");
		panel4 = panelTuto.Q<VisualElement>("Tuto4");
		panel5 = panelTuto.Q<VisualElement>("Tuto5");
		panel6 = panelTuto.Q<VisualElement>("Tuto6");
	}

	private void Start()
	{
		OpenTutorial(true);
	}

	private void OnEnable()
	{
		close.clicked += ()=>OpenTutorial(false);
	}

	private void OnDisable()
	{
		close.clicked -= () => OpenTutorial(false);
	}

	private void Update()
	{
		if (!isOpen) return;

		if (Input.anyKeyDown)
		{
			NextPage();
		}
	}

	public void OpenTutorial(bool open = true)
	{
		isOpen = open;
		panelTuto.style.display = open ? DisplayStyle.Flex : DisplayStyle.None;
		if (!open)
		{
			numberShow = 1;
			panel2.style.display = DisplayStyle.None;
			panel3.style.display = DisplayStyle.None;
			panel4.style.display = DisplayStyle.None;
			panel5.style.display = DisplayStyle.None;
			panel6.style.display = DisplayStyle.None;
		}
	}

	public void NextPage()
	{
		numberShow++;
		switch (numberShow)
		{
			case 2:
				panel2.style.display = DisplayStyle.Flex;
				break;
			case 3:
				panel3.style.display = DisplayStyle.Flex;
				break;
			case 4:
				panel4.style.display = DisplayStyle.Flex;
				break;
			case 5:
				panel5.style.display = DisplayStyle.Flex;
				break;
			case 6:
				panel6.style.display = DisplayStyle.Flex;
				break;
			case 7:
				OpenTutorial(false);
				break;
		}
	}
}
