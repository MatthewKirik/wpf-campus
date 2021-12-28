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
    public class AddSubjectWindowVM : DialogVMBase
    {
        private ISubjectRepository subjectRepository;
        public AddSubjectWindowVM(ISubjectRepository subjectRepository)
        {
            this.subjectRepository = subjectRepository;
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

        private RelayCommand _AddSubjectCmd;
        public RelayCommand AddSubjectCmd
        {
            get
            {
                return _AddSubjectCmd ??
                  (_AddSubjectCmd = new RelayCommand(obj =>
                  {
                      var newSubject = new SubjectDto
                      {
                          Name = Name
                      };
                      subjectRepository.AddSubject(newSubject).GetAwaiter().GetResult();
                      this.DialogResultValue = true;
                  }));
            }
        }
    }
}
