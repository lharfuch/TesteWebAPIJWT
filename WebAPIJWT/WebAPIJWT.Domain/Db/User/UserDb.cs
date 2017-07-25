using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIJWT.Domain.Business.User.Entities;

namespace WebAPIJWT.Domain.Db.User
{
    public class UserDb : DbContext
    {
        public UserDb() : base("DefaultConnection")
        {

        }
        public DbSet<UserAccount> Users { get; set; }

        //DbSet<UserAccount> _Users = null;
        //public UserDb() : base("DefaultConnection")
        //{
        //    Database.SetInitializer<UserDb>(null);
        //    _Users = base.Set<UserAccount>();
        //}
        ////private DbSet<UserAccount> Users { get; set; }

        //public UserAccount GetUser(UserAccount Usuario)
        //{
        //    UserAccount _Usuario = null;
        //    //Apenas para Teste - Não autenticar dessa forma
        //    _Usuario = _Users.Where(m => m.UserName == Usuario.UserName)
        //                        .Where(m => m.Password == Usuario.Password)
        //                        .FirstOrDefault();
        //    return _Usuario;
        //}

        //public void AddUser(UserAccount Novo)
        //{
        //    _Users.Add(Novo);

        //    SaveChanges();
        //}

    }
}
