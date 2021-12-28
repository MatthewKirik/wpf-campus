using DataTransfer.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Base;
using ViewModels.Commands;

namespace ViewModels.Windows
{
    public class AddStudentWindowVM : DialogVMBase
    {
        private IStudentRepository studentRepository;
        public AddStudentWindowVM(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private string _Phone;
        public string Phone
        {
            get => _Phone;
            set
            {
                _Phone = value;
                RaisePropertyChanged(nameof(Phone));
            }
        }

        private string _Email;
        public string Email
        {
            get => _Email;
            set
            {
                _Email = value;
                RaisePropertyChanged(nameof(Email));
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
                      if (ParentParameters.TryGetValue("groupId", out object groupIdStr))
                      {
                          int groupId = (int)groupIdStr;
                          StudentDto newGroup = new StudentDto
                          {
                              Name = Name.Trim(),
                              Email = Email,
                              Phone = Phone
                          };
                          studentRepository.AddStudent(groupId, newGroup)
                            .GetAwaiter().GetResult();
                          this.DialogResultValue = true;
                      }
                  }));
            }
        }

    }
}
