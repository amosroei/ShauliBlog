using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShauliBlog.Models
{
    public class Fan
    {
        [Key]
        public int FanID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthDate { get; set; }
        public int Seniority { get; set; }
    }
    //public class FanDBContext : DbContext
    //{
    //    public DbSet<Fan> Fan { get; set; }
    //    public DbSet<Post> Post { get; set; }
    //    public DbSet<Comment> Comment { get; set; }

    //    //protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    //{
    //    //    Database.SetInitializer<FanDBContext>(null);
    //    //    base.OnModelCreating(modelBuilder);
    //    //}
    //}
}
