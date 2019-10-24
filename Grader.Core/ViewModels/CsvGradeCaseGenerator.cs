using System;
using System.Collections.Generic;
using System.Linq;
using Grader;
using Console = System.Console;

namespace AsyncToolWindowSample.ToolWindows
{
    public class CsvGradeCaseGenerator
    {

        public static List<IGradeCase> ConvertToGradeCases(string[] outputs, string[] inputs)
        {
            var gradeCases = new List<IGradeCase>();
            var possibleException = new ParserException();
            var highest = new[] { outputs.Length, inputs.Length }.Max();

            for (var index = 0; index < highest; index++)
            {
                var output = "";
                var input = "";
                if (index < inputs.Length)
                {
                    input = inputs[index];
                }

                if (index < outputs.Length)
                {
                    output = outputs[index];
                }

                try
                {
                    gradeCases.Add(new GradeCase(input, output, index + 1));
                }
                catch (ParserException e)
                {
                    Console.WriteLine(e);
                    possibleException.Exceptions.Add(e);
                }

            }

            if (gradeCases.Any() != true)
            {
                try
                {
                    gradeCases.Add(new GradeCase(1));
                }
                catch (ParserException e)
                {
                    Console.WriteLine(e);
                    possibleException.Exceptions.Add(e);
                }
            }

            if (possibleException.Exceptions.Any())
            {
                throw possibleException;
            }

            return gradeCases;
        }

        public static List<IGradeCase> ConvertTextToGradeCases(string csvCases, string csvExpectedOutput)
        {
            var inputs =
                csvCases?.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) ??
                new string[0];
            var outputs =
                csvExpectedOutput?.Split(new string[] { Environment.NewLine },
                    StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            var gradeCases = ConvertToGradeCases(outputs, inputs);
            return gradeCases;
        }
    }
}