using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShauliBlog.Models
{
    public class BlogDBContext : DbContext
    {
        public DbSet<Fan> Fan { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<AprioriAlgorithm> Apriori { get; set; }

    }
}