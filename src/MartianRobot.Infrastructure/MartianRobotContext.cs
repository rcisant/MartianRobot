using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MartianRobot.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using static MartianRobot.Infrastructure.MappingsIgnore;

namespace MartianRobot.Infrastructure
{
    public class MartianRobotContext : DbContext
    {
        
        public MartianRobotContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.UseSqlServer().IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=tcp:localhost;Database=IoTHub;Trusted_Connection=True;");
            }
        }

        public MartianRobotContext(DbContextOptions<MartianRobotContext> options) : base(options)
        {

        }
     
        public DbSet<MartianRobotData> MartianRobot { get; set; }
        





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
  
            modelBuilder.ApplyConfiguration(new RequestsEntityTypeConfiguration());
            


        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                bool boolIsModified = false;
                bool boolExistTimestamp = false;
                foreach (var property in entry.OriginalValues.Properties)
                {
                    if (property.Name == "Status")
                    {
                        string test = property.Name;
                    }
                    var original = entry.OriginalValues[property.Name];
                    var current = entry.CurrentValues[property.Name];
                    if (original == null) original = string.Empty;
                    if (current == null) current = string.Empty;
                    FieldsToIgnore IgnoreField;
                    if (!original.Equals(current) && !Enum.TryParse<FieldsToIgnore>(property.Name, out IgnoreField))
                    {
                        entry.Property(property.Name).IsModified = true;
                        boolIsModified = true;
                    }
                    else
                    {
                        entry.Property(property.Name).IsModified = false;
                    }
                    if (property.Name.Equals("Timestamp")){
                        boolExistTimestamp = true;
                    }
                }
                if (!boolIsModified && entry.State != EntityState.Modified && entry.State != EntityState.Added && entry.State != EntityState.Deleted)
                {
                    entry.State = EntityState.Unchanged;
                }
                else
                {
                    if (boolExistTimestamp)
                    {
                        if (entry.State == EntityState.Modified && entry.OriginalValues != null && !Guid.Equals(entry.OriginalValues["Timestamp"], entry.CurrentValues["Timestamp"]))
                        {
                            throw new Exception(entry.Entity + " - Timestamp");
                        }
                        if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                        {
                            entry.Property("Timestamp").CurrentValue = Guid.NewGuid();
                            if (entry.State == EntityState.Added)
                            {
                                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                                entry.Property("CreatedBy").CurrentValue = 1;
                            }
                            else
                            {
                                entry.Property("LastUpdatedAt").CurrentValue = DateTime.UtcNow;
                                entry.Property("LastUpdatedBy").CurrentValue = 1;
                            }

                        }
                    }
                }
                if (entry.Entity is MartianRobotData)
                {
                    if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                    {

                    }
                }


            }
        }
    //    private T CreateWithValues<T>(PropertyValues values)
    //where T : new()
    //    {
    //        T entity = new T();
    //        Type type = typeof(T);

    //        foreach (var name in values.Properties)
    //        {
    //            var property = type.GetProperty(name.Name);
    //            property.SetValue(entity, values.GetValue<object>(name.Name));
    //        }

    //        return entity;
    //    }
        private string GetCurrentUser()
        {
            return "UserName"; 

        }



    }
}
