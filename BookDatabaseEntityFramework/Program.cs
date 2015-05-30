using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.recommender;
using BookDatabaseEntityFramework.similarity;

namespace BookDatabaseEntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Загрузка данных

            DatabaseHelper helper = new DatabaseHelper();

            // Получить список всех книг
            List<BookItem> listOfBooks = new List<BookItem>();
            listOfBooks = TypeConverter.ConvertBooks(helper.getAllBooks());

            // Получить список всех пользователей
            List<BookUser> listOfUsers = new List<BookUser>();
            listOfUsers = TypeConverter.ConvertUsers(helper.getAllUsers());


            BookData ds = new BookData();
            ds.createDataset();



            #endregion 

            #region Создание системы выработки рекомендаций на основе сходства предметов
            Delphi delphi = new Delphi(ds, RecommendationType.ITEM_BASED);

            // Создание системы выработки рекомендаций на основе сходства предметов
            delphi.setVerbose(true);


            BookUser bu1 = ds.pickUser("1");
            // Рекомендация нескольких предметов пользователю "1"
            delphi.recommend(bu1);

            BookItem bi = ds.pickItem("2");
            // Поиск похожих предметов
         //   delphi.findSimilarItems(bi);

            #endregion




        }
    }
}
