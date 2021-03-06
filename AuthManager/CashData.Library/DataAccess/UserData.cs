﻿using CashData.Library.Internal.DataAccess;
using CashData.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashData.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            SQLDataAccess sql = new SQLDataAccess();

            var p = new { Id = Id };
            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", p, "CashConnection");

            return output;
        }
    }
}
