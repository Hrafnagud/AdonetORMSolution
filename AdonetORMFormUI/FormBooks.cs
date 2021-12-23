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
using AdonetORMEntities;
using AdonetORMEntities.Entities;
using AdonetORMEntities.ViewModals;

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
            BringAllBooksToGridWithViewModal();
            BringAllBooksToComboDelete();
            BringAllBooksToComboUpdate();

        }

        private void BringAllBooksToComboUpdate()
        {
            comboBoxBookUpdate.DisplayMember = "BookName";
            comboBoxBookUpdate.ValueMember = "BookId";
            comboBoxBookUpdate.DataSource = booksORM.Select();
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

            comboUpdateGenre.DisplayMember = "GenreName";
            comboUpdateGenre.ValueMember = "GenreId";
            comboUpdateGenre.DataSource = genreORM.BringGenre();
        }

        private void BringAllAuthorsToCombo()
        {
            comboAddAuthor.DisplayMember = "AuthorFullName";
            comboAddAuthor.ValueMember = "AuthorId";
            comboAddAuthor.DataSource = authorsORM.BringAuthorsAsListFullNameTrim();
            comboUpdateAuthor.DisplayMember = "AuthorFullName";
            comboUpdateAuthor.ValueMember = "AuthorId";
            comboUpdateAuthor.DataSource = authorsORM.BringAuthorsAsListFullNameTrim();

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
                    BringAllBooksToGridWithViewModal();
                    AddPageCleanControls();
                    //All book should be brought to comboboxupdate and delete.
                    BringAllBooksToComboUpdate();
                    BringAllBooksToComboDelete();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddPageCleanControls()
        {
            textAddBookName.Clear();
            comboAddAuthor.SelectedIndex = DefaultControls.DefaultIndex;
            comboAddGenre.SelectedIndex = DefaultControls.DefaultIndex;
            numericUpDownAddPages.Value = DefaultControls.DefaultValue;
            numericUpDownAddStock.Value = DefaultControls.DefaultValue;
        }

        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            try
            {
                if ((int)comboRemoveBook.SelectedValue <= 0)
                {
                    MessageBox.Show("Please select a book!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }

                Book myBook = booksORM.SelectET((int)comboRemoveBook.SelectedValue);


                DialogResult response = MessageBox.Show($"Do you want to remove the book from list instead of deleting completely?","Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (response == DialogResult.Yes)
                {   //IsPassive = 1; This task will be performed with Update method
                    myBook.IsPassive = true;
                    switch (booksORM.Update(myBook))
                    {
                        case true:
                            MessageBox.Show($"{myBook.BookName} has been successfully removed from the list (passive in database).");
                            DeletePageCleanControls();
                            break;
                        case false:
                            throw new Exception($"Unexpected error has occured during book {myBook.BookName} update");
                            //No need to break since code will never reach there!
                    }
                }
                else if (response ==  DialogResult.No)
                {
                    var loanList = LoanBookORM.Current.Select().Where(x => x.BookId == myBook.BookId).ToList();
                    if (loanList.Count > 0)
                    {
                        MessageBox.Show("ERROR: This book has been loaned. You are not allowed to remove!", "WARNING", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        return;
                    }   //loaned books can't be removed. If count is <= 0 code will keep continue and won't execute return.

                    if (booksORM.Delete(myBook))
                    {
                        MessageBox.Show($"Book '{myBook.BookName}' has been successfully deleteted.");
                        DeletePageCleanControls();
                        BringAllBooksToComboDelete();
                        BringAllBooksToComboUpdate();
                        BringAllBooksToGridWithViewModal();
                       
                    }
                    else
                    {
                        throw new Exception($"Book '{myBook.BookName}' has not been deleted");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error has occured during book deletion!\n" + ex.Message);
            }
        }

        private void DeletePageCleanControls()
        {
            comboRemoveBook.SelectedIndex = -1;
            richTextBoxBook.Clear();
        }

        private void UpdatePageCleanControls()
        {
            textUpdateBookName.Text = string.Empty;
            numericUpDownAddPages.Value = 0;
            numericUpDownAddStock.Value = 0;
            comboUpdateGenre.SelectedIndex = -1;
            comboUpdateAuthor.SelectedIndex = -1;
        }

        private void comboBoxBookUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdatePageCleanControls();
                if (comboBoxBookUpdate.SelectedIndex >= 0)
                {
                    Book chosenBook = booksORM.SelectET((int)comboBoxBookUpdate.SelectedValue);
                    textUpdateBookName.Text = chosenBook.BookName;
                    numericUpDownAddPages.Value = chosenBook.Pages;
                    numericUpDownAddStock.Value = chosenBook.Stock;
                    comboUpdateAuthor.SelectedValue = chosenBook.AuthorId;
                    if (chosenBook.GenreId == null)
                    {
                        //comboUpdateGenre.SelectedIndex = -1;
                        //This statement is alternative but to make it more professional we can use following approach
                        comboUpdateGenre.SelectedValue = DefaultControls.DefaultIndex;

                    }
                    else
                    {
                        comboUpdateGenre.SelectedValue = chosenBook.GenreId;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void btnUpdateBook_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxBookUpdate.SelectedIndex >= 0)
                {
                    if (numericUpDownPagesUpdate.Value <= 0)
                    {
                        throw new Exception("ERROR: Page number must be greater than zero!");
                    }

                    if (numericUpDownUpdateStoc.Value <= 0)
                    {
                        throw new Exception("ERROR: Stock must be greater than zero!");
                    }
                }

                Book chosenBook = booksORM.SelectET((int)comboBoxBookUpdate.SelectedValue);
                if (chosenBook == null)
                {
                    throw new Exception("ERROR: Book not found!");
                    //or
                    //MessageBox.Show("ERROR: Book not found!");
                    //return;
                }

                chosenBook.BookName = textUpdateBookName.Text.Trim();
                chosenBook.Pages = (int)numericUpDownPagesUpdate.Value;
                chosenBook.Stock = (int)numericUpDownUpdateStoc.Value;
                chosenBook.IsPassive = false;
                chosenBook.AuthorId = (int)comboUpdateAuthor.SelectedValue;

                if ((int)comboUpdateGenre.SelectedValue == -1)
                {
                    chosenBook.GenreId = null;
                }
                else
                {
                    chosenBook.GenreId = (int)comboUpdateGenre.SelectedValue;
                }

                switch (booksORM.Update(chosenBook))
                {
                    case true:
                        MessageBox.Show($"Book {chosenBook.BookName} has been successfully updated");
                        BringAllBooksToComboUpdate();
                        BringAllBooksToGridWithViewModal();
                        BringAllBooksToComboDelete();
                        break;

                    case false:
                        throw new Exception($"Unexpected error has occured during update of book {chosenBook.BookName}.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR " + ex.Message);
            }
        }

        private void comboRemoveBook_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Filling rich text box in delete tab.

            #region FirstApproach
            if (comboRemoveBook.SelectedIndex >= 0)
            {
                //First Approach
                Book chosenBook = booksORM.SelectET((int)comboRemoveBook.SelectedValue);

                if (chosenBook != null)
                {
                    //string genre = chosenBook.GenreId == null ? "No Genre" : genreORM.Select().FirstOrDefault(x => x.GenreId == chosenBook.GenreId).GenreName;    and pass genre inside $"Genre{} <= here. That's it!
                    richTextBoxBook.Text = $"Book: {chosenBook.BookName}\n" +
                                            $"Genre: {(chosenBook.GenreId == null ? "No Genre" : genreORM.Select().FirstOrDefault(x => x.GenreId == chosenBook.GenreId).GenreName)}\n" +
                                            $"Author: {authorsORM.Select().FirstOrDefault(x => x.AuthorId == chosenBook.AuthorId).AuthorFullName}\n" +
                                            $"Pages: {chosenBook.Pages}\n" +
                                            $"Stock: {chosenBook.Stock}";
                }

            }
            #endregion FirstApproach

            #region SecondApproach
            //if (comboRemoveBook.SelectedIndex >= 0)
            //{
            //    BookViewModal chosenBook = booksORM.BringBooksWithViewModal().FirstOrDefault(x => x.BookId == (int)comboRemoveBook.SelectedValue);
            //    if (chosenBook != null)
            //    {
            //        richTextBoxBook.Clear();
            //        richTextBoxBook.Text =  $"Book: {chosenBook.BookName}\n" +
            //                                $"Genre: {chosenBook.GenreName}\n" +
            //                                $"Author: {chosenBook.AuthorFullName}\n" +
            //                                $"Pages: {chosenBook.Pages}\n" +
            //                                $"Stock: {chosenBook.Stock}";
            //    }
            //}
            #endregion SecondApproach
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            //When tabs are changed, controls will be cleaned.
            AddPageCleanControls();
            comboBoxBookUpdate.SelectedIndex = DefaultControls.DefaultIndex;
            UpdatePageCleanControls();
            DeletePageCleanControls();

        }
    }
}
