using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdonetORMCommon;
using AdonetORMEntities.Entities;

namespace AdonetORM_BLL
{
    public class GenreORM : ORMBase<Genre, GenreORM>
    {
        public List<Genre> BringGenre()
        {
            try
            {
                List<Genre> list = new List<Genre>();
                Genre genre = new Genre()
                {
                    GenreId = -1,
                    GenreName = "No Genre",
                    UpdateDate = DateTime.Now
                };
                list.Add(genre);
                list.AddRange(this.Select());
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
