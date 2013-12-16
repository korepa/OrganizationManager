using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace WPFExampleKnyazev
{
    public class DataAccess
    {
        // main collection
        ObservableCollection<PersonDTO> personDTO = new ObservableCollection<PersonDTO>();
        public ObservableCollection<PersonDTO> PersonDTO { get { return personDTO; } }

        // search collection
        ObservableCollection<PersonDTO> personDTOSearch = new ObservableCollection<PersonDTO>();
        public ObservableCollection<PersonDTO> PersonDTOSearch { get { return personDTOSearch; } }

        public DataAccess()
        {
            personDTO = new ObservableCollection<PersonDTO>();
            personDTOSearch = new ObservableCollection<PersonDTO>();
        }

        // get all persons in organization
        internal void Initialize()
        {
            using (var entities = new ORGANIZATIONEntities())
            {
                personDTO.SetRange(entities.People.ToList().GetPersonDTOCollection());
            }
        }

        // add new person to organization
        internal void AddPerson(PersonDTO personDTOAdd)
        {
            // if new person is null
            if (personDTOAdd == null) throw new ArgumentNullException("personDTOAdd", "Переданный объект не указывает на конкретный экзампляр");

            using (var entities = new ORGANIZATIONEntities())
            {
                // create transaction scope to make full transaction
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    try
                    {
                        // add new person from parameters
                        var person = personDTOAdd.GetPerson();
                        entities.AddToPeople(person);
                        entities.SaveChanges(SaveOptions.DetectChangesBeforeSave);

                        personDTOAdd.Id = person.Id;
                        personDTO.Add(personDTOAdd);

                        transaction.Complete();
                    }
                    catch (Exception)
                    {
                        // error while add person
                        // throw exception again
                        throw;
                    }
                }
            }
        }

        // delete existed person
        internal void DeletePerson(PersonDTO personDTODelete)
        {
            using (var entities = new ORGANIZATIONEntities())
            {
                try
                {
                    // get personDTO id
                    var id = personDTODelete.Id;

                    // select deleted person and delete it
                    if (entities.People.Any(pers => pers.Id == id))
                    {
                        var personToDelete = entities.People.Where(pers => pers.Id == id).First();
                        entities.People.DeleteObject(personToDelete);
                        entities.SaveChanges(SaveOptions.DetectChangesBeforeSave);

                        personDTO.Remove(personDTODelete);
                    }
                    else
                    {
                        throw new ArgumentException("Не найден сотрудник с указанным идентификатором (Id)");
                    }

                }
                catch (Exception)
                {
                    // error while delete person
                    // throw exception again
                    throw;
                }
            }
        }

        // edit existed person
        internal void EditPerson(PersonDTO personDTOEdit)
        {
            using (var entities = new ORGANIZATIONEntities())
            {
                try
                {
                    // get personDTO id
                    var id = personDTOEdit.Id;

                    // select person with specified id
                    if (entities.People.Any(pers => pers.Id == id))
                    {
                        var personToEdit = entities.People.Where(pers => pers.Id == id).First();

                        // edit existed person in DB
                        personToEdit.UpdatePerson(personDTOEdit);
                        entities.SaveChanges(SaveOptions.DetectChangesBeforeSave);

                        // update existed personDTO
                        var personDTOEdited = personDTO.Where(pers => pers.Id == id).First();
                        personDTOEdited.UpdatePersonDTO(personDTOEdit);
                    }
                    else
                    {
                        throw new ArgumentException("Не найден сотрудник с указанным идентификатором (Id)");
                    }

                }
                catch (Exception)
                {
                    // throw exception again
                    throw;
                }
            }
        }

        // edit existed person
        internal async void SearchPerson(Dictionary<string, object> searchDictionary)
        {
            using (var entities = new ORGANIZATIONEntities())
            {
                try
                {
                    // main query
                    var query = from person in entities.People
                                select person;

                    // configure query of search parameter is set
                    if (searchDictionary.ContainsKey("FirstName"))
                    {
                        var parameter = (string)searchDictionary["FirstName"];
                        query = from person in query
                                where person.FirstName.Contains(parameter)
                                select person;
                    }
                    if (searchDictionary.ContainsKey("MiddleName"))
                    {
                        var parameter = (string)searchDictionary["MiddleName"];
                        query = from person in query
                                where person.MiddleName.Contains(parameter)
                                select person;
                    }
                    if (searchDictionary.ContainsKey("SecondName"))
                    {
                        var parameter = (string)searchDictionary["SecondName"];
                        query = from person in query
                                where person.SecondName.Contains(parameter)
                                select person;
                    }
                    if (searchDictionary.ContainsKey("Position"))
                    {
                        var parameter = (string)searchDictionary["Position"];
                        query = from person in query
                                where person.Position.Contains(parameter)
                                select person;
                    }
                    if (searchDictionary.ContainsKey("Department"))
                    {
                        var parameter = (string)searchDictionary["Department"];
                        query = from person in query
                                where person.Department.Contains(parameter)
                                select person;
                    }
                    if (searchDictionary.ContainsKey("DateOfBirth"))
                    {
                        var parameter = (DateTime)searchDictionary["DateOfBirth"];
                        query = from person in query
                                where person.DateOfBirth.Equals(parameter)
                                select person;
                    }

                    // execute query and set results to collection
                    var searchResults = new ObservableCollection<PersonDTO>();
                    //await Task.Run(() =>
                    //{
                    searchResults = query.GetPersonDTOCollection();
                    //});
                    personDTOSearch.SetRange(searchResults);
                }
                catch (Exception)
                {
                    // throw exception again
                    throw;
                }
            }
        }
    }
}
