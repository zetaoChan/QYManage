using System;
using System.Data.Entity;

namespace Wy.Hr.Data
{
    public class DataContext : DbContext, IDisposable
    {
        public DataContext() : base("DefaultConnection") { }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<SysConfig> SysConfigs { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
        }

        #region IDisposable 成员

        void IDisposable.Dispose()
        {
            base.Dispose();
        }

        #endregion

    }
}