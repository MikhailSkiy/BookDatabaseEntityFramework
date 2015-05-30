using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.model;

namespace BookDatabaseEntityFramework.similarity
{
  public class RatingCountMatrix
    {
      private int[,] matrix = null;

      public RatingCountMatrix(GeneralItem itemA, GeneralItem itemB, int nRatingValues)
      {
          init(nRatingValues);
          calculate(itemA, itemB);
      }

      public RatingCountMatrix(GeneralUser userA, GeneralUser userB, int nRatingValues)
      {
          init(nRatingValues);
          calculate(userA, userB);
      }

      private void init(int nSize)
      {
          matrix = new int[nSize, nSize];
      }

      /// <summary>
      /// Заполняет матрицу, используя юзер рейтинги для соответствующих элементов.
      /// Рассматриваются юзеры, которые оценили оба элемента
      /// </summary>
      /// <param name="itemA"></param>
      /// <param name="itemB"></param>
      private void calculate(GeneralItem itemA, GeneralItem itemB)
      {
          itemA.getAllRatings().ForEach(delegate(GeneralRating r)
          {
              GeneralRating r2 = itemB.getUserRating(r.getUserId());
              if (r2 != null)
              {
                  int i = r.getRating() - 1;
                  int j = r2.getRating() - 1;
                  matrix[i, j]++;
              }
          });
      }

      /// <summary>
      /// Заполняет матрицу рейтингами элементов, которые оценили оба пользователя
      /// </summary>
      /// <param name="userA"></param>
      /// <param name="userB"></param>
      private void calculate(GeneralUser userA, GeneralUser userB)
      {
          userA.getAllRatings().ForEach(delegate(GeneralRating r)
          {
              GeneralRating r2 = userB.getItemRating(r.getItemId());
              if (r2!= null)
              {
                  int i = r.getRating() - 1;
                  int j = r2.getRating() - 1;
                  matrix[i, j]++;
              }
          });
      }

      public int getTotalCount()
      {
          int ratingCount = 0;
          int n = matrix.Length;

          for (int i = 0; i < n; i++)
          {
              for (int j = 0; j < n; j++)
              {
                  ratingCount += matrix[i, j];
              }
          }
          return ratingCount;
      }

      public int getAgreementCount()
      {
          int ratingCount = 0;
          for (int i = 0; i < matrix.Length; i++)
          {
              ratingCount += matrix[i, i];
          }
          return ratingCount;
      }

      public int[,] getMatrix()
      {
          return matrix;
      }

      public int getBandCount(int bandId)
      {
          int bandCount = 0;
          for (int i = 0; i + bandId < matrix.Length; i++)
          {
              bandCount += matrix[i, i + bandId];
              bandCount += matrix[i + bandId, i];
          }
          return bandCount;
      }

    }
}
