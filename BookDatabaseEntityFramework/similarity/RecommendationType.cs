using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDatabaseEntityFramework.similarity
{

       public enum RecommendationType
        {
            USER_BASED,
            ITEM_BASED,
            IMPROVED_USER_BASED,
            ITEM_PENALTY_BASED,
            USER_CONTENT_BASED,
            ITEM_CONTENT_BASED,
            USER_ITEM_CONTENT_BASED
        }    
}
