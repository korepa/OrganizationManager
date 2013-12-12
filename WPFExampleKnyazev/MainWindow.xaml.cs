using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
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

        public MainWindow()
        {
            InitializeComponent();
            Loaded += SearchWindow_Loaded;
        }

        void SearchWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
        }

        // function for update main datagrid after each successfull operation
        private void loadData()
        {
            var dataLayer = new DataLayer();
            var personList = dataLayer.GetAllPersons();

            dgMain.ItemsSource = personList;
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
                    var dataLayer = new DataLayer();
                    var personModel = editWindow.PersonModel;
                    var result = dataLayer.AddPerson(personModel);

                    MessageBox.Show(result ? "Сотрудник добавлен" : "Не удалось добавить сотрудника!");

                    if (result)
                        Dispatcher.BeginInvoke(DispatcherPriority.Input, (ThreadStart)loadData);
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
                var personModel = dgMain.SelectedItem as PersonModel;
                var result = false;
                if (personModel != null)
                {
                    var dataLayer = new DataLayer();
                    result = dataLayer.DeletePerson(personModel.Id);
                }

                MessageBox.Show(result ? "Сотрудник удален" : "Не удалось удалить сотрудника!");

                if (result)
                    Dispatcher.BeginInvoke(DispatcherPriority.Input, (ThreadStart)loadData);
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
                var personModel = dgMain.SelectedItem as PersonModel;
                if (personModel != null)
                {
                    var editWindow = new EditWindow(this, personModel);
                    editWindow.ShowDialog();
                    if (editWindow.DialogResult == true)
                    {
                        var dataLayer = new DataLayer();
                        personModel = editWindow.PersonModel;
                        var result = dataLayer.EditPerson(personModel.Id, personModel);

                        MessageBox.Show(result ? "Данные сотрудника изменены" : "Не удалось изменить данные сотрудника!");

                        if (result)
                            Dispatcher.BeginInvoke(DispatcherPriority.Input, (ThreadStart)loadData);
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
        private void ExecutedSearchCommand(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                var dataLayer = new DataLayer();
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

                var resultList = dataLayer.SearchPerson(searchDictionary);

                if (resultList != null)
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Input, (ThreadStart)delegate { dgSearch.ItemsSource = resultList; });
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
    }


}
