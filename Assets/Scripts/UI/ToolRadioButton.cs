using System;
using BuilderTool.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BuilderTool.LevelEditor
{
    public class ToolRadioButton : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public EMechanic Key {get; private set;}

        [SerializeField] private Image _button;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private CanvasGroup _content;

        public bool IsActivated {get; private set;}

        private Color _labelDeactiveColor = new(1, 1, 1, .5f);
        private Color _labelActiveColor = new(1, 1, 1, 1);

        public event Action<ToolRadioButton, bool> OnButtonClicked;

        public void ChangeState(bool state){
            if(state){
                ActivateContent();
            }
            else{
                DeactivateContent();
            }
        }

        public void ActivateContent(){
            IsActivated = true;
            _button.gameObject.SetActive(true);
            _label.color = _labelActiveColor;
            _content.interactable = true;
        }

        public void DeactivateContent(){
            IsActivated = false;
            _button.gameObject.SetActive(false);
            _label.color = _labelDeactiveColor;
            _content.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!IsActivated)
            {
                ActivateContent();
            }
            else{
                DeactivateContent();
            }

            OnButtonClicked?.Invoke(this, IsActivated);
        }
    }
}
