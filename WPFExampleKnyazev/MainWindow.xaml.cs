using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WPFExampleKnyazev
{
    public partial class MainWindow
    {
        // add command
        public static RoutedCommand AddCommand = new RoutedCommand();
        // delete command
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        // edit command
        public static RoutedCommand EditCommand = new RoutedCommand();
        // search command
        public static RoutedCommand SearchCommand = new RoutedCommand();

        // object model for person
        public DataAccess dataLayer = new DataAccess();

        public MainWindow()
        {
            InitializeComponent();
            dataLayer.Initialize();
            DataContext = dataLayer;
        }

        // add command 
        private void ExecutedAddCommand(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var editWindow = new EditWindow(this, null);
                editWindow.ShowDialog();

                if (editWindow.DialogResult == true)
                {
                    var personModel = editWindow.PersonDTOEdited;
                    dataLayer.AddPerson(personModel);

                    // update status of operation
                    updateStatusAnimation("Сотрудник добавлен");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления нового сотрудника!", MessageBoxButton.OK);
            }
        }

        private void CanExecuteAddCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // delete command 
        private void ExecutedDeleteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var personModel = dgMain.SelectedItem as PersonDTO;
                if (personModel != null)
                {
                    dataLayer.DeletePerson(personModel);
                }

                // update status of operation
                updateStatusAnimation("Сотрудник удален");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка удаления сотрудника!", MessageBoxButton.OK);
            }
        }

        private void CanExecuteDeleteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = dgMain.SelectedItem != null;
        }

        // edit command
        private void ExecutedEditCommand(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var personModel = dgMain.SelectedItem as PersonDTO;
                if (personModel != null)
                {
                    var editWindow = new EditWindow(this, personModel);
                    editWindow.ShowDialog();
                    if (editWindow.DialogResult == true)
                    {
                        dataLayer.EditPerson(editWindow.PersonDTOEdited);
                        dgMain.SelectedItem = null;

                        // update status of operation
                        updateStatusAnimation("Данные сотрудника изменены");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка изменения данных сотрудника!", MessageBoxButton.OK);
            }
        }

        private void CanExecuteEditCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = dgMain.SelectedItem != null;
        }

        // search command
        private async void ExecutedSearchCommand(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var searchDictionary = new Dictionary<string, object>();

                // if search fields not empty, add parameters to dictionary
                if (tbxFirstName.Text != "")
                    searchDictionary["FirstName"] = tbxFirstName.Text;
                if (tbxMiddleName.Text != "")
                    searchDictionary["MiddleName"] = tbxMiddleName.Text;
                if (tbxSecondName.Text != "")
                    searchDictionary["SecondName"] = tbxSecondName.Text;
                if (tbxPosition.Text != "")
                    searchDictionary["Position"] = tbxPosition.Text;
                if (tbxDepartment.Text != "")
                    searchDictionary["Department"] = tbxDepartment.Text;
                if (dpDateOfBirth.SelectedDate != null)
                    searchDictionary["DateOfBirth"] = dpDateOfBirth.SelectedDate;

                dataLayer.SearchPerson(searchDictionary);

                var searchCount = dataLayer.PersonDTOSearch.Count;
                if (searchCount != 0)
                {
                    updateStatusAnimation(string.Format("Найдено сотрудников: {0}", searchCount));
                }
                else
                {
                    updateStatusAnimation("Не найдено ни одного сотрудника");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка поиска данных сотрудника!", MessageBoxButton.OK);
            }
        }

        private void CanExecuteSearchCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            // if all search parameters empty, then block search button
            if (tbxFirstName.Text != "" ||
                tbxMiddleName.Text != "" ||
                tbxSecondName.Text != "" ||
                tbxPosition.Text != "" ||
                tbxDepartment.Text != "" ||
                dpDateOfBirth.SelectedDate != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        // show status change
        private void updateStatusAnimation(string statusText)
        {
            // update text
            tblkStatusMessage.Text = statusText;

            var animation1 = new DoubleAnimation();
            animation1.AutoReverse = true;
            animation1.From = 0;
            animation1.To = 1;
            animation1.Duration = TimeSpan.FromSeconds(1.5);
            Storyboard.SetTarget(animation1, tblkStatusMessage);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(OpacityProperty));

            var storyboard = new Storyboard();
            storyboard.Children = new TimelineCollection { animation1 };

            storyboard.Begin();
        }
    }
}
