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

namespace ViewModels.Windows
{
    public class AddGroupWindowVM : DialogVMBase
    {
        IGroupRepository groupRepository;
        public AddGroupWindowVM(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        private string _GroupName;
        public string GroupName
        {
            get => _GroupName;
            set
            {
                _GroupName = value;
                RaisePropertyChanged(nameof(GroupName));
            }
        }

        private string _GroupFaculty;
        public string GroupFaculty
        {
            get => _GroupFaculty;
            set
            {
                _GroupFaculty = value;
                RaisePropertyChanged(nameof(GroupFaculty));
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
                      GroupDto newGroup = new GroupDto
                      {
                          Name = GroupName,
                          Faculty = GroupFaculty
                      };
                      groupRepository.AddGroup(GroupFaculty, newGroup)
                        .GetAwaiter().GetResult();
                      this.DialogResultValue = true;
                  }));
            }
        }

    }
}
