using DataAccess;
using DataTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using ServiceConfiguration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Commands;
using ViewModels.Base;
using System.Data;
using System;
using ViewModels.Models;

namespace ViewModels.Windows
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        IGroupRepository groupRepository;
        ISubjectRepository subjectRepository;
        IScheduleRepository scheduleRepository;
        ILessonRepository lessonRepository;
        IWindowCreator windowCreator;

        public MainWindowVM(
            IGroupRepository groupRepository,
            ISubjectRepository subjectRepository,
            IScheduleRepository scheduleRepository,
            ILessonRepository lessonRepository,
            IWindowCreator windowCreator)
        {
            this.groupRepository = groupRepository;
            this.subjectRepository = subjectRepository;
            this.scheduleRepository = scheduleRepository;
            this.lessonRepository = lessonRepository;
            this.windowCreator = windowCreator;
            using (CampusContext context = new CampusContext())
            {
                context.Database.Migrate();
            }
            Year = "2021";
            Semester = 1;
            LoadGroups();
            LoadSubjects();
        }

        #region Groups
        private void LoadGroups()
        {
            Groups = groupRepository.GetGroups().GetAwaiter().GetResult();
            FilteredGroups = new ObservableCollection<GroupDto>(Groups);
            Filter = "";
        }

        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
        private ObservableCollection<GroupDto> _FilteredGroups;
        public ObservableCollection<GroupDto> FilteredGroups
        {
            get { return _FilteredGroups; }
            set
            {
                _FilteredGroups = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilteredGroups)));
            }
        }

        private string _Filter;
        public string Filter
        {
            get => _Filter;
            set
            {
                _Filter = value;
                ApplyFilter();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterLength)));
            }
        }
        public int FilterLength => (_Filter?.Length ?? 0) * 2;

        private void ApplyFilter()
        {
            if (_Filter.Length == 0)
            {
                FilteredGroups = new ObservableCollection<GroupDto>(Groups);
            }
            else
            {
                var lcs = new F23.StringSimilarity.LongestCommonSubsequence();
                var ordered = Groups
                    .Select(g => new
                    {
                        group = g,
                        key = g.Faculty + " " + g.Name,
                        distance = lcs.Distance(
                            (g.Faculty + " " + g.Name).ToLowerInvariant(),
                            _Filter.ToLowerInvariant())
                    })
                    .Where(x => x.key.Length - x.distance > 0)
                    .OrderBy(x => x.distance)
                    .Select(x => x.group);
                FilteredGroups = new ObservableCollection<GroupDto>(ordered);
            }
        }

        private GroupDto _SelectedGroup;
        public GroupDto SelectedGroup
        {
            get => _SelectedGroup;
            set
            {
                ChangeGroup(value).GetAwaiter().GetResult();
            }
        }

        private GroupDto _DisplayedGroup;
        public GroupDto DisplayedGroup
        {
            get => _DisplayedGroup;
            set
            {
                ChangeGroup(value).GetAwaiter().GetResult();
            }
        }

        private async Task ChangeGroup(GroupDto selected)
        {
            if (selected == null)
                _SelectedGroup = null;
            else
            {
                _SelectedGroup = await groupRepository.GetGroup(selected.Id);
                _DisplayedGroup = _SelectedGroup;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedGroup)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayedGroup)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAnyGroupSelected)));
        }

        public bool IsAnyGroupSelected { get => DisplayedGroup != null; }


        private RelayCommand _clearFilterCmd;
        public RelayCommand ClearFilterCmd
        {
            get
            {
                return _clearFilterCmd ??
                  (_clearFilterCmd = new RelayCommand(obj =>
                  {
                      Filter = "";
                  }));
            }
        }

        private RelayCommand _AddGroupCmd;
        public RelayCommand AddGroupCmd
        {
            get
            {
                return _AddGroupCmd ??
                  (_AddGroupCmd = new RelayCommand(obj =>
                  {
                      var addGroupWindow = windowCreator.CreateWindow("AddGroupWindow");
                      addGroupWindow.ShowDialog();
                      if (addGroupWindow.DialogResult == true)
                      {
                          LoadGroups();
                      }
                  }));
            }
        }

        private RelayCommand _AddStudentCmd;
        public RelayCommand AddStudentCmd
        {
            get
            {
                return _AddStudentCmd ??
                  (_AddStudentCmd = new RelayCommand(obj =>
                  {
                      var addStudentWindow = windowCreator.CreateWindow("AddStudentWindow",
                          ("groupId", _DisplayedGroup.Id.ToString()));
                      addStudentWindow.ShowDialog();
                      if (addStudentWindow.DialogResult == true)
                      {
                          DisplayedGroup = groupRepository.GetGroup(_DisplayedGroup.Id)
                            .GetAwaiter().GetResult();
                      }
                  }));
            }
        }
        #endregion

        #region Subjects

        private string _Year;
        public string Year
        {
            get => _Year;
            set
            {
                _Year = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Year)));
                UpdateScheduled().GetAwaiter().GetResult();
                LoadMarks().GetAwaiter().GetResult();
            }
        }

        private int _Semester;
        public int Semester
        {
            get => _Semester;
            set
            {
                _Semester = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Semester)));
                UpdateScheduled().GetAwaiter().GetResult();
                LoadMarks().GetAwaiter().GetResult();
            }
        }

        private SubjectDto _SelectedSubject;
        public SubjectDto SelectedSubject
        {
            get => _SelectedSubject;
            set
            {
                ChangeSubject(value).GetAwaiter().GetResult();
            }
        }

        private SubjectDto _DisplayedSubject;
        public SubjectDto DisplayedSubject
        {
            get => _DisplayedSubject;
            set
            {
                ChangeSubject(value).GetAwaiter().GetResult();
            }
        }

        public bool IsAnySubjectSelected { get => DisplayedSubject != null; }

        private async Task ChangeSubject(SubjectDto selected)
        {
            if (selected == null)
                _SelectedSubject = null;
            else
            {
                _SelectedSubject = selected;
                _DisplayedSubject = _SelectedSubject;
                UpdateScheduled().GetAwaiter().GetResult();
                LoadMarks().GetAwaiter().GetResult();
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSubject)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayedSubject)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAnySubjectSelected)));
        }

        private async Task UpdateScheduled()
        {
            if (DisplayedGroup == null) return;
            Schedule = await scheduleRepository
                .GetSchedule(DisplayedGroup.Id, DisplayedSubject.Id, int.Parse(Year), Semester);
            foreach (var item in Schedule.Week1)
            {
                item.OnScheduledChanged += OnScheduledChanged;
            }
            foreach (var item in Schedule.Week2)
            {
                item.OnScheduledChanged += OnScheduledChanged;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSubject)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayedSubject)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAnySubjectSelected)));
        }

        private void OnScheduledChanged(int lessonIndex, int weekIndex, int dayOfWeek, bool enabled)
        {
            var lesson = new ScheduledLessonDto()
            {
                DayOfWeek = dayOfWeek,
                Semester = Semester,
                LessonIndex = lessonIndex,
                Year = int.Parse(Year),
                WeekIndex = weekIndex
            };
            if (enabled)
                scheduleRepository.AddScheduledLesson(
                    DisplayedGroup.Id,
                    DisplayedSubject.Id,
                    lesson);
            else
                scheduleRepository.RemoveScheduledLesson(
                    DisplayedGroup.Id,
                    DisplayedSubject.Id,
                    lesson);
        }

        private void LoadSubjects()
        {
            var subjects = subjectRepository.GetSubjects().GetAwaiter().GetResult();
            Subjects = new ObservableCollection<SubjectDto>(subjects);
        }
        private ObservableCollection<SubjectDto> _Subjects;
        public ObservableCollection<SubjectDto> Subjects
        {
            get { return _Subjects; }
            set
            {
                _Subjects = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Subjects)));
            }
        }
        private ScheduleDto _Schedule;
        public ScheduleDto Schedule
        {
            get { return _Schedule; }
            set
            {
                _Schedule = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Schedule)));
            }
        }

        private RelayCommand _AddSubjectCmd;
        public RelayCommand AddSubjectCmd
        {
            get
            {
                return _AddSubjectCmd ??
                  (_AddSubjectCmd = new RelayCommand(obj =>
                  {
                      var addSubjectWindow = windowCreator.CreateWindow("AddSubjectWindow");
                      addSubjectWindow.ShowDialog();
                      if (addSubjectWindow.DialogResult == true)
                      {
                          LoadSubjects();
                      }
                  }));
            }
        }
        #endregion

        #region Lessons
        private DataView _Marks;
        public DataView Marks
        {
            get { return _Marks; }
            set
            {
                _Marks = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Marks)));
            }
        }

        private async Task LoadMarks()
        {
            if (DisplayedGroup == null || DisplayedSubject == null || !int.TryParse(Year, out int year))
                return;
            var marks = await lessonRepository.GetMarks(DisplayedGroup.Id, DisplayedSubject.Id, year, Semester);
            var view = new DataView();
            view.Table = new DataTable("Оцінки");
            view.Table.Columns.Add("Студент", typeof(WrappedString));
            foreach (var lesson in marks.Lessons)
            {
                view.Table.Columns.Add(lesson.Date.ToString("dd.MM\nHH:mm"), typeof(NotifiedCellValue));
            }
            view.Table.Columns.Add("Сума балів", typeof(WrappedInt));
            for (int i = 0; i < marks.Students.Count; i++)
            {
                var student = marks.Students[i];
                var name = new WrappedString()
                {
                    Value = string.Join(" ", student.Name.Split().Take(2))
                };
                object[] values = new object[marks.Lessons.Count + 2];
                values[0] = name;
                int sum = 0;
                for (int j = 0; j < marks.Lessons.Count; j++)
                {
                    int mark = marks.Marks[i, j];
                    sum += mark;
                    var cell = new NotifiedCellValue(student.Id, marks.Lessons[j].Id, mark.ToString());
                    values[j + 1] = cell;
                    cell.OnCellChanged += OnMarkChanged;
                }
                values[marks.Lessons.Count + 2 - 1] = new WrappedInt()
                {
                    Value = sum
                };
                view.Table.Rows.Add(values);
            }
            Marks = view;
        }

        private void OnMarkChanged(int studentId, int lessonId, string newValue)
        {
            if (int.TryParse(newValue, out int mark))
                lessonRepository.AddMark(lessonId, studentId, mark).GetAwaiter().GetResult();
            else
                lessonRepository.RemoveMark(lessonId, studentId).GetAwaiter().GetResult();
            LoadMarks();
        }

        private RelayCommand _AddLessonCmd;
        public RelayCommand AddLessonCmd
        {
            get
            {
                return _AddLessonCmd ??
                  (_AddLessonCmd = new RelayCommand(obj =>
                  {
                      if (DisplayedGroup == null || DisplayedSubject == null || !int.TryParse(Year, out int year))
                          return;
                      var lessons = scheduleRepository
                          .GetScheduledLessons(_DisplayedGroup.Id, _DisplayedSubject.Id, int.Parse(Year), Semester)
                          .GetAwaiter()
                          .GetResult();
                      var addLessonWindow = windowCreator
                        .CreateWindow("AddLessonWindow", 
                            ("groupId", _DisplayedGroup.Id),
                            ("subjectId", _DisplayedSubject.Id),
                            ("lessons", lessons),
                            ("year", int.Parse(Year)));
                      addLessonWindow.ShowDialog();
                      if (addLessonWindow.DialogResult == true)
                      {
                          LoadMarks();
                      }
                  }));
            }
        }
        #endregion
    }
}
