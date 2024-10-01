using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class OldButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
	[SerializeField] public EventReference hoverSFX;
	[SerializeField] public EventReference clickSFX;

	// Cette m�thode est appel�e lorsque le curseur survole le bouton
	public void OnPointerEnter(PointerEventData eventData)
	{
		PlaySFX(hoverSFX);  // Joue le son de survol
	}

	// Cette m�thode est appel�e lorsque le bouton est cliqu�
	public void OnPointerClick(PointerEventData eventData)
	{
		PlaySFX(clickSFX);  // Joue le son de clic
	}

	// M�thode pour jouer un son via FMOD
	private void PlaySFX(EventReference sfx)
	{
		if (!sfx.IsNull)
		{
			FMODUnity.RuntimeManager.PlayOneShot(sfx);
		}
	}
}