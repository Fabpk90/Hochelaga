using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;
	[SerializeField] private UIDocument uiDocument;
	private VisualElement panelTuto;
	private Button close;

	private void Awake()
	{
		Instance = this;
		panelTuto = uiDocument.rootVisualElement.Q<VisualElement>("TutorialWindow");
		close = panelTuto.Q<Button>("Close");
	}

	private void Start()
	{
		close.clicked += ()=>OpenTutorial(false);
		//OpenTutorial(false);
	}

	public void OpenTutorial(bool open = true)
	{
		panelTuto.style.display = open ? DisplayStyle.Flex : DisplayStyle.None;
	}
}
