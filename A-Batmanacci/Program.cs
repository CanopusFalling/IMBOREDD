using System;

using System.Collections.Generic;
using System.Numerics;

namespace A_Batmanacci
{
    class Program
    {
        static void Main(string[] args)
        {
            // Code used for testing out the sequence and alike.
            //Console.WriteLine(arrayToString(getBatmanacci(1, 16)));
            /*BigInteger[] result = fibonacci(7);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }*/

            String[] testData = getBatmanacci(1, 20);
            for (int i = 0; i < testData.Length; i++)
            {
                String testString = testData[i];
                Char[] testChars = testString.ToCharArray();
                for (int j = 0; j < testChars.Length; j++)
                {
                    Char currentChar = testChars[j];
                    Char comparisonChar = getBatmanacciCharAt(i+1, j+1);

                    if(currentChar != comparisonChar){
                        Console.WriteLine(i + " : " + j + " : " + currentChar + " : " + comparisonChar);
                    }else{
                        Console.Write('-');
                    }
                }
            }

            // Get user input.
            int[] numbers = getNumbers();

            // Find letter at position specified.
            Console.WriteLine(getBatmanacciCharAt(numbers[0], numbers[1]));
        }

        // ===== Find Char in Batmanacci =====
        // Find a spesific char in a spesific term in the batmanacci sequence.
        static char getBatmanacciCharAt(int term, int charPos)
        {
            // Get the terms needed from the fibonacci sequence.
            BigInteger[] fibonacciTerms = fibonacci(term);
            // Convert charPosition to a big integer.
            BigInteger charPosition = new BigInteger(charPos);

            // Ajust term so it's now an index.
            term += -1;

            while (term > 1)
            {
                // Debugging script.
                // Console.WriteLine(term + " : " + charPosition);

                BigInteger term2Below = fibonacciTerms[term - 2];

                // Check if the char is in the term 2 below or the term 1 below.
                if (charPosition <= term2Below)
                {
                    term += -2;
                }
                else
                {
                    charPosition = BigInteger.Subtract(charPosition, term2Below);
                    term += -1;
                }
            }

            if (term == 0)
            {
                return 'N';
            }
            else
            {
                return 'A';
            }
        }

        // ===== Brute Force Batmanacci Sequence =====
        // Return an array of the sequence from a start term to a final term.
        static String[] getBatmanacci(int start, int end)
        {
            // Generate the start of the sequence and add the first two terms.
            List<String> sequence = new List<String>();
            sequence.Add("N");
            if (end != 1) { sequence.Add("A"); }

            while (sequence.Count < end)
            {
                // Find size of list.
                int sequenceLength = sequence.Count;

                // Find the last 2 terms of the sequence.
                String lastTerm = sequence[sequenceLength - 1];
                String secondLastTerm = sequence[sequenceLength - 2];

                // Add the next Batmanacci term.
                sequence.Add(getNextBatmanacci(secondLastTerm, lastTerm));
            }

            // Cut off the start elements that arent needed.
            sequence.GetRange(start - 1, end - 1);

            // Return the resulting sequence.
            return sequence.ToArray();
        }

        static String getNextBatmanacci(String term1, String term2)
        {
            return term1 + term2;
        }

        // ===== Fibonacci Calculations =====
        // Find all the terms up to the nth Fibonacci term.
        static BigInteger[] fibonacci(int n)
        {
            // Sequence list.
            List<BigInteger> sequence = new List<BigInteger>();
            // Add the first 2 terms to the sequence.
            sequence.Add(new BigInteger(1));
            if (n != 1) { sequence.Add(new BigInteger(1)); }

            // Loop untill the nth term is reached.
            while (n > sequence.Count)
            {
                int currentTerm = sequence.Count;
                // Find the last 2 terms of the sequence.
                BigInteger lastTerm = sequence[currentTerm - 1];
                BigInteger secondLastTerm = sequence[currentTerm - 2];
                // Add the new term to the sequence.
                sequence.Add(BigInteger.Add(secondLastTerm, lastTerm));
            }

            return sequence.ToArray();
        }

        // ===== Helper Functions =====
        // Convert array to a string.
        static String arrayToString(Object[] array)
        {
            // Set up the result string.
            String result = "[";

            // Add each item to the string.
            foreach (var item in array)
            {
                result += item + ", ";
            }

            // Clean up the end of the string.
            result = result.Substring(0, result.Length - 2);
            result += "]";

            // Return the result.
            return result;
        }

        // Get the user input and split it into 2 numbers.
        static int[] getNumbers()
        {
            String rawInput = Console.ReadLine();
            String[] separatedStrings = rawInput.Split(' ');

            int[] result = new int[2];

            // Catch argument count problems.
            if (separatedStrings.Length != 2)
            {
                throw new Exception("Expected 2 arguments, found " + separatedStrings.Length + ".");
            }

            for (int i = 0; i < result.Length; i++)
            {
                /*bool success = */int.TryParse(separatedStrings[i], out result[i]);
                /*if (!success)
                {
                    throw new Exception("Invalid argument at position: " + separatedStrings.Length + ".");
                }*/
            }

            return result;
        }
    }
}
