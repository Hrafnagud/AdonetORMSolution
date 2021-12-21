﻿using AdonetORMCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdonetORMEntities.Entities
{
    [Table(PrimaryColumn = "CategoryID", TableName = "Categories", IdentityColumn = "CategoryID")]

    public class Category
    {
        //
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public override string ToString()
        {
            return CategoryName;
        }
    }
}
