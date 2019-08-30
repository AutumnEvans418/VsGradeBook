namespace Grader.Tests
{
    public static class TaxProgram
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Enter the cost (ex 12.12)");
            var cost = System.Console.ReadLine();

            var costNum = double.Parse(cost);

            System.Console.WriteLine("Enter the tax percent (ex. 10)");
            var tax = System.Console.ReadLine();

            var taxNum = double.Parse(tax);

            System.Console.WriteLine($"Original Cost: " + costNum);
            System.Console.WriteLine($"Tax Cost: " + costNum * taxNum);
            System.Console.WriteLine($"Final Cost: " + costNum + (costNum * taxNum));

        }
    }
}