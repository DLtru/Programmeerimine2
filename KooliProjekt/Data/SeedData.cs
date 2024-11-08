using KooliProjekt.Data;

public static class SeedData
{
    public static void Generate(ApplicationDbContext context)
    {
        if (context.Ingredients.Any())
        {
            return;
        }

        var ingredient1 = new Ingredient();
        ingredient1.Name = "";
        ingredient1.Unit = "";
        ingredient1.UnitPrice = 100;
        ingredient1.Quantity = 1;
        ingredient1.AmountUsed = 100;

        var beer1 = new Beer();
        beer1.Name = "";
        beer1.Description = "";

        var batch1 = new Batch();
        batch1.Date = DateTime.Now;
        batch1.Code = "";
        batch1.Description = "";

        var logentry1 = new LogEntry();
        logentry1.Date = DateTime.Now;
        logentry1.Description = "";
        logentry1.User = "";
        logentry1.UserId = "";

        var photo1 = new Photo();
        photo1.Url = "";
        photo1.Description = "";

        var tastingentry1= new TastingEntry();
        tastingentry1.Date = DateTime.Now;
        tastingentry1.Rating = "";
        tastingentry1.Comments = "";
        tastingentry1.BatchId = 1;


        context.TodoLists.Add(list);

        // Veel andmeid

        context.SaveChanges();
    }
}
