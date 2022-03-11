using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Plugins.DatePicker.Scripts
{
    public class SelectedDate : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image selectionIcon;
        [SerializeField] private Sprite borderSprite;
        [SerializeField] private Color deselectedColor = new Color(0,0,0,0);
        
        private Color borderColor;
        private int year;
        private int month;
        private int day;

        private Action<DateTime> onClickAction;

        private bool isSelected = false;

        public DateTime Date => new DateTime(year, month, day);

        private void Start() => SetBorderSprite(borderSprite);

        public void Setup(int year, int month, int day, Color borderColor, Action<DateTime> onSelected)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            SetBorderColor(borderColor);
            onClickAction = onSelected;
        }
        
        public void SetSelected(bool value) 
        {
            isSelected = value;
            selectionIcon.color = isSelected ? borderColor : deselectedColor;
        }

        public void SetBorderSprite(Sprite border) => selectionIcon.sprite = border;
        
        public void SetBorderColor(Color color)
        {
            borderColor = color;
            selectionIcon.color = isSelected ? borderColor : deselectedColor;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            var selectedDate = Date;
            onClickAction?.Invoke(selectedDate);
            SetSelected(true);
        }
    }
}