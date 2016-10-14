using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ShoppingList.Models;
using ShoppingList.Controllers;
using System.Web.Http.Results;

namespace ShoppingList.Test
{
    [TestClass]
    public class TestDrinksController
    {
        [TestMethod]
        public void GetDrink_ShouldReturnCorrectDrink()
        {

            string DrinkName = "Pepsi";
            var testDrinks = GetTestDrinks();
            DrinksController.drinks = GetTestDrinks();
            var controller = new DrinksController();
            var result = controller.GetDrink(DrinkName) as OkNegotiatedContentResult<Drink>;
            Assert.IsNotNull(result);
            Assert.AreEqual(testDrinks[3].Name, result.Content.Name);
        }


        [TestMethod]
        public void DeleteDrink_DeleteAndMatch()
        {

            string DrinkName = "Pepsi";
            var testDrinks = GetTestDrinks();
            DrinksController.drinks = GetTestDrinks();
            var controller = new DrinksController();
            var result = controller.Delete(DrinkName);
            testDrinks.RemoveAll((d) => d.Name == DrinkName);
            Assert.AreEqual(testDrinks.Count, DrinksController.drinks.Count);
            CollectionAssert.DoesNotContain(DrinksController.drinks, new Drink { Id = "4", Name = "Pepsi", Quantity = 3 });
        }


        [TestMethod]
        public void PostDrink_CheckExisting()
        {

            string DrinkName = "Sprite";
            var testDrinks = GetTestDrinks();
            DrinksController.drinks = GetTestDrinks();
            var controller = new DrinksController();
            var drink = new Drink { Name = DrinkName, Quantity = 3 };
            var result = controller.PostDrink(drink);
            CollectionAssert.Contains(DrinksController.drinks, drink);
        }


        [TestMethod]
        public void PatchDrink_CheckEditedValue()
        {

            string DrinkName = "Pepsi";
            var testDrinks = GetTestDrinks();
            DrinksController.drinks = GetTestDrinks();
            var controller = new DrinksController();
            var drink = new Drink { Name = DrinkName, Quantity = 6 };
            var result = controller.PatchDrink(drink);
            var query = controller.GetDrink(DrinkName) as OkNegotiatedContentResult<Drink>;
            Assert.AreEqual(query.Content.Quantity, 6);
        }
        private List<Drink> GetTestDrinks()
        {
            var testDrink = new List<Drink>();
            testDrink.Add(new Drink { Id = "1", Name = "Demo1", Quantity = 1 });
            testDrink.Add(new Drink { Id = "2", Name = "Demo2", Quantity = 5 });
            testDrink.Add(new Drink { Id = "3", Name = "Demo3", Quantity = 7 });
            testDrink.Add(new Drink { Id = "4", Name = "Pepsi", Quantity = 3 });
            return testDrink;
        }
    }
}
