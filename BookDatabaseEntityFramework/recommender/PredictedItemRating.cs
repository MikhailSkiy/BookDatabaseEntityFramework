using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.recommender
{
    /// <summary>
    /// Класс, описывающий предсказанный рейтинг, который может поставить пользователь элементу
    /// </summary>
    public class PredictedItemRating
    {
        private int userId;
        private int itemId;
        private double rating;

        public PredictedItemRating(int userId, int itemId, double rating)
        {
            this.userId = userId;
            this.itemId = itemId;
            this.rating = rating;
        }

        public int getUserId()
        {
            return userId;
        }

        public int getItemId()
        {
            return itemId;
        }

        public double getRating()
        {
            return rating;
        }

        public void setRating(double val)
        {
            this.rating = val;
        }

        // Дописать сортировку рейтинга

        public static void sort(List<PredictedItemRating> values)
        {
            var linq = (from PredictedItemRating pir in values
                        orderby pir.rating ascending
                        select pir) as List<PredictedItemRating>;

            var lambda = values.OrderBy(c => c.rating).ToList();
        }
        // Написать getTopNRecommendations


        public static List<PredictedItemRating> getTopNRecommendations(List<PredictedItemRating> recommendations, int topN)
        {
            PredictedItemRating.sort(recommendations);

            List<PredictedItemRating> topRecommendations = new List<PredictedItemRating>();

            recommendations.ForEach(delegate(PredictedItemRating r)
            {

                if (topRecommendations.Count >= topN)
                {

                    return;
                }

                topRecommendations.Add(r);
            });

            return topRecommendations;
        }

        public static void printUserRecommendations(BookUser user, BookData ds, List<PredictedItemRating> recommendedItems)
        {
            Console.WriteLine("\nRecommendations for user " + user.getName() + ":\n");

            recommendedItems.ForEach(delegate(PredictedItemRating r)
            {
                Console.WriteLine("Item: " + ds.getBookItem(r.getItemId()).getName());
                Console.WriteLine("Predicted rating: " + r.getRating());
            });
        }

    }
}
