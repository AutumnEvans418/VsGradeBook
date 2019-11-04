namespace Grader.Tests
{
    public class SourceCodeStrings
    {
        public const string helloWorldSrc = @"
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(""Hello World!"");
        }
    }
}";
        public const string helloWorldCalledTwiceSrc = @"
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(""Hello World!"");
            System.Console.WriteLine(""Hello, World 2.0!"");
        }
    }
}";
        public const string taxSystemSrc = @"
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Enter Price"");
            var price = double.Parse(Console.ReadLine());

            Console.WriteLine((price * 1.10));
        }
    }
}
";
        public const string taxSystemExceptionHandling = @"
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Enter Price"");
            var price = double.Parse(Console.ReadLine());
            if(price < 0){
                throw new Exception(""Invalid Price"");
            }
            Console.WriteLine((price * 1.10));
        }
    }
}
";

        public const string throwExceptionSrc = @"
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new Exception(""Test"");
        }
    }
}
";

        public const string longestWordProgram = @"
using System;
using System.Linq;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var item = Console.ReadLine();

            var words = item.Split(' ');

            var longestWord = words
                .FirstOrDefault(p => p.ToList().Count == words.Max(r => r.Length));

            Console.WriteLine(longestWord);
        }
    }
}";

        public const string longestWordProgramBestSolution = @"using System;
using System.Linq;

class MainClass {
  public static string LongestWord(string sen) { 
  
    string[] words = sen.Split(' ');


    return words.OrderByDescending( s => s.Length ).First();;
            
  }

  static void Main() {  
    // keep this function call here
    Console.WriteLine(LongestWord(Console.ReadLine()));
  } 
   
}";

        public const string emptyMain = @"class MainClass {static void Main(){}}";

        public const string taxSystem = @"
using System;

class MainClass
{
    

    static void Main()
    {
            Console.WriteLine(""Enter price"");
            var price = double.Parse(Console.ReadLine());
            Console.WriteLine(""Enter tax percent"");
            var tax = double.Parse(Console.ReadLine());

            Console.WriteLine($""Price: ${price}"");
            Console.WriteLine($""Tax Percent: {tax}%"");
            Console.WriteLine($""Tax Cost: ${tax / 100 * price}"");
            Console.WriteLine($""Total Cost: ${price + (tax / 100 * price)}"");

        }

    }
";
    }
}