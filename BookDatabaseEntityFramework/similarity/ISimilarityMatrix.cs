using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.similarity
{
    public interface ISimilarityMatrix
    {
        // Возвращает матрицу коэффициентов подобия. Для user - oriented представляет собой
        // Подобие между пользователями, а для item - oriented  матрицу подобия между элементами
          double[][] getSimilarityMatrix();

        /// <summary>
        /// Возвращает меру подобия между двумя объектами по их ID
        /// </summary>
        /// <param name="idX">Id первого элемента </param>
        /// <param name="idY">Id второго элемента</param>
        /// <returns>
        /// Значение типа double - значение подобия
        /// </returns>
        abstract double getValue(int idX, int idY);

        /// <summary>
        /// Id матрицы похожести
        /// </summary>
        /// <returns></returns>
        public abstract String getId();

        public abstract RatingCountMatrix getRatingCountMatrix(int idX, int idY);
        public abstract bool IsRatingCountMatrixAvailable();
        public void print();

    }
}
