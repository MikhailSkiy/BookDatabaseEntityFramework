using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.util;

namespace BookDatabaseEntityFramework.similarity
{
   public abstract class BaseSimilarityMatrix : ISimilarityMatrix
    {
       protected String id;
       protected double[ , ] similarityValues = null;
       protected RatingCountMatrix[,] ratingCountMatrix = null;
       protected bool keepRatingCountMatrix = false;

       protected bool useObjIdToIndexMapping = true;
       protected ValueToIndexMapping idMapping = new ValueToIndexMapping();

       protected BaseSimilarityMatrix(){}

       public void setUseObjIdToIndexMapping(bool value){
           this.useObjIdToIndexMapping = value;
       }

       public bool getUseObjIdToIndexMapping(){
           return useObjIdToIndexMapping;
       }

       public string getId(){
           return this.id;
       }

       public double[,]getSimilarityMatrix(){
           return similarityValues;
       }

       public RatingCountMatrix getRatingCountMatrix(int idX,int idY){
           int x = getIndexFromObjId(idX);
           int y = getIndexFromObjId(idY);

           return ratingCountMatrix[x,y];
       }

       public bool isRatingCountMatrixAvailable(){
           return keepRatingCountMatrix;
       }

       public abstract void calculate(BookData data);

       public double getValue(int idX, int idY){
           if (similarityValues == null){
               throw new Exception ("You have to calculate similarity first");
           }

           int x = getIndexFromObjId(idX);
           int y = getIndexFromObjId(idY);

           int i,j;
           if (x<=y){
               i=x;
               j=y;
           }else {
               i=y;
               j=x;
           }
           return similarityValues[i,j];
       }

       /// <summary>
       /// Возвращает индекс который может использоваться для доступа к объекту в матрице
       /// </summary>
       /// <param name="objId">
       /// objId Id пользователя или элемента
       /// </param>
       /// <returns></returns>
       protected int getIndexFromObjId(int objId){
           int index = 0;
           if (useObjIdToIndexMapping){
               index = idMapping.getIndex(objId.ToString());
           }
           else {
               index = objId - 1;
           }
           return index;
       }




       protected int getObjIdFromIndex(int index)
       {
           int objId;
           if (useObjIdToIndexMapping)
           {
               objId = Int32.Parse(idMapping.getValue(index));
           }
           else
           {
               objId = index + 1;
           }
           return objId;
       }

       public void print(){
           if(similarityValues!= null){
               for (int i=0;i<similarityValues.GetLength(0);i++){
                   for (int j=0;j<similarityValues.GetLength(1);j++){
                        Console.Write(string.Format("{0} ", similarityValues[i, j]));
                   }
                   Console.Write(Environment.NewLine + Environment.NewLine);
               }
           }
       }

    }
}
