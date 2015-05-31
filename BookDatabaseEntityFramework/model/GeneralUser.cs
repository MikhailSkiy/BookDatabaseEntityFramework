using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.model
{
   public class GeneralUser
    {
        public int UserId { get; set; }
       public string Location { get; set; }
       public int Age { get; set; }

      

        // Список всех книг, которые оценил пользователь
       public Dictionary<int,GeneralRating> DictionaryOfBooksRating {get;set;}

        // Конструктор, позволяющий создать объект типа пользователь со списком всех книг, которые он оценил
        public GeneralUser(int id, string location, int age, List<GeneralRating> ratings) {
        this.UserId= id;
        this.Location = location;
        this.Age = age;
        this.DictionaryOfBooksRating = new Dictionary<int, GeneralRating>(ratings.Count);

        // Заполняются значения словаря данными в виде (ISBN - то есть bookId, и экземаляром объекта типа BookRating)
        for (int i = 0; i < ratings.Count; i++ )
        {
            DictionaryOfBooksRating.Add(ratings[i].getItemId(), ratings[i]);
        }
      }

        public GeneralUser() { }

        public GeneralUser(int id)
        {
            this.UserId = id;
            this.Location = "city";
            this.Age = 18;
            this.DictionaryOfBooksRating = new Dictionary<int, GeneralRating>(3);
        }



        // Инициализирует новый список рейтингов
        public void setRatings(List<GeneralRating> ratings)
        {
            if (DictionaryOfBooksRating == null)
            {
                DictionaryOfBooksRating = new Dictionary<int, GeneralRating>(ratings.Count);
            }
            else
            {
                DictionaryOfBooksRating.Clear();
            }

            // Загружаем рейтинги
            for (int i = 0; i < ratings.Count;i++ )
            {
                DictionaryOfBooksRating.Add(ratings[i].getItemId(), ratings[i]);
            }
        }

        // Добавить один рейтинг
        public void addRating(GeneralRating rating)
        {
            DictionaryOfBooksRating.Add(rating.getItemId(), rating);
        }

        // Получает все рейтинги, у данного пользователя
        public List<GeneralRating> getAllRatings()
        {
            List<GeneralRating> rating = new List<GeneralRating>();
            foreach (KeyValuePair<int, GeneralRating> kvp in DictionaryOfBooksRating)
            {
                rating.Add(kvp.Value);
            }
            return rating;
        }

        // Получает объект типа BookRating по заданному Id книги
        public GeneralRating getItemRating(int itemId)
        {
            return DictionaryOfBooksRating[itemId];
        }

        /**
      * Utility method to extract item ids that are shared between user A and user B.
      */
        public static int[] getSharedItems(GeneralUser x, GeneralUser y) {
        List<int> sharedItems = new List<int>();
        x.getAllRatings().ForEach(delegate(GeneralRating r)
        {
            if (y.getItemRating(r.getItemId()) != null)
            {
                sharedItems.Add(r.getItemId());
            }
        });
        return sharedItems.ToArray();
    }

 
      /*
      * Utility method to extract array of ratings based on array of item ids.
      */
        public double[] getRatingsForItemList(int[] itemIds)
        {
            double[] ratings = new double[itemIds.Count()];
            for (int i = 0, n = itemIds.Count(); i < n; i++)
            {
                GeneralRating r = getItemRating(itemIds[i]);
                if (r == null)
                {
                    throw new Exception(
                            "User doesn't have specified item id (" +
                            "userId=" + UserId + ", itemId=" + itemIds[i]);
                }
                ratings[i] = r.getRating();
            }
            return ratings;
        }

        public double getSimilarity(GeneralUser u, int simType)
        {
            double sim = 0.0d;
            int commonItems = 0;

            switch (simType)
            {
                // 0 - вычисление Евклидова расстояния
                case 0:
                    foreach (KeyValuePair<int, GeneralRating> kvp in this.DictionaryOfBooksRating)
                    {
                        foreach (KeyValuePair<int, GeneralRating> kvp2 in u.DictionaryOfBooksRating)
                        {
                            if (kvp.Value.getItemId() == kvp2.Value.getItemId())
                            {
                                commonItems++;
                                sim+=Math.Pow((kvp.Value.getRating() - kvp2.Value.getRating()),2);
                            }
                        }
                    }

                    if (commonItems > 0)
                    {
                        // Вычисление среднеквадратичной ошибки
                        sim = Math.Sqrt(sim / commonItems);
                        // Значение должно быть от 0 до 1
                        // Если значение 0, значит пользователи абсолютно разные
                        // Если значени 1, значит интрересы пользоватeлей идентичны
                        sim = 1.0d - Math.Tanh(sim);
                    }
                    break;

                // 1 - вычисление Евклидова расстояния принимая во внмание кол-во общих элементов
                case 1:
                    foreach (KeyValuePair<int, GeneralRating> kvp in this.DictionaryOfBooksRating)
                    {
                        foreach (KeyValuePair<int, GeneralRating> kvp2 in u.DictionaryOfBooksRating)
                        {
                            if (kvp.Value.getItemId() == kvp2.Value.getItemId())
                            {
                                commonItems++;
                                sim += Math.Pow((kvp.Value.getRating() - kvp2.Value.getRating()), 2);
                            }
                        }
                    }

                    if (commonItems > 0)
                    {
                        sim = Math.Sqrt(sim / commonItems);
                        sim = 1.0d - Math.Tanh(sim);

                        // Максимальное кол-во элементов, которые общие для обоих пользователей
                        int maxCommonItems = Math.Min(this.DictionaryOfBooksRating.Count, u.DictionaryOfBooksRating.Count);
                        sim = sim * ((double)commonItems / (double)maxCommonItems);
                    }
                    break;
                default: break;
            }
            return sim;
         }

       // СДелать жакарда

    }
}
