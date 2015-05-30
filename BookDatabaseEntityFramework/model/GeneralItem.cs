using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.model
{
    // Общее представление продукта или сервиса, который может быть оценен пользователем
   public class GeneralItem
    {
        private int Id;
        private string Name;
        
        // Все рейтинги для заданного элемента. 
        // userId -> rating
        private Dictionary<int, GeneralRating> ratingByUserId;
        public GeneralItem(int id, List<GeneralRating> ratings)
        {
            this.Id = id;
            this.Name = id.ToString();
            ratingByUserId = new Dictionary<int, GeneralRating>(ratings.Count);

            for (int i = 0; i < ratings.Count; i++)
            {
                ratingByUserId.Add(ratings[i].getItemId(), ratings[i]);
            }
        }

        public GeneralItem(int id, string name, List<GeneralRating> ratings)
        {
            this.Id = id;
            this.Name = name;
            ratingByUserId = new Dictionary<int, GeneralRating>(ratings.Count);

            for (int i = 0; i < ratings.Count; i++)
            {
                ratingByUserId.Add(ratings[i].getUserId(), ratings[i]);
            }
        }

        public GeneralItem() { }

        public int getId()
        {
            return Id;
        }

        public String getName()
        {
            return Name;
        }

        // Вычисляет средний рейтинг (число)
        public double getAverageRating()
        {
            double allRatingsSum = 0.0;
            foreach (KeyValuePair<int, GeneralRating> kvp in ratingByUserId)
            {
                allRatingsSum += kvp.Value.getRating();
            }
            return ratingByUserId.Count > 0 ? allRatingsSum / ratingByUserId.Count : 2.5;
        }

        // Возвращает все рейтинги (объекты) для элемента
        public List<GeneralRating> getAllRatings()
        {
            List<GeneralRating> allRatings = new List<GeneralRating>();
            foreach (KeyValuePair<int, GeneralRating> kvp in ratingByUserId)
            {
                allRatings.Add(kvp.Value);
            }
            return allRatings;
        }

        // Возвращает рейтинг, который заданный пользователь поставил элементу
        public GeneralRating getUserRating(int userId)
        {
            return ratingByUserId[userId];
        }

        // Обновляет существующий пользовательский рейтинг или добавляет 
       // новый объект-рейтинг выставленный пользователем для элемента
        public void addUserRating(GeneralRating r)
        {
            ratingByUserId.Add(r.getUserId(), r);
        }

        // Получает пользователей оценивших одни и тежи предметы
        public static int[] getSharedUserIds(GeneralItem x, GeneralItem y)
        {
            List<int> sharedUsers = new List<int>();
            x.getAllRatings().ForEach(delegate(GeneralRating r)
            {
                // тот же пользователь оценил этот элемент
                if (y.getUserRating(r.getUserId()) != null)
                {
                    sharedUsers.Add(r.getUserId());
                }
            });
            return sharedUsers.ToArray();
        }

        // Утилитный метод, позволяющий получить массив рейтингов по массиву пользовательских id
        public double[] getRatingsForItemList(int[] userIds)
        {
            double[] ratings = new double[userIds.Length];
            for (int i = 0; i < userIds.Length; i++)
            {
                GeneralRating r = getUserRating(userIds[i]);
                if (r == null)
                {
                    throw new Exception("Item doesn't have rating by specified user id (" +
                        "userId=" + userIds[i] + ", itemId=" + getId());
                }
                ratings[i] = r.getRating();
            }
            return ratings;
        }
    }
}
