using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIJWT.Domain.Business.User.Entities;

namespace WebAPIJWT.Domain.Business.User
{
    public interface IAccount
    {
        void New(UserAccount Novo);

        UserAccount Login(UserAccount Usuario);
    }
}
