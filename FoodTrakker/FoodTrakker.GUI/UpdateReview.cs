﻿using FoodTrakker.BusinessLogic;
using FoodTrakker.BusinessLogic.Models;
using FoodTrakker.BusinessLogic.Repository;
using System;
using System.Linq;
using System.Threading;

namespace FoodTrakker.GUI
{
    public static class UpdateReview
    {
        public static void ReviewUpdate()
        {
            int id;
            Console.WriteLine("Enter the id of Review.");
            var input = Console.ReadLine();
            bool isinputInt = int.TryParse(input, out id);
            while (!isinputInt)
            {
                Console.WriteLine("It isn't a number.");
                input = Console.ReadLine();
                isinputInt = int.TryParse(input, out id);
            }
            var reviewsList = DataRepository<Review>.GetData();
            var review = reviewsList.FirstOrDefault(r => r.Id == id);
            if (review == null)
            {
                Console.WriteLine("Your Review doesn't exist.Please choose AddReview.");
                Thread.Sleep(3000);
                MainMenu.Create();
            }
            else
            {
                Console.WriteLine("Write number which property want to change and press enter.");
                Console.WriteLine("Press 1 if you want to update the title.");
                Console.WriteLine("Press 2 if you want to update description.");
                Console.WriteLine("Press 3 if you want to update rating.");
                Console.WriteLine("Press Q when you do all changes to quit.");
                do
                {
                    var input2 = Console.ReadLine();
                    Quit(review, input2);
                    int inputAsInt;
                    bool isinputInt2 = int.TryParse(input2, out inputAsInt);
                    while (!isinputInt2)
                    {
                        Console.WriteLine("It isn't a number. Please enter value from 1 to 3, or Q to quit");
                        input2 = Console.ReadLine();
                        isinputInt2 = int.TryParse(input2, out inputAsInt);
                        if (input2 == "q" || input2 == "Q")
                        {
                            Quit(review, input2);
                        }
                    }
                    while (inputAsInt != 1 && inputAsInt != 2 && inputAsInt != 3)
                    {
                        Console.WriteLine("You type wrong value.Please select from 1 to 3, or Q to quit.");
                        input2 = Console.ReadLine();
                        isinputInt2 = int.TryParse(input2, out inputAsInt);
                        if (input2 == "q" || input2 == "Q")
                        {
                            Quit(review, input2);
                        }
                    }
                    if (inputAsInt == 1)
                    {
                        review.Title = Console.ReadLine();
                    }
                    if (inputAsInt == 2)
                    {
                        review.Description = Console.ReadLine();
                    }
                    if (inputAsInt == 3)
                    {
                        var checkInput = Console.ReadLine();
                        int intInput;
                        bool isInputInt = int.TryParse(checkInput, out intInput);
                        while (!isInputInt)
                        {
                            Console.WriteLine("It isn't a number. Please enter value from 1 to 10.");
                            checkInput = Console.ReadLine();
                            isInputInt = int.TryParse(checkInput, out intInput);
                            review.Rating = intInput;
                        }
                        review.Rating = intInput;
                    }

                }
                while (true);
                
           }
        }

        public static void Quit(Review review, string input2)
        {
            if (input2 == "q" || input2 == "Q")
            {
                Console.WriteLine("This is your updated Review: ");
                Console.WriteLine($"{review.Title}");
                Console.WriteLine($"{review.Description}");
                Console.WriteLine($"{review.Rating}");
                Thread.Sleep(5000);
                MainMenu.Create();
            }
        }
    }

}
