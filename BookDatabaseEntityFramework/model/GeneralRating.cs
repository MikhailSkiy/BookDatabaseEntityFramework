using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.model
{
   public class GeneralRating
    {
        protected GeneralItem item;

        private int userId;
        private int itemId;
        private int rating;

        public GeneralRating(int userId, int itemId, int rating)
        {
            this.userId = userId;
            this.itemId = itemId;
            this.rating = rating;
        }

        public int getUserId()
        {
            return userId;
        }

        public void setUserId(int userId)
        {
            this.userId = userId;
        }

        public int getItemId()
        {
            return itemId;
        }

        public void setItemId(int itemId)
        {
            this.itemId = itemId;
        }

        public int getRating()
        {
            return rating;
        }

        public void setRating(int rating)
        {
            this.rating = rating;
        }

        public GeneralItem getItem()
        {
            return item;
        }

        public void setItem(GeneralItem item)
        {
            this.item = item;
        }
    }
}
