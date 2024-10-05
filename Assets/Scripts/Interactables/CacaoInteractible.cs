using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CacaoInteractible : Interactables
{
	public VisualTreeAsset cacaoView;
	private VisualElement cacaoPoint;

	public int amount = 1;

	private void Start()
	{
		cacaoPoint = UIInventory.Instance.uiMainDocument.rootVisualElement.Q<VisualElement>("CacaoPoint");
	}

	public override void PlayerCollisioned(PlayerController _player)
	{
		base.PlayerCollisioned(_player);

		_player.controls.Interact.Enable();
		_player.controls.Interact.Interact.performed += InteractOnperformed; // This should have been in the interactables class eheh
		_player.controls.Interact.Close.performed += CloseOnperformed;
	}

	private void CloseOnperformed(InputAction.CallbackContext obj)
	{
	}

	public override void InteractOnperformed(InputAction.CallbackContext obj)
	{
		if (objectIsNearby)
		{
			base.InteractOnperformed(obj);

			MoneyManager.instance.AddMoney(amount);
			StartCoroutine(ShowCacaoEffect());
			PlayerController.instance?.TwinkleE();
		}

	}

	private WaitForSeconds waitFourSeconds = new WaitForSeconds(1);
	public IEnumerator ShowCacaoEffect()
	{
		VisualElement cacaoElement = cacaoView.Instantiate();
		cacaoPoint.Add(cacaoElement);
		yield return null;
		cacaoElement.Q(className:"cacaoAnimation").AddToClassList("cacaoAnimationPlay");
		yield return waitFourSeconds;
		cacaoPoint.Remove(cacaoElement);
	}
}
