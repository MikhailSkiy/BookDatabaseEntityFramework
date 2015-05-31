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
        List<PredictedItemRating> recommend(BookUser user);

          /// <summary>
        /// Возвращает N самых "точных" (т.е. большим по показателям) рекомендаций
        /// </summary>
        /// <param name="user">GeneralUser</param>
        /// <returns>Рекомендуемые элементы с предсказанным рейтингом</returns>
        List<PredictedItemRating> recommend(BookUser user, int topN);

        double predictRating(BookUser user, BookItem item);

        List<SimilarUser> findSimilarUsers(BookUser user);

        List<SimilarUser> findSimilarUsers(BookUser user, int topN);

       // public SimilarItem[] findSimilarItems(Item item);

    //    public SimilarItem[] findSimilarItems(Item item, int topN);

        // WTF???
         BookData getDataset();

         double getSimilarityThreshold();

         void setSimilarityThreshold(double similarityThreshold);

    }
}
