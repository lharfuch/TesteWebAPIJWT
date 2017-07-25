using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIJWT.Domain.Business.User.Entities;
using WebAPIJWT.Domain.Db.User;

namespace WebAPIJWT.Domain.Business.User
{
    public class Account : IAccount
    {
        UserDb _db = null;
        public Account(UserDb db)
        {
            _db = db;
        }
        public UserAccount Login(UserAccount Usuario)
        {
            UserAccount _Usuario = null;
            //Apenas para Teste - Não autenticar dessa forma
            _Usuario = _db.Users.Where(m => m.UserName == Usuario.UserName)
                                .Where(m => m.Password == Usuario.Password)
                                .FirstOrDefault();
            return _Usuario;
        }

        public void New(UserAccount Novo)
        {
            UserAccount use = _db.Users.Where(m => m.UserName == Novo.UserName).FirstOrDefault();
            if (use == null)
            {
                _db.Users.Add(Novo);
                _db.SaveChanges();
            }
            else
            {
                throw new Exception("Conta já existe");
            }
        }
    }
}
