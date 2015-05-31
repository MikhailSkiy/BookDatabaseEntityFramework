using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.model
{
    public interface IDataset
    {
        // Полное количество рейтингов элементов
        int getRatingsCount();

        // Полное количество доступных пользователей
        int getUserCount();

        // Полное количество доступных элементов
       int getItemCount();

        // Получает количество всех пользователей 
        List<User> getUsers();

        // Получает количество всех элементов
        List<Book> getItems();
    }
}
