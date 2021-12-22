using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdonetORMCommon;
using AdonetORMEntities.Entities;

namespace AdonetORM_BLL
{
    public class AuthorsORM : ORMBase<Author, AuthorsORM>
    {
        
        public List<Author> BringAuthorsAsListFullNameTrim()
        {
            List<Author> list = new List<Author>();
            list = this.Select();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].AuthorFullName = list[i].AuthorFullName.Trim();
                
            }

            return list;
        }
    }
}
