using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.model
{
    /// <summary>
    /// Утилитный класс, использующийся для хранения пользователяи значений сходства, для этого пользователя
    /// </summary>
    public class SimilarUser
    {
        /// <summary>
        /// Наиболее похожий на текущего пользователя пользователь
        /// </summary>
        private GeneralUser friend;

        /// <summary>
        /// Показатель сходства
        /// </summary>
        private double similarity = -1;

        public SimilarUser(GeneralUser user, double similarity)
        {
            friend = user;
            this.similarity = similarity;
        }

        public int getId()
        {
            return friend.UserId;
        }

        public string getName()
        {
            return friend.UserId.ToString();
        }

        public GeneralUser getUser()
        {
            return friend;
        }

        public double getSimilarity()
        {
            return similarity;
        }

        public class UsersComparer : IComparer<SimilarUser>
        {
            public int Compare(SimilarUser f1, SimilarUser f2)
            {
                if (f1.getSimilarity() < f2.getSimilarity())
                {
                    return 1; // reverse order
                }
                else if (f1.getSimilarity() > f2.getSimilarity())
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        //public static void sort(List<SimilarUser> similarUsers)
        //{
        //    UsersComparer uc = new UsersComparer();
        //    similarUsers.Sort(uc);
        //}

        public static List<SimilarUser> getTopNFriends(List<SimilarUser> similarUsers, int topN)
        {
            // Отсортировать друзей на основе элементов (itemAgreement)
            UsersComparer uc = new UsersComparer();
            similarUsers.Sort(uc);

            // Выбрать топ N друзей
            List<SimilarUser> topFriends = new List<SimilarUser>();
            for (int i = 0; i < similarUsers.Count; i++)
            {
                // Проверка достаточно ли друзей у этого пользователя
                if (topFriends.Count >= topN)
                { 
                    break;
                }

                if (!topFriends.Contains(similarUsers[i]))
                {
                    topFriends.Add(similarUsers[i]);
                }
            }

            return topFriends.ToList();
        }

        public static void print(List<SimilarUser> friends, String header)
        {
            Console.WriteLine("\n" + header + "\n");
            friends.ForEach(delegate(SimilarUser f)
            {
                Console.WriteLine("Name: " + f.getName());
                Console.WriteLine("Similarity: " + f.getSimilarity());
            });
        }
    }
}
