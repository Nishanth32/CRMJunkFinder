using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Threading;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Diagnostics;
using ClosedXML.Excel;

namespace JunkResourceFinder
{
    /// <summary>
    /// Interaction logic for ExecuteWindow.xaml
    /// </summary>
    public partial class ExecuteWindow : Window
    {

        public static IOrganizationService service { get; private set; }
        public static string localPath { get; set; }

        public static string pubPrefix { get; set; }    
        
        private static string[] AllFiles { get; set; }  
        
        private static int TotalResource { get; set; } 
        private static int TotalMatches { get; set; }

        private DataTable bckDatatable = null;

        public enum WebResourceType
        {
            HTML =1,
            CSS=2,
            JS=3,
            resx=4,
            PNG=5,
            none=6,
            GIF=7,
            xap=8,
        }

        public ExecuteWindow(IOrganizationService Service,string RepoPath, string prefix)
        {
            TotalResource = 0;TotalMatches = 0;
            InitializeComponent();
            service = Service;
            localPath = RepoPath;
            pubPrefix = prefix;
            CreateReport();
        }


        public async void CreateReport()
        {
            try
            {
                loadPanel.Visibility = Visibility.Visible;

                bckDatatable =await Task.Factory.StartNew<DataTable>(() => PrepareDataTable(), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                dataGrid.ItemsSource = bckDatatable.DefaultView;

                grpBoxInfo.Visibility = Visibility.Visible;
                lblTotalResource.Content = TotalResource;lblTotalMatches.Content = TotalMatches;
                lblTotalJunks.Content = TotalResource - TotalMatches;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured - " + ex.Message,"Junk File Explorer",MessageBoxButton.OK,MessageBoxImage.Error);
            } finally              
            {
                loadPanel.Visibility = Visibility.Collapsed;
            }
        }

        public DataTable PrepareDataTable()
        {
            DataTable currentDatatable = null;
            try
            {
                currentDatatable = GetDataTaleInstance();
                IEnumerable<Entity> webResources = GetWebResourcesByPublisher(pubPrefix);
                TotalResource = webResources.Count();
                foreach(var record in webResources)
                {                    
                    string DisplayName = record.GetAttributeValue<string>("displayname");
                    string name = record.GetAttributeValue<string>("name");
                   
                    int webresourceType = record.GetAttributeValue<OptionSetValue>("webresourcetype").Value;
                    string ResourceType = ((WebResourceType)webresourceType).ToString();

                    DateTime createdOnDate = record.GetAttributeValue<DateTime>("createdon");
                    Uri matchPath;
                    bool MatchFound = CheckForMatch(name,out matchPath);    if(MatchFound)
                    {
                        TotalMatches++;
                    }
                    currentDatatable.Rows.Add(DisplayName, name, ResourceType, createdOnDate, MatchFound, matchPath);                    
                }             
            }
            catch (Exception)
            {

                throw;
            }
            return currentDatatable;
        }

        public List<Entity> GetWebResourcesByPublisher(string prefix)
        {
            List<Entity> webresources = null;
            try
            {
                QueryExpression queryExpression = new QueryExpression("webresource");
                queryExpression.ColumnSet = new ColumnSet("displayname","name","createdon","webresourcetype");
                queryExpression.Criteria = new FilterExpression(LogicalOperator.And);
                 queryExpression.Criteria.AddCondition(new ConditionExpression("name", ConditionOperator.Like,prefix+"%"));
                queryExpression.NoLock = true;
                queryExpression.PageInfo = new PagingInfo() { PageNumber = 1 ,PagingCookie=null};

                var response = service.RetrieveMultiple(queryExpression);
                webresources = new List<Entity>();
                webresources.AddRange(response.Entities);
                while(response.MoreRecords)
                {
                    queryExpression.PageInfo.PageNumber++;
                    queryExpression.PageInfo.PagingCookie = response.PagingCookie;
                    response = service.RetrieveMultiple(queryExpression);
                    webresources.AddRange(response.Entities);
                }                                                     
            }
            catch (Exception ex)
            {

                throw;
            }
            return webresources;
        }

        private void grid_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            if (link.NavigateUri.AbsoluteUri != null)
            {
                Process.Start(System.IO.Path.GetDirectoryName(link.NavigateUri.AbsoluteUri));
            }
        }

        public DataTable GetDataTaleInstance()
        {
            DataTable table = new DataTable();
            table.Columns.Add("DISPLAY NAME", typeof(string));
            table.Columns.Add("NAME", typeof(string));
            table.Columns.Add("TYPE", typeof(string));
            table.Columns.Add("CREATED ON", typeof(DateTime));
            table.Columns.Add("MATCH Found", typeof(Boolean));
            table.Columns.Add("Location", typeof(Uri));

            return table;
        }

        public bool CheckForMatch(string FileFullName, out Uri MatchedPath)
        {
            if(AllFiles==null)
            {
                AllFiles = Directory.GetFiles(localPath, "*", SearchOption.AllDirectories);
                AllFiles = AllFiles.Select(x => x.Replace("\\", "/")).ToArray();
            }
            var result = AllFiles.Where(x => x.Contains(FileFullName)).FirstOrDefault();
            if (result != null) {
                MatchedPath = new Uri(result.Replace("/", "\\"));
                return true;
            }
            MatchedPath = null;
            return false;
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
                {
                    if (Directory.Exists(dialog.SelectedPath))
                    {
                        var workbook = new XLWorkbook();                       
                        var worksheet = workbook.Worksheets.Add(bckDatatable,"Report");
                        worksheet.Cell("A1").Value = "JunkFileReport";
                        workbook.SaveAs(System.IO.Path.Combine(dialog.SelectedPath, "JunkReport_" + DateTime.UtcNow.Ticks + ".xlsx"));
                    }
                }
            }
        }
    }
}
