using UnityEngine;
using UnityEngine.EventSystems;

namespace Planetarity.UI
{
    public class UIButtonSoundEvent : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {

        public void OnPointerEnter(PointerEventData ped)
        {
            SoundManager.Play(SoundManager.Sound.ButtonHover);
        }

        public void OnPointerDown(PointerEventData ped)
        {
            SoundManager.Play(SoundManager.Sound.ButtonClick);
        }
    }
}
