using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;
using System.Configuration;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace JunkResourceFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IOrganizationService _service { get; private set; }

        //Constants
        public const string publisherEntityName = "publisher";
        public MainWindow()
        {
            InitializeComponent();            

            // Update the Main Window with Configurations
            txtBoxServerUri.Text = ConfigurationManager.AppSettings.Get("ServerUri");
            txtBoxUserName.Text = ConfigurationManager.AppSettings.Get("UserName");
            txtBoxDomain.Text = ConfigurationManager.AppSettings.Get("Domain");
            txtBoxPassword.Password = ConfigurationManager.AppSettings.Get("Password");
        }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK || result== System.Windows.Forms.DialogResult.Yes)
                {
                    if (Directory.Exists(dialog.SelectedPath))
                    {
                        txtBoxBrowseFolder.Text = dialog.SelectedPath;
                    }else
                    {
                        txtBoxBrowseFolder.Text = string.Empty;
                    }
                }
            }
        }

        private async void btnTestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleLoader(true);
                btnTestConnection.Content = "Validating....";
                await Task.Delay(1000);
               var result = await this.Dispatcher.InvokeAsync<IOrganizationService>(() => ValidateConnection());
                await this.Dispatcher.InvokeAsync(() => BindPublisherToDropdown());
                if ((result !=null))
                {
                    MessageBox.Show("You are now connected to CRM.", "Junk Explorer", MessageBoxButton.OK,MessageBoxImage.Information);
                    grpBoxPublisherSettings.Visibility = Visibility.Visible;
                }
                                            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to Authenticate - " + ex.Message,"Junk Explorer",MessageBoxButton.OK,MessageBoxImage.Error);            
            }
            finally
            {
                btnTestConnection.Content = "Update Connection";
                ToggleLoader(false);
            }
        }


        public IOrganizationService ValidateConnection()
        {
            try
            {              
                string connectionString = string.Format("Url={0}; Domain={1}; Username={2}; Password={3};", txtBoxServerUri.Text, txtBoxDomain.Text, txtBoxUserName.Text, txtBoxPassword.Password);
                CrmConnection connection = CrmConnection.Parse(connectionString);
                _service = new OrganizationService(connection);
                Guid userid = ((WhoAmIResponse)_service.Execute(new WhoAmIRequest())).UserId;               
            }
            catch (Exception)
            {

                throw;
            }

            return _service;
        }

        public async Task BindPublisherToDropdown()
        {
            if (_service == null) return;
            List<ComboBoxPairs> itemSource = new List<ComboBoxPairs>();
            try
            {
                QueryExpression queryExp = new QueryExpression(publisherEntityName);
                queryExp.ColumnSet = new ColumnSet("friendlyname", "customizationprefix");
                queryExp.Distinct = true;
                await Task.Yield();
                var response = _service.RetrieveMultiple(queryExp).Entities;

                foreach(var record in response)
                {
                    ComboBoxPairs newPair = new ComboBoxPairs(record.GetAttributeValue<string>("friendlyname"), record.GetAttributeValue<string>("customizationprefix"));
                    itemSource.Add(newPair);                   
                }
                cmmbBoxPublisherList.ItemsSource = itemSource;                             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void ToggleLoader(bool show)
        {
            if(show)
            {
                this.IsEnabled = false;
                progressBar.Visibility = Visibility.Visible;
            }else
            {
                this.IsEnabled = true;
                progressBar.Visibility = Visibility.Collapsed;
            }

        }

        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            if(txtBoxBrowseFolder.Text==string.Empty)
            {
                txtBoxBrowseFolder.BorderBrush = Brushes.Red;
                txtBoxBrowseFolder.Focus();
                return;
            }
            if(cmmbBoxPublisherList.SelectedIndex==-1)
            {
                cmmbBoxPublisherList.BorderBrush = Brushes.Red;
                cmmbBoxPublisherList.Focus();
                return;
            }

            this.Hide();
            string pubPrefix = cmmbBoxPublisherList.SelectedValue.ToString();
            ExecuteWindow execWindow = new ExecuteWindow(_service,txtBoxBrowseFolder.Text,pubPrefix);
            execWindow.ShowDialog();
            this.Show();
        }
    }

    public class ComboBoxPairs
    {
        public string _Key { get; set; }
        public string _Value { get; set; }

        public ComboBoxPairs(string _key, string _value)
        {
            _Key = _key;
            _Value = _value;
        }
    }
}
