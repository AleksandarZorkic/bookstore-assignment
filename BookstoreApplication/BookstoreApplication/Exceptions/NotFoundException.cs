namespace BookstoreApplication.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(int id) : base($"Stavka sa id={id} nije pronadjena.") { }

        public NotFoundException(string message) : base(message) { }
    }
}
