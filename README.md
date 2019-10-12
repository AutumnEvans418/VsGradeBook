# Visual Studio GradeBook
Simplyifing grading code in visual studio.  No more sending compressed files, downlowding, and opening student's code.

## Features

- Teachers can create a project for students to complete
  - Teachers can create inputs for their program that will be tested against the student's program.
- When students create the program, they'll be able to test it within visual studio with the inputs the teacher created.  These inputs can be made invisible or visible.
- Students will get a percent of passing cases based on the inputs the teacher provided
- Once the students are finished with their program, 
  they can submit the program within visual studio
- Teachers can then see the submissions from students within visual studio and be able to open the student's submissions from within visual studio.  Teachers then can review the code and see their output and percent passed.
- You can use [VsGradeBook.com](https://www.VsGradeBook.com) to see the APIs
  and options to submit the code

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

- Roslyn Compiler
  - [Analyzing and changing the c# programs](https://github.com/dotnet/roslyn/wiki/Getting-Started-C%23-Syntax-Transformation)
  - [In memory compilation](https://josephwoodward.co.uk/2016/12/in-memory-c-sharp-compilation-using-roslyn)
  - [How to use the CSharp Syntax Rewriter](https://johnkoerner.com/csharp/using-a-csharp-syntax-rewriter/)
  - [Removing Empty Statements with the CSharp Syntax Rewriter](https://joshvarty.com/2014/08/15/learn-roslyn-now-part-5-csharpsyntaxrewriter/)
- VSIX
  - [Visual Studio Extension Samples](https://github.com/Microsoft/VSSDK-Extensibility-Samples)
  - [VSIX Extension Docs](https://docs.microsoft.com/en-us/visualstudio/extensibility/starting-to-develop-visual-studio-extensions?view=vs-2019)
  - [Example of code to add themes to VSIX](https://github.com/madskristensen/KnownMonikersExplorer/blob/master/src/ToolWindows/VsTheme.cs)
  - [Ask Questions about VSIX here](https://gitter.im/Microsoft/extendvs)
  - [Developing Visual Studio Extensions](https://michaelscodingspot.com/visual-studio-2017-extension-development-tutorial-part-1/)

## Example

### Longest Word

Have the function LongestWord(sen) take the sen parameter being passed and return the largest word in the string. If there are two or more words that are the same length, return the first word from the string with that length. Ignore punctuation and assume sen will not be empty.

**Inputs:**  
test method  
super long something  
I love dogs  
fun& time  

**Expected Outputs:**  
method  
something  
love  
time  

```
using System;
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
   
}

```



[Source](https://www.coderbyte.com/editor/Longest%20Word:Csharp)

#### Implementation
Home View/Start Screen
![Home View](Images/LongestWordHomeView.png)
Create a new project view
![Project View](Images/LongestWordProjectView.png)
