namespace KooliProjekt.Data
{
    public class Beer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public IList<Batch> Batches { get; set; }

        public Beer()
        {
            Batches = new List<Batch>();
        }
    }
}
