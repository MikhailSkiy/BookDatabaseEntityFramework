using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.model;

namespace BookDatabaseEntityFramework
{
   public static class TypeConverter
    {
       public static List<BookItem> ConvertBooks(List<Book> books)
       {
           List<BookItem> bookItems = new List<BookItem>();

           for (int i = 0; i < books.Count; i++)
           {
               DatabaseHelper dbHelper = new DatabaseHelper();
               List<Rating> ratings = dbHelper.getAllRatingsByItemId(books[i].BookId);
               BookItem singleBook = new BookItem(books[i].BookId,
                                                    books[i].ISBN,
                                                    books[i].Title,
                                                    books[i].Author,
                                                    books[i].YearOfPublication.GetValueOrDefault(),
                                                    books[i].Publisher,
                                                    books[i].ImageURLS,
                                                    books[i].ImageURLM,
                                                    books[i].ImageURLL,
                                                    ConvertRatings(ratings));
               bookItems.Add(singleBook);
           }
           return bookItems;

       }

       public static List<BookUser> ConvertUsers(List<User> users)
       {
            List<BookUser> bookUsers = new List<BookUser>();

           for (int i=0;i<users.Count;i++){
               DatabaseHelper dbHelper = new DatabaseHelper();
               List<Rating> ratings = dbHelper.getAllRatingsByItemId(users[i].UserId);
               BookUser bu = new BookUser(users[i].UserId,
                                          users[i].Location, 
                                          users[i].Age.GetValueOrDefault(), 
                                          ConvertRatings(ratings));
               bookUsers.Add(bu);        
            
           }

           return bookUsers;
       }

       public static List<GeneralRating> ConvertRatings(List<Rating> ratings)
       {
           List<GeneralRating> ListOfRatings = new List<GeneralRating>();
           for (int i = 0; i < ratings.Count; i++)
           {
               GeneralRating gRating = new GeneralRating(ratings[i].UserId, ratings[i].BookId, ratings[i].Rating1);
               ListOfRatings.Add(gRating);
           }
           return ListOfRatings;
       }
    }
}
