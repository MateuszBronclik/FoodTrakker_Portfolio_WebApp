﻿using System;
using System.Collections.Generic;
using FoodTrakker.BusinessLogic;
using FoodTrakker.BusinessLogic.Models;
using FoodTrakker.BusinessLogic.Repository;
using System.Threading;
using System.Globalization;
using System.Linq;


namespace FoodTrakker.GUI.ConsoleInput
{
    internal class AddingEvent
    {

        public static void AddNewEvent()
        {
            Event newEvent = new Event();

            Console.Clear();
            Console.WriteLine("Please specify the name of the Event you wish to add:\n");
            newEvent.Name = Console.ReadLine();

            Console.Clear();
            Console.WriteLine($"\nPlease describe {newEvent.Name} in few sentences.\n");
            newEvent.Description = Console.ReadLine();

            Console.Clear();
            Console.WriteLine($"\nEnter the location of the {newEvent.Name}:"); 
            newEvent.Location = Console.ReadLine();

            Console.Clear();
            Console.WriteLine($"\nWhen does the {newEvent.Name} start?\n");
            var date = Console.ReadLine();
            DateTime startDateTime;
            bool isInputDate = DateTime.TryParse(date, out startDateTime);
            while (!isInputDate)
            {
                Console.WriteLine("You type wrong date.");
                date = Console.ReadLine();
                isInputDate = DateTime.TryParse(date, out startDateTime);
            }

            newEvent.StartDate = startDateTime; 

            Console.Clear();
            Console.WriteLine($"\nWhen does it end?\n");
            var endDate = Console.ReadLine();
            DateTime endDateTime;
            bool isDate = DateTime.TryParse(endDate, out endDateTime);
            while (!isDate)
            {
                Console.WriteLine("You type wrong date.");
                date = Console.ReadLine();
                isDate = DateTime.TryParse(endDate, out endDateTime);
            }
            

            newEvent.EndDate = endDateTime;
            
            var message =
                ($"The event is {newEvent.Name}. \nIt will take place in {newEvent.Location}." +
                 $"\n\nHere's a little description of what it has to offer: \n\t{newEvent.Description}. " +
                 $"\nIt starts on {newEvent.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)} and ends " +
                 $"{newEvent.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}.");
            
            Console.Clear();
            Console.WriteLine("Would you like do add a FoodTruck to his event (Y/N)? ");
            string truckToEventDecision = Console.ReadLine().ToLower();
            if (truckToEventDecision == "y")
            {
                var foodTrucks = DataRepository<FoodTruck>.GetData();
                Console.Clear();
                foreach (var foodTruck in foodTrucks)
                {
                   var foodTruckMessage =
                        $"Id: {foodTruck.Id} Name: {foodTruck.Name}, Type {foodTruck.Type.Name}, Located: {foodTruck.Location.City}, Street: {foodTruck.Location.Street}";
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{foodTruckMessage}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                int truckId;
                Console.WriteLine("Enter the id of FoodTruck.");
                var addingTruckToEvent = Console.ReadLine();
                bool isInptInt = int.TryParse(addingTruckToEvent, out truckId);
                while (!isInptInt)
                {
                    Console.WriteLine("It isn't a number.");
                    addingTruckToEvent = Console.ReadLine();
                    isInptInt = int.TryParse(addingTruckToEvent, out truckId);
                }
                var foodTruckList = DataRepository<FoodTruck>.GetData();
                var foodTruckToAdd = foodTruckList.FirstOrDefault(f => f.Id == truckId);
                newEvent.FoodTrucks.Add(foodTruckToAdd);
            }
            else
            {
                DataRepository<Event>.AddElement(newEvent);
                MainMenu.Create();
                
            }

            

            Console.WriteLine(message);
            Thread.Sleep(2500);
            DataRepository<Event>.AddElement(newEvent);

            Console.Clear();
            Console.WriteLine("Do you want to add another event(Y/N)?");
            string decision = Console.ReadLine().ToLower();
            if (decision == "y")
            {
                AddNewEvent();
            }
            else
            {
                MainMenu.Create();
            }
        }
    }
}
