using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.model;

namespace BookDatabaseEntityFramework.recommender
{
    interface IRecommender
    {
        /// <summary>
        /// Возвращает 5 самых "точных" (т.е. большим по показателям) рекомендаций
        /// </summary>
        /// <param name="user">GeneralUser</param>
        /// <returns>Рекомендуемые элементы с предсказанным рейтингом</returns>
        public List<PredictedItemRating> recommend(GeneralUser user);

          /// <summary>
        /// Возвращает N самых "точных" (т.е. большим по показателям) рекомендаций
        /// </summary>
        /// <param name="user">GeneralUser</param>
        /// <returns>Рекомендуемые элементы с предсказанным рейтингом</returns>
        public List<PredictedItemRating> recommend(User user, int topN);

        public double predictRating(User user, BookItem item);

        public SimilarUser[] findSimilarUsers(User user);

        public SimilarUser[] findSimilarUsers(User user, int topN);

       // public SimilarItem[] findSimilarItems(Item item);

    //    public SimilarItem[] findSimilarItems(Item item, int topN);

        // WTF???
        public BookData getData();

        public double getSimilarityThreshold();

        public void setSimilarityThreshold(double similarityThreshold);

    }
}
