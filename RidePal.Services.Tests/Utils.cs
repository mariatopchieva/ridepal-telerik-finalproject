using Microsoft.EntityFrameworkCore;
using RidePal.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Services.Tests
{
    public class Utils
    {
        public static DbContextOptions<RidePalDbContext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<RidePalDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }
    }
}
