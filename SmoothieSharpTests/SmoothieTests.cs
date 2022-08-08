using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmoothieBar;

namespace SmoothieSharpTests
{
    [TestClass]
    public class SmoothieTests
    {
        [TestMethod]
        public void InvalidInitialization_Throws()
        {
            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => new Smoothie(new string[] { "Banana", "Cucumber" }));
        }
        [TestMethod]
        public void InvalidSetter_Throws()
        {
            Smoothie initSmoothie = new Smoothie(new string[] { "Strawberries", "Banana" });

            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => initSmoothie.Ingredients = new string[] { "Watermelon" });
        }
        [TestMethod]
        public void DecimalPlaces_Validation()
        {
            Smoothie initSmoothie = new Smoothie(new string[] { "Strawberries", "Banana" });
            string costString = initSmoothie.GetCost();
            string priceString = initSmoothie.GetPrice();
            string pattern = @"£.*\.\d{2}$";

            Assert.IsTrue(Regex.IsMatch(costString, pattern) && Regex.IsMatch(priceString, pattern));
        }
        [TestMethod]
        public void TypeName_Validation()
        {
            Smoothie singleSmoothie = new Smoothie(new string[] {"Banana" });
            Smoothie doubleSmoothie = new Smoothie(new string[] { "Strawberries", "Banana" });
            string[] singleCollection = singleSmoothie.GetName().Split(' ');
            string[] doubleCollection = doubleSmoothie.GetName().Split(' ');

            Assert.IsTrue(singleCollection.Last()=="Smoothie" && doubleCollection.Last()=="Fusion");
        }
    }
}

