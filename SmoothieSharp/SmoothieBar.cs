using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmoothieBar
{
    public class Smoothie
    {
        private readonly Dictionary<string, string> ingMenu = new Dictionary<string, string>() {
            {"Strawberries", "£1.50"},
            {"Banana", "£0.50"},
            {"Mango", "£2.50"},
            {"Blueberries", "£1.00"},
            {"Raspberries","£1.00"},
            {"Apple", "£1.75"},
            {"Pineapple","£3.50"}
        };
        private string[] smoothieIngs;
        public Smoothie(string[] ingredientList)
        {
            if (ingredientList.All<string>(mem => ingMenu.ContainsKey(mem)))
            {
                smoothieIngs = ingredientList;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
        public string[] Ingredients
        {
            get => smoothieIngs;
            set
            {
                if (value.All<string>(mem => ingMenu.ContainsKey(mem)))
                {
                    smoothieIngs = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
        public Dictionary<string, string> Menu
        {
            get => ingMenu;
        }
        public string GetCost()
        {
            double total = 0;
            foreach (string item in this.Ingredients)
            {
                double itemCost = double.Parse(this.Menu[item].Replace("£", ""), CultureInfo.InvariantCulture);
                total += itemCost;
            };
            string currencyTotal = total.ToString("F2", CultureInfo.InvariantCulture).Insert(0, "£");
            return currencyTotal;
        }
        public string GetPrice()
        {
            double priceTotal = double.Parse(this.GetCost().Replace("£", ""), CultureInfo.InvariantCulture) * 1.5;
            string priceStr = priceTotal.ToString("F2", CultureInfo.InvariantCulture).Insert(0, "£");
            return priceStr;
        }
        public string GetName()
        {
            string ingStr = Regex.Replace(String.Join(" ", this.Ingredients.OrderBy(ing => ing)), "ies", "y");
            string typeStr = this.Ingredients.Length > 1 ? "Fusion" : "Smoothie";
            string resultName = ingStr + " " + typeStr;
            return resultName;
        }
        static void Main()
        {
            Smoothie myFirstSmoothie = new Smoothie(new string[] { "Banana", "Strawberries" });
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine($"{myFirstSmoothie.GetName()} costs {myFirstSmoothie.GetCost()}");
            Console.WriteLine($"{myFirstSmoothie.GetName()}'s price is {myFirstSmoothie.GetPrice()}");
            Console.WriteLine();
            Smoothie mySecondSmoothie = new Smoothie(new string[] { "Apple" });
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine($"{mySecondSmoothie.GetName()} costs {mySecondSmoothie.GetCost()}");
            Console.WriteLine($"{mySecondSmoothie.GetName()}'s price is {mySecondSmoothie.GetPrice()}");
        }
    }
}