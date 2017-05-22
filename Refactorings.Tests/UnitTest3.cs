using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;

namespace Refactorings.Tests
{
    [TestFixture]
    public class UnitTest3
    {
        public void Test102()
        {

        }
    }

    public class PersonRepository
    {
        public IList<IPersonDto> GetAll()
        {
            var list = new List<IPersonDto>();
            using (IDataReader reader = GetReader())
            {
                while (reader.Read())
                {
                    var person = new PersonDto();
                    person.Name = reader.GetString(0);
                    person.Age = reader.GetInt32(1);

                    list.Add(person);
                }
            }
            return list;
        }

        public IDataReader GetReader()
        {
            throw new NotImplementedException();
        }
    }

    public class PersonRepository2
    {
        public IList<IPersonDto> GetAll()
        {
            var list = new List<IPersonDto>();
            using (IDataReader reader = GetReader())
            {
                while (reader.Read())
                {
                    list.Add(ToDto(reader));
                }
            }
            return list;
        }

        public IList<IPersonDto> GetAll2()
        {
            var list = new List<IPersonDto>();

            Template(GetReader, r => list.Add(ToDto(r)));

            return list;
        }

        public void Template(Func<IDataReader> getReader, Action<IDataRecord> action)
        {
            using (IDataReader reader = getReader())
            {
                while (reader.Read())
                {
                    action(reader);
                }
            }
        }

        public IPersonDto ToDto(IDataRecord record)
        {
            return new PersonDto()
            {
                Name = record.GetString(0),
                Age = record.GetInt32(1)
            };
        }

        public IDataReader GetReader()
        {
            throw new NotImplementedException();
        }
    }

    public interface IPersonDto
    {
        string Name { get; set; }
        int Age { get; set; }
    }

    public class PersonDto : IPersonDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
