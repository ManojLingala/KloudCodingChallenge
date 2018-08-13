using System;
using System.Collections.Generic;
using Xunit;
using KloudCodingChallenge.Common;
using KloudCodingChallenge.Model.Interfaces;
using KloudCodingChallenge.Model.Entities;
using System.Linq;

namespace KloudCodingChallenge.Test
{
    public class ListExtention
    {

		
		[Fact]
		public void AddItemToDictionary_ShouldIgnore_WhenKeyEmpty()
		{
			SortedDictionary<string, SortedList<string, string>> dict = new SortedDictionary<string, SortedList<string, string>>();
			var owerName1= "Daniel";
            var color = "Black";
            ListExtentions.AddItemToDictionary(dict, null, color,owerName1);

            Assert.Empty(dict.Keys);
		}

		
		[Fact]
		public void AddItemToDictionary_ShouldIgnore_WhenValueEmpty()
		{
			SortedDictionary<string, SortedList<string, string>> dict = new SortedDictionary<string, SortedList<string, string>>();
			var owerName1 = "Daniel";
            var name = "Toyota";
            var color = "Blue";

            ListExtentions.AddItemToDictionary(dict, name, null,color);
			Assert.Empty(dict.Keys);

            ListExtentions.AddItemToDictionary(dict, name, owerName1,color);
            Assert.Single(dict.Keys);
            Assert.Single(dict[name]);

            ListExtentions.AddItemToDictionary(dict, name, null,color);
            Assert.Single(dict[name]);
		}
	

		
		[Fact]
		public void ToFlattenList_ShouldHas3Item_WhenInputsValid()
		{
			List<IOwner> input = new List<IOwner>() {
				new Owner(new List<Car>(){ new Car(){Brand= "Toyota"}}) {Name = "Daniel"},
				new Owner(new List<Car>(){ new Car(){Brand= "Toyota"}, new Car() { Brand = "BMW" } }) {Name = "Daniel"}
			};

            var flatten = input.ToFlattenList();

			Assert.Equal(3, flatten.Count);
		}

        [Fact]
        public void ToFlattenList_ShouldReturnEmptyList_WhenInputIsNull()
        {
            List<IOwner> input = null;
            var flatten = input.ToFlattenList();

            Assert.Empty(flatten);
        }

        [Fact]
		public void ToFlattenList_ShouldConvertCorrectValue_WhenInputsValid()
		{
			List<IOwner> input = new List<IOwner>() {
                new Owner(new List<Car>(){ new Car(){Brand= "Toyota", Color = "Red"}}) {Name = "Daniel"}
			};

			var flatten = input.ToFlattenList();

            var result = flatten.First();
			Assert.Single(flatten);

            Assert.Equal("Red", result.Color);

            Assert.Equal("Daniel", result.OwnerName);
		}
        [Fact]
        public void ToOwnerList_ShouldHas2Item()
        {
            List<IData> data = new List<IData>()
            {
                new Data() {BrandName = "Toyota", Color="Red", OwnerName = "User1"},
                new Data() {BrandName = "BMW", Color="Red", OwnerName = "User2"},
            };

            var owners = data.ToOwnerList();

            Assert.Equal(2, owners.Count);
        }
        // In the real project, To make unit test code clean and reuse, We may have to build the data sample insite the json file and using it inside the code.

        [Fact]
        public void ToOwnerList_ShouldHas1Item()
        {
            List<IData> data = new List<IData>()
            {
                new Data() {BrandName = "Toyota", Color="Red", OwnerName = "User1"},
                new Data() {BrandName = "BMW", Color="Red", OwnerName = "User1"},
            };

            var owners = data.ToOwnerList();

            Assert.Single(owners);
            Assert.Equal(2, owners.First().Cars.Count);
        }

    }
}
