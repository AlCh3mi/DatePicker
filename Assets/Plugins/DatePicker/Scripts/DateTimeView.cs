using System;
using UnityEngine;
using UnityEngine.Events;

namespace Plugins.DatePicker.Scripts
{
    public class DateTimeView : MonoBehaviour
    {
        [SerializeField] private string culture;
        [SerializeField] private DateFormattingType dateFormatType;
        
        private DateView dateView;

        private void Start()
        {
            dateView = new DateView(DateView.GetCulture(culture));
        }

        // this can update buttons, labels etc.
        public UnityEvent<string> valueChanged; 

        public void SetValue(DateTime dateTime){
            switch (dateFormatType)
            {
                case DateFormattingType.FormattedDate:
                    valueChanged?.Invoke(dateView.ToFormattedDate(dateTime));
                    break;
                case DateFormattingType.ShortDate:
                    valueChanged?.Invoke(dateView.ToShortDate(dateTime));
                    break;
                case DateFormattingType.FullDate:
                    valueChanged?.Invoke(dateView.ToLongDate(dateTime));
                    break;
                case DateFormattingType.YearAndMonth:
                    valueChanged?.Invoke(dateView.ToMonthOfYear(dateTime));
                    break;
            }
        }
    }

    public enum DateFormattingType
    {
           FormattedDate,
           ShortDate,
           FullDate,
           YearAndMonth
    }
}