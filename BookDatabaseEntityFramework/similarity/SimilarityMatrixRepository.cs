using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.similarity
{
    /// <summary>
    ///  Пока используется без кэша
    /// </summary>
   public class SimilarityMatrixRepository
    {
       SimilarityMatrixCache cache;
       public SimilarityMatrixRepository(bool useCache)
       {
           if (useCache)
           {
               // ВНИМАНИЕ СДЕЛАТЬ С КЭШЭМ
           }
           else
           {
               cache = null;
           }
       }

       public ISimilarityMatrix load(RecommendationType type, BookData data)
       {
           bool keepRatingCountMatrix = true;
           return load(type, data, keepRatingCountMatrix);
       }

       public ISimilarityMatrix load(RecommendationType type, BookData data, bool keepRatingCountMatrix)
       {
           ISimilarityMatrix m = null;

           switch (type)
           {
               case RecommendationType.ITEM_BASED:
                   m = new ItemBasedSimilarity(data, keepRatingCountMatrix);
                   break;
               case RecommendationType.ITEM_PENALTY_BASED:
                  // m = new ItemPenaltyBasedSimilarity(IDictionary, data, keepRatingMatrix);
                   break;
               case RecommendationType.USER_BASED:
                   m = new UserBasedSimilarity(data, keepRatingCountMatrix);
                   break;
               case RecommendationType.IMPROVED_USER_BASED:
                  // m = new ImprovedUserBasedSimilarity(id, data, keepRatingCountMatrix);
                   break;
               case RecommendationType.USER_CONTENT_BASED:
                  // m = new UserContentBasedSimilarity(id, data);
                   break;
               case RecommendationType.ITEM_CONTENT_BASED:
                  // m = new ItemContentBasedSimilarity(id, data);
                   break;
               case RecommendationType.USER_ITEM_CONTENT_BASED:
                  // m = new UserItemContentBasedSimilarity(id, data);
                   break;
               default: break;
           }

           if (cache != null)
           {
              
           }

           return m;
       }
    }
}
