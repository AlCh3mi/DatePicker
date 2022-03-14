using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Plugins.DatePicker.Scripts
{
    public class DatePicker : MonoBehaviour
    {
        [SerializeField] private int minYear = 2022;
        [SerializeField] private int maxYear = 2030;
        [SerializeField] private Color borderColor;
        [SerializeField] private Color fillColor;
        [SerializeField] private Color textColor;
        
        public UnityEvent<DateTime> dateSelected;
        
        [Header("Dependencies")]
        [SerializeField] private TMP_Dropdown monthDropdown;
        [SerializeField] private TMP_Dropdown yearDropdown;
        [SerializeField] private RectTransform dateObjectsParent;
        [SerializeField] private GameObject dateObjectPrefab;
        [SerializeField] private GameObject dudObjectPrefab;
        [SerializeField] private Image[] borderColorComponents;
        [SerializeField] private Image[] fillColorComponents;
        [SerializeField] private TMP_Text[] textsToColor;
        
        private int SelectedMonth => monthDropdown.value + 1;
        private int SelectedYear => Convert.ToInt32(yearDropdown.options[yearDropdown.value].text);
        
        private readonly Calendar calendar = Calendar.ReadOnly(new GregorianCalendar());
        
        public DateTime SelectedDate = DateTime.Today;

        private void ClearDays()
        {
            while (dateObjectsParent.childCount > 0)
            {
                var currentObject = dateObjectsParent.GetChild(0).gameObject;
                DestroyImmediate(currentObject);
            }
        }
        
        private void PopulateYearList()
        {
            yearDropdown.ClearOptions();
            
            for (var i = maxYear; i >= minYear; i--)
                yearDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
        }

        private void PopulateMonthList()
        {
            monthDropdown.ClearOptions();
            
            for (int i = 0; i < 12; i++)
            {
                var option = new TMP_Dropdown.OptionData(CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i]);
                monthDropdown.options.Add(option);
            }
        }

        private void PopulateDayList(int year, int month)
        {
            ClearDays();

            //Spawn all days of the month
            for (int i = 0; i < calendar.GetDaysInMonth(year, month); i++)
            {
                var dateObject = Instantiate(dateObjectPrefab, dateObjectsParent);
                var date = (i + 1);
                dateObject.GetComponent<TMP_Text>().text = date.ToString();
                dateObject.name = date.ToString();
                var selectedDate =  dateObject.GetComponent<SelectedDate>();
                selectedDate.Setup(SelectedYear, SelectedMonth, date, borderColor, OnDateSelectedCallback);

                var isSelectedDate = SelectedDate.Year == SelectedYear && 
                                     SelectedDate.Month == SelectedMonth &&
                                     SelectedDate.Day == date;
                
                if(isSelectedDate)
                    selectedDate.SetSelected(true);
            }

            //spawn dudObject till the first day of the week (eg 6)
            var dayOfWeek = FirstDayOfMonth(year, month);
            var weekEnum = (int) dayOfWeek;
            
            for (int i = 0; i < weekEnum; i++)
            {
                var dud = Instantiate(dudObjectPrefab, dateObjectsParent);
                dud.transform.SetAsFirstSibling();
            }
            
            //spawn dudObjects to complete the calendar
            for (int i = dateObjectsParent.childCount; i < 42; i++)
            {
                var dud = Instantiate(dudObjectPrefab, dateObjectsParent);
                dud.transform.SetAsLastSibling();
            }
            
            Repaint();
        }

        private void DeselectAllDates()
        {
            foreach (var selectedDate in GetComponentsInChildren<SelectedDate>())
                selectedDate.SetSelected(false);
        }

        private void SelectLowestYear()
        {
            yearDropdown.value = 0;
            yearDropdown.RefreshShownValue();
        }
        
        private void SelectLowestMonth()
        {
            monthDropdown.value = 0;
            monthDropdown.RefreshShownValue();
        }
        
        private void SelectLowestDay() 
        {
            var lowest = GetComponentInChildren<SelectedDate>();
            lowest.SetSelected(true);
            SelectedDate = lowest.Date;
        }

        private DayOfWeek FirstDayOfMonth(int year, int month) => calendar.GetDayOfWeek(new DateTime(year, month, 1));
        
        private void OnDateSelectedCallback(DateTime value)
        {
            SelectedDate = value;
            DeselectAllDates();
            dateSelected?.Invoke(value);
            Debug.Log($"DateSelected : {value.ToShortDateString()}");
        }

        private void CalculateSpacing()
        {
            const int amountOfColumns = 7;
            const int amountOfRows = 6;
            
            var rectTransform = (RectTransform) transform;
            var smallest = Math.Min(rectTransform.rect.width, rectTransform.rect.height);

            var bounds = dateObjectsParent.rect;
            var gridLayout = dateObjectsParent.GetComponent<GridLayoutGroup>();
            
            gridLayout.cellSize = new Vector2(smallest/10, smallest/10);
            
            var xPadding = gridLayout.padding.horizontal;
            var xSpacing = ((bounds.width - xPadding) - (amountOfColumns * gridLayout.cellSize.x)) / (amountOfColumns - 1);

            var yPadding = gridLayout.padding.vertical;
            var ySpacing = ((bounds.height - yPadding) - (amountOfRows * gridLayout.cellSize.y)) / (amountOfRows - 1); 
            
            gridLayout.spacing = new Vector2(xSpacing, ySpacing);
        }
        
        private void Repaint()
        {
            foreach (var image in borderColorComponents)
                image.color = borderColor;

            foreach (var image in fillColorComponents)
                image.color = fillColor;

            foreach (var text in textsToColor)
                text.color = textColor;

            foreach (var selectedDate in transform.GetComponentsInChildren<SelectedDate>())
            {
                selectedDate.GetComponentInChildren<TMP_Text>().color = textColor;
                selectedDate.SetBorderColor(borderColor);
            }
        }

        #region UnityFunctions
        private void OnEnable()
        {
            CalculateSpacing();
            
            yearDropdown.onValueChanged.AddListener((_) => PopulateDayList(SelectedYear, SelectedMonth));
            monthDropdown.onValueChanged.AddListener((_) => PopulateDayList(SelectedYear, SelectedMonth));
        }

        private void Start()
        {
            PopulateYearList();
            SelectLowestYear();
            
            PopulateMonthList();
            SelectLowestMonth();

            PopulateDayList(SelectedYear, SelectedMonth);
            SelectLowestDay();
        }
        
        private void OnDisable()
        {
            yearDropdown.onValueChanged.RemoveAllListeners();
            monthDropdown.onValueChanged.RemoveAllListeners();
        }
        
        private void OnValidate()
        {
            CalculateSpacing();
            Repaint();

            if (minYear > maxYear)
                maxYear = minYear;
        }

        private void OnRectTransformDimensionsChange() => CalculateSpacing();
        #endregion
    }
}