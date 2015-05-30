using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.similarity
{
    public class ItemBasedSimilarity : BaseSimilarityMatrix
    {
        public ItemBasedSimilarity(String id, BookData dataSet, bool keepRatingCountMatrix)
        {
            this.id = id;
            this.keepRatingCountMatrix = keepRatingCountMatrix;
            this.useObjIdToIndexMapping = dataSet.isIdMappingRequired();
            calculate(dataSet);
        }

        public ItemBasedSimilarity(BookData dataSet, bool keepRatingCountMatrix)
        {
            this.keepRatingCountMatrix = keepRatingCountMatrix;
            this.useObjIdToIndexMapping = dataSet.isIdMappingRequired();
            calculate(dataSet);
        }

        public void calculate(BookData dataset)
        {
            int nItems = dataset.getBookCount();
            int nRatingValues = 5; // ИЛИ 6
            similarityValues = new double[nItems, nItems];

            if (keepRatingCountMatrix)
            {
                ratingCountMatrix = new RatingCountMatrix[nItems, nItems];
            }

            // if we want to use mapping from itemId to index then generate 
            // index for every itemId

            if (useObjIdToIndexMapping)
            {
                // Для каждой книги из dataSet берем книгу, получаем её Ид и получаем индекс с помощью класса idMapping
                List<BookItem> bi = dataset.getBooks();
                foreach (BookItem b in bi)
                {
                    idMapping.getIndex(b.getId().ToString());
                }
            }

            // By using these variables we reduce the number of method calls
            // inside the double loop.

            int totalCount = 0;
            int agreementCount = 0;

            for (int u = 0; u < nItems; u++)
            {
                int itemAId = getObjIdFromIndex(u);
                BookItem itemA = dataset.getBookItem(itemAId);

                // // we only need to calculate elements above the main diagonal.
                for (int v = u + 1; v < nItems; v++)
                {
                    int itemBId = getObjIdFromIndex(v);
                    BookItem itemB = dataset.getBookItem(itemBId);
                    RatingCountMatrix rcm = new RatingCountMatrix(itemA, itemB, nRatingValues);

                    totalCount = rcm.getTotalCount();
                    agreementCount = rcm.getAgreementCount();

                    if (agreementCount > 0)
                    {
                        similarityValues[u,v] = (double)agreementCount / (double)totalCount;
                    }
                    else
                    {
                        similarityValues[u,v] = 0.0;
                    }

                    if (keepRatingCountMatrix)
                    {
                        ratingCountMatrix[u, v] = rcm;
                    }
                }

                similarityValues[u, u] = 1.0;
            }


        }
    }
}
