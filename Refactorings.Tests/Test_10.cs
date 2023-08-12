using NSubstitute;
using NUnit.Framework;
using System.Collections;

namespace Refactorings.TestClasses.Business
{
    [TestFixture]
    public class Test_10 //Replace_Inheritance_with_Composition
    {
        [Test]
        public void GivenStack_BeforeWithItem_WhenPop_ItemIsReturned()
        {
            var stack = new Stack_Before();
            stack.Push(1);

            var result = stack.Pop();

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GivenStack_AfterWithItem_WhenPop_ItemIsReturned()
        {
            var stack = new Stack_After();
            stack.Push(1);

            var result = stack.Pop();

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GivenPerson_Before_WhenSave_PersonShouldBeStoredInTheDatabase() //Integration test?
        {
            var person = new Person_Before();
            person.Save();

            //Assert what?
        }

        [Test]
        public void GivenPerson_After_WhenSave_RepositoryIsCalled() //Integration test for entity repository elswhere
        {
            var personRepository = Substitute.For<EntityRepository>();
            var person = new Person_After(personRepository);

            person.Save();

            personRepository.Received(1).Store(person);
        }
    }

    #region Example 1

    /*
        Class is leaking unrelated operations such as RemoveAt.
        Inheritance is britle.
        Implementational details are showing.
    */
    public class Stack_Before : ArrayList
    {
        public void Push(object value)
        {
            Add(value);
        }
        public object Pop()
        {
            var lastIndex = Count - 1;
            var last = this[lastIndex];
            RemoveAt(lastIndex);

            return last;
        }
    }

    /*
        Only Stack related operations are visible. 
        Inheritance tree is shortened. 
        ArrayList is internal implementational detail.
    */
    public class Stack_After
    {
        private readonly ArrayList _arrayList;

        public Stack_After()
        {
            _arrayList = new ArrayList();
        }
        
        public void Push(object value)
        {
            _arrayList.Add(value);
        }
        public object Pop()
        {
            var lastIndex = _arrayList.Count - 1;
            var last = _arrayList[lastIndex];
            _arrayList.Remove(lastIndex);

            return last;
        }
    }

    #endregion

    #region Example 2
    
    /*
        Class is leaking unrelated operations such as RemoveAt.
        Inheritance is britle.
        Implementational details are showing.
    */
    public class Person_Before : EntityRepository, IEntity
    {
        public void Save()
        {
            Store(this);
        }
    }

    /*
        Only Person related operations are visible. 
        Inheritance tree is shortened.
        Dependencies can be injected/changed.
    */

    public class Person_After : IEntity
    {
        readonly EntityRepository _repository;
        public Person_After(EntityRepository repository)
        {
            _repository = repository;
        }

        public void Save()
        {
            _repository.Store(this);
        }
    }

    #endregion
}
