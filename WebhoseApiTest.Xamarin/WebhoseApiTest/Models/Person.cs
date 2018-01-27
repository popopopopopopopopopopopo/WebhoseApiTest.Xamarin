using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebhoseApiTest.Models
{
    public class Person : BindableBase
    {
        public static ObservableCollection<Person> GetSampleList()
        {
            var collec = new ObservableCollection<Person>();
            collec.Add(new Person());
            collec.Add(new Person());
            return collec;
        }

        private Guid _myID = Guid.NewGuid();
        public Guid ID
        {
            get => _myID;
            set => SetProperty(ref _myID, value);
        }

        private string _myName = Guid.NewGuid().ToString();
        public string Name
        {
            get => _myName;
            set => SetProperty(ref _myName, value);
        }
    }
}
