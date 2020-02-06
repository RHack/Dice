using System;
using System.Text.RegularExpressions;

namespace Dice
{
    class Program
    {
        // Main function that handles state based on user input.
        static void Main(string[] args)
        {
            Console.WriteLine("Press ENTER without a roll to quit.\n");
            int[] processedInput;
            string results;
            int processedInputLength;
            string input;

            while (true)
            {
                Console.WriteLine("Enter a roll (Ex: 4d8+2): ");
                input = Console.ReadLine();

                if(input == String.Empty)
                {
                    break;
                }

                processedInput = ProcessInput(input);
                processedInputLength = processedInput.Length;

                if(processedInputLength < 2 || processedInputLength > 3)
                {
                    results = "Please enter a valid dice roll.\n";
                }
                else
                {
                    results = CalculateResults(processedInput);
                }

                PrintRoll(results);
            }
        }

        // Function for parsing user input.
        // Accepts a roll string.
        // Returns an array of individual results.
        public static int[] ProcessInput(String input)
        {
            input = Regex.Replace(input, "[^0-9d+-]", string.Empty);
            string[] roll = input.Split('d', '+', '-');

            int rollLength = roll.Length;
            int modifierPosition = rollLength - 1;
            int[] rollNumbers = new int[rollLength];

            if (rollLength > 1 && rollLength < 4)
            {
                if (input.Contains('-'))
                {
                    roll[modifierPosition] = "-" + roll[modifierPosition];
                }

                for (int rollElement = 0; rollElement < rollLength; rollElement++)
                {
                    rollNumbers[rollElement] = Convert.ToInt16(roll[rollElement]);
                }
            }

            return rollNumbers;
        }

        // Calculates the result of all dice rolls and appends them to a string.
        // Accepts individual roll results as an array of integers.
        // Returns the text of results and the final total as a string.
        public static String CalculateResults(int[] roll)
        {
            int quantity = roll[0], faces = roll[1];
            Tuple<string, int> rollResult = Roll(quantity, faces);
            string individualRolls = rollResult.Item1;
            int rollTotal = rollResult.Item2;

            String printResult = $"Rolling {quantity}d{faces} ";

            if (roll.Length > 2)
            {
                String modifierSign = "+";
                int modifier = roll[2];
                if (modifier < 0)
                {
                    modifierSign = "-";
                }

                rollTotal += modifier;
                printResult += $"{modifierSign} {Math.Abs(modifier)} ";
            }

            printResult += $"resulted in {rollTotal}\nIndividual Rolls: {individualRolls}\n";

            return printResult;
        }

        // Function responsible for rolling all the dice.
        // Accepts the number of dice and their face count.
        // Returns the individual result of each die as tuple with a string
        //  of roll results and the total as an integer.
        public static Tuple<string, int> Roll(int quantity, int faces)
        {
            Random diceSeed = new Random();
            int sum = 0;
            string rollText = "";
            faces += 1;

            for (int rolls = 0; rolls < quantity; rolls++)
            {
                var result = diceSeed.Next(1, faces);
                rollText += $"{result} ";
                sum += result;
            }

            return Tuple.Create(rollText, sum);
        }

        // Function that prints the result of the roll.
        // Accepts a list of roll elements as input.
        // Prints the result without returning anything. 
        public static void PrintRoll(string roll)
        {
            Console.WriteLine(roll);
        }
    }
}
