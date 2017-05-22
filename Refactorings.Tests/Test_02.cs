using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Refactorings.TestClasses.Business
{
    [TestFixture]
    public class Test_02 // Form Template Method
    {
        [Test]
        public void GivenPersonRepository_Before_WhenGetAll_ListOfAllPersonDtosIsReturned()
        {
            var personRepository = new PersonRepository_Before();

            //we need a test database, this is an integration test
            var result = personRepository.GetAll();

            //Assert ???
        }

        [Test]
        public void GivenPersonRepository_After_WhenToDto_ConvertsRecordToPersonDto()
        {
            var personRepository = new PersonRepository_After();
            //move to setup method
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(new[] 
            {
                new DataColumn("Name", typeof(string)),
                new DataColumn("Age", typeof(int))
            });
            var personData = dataTable.NewRow();
            personData["Name"] = "A";
            personData["Age"] = 1;
            dataTable.Rows.Add(personData);

            using (var reader = dataTable.CreateDataReader())
            {
                reader.Read();
                var result = personRepository.ToDto(reader);

                Assert.AreEqual("A", result.Name);
                Assert.AreEqual(1, result.Age);
            }
        }
    }

    #region Example 1

    /*
        GetAll method is long.
        Method contains retrieval and conversion aspects.
        Executing reader code will be repeated many times. Any additions will need to be manually merged to each instance of the code (eg. logging)
        Conversion logic can not be tested in isolation.
    */

    public class PersonRepository_Before
    {
        public string ConnectionString { get; private set; }
        public string GetAllPersonsQuery { get; private set; }

        public IList<PersonDto> GetAll()
        {
            var _allPersons = new List<PersonDto>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = GetAllPersonsQuery;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var person = new PersonDto();
                            person.Name = reader.GetString(0);
                            person.Age = reader.GetInt32(1);

                            _allPersons.Add(person);
                        }
                        return _allPersons;
                    }
                }
            }
        }
    }

    /*
        ExecuteReaderTemplate can be reused and extended once for all usages (ie. adding logging, error handling etc. in one place)
        Conversion logic can be tested with no dependency to database.
    */
    public class PersonRepository_After
    {
        public string ConnectionString { get; private set; }
        public string GetAllPersonsQuery { get; private set; }

        public IList<PersonDto> GetAll()
        {
            var allPersons = new List<PersonDto>();

            ExecuteReaderTemplate(personRecord => allPersons.Add(ToDto(personRecord)));

            return allPersons;
        }

        public PersonDto ToDto(IDataRecord personRecord)
        {
            var person = new PersonDto();
            person.Name = personRecord.GetString(0);
            person.Age = personRecord.GetInt32(1);

            return person;
        }

        protected void ExecuteReaderTemplate(Action<IDataRecord> map)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = GetAllPersonsQuery;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            map(reader);
                        }
                    }
                }
            }
        }
    }

    #endregion
}

