using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFExampleKnyazev
{
    public static class DataBuilderExtentions
    {
        public static Person GetPerson(this PersonDTO personDTO)
        {
            var person = new Person();
            person.Id = personDTO.Id;
            person.FirstName = personDTO.FirstName;
            person.MiddleName = personDTO.MiddleName;
            person.SecondName = personDTO.SecondName;
            person.Position = personDTO.Position;
            person.Department = personDTO.Department;
            person.DateOfBirth = personDTO.DateOfBirth;

            return person;
        }

        public static void UpdatePerson(this Person person, PersonDTO newPersonDTO)
        {
            person.FirstName = newPersonDTO.FirstName;
            person.MiddleName = newPersonDTO.MiddleName;
            person.SecondName = newPersonDTO.SecondName;
            person.Position = newPersonDTO.Position;
            person.Department = newPersonDTO.Department;
            person.DateOfBirth = newPersonDTO.DateOfBirth;
        }

        public static PersonDTO GetPersonDTO(this Person person)
        {
            var personDTO = new PersonDTO();
            personDTO.Id = person.Id;
            personDTO.FirstName = person.FirstName;
            personDTO.MiddleName = person.MiddleName;
            personDTO.SecondName = person.SecondName;
            personDTO.Position = person.Position;
            personDTO.Department = person.Department;
            personDTO.DateOfBirth = person.DateOfBirth;

            return personDTO;
        }

        public static void UpdatePersonDTO(this PersonDTO personDTO, PersonDTO newPersonDTO)
        {
            personDTO.Id = newPersonDTO.Id;
            personDTO.FirstName = newPersonDTO.FirstName;
            personDTO.MiddleName = newPersonDTO.MiddleName;
            personDTO.SecondName = newPersonDTO.SecondName;
            personDTO.Position = newPersonDTO.Position;
            personDTO.Department = newPersonDTO.Department;
            personDTO.DateOfBirth = newPersonDTO.DateOfBirth;
        }

        public static ObservableCollection<PersonDTO> GetPersonDTOCollection(this IEnumerable<Person> personList)
        {
            var personModelList = new ObservableCollection<PersonDTO>();
            foreach (var person in personList)
            {
                var personModel = person.GetPersonDTO();
                personModelList.Add(personModel);
            }

            return personModelList;
        }

        public static void SetRange(this ObservableCollection<PersonDTO> personDTOList, ObservableCollection<PersonDTO> newPersonDTOList)
        {
            personDTOList.Clear();
            foreach (var item in newPersonDTOList)
            {
                personDTOList.Add(item);
            }
        }
    }
}
