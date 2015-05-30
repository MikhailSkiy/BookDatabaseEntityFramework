using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.model;

namespace BookDatabaseEntityFramework
{
   public class BookData
    {
        /// <summary>
        /// Все рейтинги элементов
        /// </summary>
        private List<GeneralRating> allRatings = new List<GeneralRating>();

        /// <summary>
        ///  Словарь всех пользователей
        /// </summary
        private Dictionary<int, BookUser> allUsers = new Dictionary<int, BookUser>();

        /// <summary>
        /// Список всех пользователей
        /// </summary>
        private List<BookUser> listOfAllUsers = new List<BookUser>();

        /// <summary>
        /// Словарь всех элементов
        /// </summary>
        private Dictionary<int, BookItem> allBooks = new Dictionary<int, BookItem>();

        /// <summary>
        /// Словарь оцененных предметов по пользовательскому ID
        /// т.е. Id пользователя и список оцененных им предметов
        /// </summary>
        /// <returns></returns>
        Dictionary<int, List<GeneralRating>> ratingsByUserId = new Dictionary<int, List<GeneralRating>>();

        /// <summary>
        /// Находит книгу по заданному Id
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>
        /// Объект типа BookItem
        /// </returns>
        public BookItem getBookItem(int itemId)
        {
            return allBooks[itemId];
        }

        /// <summary>
        /// Находит пользователя по заданному Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>
        /// Объект типа BookUser
        /// </returns>
        public BookUser getUser(int userId)
        {
            return allUsers[userId];
        }

        /// <summary>
        /// Получает количество пользователей
        /// </summary>
        /// <returns></returns>
        public int getUserCount()
        {
            return allUsers.Count;
        }

        /// <summary>
        /// Получает список пользователей
        /// </summary>
        /// <returns>
        /// Список всех пользователей типа BookUser
        /// </returns>
        public List<BookUser> getUsers()
        {
            return allUsers.Values.ToList();
        }

        ///// <summary>
        ///// Возвращает количество пользователей
        ///// </summary>
        ///// <returns>
        ///// Число пользователей.
        ///// </returns>
        //public int getUserCount()
        //{
        //    return allUsers.Count;
        //}

        /// <summary>
        /// Вычисление среднего арифметического рейтинга для элемента
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>
        /// Значение рейтинга типа double
        /// </returns>
        public double getAverageItemRating(int itemId)
        {
            return getBookItem(itemId).getAverageRating();
        }

        /// <summary>
        /// Вычисление среднего арифметического рейтинга для пользоватля
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns>
        /// Значение рейтинга типа double
        /// </returns>
        public double getAverageUserRating(int userId)
        {
            return getUser(userId).getAverageRating();
        }

        /// <summary>
        /// Вычисляет количество элементов
        /// </summary>
        /// <returns>
        /// Число элементов типа int
        /// </returns>
        public int getBookCount(){
            return allBooks.Count;
        }

        /// <summary>
        /// Получает количество всех книг
        /// </summary>
        /// <returns>
        /// Список книг типа BookItem
        /// </returns>
        public List<BookItem> getBooks()
        {
            return allBooks.Values.ToList();
        }

        /// <summary>
        /// Получает все рейтинги
        /// </summary>
        /// <returns>
        /// Список объектов типа GeneralRating
        /// </returns>
        public List<GeneralRating> getRatings()
        {
            return allRatings;
        }

        /// <summary>
        /// Получает количество всех рейтингов
        /// </summary>
        /// <returns></returns>
        public int getRatingsCount()
        {
            return allRatings.Count;
        }

        /// <summary>
        /// Получает список книг, который оценил пользователь
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>
        /// Список книг, которые оценил тот или иной пользователь
        /// </returns>
        public List<BookItem> getRatedBooks(int userId)
        {
            List<BookItem> ratedBooks = new List<BookItem>();
            BookUser user = getUser(userId);
            List<GeneralRating> userRatings = user.getAllRatings();
            for (int i = 0; i < userRatings.Count; i++)
            {
                BookItem ratedBook = getBookItem(userRatings[i].getItemId());
                ratedBooks.Add(ratedBook);
            }
            return ratedBooks;
        }


       /// <summary>
        /// Возвращает объект типа BookUser из списка пользователей
       /// </summary>
       /// <param name="name">Имя пользователя</param>
       /// <returns>Пользователь BookUser</returns>       
        public BookUser pickUser(String name)
        {
            BookUser user = null;
            foreach (KeyValuePair<int, BookUser> kvp in allUsers)
            {
                BookUser aUser = kvp.Value;
                if (name.Equals(aUser.getName()))
                {
                    user = kvp.Value;
                    break;
                }
            }
            return user;
        }

        public BookItem pickItem(String name)
        {
            BookItem item = null;
            foreach (KeyValuePair<int, BookItem> kvp in allBooks)
            {
                BookItem anItem = kvp.Value;
                if (name.Equals(anItem.getName()))
                {
                    item = kvp.Value;
                    break;
                }
            }
            return item;
        }


        /**
  * Provides information about user and item ids returned by this dataset.
  * 
  * @return true if ids aren't in sequence and can't be used as array indexes.
  * false if user or items ids can be treated as sequences that start with 1. 
  * In this case index will be derived from id: index = id - 1.
  */
        public bool isIdMappingRequired();



        // ВНИМАНИЕ ПРОВЕРИТЬ!!!
        public void printItemRatings()
        {
            Console.WriteLine("\nItem ratings:\n");
            foreach (KeyValuePair<int, BookItem> kvp in allBooks)
            {
                Console.WriteLine("Book " + kvp.Value.getName());
                for (int i = 0; i < kvp.Value.getAllRatings().Count;i++ )

                    {
                        BookUser user = this.allUsers[kvp.Value.getAllRatings()[i].getUserId()];

                        Console.WriteLine("Rated by " + user.getName() + " as " + kvp.Value.getAllRatings()[i]);
                    }
            }
        }

        // Сделвть тоже самое для User


        public void createDataset()
        {       
            DatabaseHelper helper = new DatabaseHelper();
   
            // Получить из БД всех пользователей
            this.listOfAllUsers = TypeConverter.ConvertUsers(helper.getAllUsers());

            // Заполнить словарь allUsers полученными пользователями
            for (int i = 0; i < listOfAllUsers.Count(); i++)
            {
                allUsers.Add(listOfAllUsers[i].UserId, listOfAllUsers[i]);
            }

          List<BookItem>  listOfBooks = TypeConverter.ConvertBooks(helper.getAllBooks());

          // Заполнить словарь allBooks полученными книгами
          for (int i = 0; i < listOfBooks.Count(); i++)
          {
              allBooks.Add(listOfBooks[i].getId(), listOfBooks[i]);
          }

          this.allRatings = TypeConverter.ConvertRatings(helper.getRatingsFromDatabase());

        }


        // Возвращает список всех книг
        public static List<BookItem> loadAllBookItems()
        {
            List<BookItem> allItems = new List<BookItem>();
            DatabaseHelper db = new DatabaseHelper();
            List<Book> books = db.getAllBooks();
            return TypeConverter.ConvertBooks(books);
        }
    }
}
