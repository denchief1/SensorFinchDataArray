using System;
using FinchAPI;
using ConsoleApp;
using System.ComponentModel;


namespace SensorFinchDataArray
{
    class Program
    {
        static void Main(string[] args)
        {
            Display.DisplayWelcomeScreen("Finch App","Welcome to the Finch as a sensor app");
            DisplayMenu();
            Display.DisplayClosingScreen("Press any key to exit");
        }

        static void DisplayMenu()
        {
            bool loopRunning = true;
            string system = "celsius";
            double[] temperatures;
            string menuChoice;
            int numberOfReadings = 0;
            double secondsBetweenReadings = 0;

            while (loopRunning)
            {
                Console.Clear();
                Display.Header("Menu");
                Console.WriteLine("\t1) Set Number of Data Points");
                Console.WriteLine("\t2) Set Data point Interval");
                Console.WriteLine("\t3) Aquire Data");
                Console.WriteLine("\t4) Select Celsius or fahrenheit");
                Console.WriteLine("\tE) Exit");
                Console.WriteLine();
                Console.Write("Please enter your the number of your choice: ");
                menuChoice = Console.ReadLine().ToLower();

                switch (menuChoice)
                {
                    case "1":
                        numberOfReadings = DisplayGetNumberOfReadings();
                        temperatures = new double[numberOfReadings];
                        break;

                    case "2":
                        secondsBetweenReadings = DisplayGetInterval();
                        break;

                    case "3":
                        temperatures = new double[numberOfReadings];
                        temperatures = DisplayAcquireDataSet(temperatures, numberOfReadings, secondsBetweenReadings);
                        if (system == "fahrenhiet")
                        {
                            temperatures = convertCelsiusToFahrenheit(temperatures);
                            for (int i = 0; i < numberOfReadings; i++)
                            {
                                Console.WriteLine($"Temperature reading {i + 1} in Fahrenheit is: {temperatures[i]} degrees.");
                            }
                        }
                        else if(system == "celsius")
                        {
                            for (int i = 0; i < numberOfReadings; i++)
                            {
                                Console.WriteLine($"Temperature reading {i + 1} in Celsius is: {temperatures[i]} degrees.");
                            }
                        }
                        Display.DisplayContinuePrompt();
                        break;

                    case "4":
                        system = CelsiusOrFahrenhiet();
                        break;

                    case "e":
                    case "E":
                        loopRunning = false;
                        break;

                    default:
                        break;
                }
            }
        }

        static int DisplayGetNumberOfReadings()
        {
            int numberOfReadings;

            Display.Header("Number of Readings");
 


            return numberOfReadings = Validate(typeof(int),"enter number of readings: ");


        }

        static double DisplayGetInterval() {
            double interval;

            Display.Header("Frequency of Intervals");



            return interval = Validate(typeof(double), "Enter the time between readings: ");


        }

        static double[] DisplayAcquireDataSet(double[] temperatures, int numberOfDataPoints, double secondsBetweenReadings) {
            Finch finch = new Finch();
            finch.connect();

            Display.Header("Aquire Data Set");

            Console.WriteLine("The Finch Robot will now get temperature data.");
            Display.DisplayContinuePrompt();
            for (int i = 0; i < numberOfDataPoints; i++)
            {
                temperatures[i] = finch.getTemperature();
                Console.WriteLine($"Temperature Reading {i + 1}: {temperatures[i]}");
                finch.wait((int)(secondsBetweenReadings * 1000));
            }


            Display.DisplayContinuePrompt();
            finch.disConnect();
            return temperatures;
        }

        static double[] convertCelsiusToFahrenheit(double[] temperatures) {

            for (int i = 0; i < temperatures.Length; i++)
            {
                temperatures[i] = (((9 * temperatures[i]) /5 ) + 32);
            }

            return temperatures;
        }

        static string CelsiusOrFahrenhiet() {
            string system = "1";
            string userResponse;
            bool success = false;


            Display.Header("Fahrenhiet or Celsius");
            

            do
            {
                Console.WriteLine("Would you like the temperature readings in Fahrenhiet or Celsius: ");
                userResponse = Console.ReadLine().ToLower();
                if (userResponse == "fahrenhiet")
                {
                    system = "fahrenhiet";
                    success = true;
                }
                else if(userResponse == "celsius")
                {
                    system = "celcius";
                    success = true;
                }
                else
                {
                    success = false;
                }
            } while (success == false);

            return system;
        }

        static dynamic Validate(Type targetType, string input)
        {
            bool success;
            string userResponse;
            dynamic values = 0;

            do
            {
                Console.Write(input);
                userResponse = Console.ReadLine();
                try
                {
                    values = TypeDescriptor.GetConverter(targetType).ConvertFromString(userResponse);
                    success = true;
                    

                }
                catch
                {
                    success = false;
                   
                }

            } while (!success);

            return values;

        }
    }
}
