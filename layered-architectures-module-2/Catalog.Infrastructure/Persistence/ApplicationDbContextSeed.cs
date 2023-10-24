using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
            {
                Name = "Khortytsa",
                Description = "Ну що можна сказати, прогріває як треба. Обов'язково придбайте наше сало.",
                Amount = 20,
                Price = 2.55m,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a0/VodkaKhortytsa.jpg/330px-VodkaKhortytsa.jpg",
                Category = new Category()
                {
                    Name = "Alchocol",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a8/Common_alcoholic_beverages.jpg",
                    ParentCategory = new Category()
                    {
                        Name = "Drinks",
                        ImageUrl = "https://i.redd.it/yyr6vtruhzbb1.jpg"
                    }
                }
            }, 
                new Product
                {
                    Name = "Salo",
                    Description = "Хто сказав що ідеального раціону не існує, смачне та незабутнє сало. Найкраще смакує з луком",
                    Amount = 20,
                    Price = 2.55m,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/1/1d/Salo_with_pepper_closeup.jpg",
                    Category = new Category()
                    {
                        Name = "Meat",
                        ImageUrl = "https://www.redefinemeat.com/wp-content/uploads/2022/04/BLOG1.jpg",
                    }
                },
                new Product
                {
                    Name = "Onion",
                    Description = "Найкарщий у світі лук, зібраний бабусею на городі, ням, ням.",
                    Amount = 20,
                    Price = 2.55m,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/6/64/Zwiebel_2008-3-3-.JPG",
                    Category = new Category()
                    {
                        Name = "Vegetables",
                        ImageUrl = "https://cdn.britannica.com/17/196817-050-6A15DAC3/vegetables.jpg?w=400&h=300&c=crop",
                    }
                }
                );

            await context.SaveChangesAsync();
        }
    }
}
