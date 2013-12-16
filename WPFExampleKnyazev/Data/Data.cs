using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFExampleKnyazev
{
    public class PersonDTO : INotifyPropertyChanged
    {
        // id
        public int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        // first name
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        // middle name
        private string middleName;
        public string MiddleName
        {
            get { return middleName; }
            set
            {
                middleName = value;
                OnPropertyChanged();
            }
        }

        // second name
        private string secondName;
        public string SecondName
        {
            get { return secondName; }
            set
            {
                secondName = value;
                OnPropertyChanged();
            }
        }

        // position
        private string position;
        public string Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged();
            }
        }

        // department
        private string department;
        public string Department
        {
            get { return department; }
            set
            {
                department = value;
                OnPropertyChanged();
            }
        }

        // date of birth
        public DateTime dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged();
                OnPropertyChanged("DateOfBirthString");
            }
        }

        // date of birth in string format
        public string DateOfBirthString
        {
            get { return DateOfBirth.ToString("dd.MM.yyyy"); }
        }


        // inotifypropertychanged realization
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string caller = null)
        {
            // make sure only to call this if the value actually changes
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
