using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDatabaseEntityFramework.model;

namespace BookDatabaseEntityFramework
{
    public class BookItem : GeneralItem
    {
        public string ISBN {get;set;}
        public string Title { get; set; }
        public string Author { get; set; }
        public int YearOfPublication { get; set; }
        public string Publisher { get; set; }
        public string ImageURLS { get; set; }
        public string ImageURLM { get; set; }
        public string ImageURLL { get; set; }

        public BookItem(int id,string ISBN, string Title, string Author,
                        int Year, string Publisher, string URLS, string URLM,
                        string URLL, List<GeneralRating> ratings)
                        : base(id,ratings)
        {
            this.ISBN = ISBN;
            this.Title = Title;
            this.Author = Author;
            this.YearOfPublication = Year;
            this.Publisher = Publisher;
            this.ImageURLS = URLS;
            this.ImageURLM = URLM;
            this.ImageURLL = URLL;
        } 

        public BookItem() { }

    }
}
