using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.model;

namespace BookDatabaseEntityFramework
{
   public  class DatabaseHelper 
    {
        BookDatabaseEntityFramework.BooksDBContext db = new BooksDBContext();

        // Возвращает объекты типа Рейтинг из БД
       // ВНИМАНИЕ!!!! Проверить сколько элементов возвращает 1 или много?
        public List<Rating> getRatingsFromDatabase (){

        var query = from rating in db.Ratings
                    select rating;

        List<Rating> ratings = query.ToList();
        return ratings;
        }

        public List<User> getAllUsers()
        {
             List<User> users = new List<User>();
             List<Rating> ratings = getRatingsFromDatabase ();
             for (int i = 0; i < ratings.Count; i++)
             {
                 var userId = ratings[i].UserId;
                 var query = from allRatings in db.Ratings
                             where allRatings.UserId == userId
                             select allRatings.User;
                 users.Add(query.First<User>());
             }
             return users;
  
        }

        public List<Book> getAllBooks()
        {
            var query = from books in db.Books
                        select books;

            List<Book> allBooks = query.ToList();

            return allBooks;
        }

        public  List<Rating> getAllRatingsByItemId(int id)
        {
            var query = from ratings in db.Ratings
                        where ratings.BookId == id
                        select ratings ;
            List<Rating> allRatings = query.ToList();
            return allRatings;
        }

        public List<Rating> getAllRatingsByUserId(int id)
        {
            var query = from ratings in db.Ratings
                        where ratings.UserId == id
                        select ratings;
            List<Rating> allRatings = query.ToList();
            return allRatings;
        }



    }
}
