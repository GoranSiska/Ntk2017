using System;
using NUnit.Framework;
using System.Collections;

namespace Refactorings.Tests
{
    [TestFixture]
    public class UnitTest2
    {
        public void Test102()
        {

        }
    }

    public class Stack : ArrayList
    {
    public void Push(Object value)
    {
        // 
    }
    public Object Pop()
    {
            //
            return null;
    }
}

public class Person : EntityRepository, IEntity
    {
        public Person()
        {}

        public void Save()
        {
            Store(this);
        }
    }

    public interface IEntity
    { }

    public class Person2 : IEntity
    {
        EntityRepository _repository;
        public Person2(EntityRepository repository)
        {
            _repository = repository;
        }

        public void Save()
        {
            _repository.Store(this);
        }
    }

    public class EntityRepository
    {
        public void Store(IEntity entity)
        {
            //store
        }
    }
}
