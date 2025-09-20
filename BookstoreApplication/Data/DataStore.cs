using BookstoreApplication.Models;

namespace BookstoreApplication.Data
{
    public static class DataStore
    {
        public static List<Author> Authors = new()
        {
            new Author { Id = 1, FullName = "George Orwell", Biography = "British writer and journalist.", DateOfBirth = new DateTime(1903, 6, 25) },
            new Author { Id = 2, FullName = "Jane Austen", Biography = "English novelist known for romantic fiction.", DateOfBirth = new DateTime(1775, 12, 16) },
            new Author { Id = 3, FullName = "J.K. Rowling", Biography = "British author best known for Harry Potter series.", DateOfBirth = new DateTime(1965, 7, 31) },
            new Author { Id = 4, FullName = "Mark Twain", Biography = "American writer and humorist.", DateOfBirth = new DateTime(1835, 11, 30) },
            new Author { Id = 5, FullName = "Leo Tolstoy", Biography = "Russian novelist, known for War and Peace.", DateOfBirth = new DateTime(1828, 9, 9) }
        };

        public static List<Publisher> Publishers = new()
        {
            new Publisher { Id = 1, Name = "Penguin Books", Address = "80 Strand, London", Website = "https://penguin.co.uk" },
            new Publisher { Id = 2, Name = "Bloomsbury", Address = "50 Bedford Square, London", Website = "https://www.bloomsbury.com" },
            new Publisher { Id = 3, Name = "Vintage Books", Address = "New York, USA", Website = "https://www.vintagebooks.com" }
        };

        public static List<Book> Books = new()
        {
            new Book
            {
                Id = 1, Title = "1984", PageCount = 328, PublishedDate = new DateTime(1949, 6, 8), ISBN = "9780451524935",
                AuthorId = Authors[0].Id, Author = Authors[0], PublisherId = Publishers[0].Id, Publisher = Publishers[0]
            },
            new Book
            {
                Id = 2, Title = "Animal Farm", PageCount = 112, PublishedDate = new DateTime(1945, 8, 17), ISBN = "9780451526342",
                AuthorId = Authors[0].Id, Author = Authors[0], PublisherId = Publishers[0].Id, Publisher = Publishers[0]
            },
            new Book
            {
                Id = 3, Title = "Pride and Prejudice", PageCount = 432, PublishedDate = new DateTime(1813, 1, 28), ISBN = "9780141439518",
                AuthorId = Authors[1].Id, Author = Authors[1], PublisherId = Publishers[0].Id, Publisher = Publishers[0]
            },
            new Book
            {
                Id = 4, Title = "Emma", PageCount = 474, PublishedDate = new DateTime(1815, 12, 23), ISBN = "9780141439587",
                AuthorId = Authors[1].Id, Author = Authors[1], PublisherId = Publishers[2].Id, Publisher = Publishers[2]
            },
            new Book
            {
                Id = 5, Title = "Harry Potter and the Philosopher's Stone", PageCount = 223, PublishedDate = new DateTime(1997, 6, 26), ISBN = "9780747532699",
                AuthorId = Authors[2].Id, Author = Authors[2], PublisherId = Publishers[1].Id, Publisher = Publishers[1]
            },
            new Book
            {
                Id = 6, Title = "Harry Potter and the Chamber of Secrets", PageCount = 251, PublishedDate = new DateTime(1998, 7, 2), ISBN = "9780747538493",
                AuthorId = Authors[2].Id, Author = Authors[2], PublisherId = Publishers[1].Id, Publisher = Publishers[1]
            },
            new Book
            {
                Id = 7, Title = "Harry Potter and the Prisoner of Azkaban", PageCount = 317, PublishedDate = new DateTime(1999, 7, 8), ISBN = "9780747542155",
                AuthorId = Authors[2].Id, Author = Authors[2], PublisherId = Publishers[1].Id, Publisher = Publishers[1]
            },
            new Book
            {
                Id = 8, Title = "Adventures of Huckleberry Finn", PageCount = 366, PublishedDate = new DateTime(1884, 12, 10), ISBN = "9780142437179",
                AuthorId = Authors[3].Id, Author = Authors[3], PublisherId = Publishers[2].Id, Publisher = Publishers[2]
            },
            new Book
            {
                Id = 9, Title = "The Adventures of Tom Sawyer", PageCount = 274, PublishedDate = new DateTime(1876, 6, 1), ISBN = "9780143039563",
                AuthorId = Authors[3].Id, Author = Authors[3], PublisherId = Publishers[0].Id, Publisher = Publishers[0]
            },
            new Book
            {
                Id = 10, Title = "War and Peace", PageCount = 1225, PublishedDate = new DateTime(1869, 1, 1), ISBN = "9780140447934",
                AuthorId = Authors[4].Id, Author = Authors[4], PublisherId = Publishers[2].Id, Publisher = Publishers[2]
            },
            new Book
            {
                Id = 11, Title = "Anna Karenina", PageCount = 864, PublishedDate = new DateTime(1877, 4, 1), ISBN = "9780143035008",
                AuthorId = Authors[4].Id, Author = Authors[4], PublisherId = Publishers[2].Id, Publisher = Publishers[2]
            },
            new Book
            {
                Id = 12, Title = "The Death of Ivan Ilyich", PageCount = 86, PublishedDate = new DateTime(1886, 1, 1), ISBN = "9780553210354",
                AuthorId = Authors[4].Id, Author = Authors[4], PublisherId = Publishers[0].Id, Publisher = Publishers[0]
            }
        };

        public static int GetNewAuthorId()
        {
            return Authors.Count() > 0 ? Authors.Max(x => x.Id) + 1 : 1;
        }

        public static int GetNewPublisherId()
        {
            return Publishers.Count() > 0 ? Publishers.Max(x => x.Id) + 1 : 1;
        }

        public static int GetNewBookId()
        {
            return Books.Count() > 0 ? Books.Max(x => x.Id) + 1 : 1;
        }
    }
}
