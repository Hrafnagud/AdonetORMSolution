using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdonetORM_BLL;
using AdonetORMEntities.Entities;

namespace AdonetORMFormUI
{
    public partial class FormBooks : Form
    {
        public FormBooks()
        {
            InitializeComponent();
        }

        //Global
        AuthorsORM authorsORM = new AuthorsORM();
        GenreORM genreORM = new GenreORM();
        BooksORM booksORM = new BooksORM();

        private void FormBooks_Load(object sender, EventArgs e)
        {
            BringAllAuthorsToCombo();
            BringAllGenreToCombo();
            //BringAllBooksToGridWithViewModal();
            BringAllBooksToComboDelete();

        }

        private void BringAllBooksToComboDelete()
        {
            comboRemoveBook.DisplayMember = "BookName";
            comboRemoveBook.ValueMember = "BookId";
            //comboRemoveBook.DataSource = booksORM.Select();
            comboRemoveBook.DataSource = BooksORM.Current.Select();
        }

        private void BringAllBooksToGridWithViewModal()
        {
            dataGridViewBooks.DataSource = booksORM.BringBooksWithViewModal();
            dataGridViewBooks.Columns["IsPassive"].Visible = false;
            dataGridViewBooks.Columns["GenreId"].Visible = false;
            dataGridViewBooks.Columns["AuthorId"].Visible = false;
            for (int i = 0; i < dataGridViewBooks.Columns.Count; i++)
            {
                dataGridViewBooks.Columns[i].Width = 120;
            }
        }

        private void BringAllGenreToCombo()
        {
            comboAddGenre.DisplayMember = "GenreName";
            comboAddGenre.ValueMember = "GenreId";
            comboAddGenre.DataSource = genreORM.BringGenre();
            comboAddGenre.SelectedIndex = 0;
        }

        private void BringAllAuthorsToCombo()
        {
            comboAddAuthor.DisplayMember = "AuthorFullName";
            comboAddAuthor.ValueMember = "AuthorId";

            var l= authorsORM.BringAuthorsAsListFullNameTrim();
            comboAddAuthor.DataSource = authorsORM.BringAuthorsAsListFullNameTrim();

        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (numericUpDownAddPages.Value <= 0)
                {
                    MessageBox.Show("ERROR: Page number must be greater than zero!");
                    return;
                }
                if (numericUpDownAddStock.Value <= 0)
                {
                    MessageBox.Show("ERROR: Stock left must be greater than zero!");
                    return;
                }

                if ((int)comboAddAuthor.SelectedValue <= 0)
                {
                    MessageBox.Show("ERROR: Book must have an Author. Choose an Author!");
                    return;
                }

                Book newBook = new Book()
                {
                    RegisterDate = DateTime.Now,
                    BookName = textAddBookName.Text.Trim(),
                    Pages = (int)numericUpDownAddPages.Value,
                    Stock = (int)numericUpDownAddStock.Value,
                    IsPassive = false,
                    AuthorId = (int)comboAddAuthor.SelectedValue
                };

                //Check whether GenreId is null or not.
                if ((int)comboAddGenre.SelectedValue == -1)
                {
                    newBook.GenreId = null;
                }
                else
                {
                    newBook.GenreId = (int)comboAddGenre.SelectedValue;
                }

                if (booksORM.Insert(newBook))
                {
                    MessageBox.Show($"New book '{newBook.BookName}' has been added successfully");
                    //BringAllBooksToGridWithViewModal();
                    //Cleaning
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult response = MessageBox.Show($"Do you want to remove the book from list instead of deleting completely?","Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (response == DialogResult.Yes)
                {   //IsPassive = 0;
                    
                }
                else if (response ==  DialogResult.No)
                {
                    int bookID = (int)comboRemoveBook.SelectedValue;
                    var loanList = LoanBookORM.Current.Select().Where(x => x.BookId == bookID).ToList();
                    if (loanList.Count > 0)
                    {
                        MessageBox.Show("ERROR: This book has been loaned. You are not allowed to remove!", "WARNING", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        return;
                    }   //loaned books can't be removed. If count is <= 0 code will keep continue and won't execute return.

                    Book myBook = booksORM.SelectET(bookID);
                    if (booksORM.Delete(myBook))
                    {
                        MessageBox.Show($"Book '{myBook.BookName}' has been successfully deleteted.");
                       
                    }
                    else
                    {
                        throw new Exception($"Book '{myBook.BookName}' has not been deleted");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error has occured during book deletion!");
            }
        }
    }
}
