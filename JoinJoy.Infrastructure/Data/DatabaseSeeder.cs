using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JoinJoy.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace JoinJoy.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Music" },
                    new Category { Name = "Sports" }
                    // Add more categories as needed
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();

                var subcategories = new List<Subcategory>
                {
                    new Subcategory { Name = "Jazz", CategoryId = categories.Single(c => c.Name == "Music").Id },
                    new Subcategory { Name = "Rock", CategoryId = categories.Single(c => c.Name == "Music").Id },
                    new Subcategory { Name = "Football", CategoryId = categories.Single(c => c.Name == "Sports").Id },
                    new Subcategory { Name = "Basketball", CategoryId = categories.Single(c => c.Name == "Sports").Id }
                    // Add more subcategories as needed
                };

                context.Subcategories.AddRange(subcategories);
                await context.SaveChangesAsync();
            }
        }
    }
}
