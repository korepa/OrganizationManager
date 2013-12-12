using System;
using System.ComponentModel;

namespace WPFExampleKnyazev
{
    // class PersonModel for wrapping entity framework class Person
    public class PersonModel
    {
        public int Id { get; set; }
        [DisplayName(@"№")]
        public int InnerId { get; set; }
        [DisplayName(@"Имя")]
        public string FirstName { get; set; }
        [DisplayName(@"Отчество")]
        public string MiddleName { get; set; }
        [DisplayName(@"Фамилия")]
        public string SecondName { get; set; }
        [DisplayName(@"Должность")]
        public string Position { get; set; }
        [DisplayName(@"Подразделение")]
        public string Department { get; set; }
        [DisplayName(@"Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName(@"Дата рождения")]
        public string DateOfBirthString
        {
            get
            {
                return DateOfBirth.ToString("dd.MM.yyyy");
            }
        }
    }
}
