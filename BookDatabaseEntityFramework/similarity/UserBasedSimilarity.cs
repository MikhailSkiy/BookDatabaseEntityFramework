using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.similarity
{
    public class UserBasedSimilarity : BaseSimilarityMatrix
    {

        public UserBasedSimilarity(BookData dataset)
        {
            new UserBasedSimilarity(dataset, true);
        }

        public UserBasedSimilarity(BookData dataset, bool keepRatingCountMatrix)
        {
            this.id = id;
            this.keepRatingCountMatrix = keepRatingCountMatrix;
            this.useObjIdToIndexMapping = dataset.isIdMappingRequired();
            calculate(dataset);
        }

        protected override void calculate(BookData dataset)
        {
            int nUsers = dataset.getUserCount();
            int nRatingValues = 5;

            similarityValues = new double[nUsers, nUsers];

            if (keepRatingCountMatrix)
            {
                ratingCountMatrix = new RatingCountMatrix[nUsers, nUsers];
            }

            // if we want to use mapping from userId to index then generate 
            // index for every userId

            if (useObjIdToIndexMapping)
            {
                dataset.getUsers().ForEach(delegate(BookUser u)
                {
                    idMapping.getIndex(u.UserId.ToString());
                });
            }

            for (int u = 0; u < nUsers; u++)
            {
                int userAId = getObjIdFromIndex(u);
                BookUser userA = dataset.getUser(userAId);

                for (int v = u + 1; v < nUsers; v++)
                {
                    int userBId = getObjIdFromIndex(v);
                    BookUser userB = dataset.getUser(userBId);

                    RatingCountMatrix rcm = new RatingCountMatrix(userA, userB, nRatingValues);

                    int totalCount = rcm.getTotalCount();
                    int agreementCount = rcm.getAgreementCount();

                    if (agreementCount > 0)
                    {
                        similarityValues[u, v] = (double)agreementCount / (double)totalCount;

                    }
                    else
                    {
                        similarityValues[u, v] = 0.0;
                    }

                    // For large datasets

                    if (keepRatingCountMatrix)
                    {
                        ratingCountMatrix[u, v] = rcm;
                    }

                }
                // for u == v assign 1. 
                // RatingCountMatrix wasn't created for this case+
                similarityValues[u, u] = 1.0;
            }
        }
    }
}
