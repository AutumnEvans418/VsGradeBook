using System.Reflection.Metadata;
using Grader;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Grade()
        {
            var grader = new ConsoleAppGrader();

           var result =  grader.Grade("");
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}