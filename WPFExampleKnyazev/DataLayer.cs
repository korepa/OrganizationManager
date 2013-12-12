using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Transactions;

namespace WPFExampleKnyazev
{
    public class DataLayer
    {
        // get all persons in organization
        internal IEnumerable<PersonModel> GetAllPersons()
        {
            using (var entities = new ORGANIZATIONEntities())
            {
                var personModelList = getPersonModelList(entities.People.ToList());
                return personModelList;
            }
        }

        // add new person to organization
        internal bool AddPerson(PersonModel personModel)
        {
            // if new person is null
            if (personModel == null) return false;

            using (var entities = new ORGANIZATIONEntities())
            {
                // create transaction scope to make full transaction
                using (var transaction = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    try
                    {
                        // add new person from parameters
                        var person = setPerson(personModel);
                        entities.AddToPeople(person);
                        entities.SaveChanges(SaveOptions.DetectChangesBeforeSave);

                        transaction.Complete();

                        if (person.Id > 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // error while add person
                        // whrow exception again
                        throw;
                    }

                    return false;
                }
            }
        }

        // delete existed person
        internal bool DeletePerson(int id)
        {
            using (var entities = new ORGANIZATIONEntities())
            {
                try
                {
                    // select deleted person and delete it
                    if (entities.People.Any(pers => pers.Id == id))
                    {
                        var personToDelete = entities.People.Where(pers => pers.Id == id).First();
                        entities.People.DeleteObject(personToDelete);
                        entities.SaveChanges(SaveOptions.DetectChangesBeforeSave);

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    // error while delete person
                    // whrow exception again
                    throw;
                }

                return false;
            }
        }

        // edit existed person
        internal bool EditPerson(int id, PersonModel personModel)
        {
            using (var entities = new ORGANIZATIONEntities())
            {
                try
                {
                    // select person with specified id
                    if (entities.People.Any(pers => pers.Id == id))
                    {
                        var personToEdit = entities.People.Where(pers => pers.Id == id).First();

                        // edit existed person
                        personToEdit.FirstName = personModel.FirstName;
                        personToEdit.MiddleName = personModel.MiddleName;
                        personToEdit.SecondName = personModel.SecondName;
                        personToEdit.Position = personModel.Position;
                        personToEdit.Department = personModel.Department;
                        personToEdit.DateOfBirth = personModel.DateOfBirth;

                        entities.SaveChanges(SaveOptions.DetectChangesBeforeSave);

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    // whrow exception again
                    throw;
                }

                return false;
            }
        }

        // edit existed person
        internal IEnumerable<PersonModel> SearchPerson(Dictionary<string, object> searchDictionary)
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

                    var list = getPersonModelList(query);
                    return list;

                }
                catch (Exception ex)
                {
                    // whrow exception again
                    throw;
                }
            }
        }

        // private function for convert entity framework class Person to class PersonModel
        private IEnumerable<PersonModel> getPersonModelList(IEnumerable<Person> personList)
        {
            var personModelList = new List<PersonModel>();
            var counter = 0;

            foreach (var person in personList)
            {
                var personModel = new PersonModel();
                personModel.Id = person.Id;
                personModel.InnerId = ++counter;
                personModel.FirstName = person.FirstName;
                personModel.MiddleName = person.MiddleName;
                personModel.SecondName = person.SecondName;
                personModel.Position = person.Position;
                personModel.Department = person.Department;
                personModel.DateOfBirth = person.DateOfBirth;

                personModelList.Add(personModel);
            }

            return personModelList;
        }

        // private function for convert class PersonModel to entity framework class Person
        private Person setPerson(PersonModel personModel)
        {
            var person = new Person();
            person.FirstName = personModel.FirstName;
            person.MiddleName = personModel.MiddleName;
            person.SecondName = personModel.SecondName;
            person.Position = personModel.Position;
            person.Department = personModel.Department;
            person.DateOfBirth = personModel.DateOfBirth;

            return person;
        }
    }
}
