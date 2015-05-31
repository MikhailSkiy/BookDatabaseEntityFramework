using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.model;

namespace BookDatabaseEntityFramework
{
    // Класс, обозначающий пользователя интернет-магазина книг
   public class BookUser : GeneralUser
    {
        public BookUser(int id, string location, int age, List<GeneralRating> ratings)
            :base(id,location,age,ratings) {}

        public double getAverageRating()
        {
            double allRatingsSum = 0.0;
            foreach (KeyValuePair<int, GeneralRating> kvp in DictionaryOfBooksRating)
            {
                allRatingsSum += kvp.Value.getRating();
            }
            return DictionaryOfBooksRating.Count > 0 ? allRatingsSum / DictionaryOfBooksRating.Count : 2.5;
        }

        public string getName()
        {
            return this.UserId.ToString();
        }
    }
}
