using System;
using System.Windows;

namespace WPFExampleKnyazev
{
    public partial class EditWindow
    {
        private PersonModel personModel;
        public PersonModel PersonModel { get { return personModel; } }

        public EditWindow(Window _owner, PersonModel _model)
        {
            InitializeComponent();
            Owner = _owner;

            personModel = new PersonModel();
            if (_model != null)
            {
                personModel.Id = _model.Id;
                personModel.InnerId = _model.InnerId;
                personModel.FirstName = _model.FirstName;
                personModel.MiddleName = _model.MiddleName;
                personModel.SecondName = _model.SecondName;
                personModel.Position = _model.Position;
                personModel.Department = _model.Department;
                personModel.DateOfBirth = _model.DateOfBirth;
            }
            else
            {
                personModel.FirstName = "";
                personModel.MiddleName = "";
                personModel.SecondName = "";
                personModel.Position = "";
                personModel.Department = "";
                personModel.DateOfBirth = new DateTime(1990, 1, 1);
            }

            loadData();
        }

        // load data from input parameter
        private void loadData()
        {
            tbxFirstName.DataContext =
                tbxMiddleName.DataContext =
                tbxSecondName.DataContext =
                tbxPosition.DataContext =
                tbxDepartment.DataContext =
                dpDateOfBirth.DataContext
                = personModel;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            // check model state
            if (personModel.FirstName == "" ||
                personModel.SecondName == "" ||
                personModel.Position == "" ||
                personModel.Department == "" ||
                personModel.DateOfBirth == DateTime.MinValue)
            {
                // if some parameter invalid
                MessageBox.Show("Не удается добавить сотрудника. Не все поля заполнены!" +
                    Environment.NewLine + "(отчество - необязательный параметр)");
                return;
            }

            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // just cancel operation
            DialogResult = false;
        }
    }
}
