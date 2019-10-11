# Visual Studio GradeBook
Simplyifing grading code in visual studio.  No more sending compressed files, downlowding, and opening student's code.

## Features

- Teachers can create master programs to compare to students
- Teachers can create inputs for their program that will be tested against the student's program.  
- Teachers can then create assignments for students to complete
- When students create the program, they'll be able to test it within visual studio with the inputs the teacher created.  This inputs can be made invisible or visible.
- Students will get a percent of passing cases based on the inputs the teacher provided
- Once the students are finished with their program, 
  they can submit the program within visual studio
- You can use [VsGradeBook.com]("www.VsGradeBook.com") to see the APIs
  and options to submit the code

## Integrations

- Submit with GitHub
- Test with Unit Tests
- OAuth for authentication
- Integrate assignments with Canvas

## Api

### Authentication (People)

- POST api/auth
- PUT api/auth

### Assignments

- GET api/project?classId&studentId
- GET api/project/projectId
- GET api/submission/classId
- POST api/submission
- PUT api/submission/submissionId

## Packages/Tools Used

### HttpClient/Api packages
- NSwag.MSBuild
- NSwagStudio
- Swashbuckle

### Database
- Sql server
- EntityFrameworkCore

### Dependency Service
- Unity

### UI
- Xaml
- WPF
- Visual Studio ToolWindow

### Compiler tools
- Microsoft.CodeAnalysis.Analyzers
- Microsoft.CodeAnalysis.CSharp
- Mono.Cecil
- Microsoft.Net.Compilers

### Testing
- Autofixture
- FluentAssertions
- NUnit

## Helpful Resources

- [Developing Visual Studio Extensions](https://michaelscodingspot.com/visual-studio-2017-extension-development-tutorial-part-1/)
- [Analyzing and changing the c# programs](https://github.com/dotnet/roslyn/wiki/Getting-Started-C%23-Syntax-Transformation)
- [In memory compilation](https://josephwoodward.co.uk/2016/12/in-memory-c-sharp-compilation-using-roslyn)

## Example

### Longest Word

Have the function LongestWord(sen) take the sen parameter being passed and return the largest word in the string. If there are two or more words that are the same length, return the first word from the string with that length. Ignore punctuation and assume sen will not be empty.

Inputs:
test method
super long something
I love dogs
fun& time

Expected Outputs:
method
something
love
time

```
using System;
using System.Linq;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var item = Console.ReadLine();

            var words = item.Split(" ");

            var longestWord = words
                .FirstOrDefault(p => p.ToList().Count == words.Max(r => r.Length));

            Console.WriteLine(longestWord);
        }
    }
}

```

[Source](https://www.coderbyte.com/editor/Longest%20Word:Csharp)