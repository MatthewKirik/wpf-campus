using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Models
{

    public class IsScheduledValue
    {
        public event Action<int> OnValueChanged;

        public int DayOfWeek { get; set; }

        private bool _Value;
        public bool Value
        {
            get { return _Value; }
            set 
            { 
                _Value = value;
                OnValueChanged?.Invoke(DayOfWeek);
            }
        }
    }

    public class LessonScheduleDto
    {
        public int LessonIndex { get; set; }
        public int WeekIndex { get; set; }
        public string Time { get; set; }
        public event Action<int, int, int, bool> OnScheduledChanged;

        private List<IsScheduledValue> _IsScheduled;
        public List<IsScheduledValue> IsScheduled
        {
            get { return _IsScheduled; }
            set 
            {
                foreach (var item in value)
                {
                    if (item != null)
                        item.OnValueChanged += Item_OnValueChanged;
                }
                _IsScheduled = value; 
            }
        }

        private void Item_OnValueChanged(int dayOfWeek)
        {
            OnScheduledChanged?.Invoke(LessonIndex, WeekIndex, dayOfWeek, IsScheduled[dayOfWeek].Value);
        }

        public LessonScheduleDto()
        {
            IsScheduled = new List<IsScheduledValue>();
        }
    }

    public class ScheduleDto
    {
        public int GroupId { get; set; }
        public int SubjectId { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }

        public List<LessonScheduleDto> Week1 { get; set; }
        public List<LessonScheduleDto> Week2 { get; set; }
    }
}
