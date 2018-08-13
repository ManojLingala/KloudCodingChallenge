using System;
using System.Collections.Generic;
using System.Linq;
using KloudCodingChallenge.Model.Interfaces;
using KloudCodingChallenge.Model.Entities;

namespace KloudCodingChallenge.Common
{
    public static class ListExtentions
    {
        /// <summary>
        /// This function to transform the ICollection to Ordered Data structure
        /// </summary>
        /// <returns>The dictionary.</returns>
        /// <param name="data">A list of object , intance of IList<IOwner>
        /// This function contains the result set with sorted and grouped information , and here we are using Sorted{Dictionary,List} for better 
        /// performane.
        public static SortedDictionary<string, SortedList<string, string>> ToSortedDictionary(this List<IData> data)
        {
            var result = new SortedDictionary<string, SortedList<string, string>>();

            data.ForEach((item) => AddItemToDictionary(result, item.BrandName, item.Color,item.OwnerName));

            return result;
        }

        /// <summary>
        /// Adds the item to dictionary.
        /// </summary>
        /// <param name="dictionary">Dictionary.</param>
        /// <param name="key">A unique string for key :ex Toyota</param>
        /// <param name="value">A string for value items list, ex : brand name</param>
        /// <param name="ownerName">A string for value items list, ex : krisitin </param>
        public static void AddItemToDictionary(SortedDictionary<string, SortedList<string, string>> dictionary, string key, string value, string ownerName) {

            //Ignore if brand or name is empty
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) return;

            //avoid duplicates and handle the scenario where single person haveing mutiple cars.
            if(dictionary.ContainsKey(key) && !string.IsNullOrWhiteSpace(ownerName)){
                if (!dictionary[key].ContainsValue(ownerName))
                {
                    dictionary[key].Add(value, ownerName);
                }
              
            }
            else if(!string.IsNullOrWhiteSpace(ownerName)){
                var newItem = new SortedList<string, string>();
                newItem.Add(value,ownerName);
                dictionary.Add(key, newItem);
            }
        }

		/// <summary>
		/// This functon to break the data from nest structure to a flat struct
		/// <returns>The flatten list with IData type</returns>
		public static List<IData> ToFlattenList(this List<IOwner> data) {
            if (data == null) return new List<IData>();

            var flattenData = data.SelectMany(owner => owner.Cars.Select(car => new Data() {
                OwnerName = owner.Name,
                BrandName = car.Brand,
                Color = car.Color
                
            }));
            return new List<IData>(flattenData);
            
        }

        /// <summary>
        /// This transform will convert flatten list data to become a dictionary of form : {ownername , cars: {brand, color}}.
        /// </summary>
        /// <returns>an array of instance of List ex:[{ownername , cars: {brand, color}}]</returns>
        /// <param name="data">Flatten list IList</param>
        public static List<IOwner> ToOwnerList(this List<IData> data)
		{
            var grouped = data.GroupBy(p => p.OwnerName);

            var list = grouped.Select(group => new Owner(group.Select(item => new Car() { Brand = item.BrandName, Color = item.Color}).ToList())
            {
                Name = group.Key,
            });
			
            return new List<IOwner>(list);
		}
	}
}
