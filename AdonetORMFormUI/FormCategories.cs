using AdonetORMEntities.Entities;
using AdonetORM_BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdonetORMFormUI
{
    public partial class FormCategories : Form
    {
        public FormCategories()
        {
            InitializeComponent();
        }

        //Global
        List<Category> categoryList = new List<Category>();
        CategoriesORM myCategoriesORM = new CategoriesORM();

        private void FormCategories_Load(object sender, EventArgs e)
        {
            FillGridWithAllCategories();
        }

        private void FillGridWithAllCategories()
        {
            categoryList = myCategoriesORM.Select();
            dataGridViewCategories.DataSource = categoryList;
        }
    }
}
