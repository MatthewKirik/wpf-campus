using DataAccess;
using DataTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewModels.Commands;

namespace ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        IGroupRepository groupRepository;

        public event PropertyChangedEventHandler PropertyChanged;

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
            }
        }

        private void ApplyFilter()
        {
            if (_Filter.Length == 0)
            {
                FilteredGroups = new ObservableCollection<GroupDto>(Groups);
                return;
            }
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

        private GroupDto _SelectedGroup;
        public GroupDto SelectedGroup
        {
            get => _SelectedGroup;
            set
            {
                ChangeDroup(value);
            }
        }
        private async void ChangeDroup(GroupDto selected)
        {
            _SelectedGroup = await groupRepository.GetGroup(selected.Id);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedGroup"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsAnyGroupSelected"));
        }

        public bool IsAnyGroupSelected { get => SelectedGroup != null; }

        private RelayCommand _clearFilterCmd;

        public ICommand ClearFilterCmd
        {
            get { return _clearFilterCmd; }
            set { _clearFilterCmd = value; }
        }



        public MainWindowVM(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
            using (CampusContext context = new CampusContext())
            {
                context.Database.Migrate();
            }
            Groups = groupRepository.GetGroups().GetAwaiter().GetResult();
            FilteredGroups = new ObservableCollection<GroupDto>(Groups);
        }
    }
}
