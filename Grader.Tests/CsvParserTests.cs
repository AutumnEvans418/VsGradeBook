using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Grader.Tests
{
    public class CsvParserTests
    {
        [Test]
        public void Test()
        {
          var parser = new CsvParser();

          var result = parser.Parse("test,\"hello, world\", asdf\r\ntest");

          result.Count.Should().Be(2);

          result.First().Count.Should().Be(3);
        }

        [Test]
        public void TestLines()
        {
            var parser = new CsvParser();

            var result = parser.Parse("t\r\n2\r\n3");

            result.Count.Should().Be(3);

            result.First().Count.Should().Be(1);
        }

        [Test]
        public void TestOne()
        {
            var parser = new CsvParser();

            var result = parser.Parse("test,\"hello, world\", asdf");

            result.Count.Should().Be(1);

            result.First().Count.Should().Be(3);
        }
    }
}