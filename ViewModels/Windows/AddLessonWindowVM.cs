using DataTransfer.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Base;
using ViewModels.Commands;
using ViewModels.Models;

namespace ViewModels.Windows
{
    public class AddLessonWindowVM : DialogVMBase
    {
        ILessonRepository lessonRepository;
        public AddLessonWindowVM(ILessonRepository lessonRepository)
        {
            this.lessonRepository = lessonRepository;
        }

        protected override void OnParentParametersSet()
        {
            if (this.ParentParameters.TryGetValue("lessons", out object obj))
            {
                var lessons = (List<ScheduledLessonDto>)obj;
                HighlightedDates = GenerateDates(lessons);
            }
            if (this.ParentParameters.TryGetValue("year", out object yearObj))
            {
                var year = (int)yearObj;
                if (year == DateTime.Now.Year)
                    SelectedDate = DateTime.Today;
                else
                    SelectedDate = new DateTime(year, 1, 1);
            }
        }

        private string[] times =
        {
            "",
            "08:30",
            "10:25",
            "12:20",
            "14:15",
            "16:10",
            "18:30",
            "",
            "",
            "",
            "",
            "",
            ""
        };
        private List<HighlightedDate> GenerateDates(List<ScheduledLessonDto> scheduledLessons)
        {
            List<HighlightedDate> dates = new List<HighlightedDate>();
            foreach (var scheduled in scheduledLessons)
            {
                DateTime fromDate, toDate;
                if (scheduled.Semester == 1)
                {
                    fromDate = new DateTime(scheduled.Year, 9, 1);
                    toDate = new DateTime(scheduled.Year + 1, 1, 1);
                }
                else
                {
                    fromDate = new DateTime(scheduled.Year + 1, 1, 1);
                    toDate = new DateTime(scheduled.Year + 1, 6, 30);
                }
                if (scheduled.WeekIndex == 2)
                    fromDate = fromDate.AddDays(7);
                int day = ((int)fromDate.DayOfWeek == 0) ? 7 : (int)DateTime.Now.DayOfWeek + 1;
                if (scheduled.DayOfWeek > day) 
                    fromDate = fromDate.AddDays(scheduled.DayOfWeek - day);
                else 
                    fromDate = fromDate.AddDays(7 - day + scheduled.DayOfWeek);
                for (DateTime d = fromDate; d < toDate; d = d.AddDays(14))
                {
                    try
                    {
                        var existing = dates.First(x => x.Date == d);
                        existing.Description += $"; {times[scheduled.LessonIndex]}";
                    }
                    catch (Exception)
                    {
                        dates.Add(new HighlightedDate(d, $"Запланована пара на {times[scheduled.LessonIndex]}"));
                    }

                }
            }
            return dates;
        }

        public DateTime SelectedDate { get; set; }

        private IList<HighlightedDate> _highlightedDates;
        public IList<HighlightedDate> HighlightedDates
        {
            get { return _highlightedDates; }
            set
            {
                _highlightedDates = value;
                RaisePropertyChanged(nameof(HighlightedDates));
            }
        }

        private string _Topic;

        public string Topic
        {
            get { return _Topic; }
            set
            { 
                _Topic = value;
                RaisePropertyChanged(nameof(Topic));
            }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        private DateTime _SelectedTime;

        public DateTime SelectedTime
        {
            get { return _SelectedTime; }
            set
            {
                _SelectedTime = value;
                RaisePropertyChanged(nameof(SelectedTime));
            }
        }

        private RelayCommand _AddLessonCmd;
        public RelayCommand AddLessonCmd
        {
            get
            {
                return _AddLessonCmd ??
                  (_AddLessonCmd = new RelayCommand(obj =>
                  {
                      int groupId = (int)this.ParentParameters["groupId"];
                      int subjectId = (int)this.ParentParameters["subjectId"];
                      lessonRepository.AddLesson(groupId, subjectId, new LessonDto()
                      {
                          Date = SelectedDate.Add(SelectedTime.TimeOfDay),
                          Description = Description,
                          Topic = Topic
                      }).GetAwaiter().GetResult();
                      this.DialogResultValue = true;
                  }));
            }
        }
    }
}
