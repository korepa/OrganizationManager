using System;
using System.Windows;

namespace WPFExampleKnyazev
{
    public partial class EditWindow
    {
        private PersonDTO personDTOEdited;
        public PersonDTO PersonDTOEdited { get { return personDTOEdited; } }

        public EditWindow(Window owner, PersonDTO personDTO)
        {
            InitializeComponent();
            Owner = owner;

            personDTOEdited = new PersonDTO();
            if (personDTO != null)
            {
                personDTOEdited.Id = personDTO.Id;
                personDTOEdited.FirstName = personDTO.FirstName;
                personDTOEdited.MiddleName = personDTO.MiddleName;
                personDTOEdited.SecondName = personDTO.SecondName;
                personDTOEdited.Position = personDTO.Position;
                personDTOEdited.Department = personDTO.Department;
                personDTOEdited.DateOfBirth = personDTO.DateOfBirth;
            }
            else
            {
                personDTOEdited.FirstName = "";
                personDTOEdited.MiddleName = "";
                personDTOEdited.SecondName = "";
                personDTOEdited.Position = "";
                personDTOEdited.Department = "";
                personDTOEdited.DateOfBirth = new DateTime(1990, 1, 1);
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
                = personDTOEdited;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            // check model state
            if (personDTOEdited.FirstName == "" ||
                personDTOEdited.SecondName == "" ||
                personDTOEdited.Position == "" ||
                personDTOEdited.Department == "" ||
                personDTOEdited.DateOfBirth == DateTime.MinValue)
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
