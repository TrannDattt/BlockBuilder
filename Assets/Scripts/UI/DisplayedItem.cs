using BuilderTool.Enums;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BuilderTool.LevelEditor
{
    public class DisplayedItem : MonoBehaviour, IPointerDownHandler
    {
        [field: SerializeField] public EShape Shape { get; private set; }
        [field: SerializeField] public RectTransform RectTransform { get; private set; }

        public event Action<DisplayedItem> OnItemClick;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnItemClick?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnItemClick = null;
        }
    }
}
