using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.model
{
    class IDataset
    {
        // Полное количество рейтингов элементов
        public int getRatingsCount();

        // Полное количество доступных пользователей
        public int getUserCount();

        // Полное количество доступных элементов
        public int getItemCount();

        // Получает количество всех пользователей 
        public List<User> getUsers();

        // Получает количество всех элементов
        public List<Book> getItems();
    }
}
