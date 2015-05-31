using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.similarity;

using BookDatabaseEntityFramework.model;

namespace BookDatabaseEntityFramework.recommender
{
    public class Delphi : IRecommender
    {
        private static double DEFAULT_SIMILARITY_THRESHOLD = 0.50;
	    private static  double MAX_RATING = 6;

        private RecommendationType type;
        private ISimilarityMatrix similarityMatrix;
        private BookData dataSet; // Like BaseDataset
        private bool verbose = true;
        private double similarityThreshold = DEFAULT_SIMILARITY_THRESHOLD;
        private Dictionary<int, double> maxPredictedRating;

        public Delphi(BookData dataSet, RecommendationType type)
        {
            this.type = type;
            bool useCache = false; // by default don't use cache
            SimilarityMatrixRepository smRepo = new SimilarityMatrixRepository(useCache);
            this.similarityMatrix = smRepo.load(type, dataSet);
            this.dataSet = dataSet;

            maxPredictedRating = new Dictionary<int, double>(dataSet.getUserCount() / 2);
        }

        public Delphi (BookData dataset,RecommendationType type, bool useSimilarityCache){
            Console.WriteLine("Enetring Delphi(Dataset,RecomendationType, bool) constructor...");

            this.type = type;
            SimilarityMatrixRepository smRepo = new SimilarityMatrixRepository(useSimilarityCache);
            this.similarityMatrix = smRepo.load(type,dataset);
            this.dataSet = dataSet;

            maxPredictedRating = new Dictionary<int,double>(dataSet.getUserCount()/2);

            Console.WriteLine("Leaving Delphi(Dataset,RecomendationType,boolean) constructor...");
        }

        public Delphi(BookData dataSet,RecommendationType type,ISimilarityMatrix similarityMatrix){
               Console.WriteLine("Enetring Delphi(Dataset,RecomendationType, SimilarityMatrix) constructor...");

            this.type = type;
            this.similarityMatrix = similarityMatrix;
            this.dataSet = dataSet;

            maxPredictedRating = new Dictionary<int,double>(dataSet.getUserCount()/2);

            
            Console.WriteLine("Leaving Delphi(Dataset,RecomendationType,SimilarityMatrix) constructor...");
        }

            //--------------------------------------------------------------------
            // USER BASED SIMILARITY
            //--------------------------------------------------------------------

        public List<SimilarUser> findSimilarUsers(BookUser user) {
            List<SimilarUser> topFriends = findSimilarUsers(user,5);

            if (verbose) {
                SimilarUser.print(topFriends,"Top Friends for user " + user.getName() + ":");
            }

            return topFriends;
        }

        public List<SimilarUser> findSimilarUsers(BookUser user,int topN){

            List<SimilarUser> similarUsers = new List<SimilarUser>();

            if (isUserBased()){

                similarUsers = findFriendsBasedOnUserSimilarity(user);
            }
            else{

                Console.WriteLine("Finding friends based on Item similarity is not supported!");
            }

            return SimilarUser.getTopNFriends(similarUsers,topN);
        }

        private List<SimilarUser> findFriendsBasedOnUserSimilarity(BookUser bookUser){
            List<SimilarUser> similarUsers = new List<SimilarUser>();

            foreach (BookUser bu in dataSet.getUsers()){
                if (bookUser.UserId != bu.UserId){
                    double similarity = similarityMatrix.getValue(bookUser.UserId, bu.UserId);
                    similarUsers.Add(new SimilarUser(bu,similarity));
                }
            }
            return similarUsers;
        }

        public ISimilarityMatrix getSimilarityMatrix()
        {
            return similarityMatrix;
        }

        //public void setVerbose(bool verbose){
        //    this.verbose = verbose;
        //}

        public bool isVerbose(){return verbose;}

        private bool isUserBased(){
            return type.ToString().IndexOf("USER")>= 0 && type.ToString().IndexOf("USER_ITEM") <0;
        }


        //--------------------------------------------------------------------
        // RECOMMENDATIONS
        //--------------------------------------------------------------------
        public List<PredictedItemRating> recommend (BookUser User){
            List<PredictedItemRating> recommendedItems = recommend(User,5);
            return recommendedItems;
        }

        private bool skipItem(BookUser user, BookItem item){
            bool skipItem = true;
            //if (isContentBased()) {
            //    if (user.getUserContent(item.getItemContent().getId()) ==null){
            //        skipItem = false;
            //    }
            //}

                if (user.getItemRating(item.getId()) == null){
                    skipItem = false;
                }           
            return skipItem;
        }

        public List<PredictedItemRating> recommend (BookUser user, int topN){
            List<PredictedItemRating> recommendations = new List<PredictedItemRating>();

            double maxRating = -1.0d; 
            for (int i=0;i<dataSet.getBooks().Count;i++){
                if (!skipItem(user,dataSet.getBooks()[i])){
                    double predictedRating = predictRating(user,dataSet.getBooks()[i]);
                    if (maxRating < predictedRating){
                        maxRating = predictedRating;
                    }

                    if(!Double.IsNaN(predictedRating)){
                        recommendations.Add(new PredictedItemRating(user.UserId,dataSet.getBooks()[i].getId(),predictedRating));
                    }
                }
                else {
                    if(verbose){
                        Console.WriteLine("Skipping item: " + dataSet.getBooks()[i].getName());
                    }
                }
            }

            this.maxPredictedRating.Add(user.UserId,maxRating);

            List<PredictedItemRating> topNRecommendations = PredictedItemRating.getTopNRecommendations(recommendations,topN);

            if (verbose){
                PredictedItemRating.printUserRecommendations(user,dataSet,topNRecommendations);
            }

            return topNRecommendations;
        }

        public List<PredictedItemRating> recommend (int userId){
            return recommend(dataSet.getUser(userId));
        }

    //--------------------------------------------------------------------
    // RATING PREDICTIONS
    //--------------------------------------------------------------------

        public double predictRating(int userId, int itemId){
            return predictRating(dataSet.getUser(userId),dataSet.getBookItem(itemId));
        }

        public double predictRating(BookUser user, BookItem item) {
            switch (type) {
                case RecommendationType.USER_BASED:
                    return estimateUserBasedRating(user,item);
                case RecommendationType.IMPROVED_USER_BASED:
                    return estimateUserBasedRating(user,item);
                case RecommendationType.ITEM_BASED:
                    return estimateItemBasedRating(user,item);
                case RecommendationType.ITEM_PENALTY_BASED:
                    return estimateItemBasedRating(user,item);
                case RecommendationType.USER_CONTENT_BASED:
                    throw new Exception("Not valid for current similarity type:" + type);
                case RecommendationType.USER_ITEM_CONTENT_BASED:
                    return MAX_RATING * similarityMatrix.getValue(user.UserId,item.getId());
                default: 
                    throw new Exception ("Unknown recommendation type:" + type);
            }

        }

        //--------------------------------------------------------------------
        // Вспомогательные методы
        //--------------------------------------------------------------------

        public double getSimilarityThreshold(){
            return similarityThreshold;
        }

        public void setSimilarityThreshold(double similarityThreshold){
            this.similarityThreshold = similarityThreshold;
        }

        public RecommendationType getType() {
            return type;
        }

        public double getSimilarity(BookItem i1, BookItem i2){
            double sim = similarityMatrix.getValue(i1.getId(), i2.getId());

            // verbose - флаг, отвечающий за пояснение всего (многословный дословно)
            if (verbose){
                Console.WriteLine("Item similarity between");
                Console.WriteLine(" ItemID: " + i1.getId());
                Console.WriteLine(" and");
                Console.WriteLine(" ItemID: " + i2.getId());
                Console.WriteLine(" is equal to " + sim);
            }

            return sim;
        }

        public double getSimilarity(BookUser u1, BookUser u2){
            double sim = similarityMatrix.getValue(u1.UserId,u2.UserId);
            if (verbose) {
                Console.WriteLine("User Similarity between ");
                Console.WriteLine(" UserId: "+ u1.UserId);
                Console.WriteLine(" and");
                Console.WriteLine(" UserID: " + u2.UserId);
                Console.WriteLine(" is equal to" + sim);
            }

            return sim;
        }

        //public double getUserItemSimilarity(BookUser user,BookItem item){
        //}

        //WTF!!!!!!!!!!!
        //public ISimilarityMatrix getSimilarityMatrix(){
        //    return similarityMatrix;
        //}

        //public bool isVerbose() {
        //return verbose;
        //}

       public void setVerbose(bool verbose) {
        this.verbose = verbose;
    }

        //--------------------------------------------------------------------
        // Приватные вспомогательные методы
        //--------------------------------------------------------------------

        private double estimateUserBasedRating(BookUser user, BookItem item){
            double estimatedRating = user.getAverageRating();

            int itemId = item.getId();
            int userId = user.UserId;

            double similaritySum = 0.0;
            double weightedRatingSum = 0.0;

            // Проверка, оценивал ли пользователь этот элемент
            GeneralRating existingRatingByUser = user.getItemRating(item.getId());

            if (existingRatingByUser != null){
                estimatedRating = existingRatingByUser.getRating();
            }
            else
            {
                dataSet.getUsers().ForEach(delegate(BookUser anotherUser){

                    GeneralRating itemRating = anotherUser.getItemRating(itemId);

                    if (itemRating != null){
                        double similarityBetweenUsers = similarityMatrix.getValue(userId,anotherUser.UserId);
                        double ratingByNeighbor = itemRating.getRating();
                        double weightedRating = similarityBetweenUsers * ratingByNeighbor;

                        weightedRatingSum +=weightedRating;
                        similaritySum +=similarityBetweenUsers;
                    }
                });

                if (similaritySum > 0.0) {
                    estimatedRating = weightedRatingSum / similaritySum;
                }
            }

            return estimatedRating;
        }

        private double estimateItemBasedRating(BookUser user, BookItem item){

            double estimatedRating = item.getAverageRating();
            int itemId = item.getId();
            int userId = user.UserId;
            double similaritySum = 0.0;
            double weightedRatingSum = 0.0;

            GeneralRating existingRatingByUser = user.getItemRating(item.getId());

            if (existingRatingByUser != null){
                estimatedRating = existingRatingByUser.getRating();
            } 
            else
            {
                double similarityBetweenItems = 0;
                double weightedRating = 0;

                dataSet.getBooks().ForEach(delegate(BookItem anotherItem) {

                    // Рассматриваются только те элементы, которые оценивал пользователь

                    GeneralRating anotherItemRating = anotherItem.getUserRating(userId);

                    if (anotherItemRating != null){

                        similarityBetweenItems = similarityMatrix.getValue(itemId, anotherItem.getId());

                        if (similarityBetweenItems > similarityThreshold){
                            weightedRating = similarityBetweenItems * anotherItemRating.getRating();

                            weightedRatingSum += weightedRating;
                            similaritySum += similarityBetweenItems;
                        }
                    }

                });

                if (similaritySum >0.0){
                    estimatedRating = weightedRatingSum / similaritySum;
                }
            }

            return estimatedRating;
        }

        // Возвращает maxPredictedRating конкретного пользователя

        public double getMaxPredictedRating(int uID){
            double maxPr = maxPredictedRating[uID];

            return (maxPr == null) ? 5.0d :maxPr;
        }

        public BookData getDataset(){
            return this.dataSet;
        }



    }
}
