using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppointmentScheduler_MarcinJunka.Data;
using AppointmentScheduler_MarcinJunka.Managers;
using AppointmentScheduler_MarcinJunka.Models;
using Microsoft.Win32;

namespace AppointmentScheduler_MarcinJunka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PatientManager patientManager = new PatientManager();
        AppointmentManager appointmentManager = new AppointmentManager();
        public MainWindow()
        {
            InitializeComponent();
            InitializeGui();
            FirstInitializeButtons();
        }

        private void InitializeGui()
        {
            this.Title = "Appointment Scheduler by Marcin Junka";
            datePicker.DisplayDateStart = DateTime.Now;
            datePicker.DisplayDateEnd = DateTime.Now.AddDays(7);

            comboService.ItemsSource = Enum.GetValues(typeof(ServicesEnum));
            comboService.SelectedIndex = 0;

            foreach (Button button in btnContainer.Children.OfType<Button>())
            {
                button.Background = Brushes.LightGreen;
                //MessageBox.Show(button.Content.ToString());
                //if (button.Content.ToString() == "12:00")
                //{
                //}
            }
        }

        private void FirstInitializeButtons()
        {
            foreach (Button button in btnContainer.Children.OfType<Button>())
            {
                button.Click += Button_Time_Click;
            }
        }

        private void Button_Time_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            Patient selectedPatient = (Patient)lstCreatedPatientsList.SelectedItem;

            if (datePicker.SelectedDate == null)
            {
                MessageBox.Show("You need to pick the date!");
                return;
            }

            if (selectedPatient == null)
            {
                MessageBox.Show("You need to select the patient!");
                return;
            }

            Appointment createdAppointment = new Appointment()
            {
                Date = datePicker.SelectedDate.Value,
                Time = button.Content.ToString(),
                Patient = selectedPatient,
                Service = (ServicesEnum)comboService.SelectedItem,
            };

            List<Appointment> appointmentsToDate = appointmentManager
                .GetAllAppointments().Where(x => x.Date == createdAppointment.Date).ToList();
            
            if (appointmentsToDate != null)
            {
                var isAppointmentThatTime = appointmentsToDate.FirstOrDefault(x => x.Time == createdAppointment.Time);
                if (isAppointmentThatTime != null)
                {
                    MessageBox.Show("The time is not available, pick another time.");
                    return;
                }
            }


            appointmentManager.AddToList(createdAppointment);
            RefreshChosenPatientAppointmentList();
            RefreshAllAppointments();
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            var selectedDate = datePicker.SelectedDate;
            if (selectedDate == null) return;

            List <Appointment> appointments = appointmentManager.GetAllAppointments();
            var appointmentsThatDate = appointments.Where(a => a.Date == selectedDate);

            foreach (Button button in btnContainer.Children.OfType<Button>())
            {
                string content = button.Content.ToString();
                Appointment isThenAppointment = appointmentsThatDate.FirstOrDefault(x => x.Time == content);

                if (isThenAppointment == null )
                {
                    button.Background = Brushes.LightGreen;
                }
                else
                {
                    button.Background = Brushes.Red;
                }
            }
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshButtons();
        }



        private void btnCreatePatient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtPhone.Text))
            {
                MessageBox.Show("All patient data must be provided!");
                return;
            }

            string phone = txtPhone.Text;

            if (Regex.IsMatch(phone, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers in phone field.");
                return;
            }


            if (checkInvalid.IsChecked == true)
            {
                SpecialPatient patient = new SpecialPatient(txtName.Text, txtEmail.Text, txtPhone.Text);
                patientManager.addPatientToList(patient);
            }
            else
            {
                Patient patient = new Patient(txtName.Text, txtEmail.Text, txtPhone.Text);
                patientManager.addPatientToList(patient);
            }

            RefreshPatientsList();
        }


        private void RefreshPatientsList()
        {
            lstCreatedPatientsList.ItemsSource = null;
            List<Patient> patients = new List<Patient>();
            for (int i = 0; i < patientManager.Count; i++)
            {
                Patient patient = patientManager.getPatientById(i);
                if (patient != null)
                {
                    patients.Add(patient);
                }
            }

            lstCreatedPatientsList.ItemsSource = patients;
        }

        private void RefreshAllAppointments()
        {
            lstAllPatientsAppointments.ItemsSource = null;
            List<Appointment> appointments = appointmentManager.GetAllAppointments();
            lstAllPatientsAppointments.ItemsSource = appointments;
        }

        private void RefreshChosenPatientAppointmentList()
        {
            lstChosenPatientAppointments.ItemsSource = null;
            Patient selectedPatient = (Patient)lstCreatedPatientsList.SelectedItem;
            List<Appointment> chosenPatientAppointments = appointmentManager.GetPatientAppointments(selectedPatient);

            if (selectedPatient != null && chosenPatientAppointments != null)
            {
                lstChosenPatientAppointments.ItemsSource = chosenPatientAppointments;
            }

        }

        private void lstCreatedPatientsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Patient selectedPatient = lstCreatedPatientsList.SelectedItem as Patient;

            if (selectedPatient != null)
            {
                txtChosenPatient.Text = selectedPatient.Name;
            }

            //datePicker.SelectedDate = null;

            // refresh appointment list when changing selected patient
            RefreshChosenPatientAppointmentList();
        }

        private async void btnSavePatientsList_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            bool dialogResult = saveFileDialog.ShowDialog().Value;
            if (dialogResult == true)
            {
                string path = saveFileDialog.FileName;
                bool saveOk = await FileManager<Patient>.SaveListToJson(patientManager.getPatients(),path);

                if (saveOk)
                {
                    MessageBox.Show("File saved at: \n" +path);
                }
            }
        }

        private async void btnLoadPatientsList_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            bool dialogResult = openFileDialog.ShowDialog().Value;

            if (dialogResult == true)
            {
                string path = openFileDialog.FileName;
                (bool isOk,List<Patient> listOfPatients) = await FileManager<Patient>.LoadListFromJson(path);

                if (isOk && listOfPatients != null)
                {
                    MessageBox.Show("Loaded from: \n" + path);
                    lstCreatedPatientsList.ItemsSource = listOfPatients;
                }
            }
        }

        private void btnGetPatientDetails_Click(object sender, RoutedEventArgs e)
        {
            // I intended to use tuple here but I used tuple in generic class FileManager in LoadListFromJson method.
            Appointment appointment = (Appointment)lstAllPatientsAppointments.SelectedItem;

            if (appointment != null)
            {
                var patientDetails = appointment.Patient.GetPatientDetails();
                MessageBox.Show($"Name: {patientDetails.Item1} \nEmail: {patientDetails.Item2} \nPhone: {patientDetails.Item3} \nId: {patientDetails.Item4}","Patient Details");
            }
        }
    }
}
