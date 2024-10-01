using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class OldButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
	[SerializeField] public EventReference hoverSFX;
	[SerializeField] public EventReference clickSFX;

	// Cette méthode est appelée lorsque le curseur survole le bouton
	public void OnPointerEnter(PointerEventData eventData)
	{
		PlaySFX(hoverSFX);  // Joue le son de survol
	}

	// Cette méthode est appelée lorsque le bouton est cliqué
	public void OnPointerClick(PointerEventData eventData)
	{
		PlaySFX(clickSFX);  // Joue le son de clic
	}

	// Méthode pour jouer un son via FMOD
	private void PlaySFX(EventReference sfx)
	{
		if (!sfx.IsNull)
		{
			FMODUnity.RuntimeManager.PlayOneShot(sfx);
		}
	}
}