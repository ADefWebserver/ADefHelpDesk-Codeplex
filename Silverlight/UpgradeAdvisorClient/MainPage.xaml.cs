//
// http://ADefwebserver.com
// Copyright (c) 2009
// by Michael Washington
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using UpgradeAdvisorClient.UpgradeAdvisorWebService;
using System.ServiceModel;

namespace UpgradeAdvisorClient
{
    public partial class MainPage : UserControl
    {
        #region Global Variables

        public int intPortalID;
        public int intModuleID;
        public int intUserID;
        public string strPassword;
        public string strWebServiceBase;
        public string strCurrentVersion;
        List<string> ColMethodsToCheck = new List<string>();
        int intMethodsChecked = 0;

        #endregion

        public MainPage(string PortalID, string ModuleId, string UserID, string Password, string WebServiceBase)
        {
            // Required to initialize variables
            InitializeComponent();

            intPortalID = Convert.ToInt32(PortalID);
            intModuleID = Convert.ToInt32(ModuleId);
            intUserID = Convert.ToInt32(UserID);
            strPassword = Password;
            strWebServiceBase = WebServiceBase;

            SayHello();
        }

        #region SayHello
        private void SayHello()
        {
            WebServiceSoapClient objWebServiceSoapClient = new WebServiceSoapClient();
            EndpointAddress MyEndpointAddress = new EndpointAddress(strWebServiceBase + "Webservice.asmx");
            objWebServiceSoapClient.Endpoint.Address = MyEndpointAddress;

            objWebServiceSoapClient.HelloUserCompleted += new EventHandler<HelloUserCompletedEventArgs>(objWebServiceSoapClient_HelloUserCompleted);
            objWebServiceSoapClient.HelloUserAsync(intPortalID, intModuleID, intUserID, strPassword);
        }

        void objWebServiceSoapClient_HelloUserCompleted(object sender, HelloUserCompletedEventArgs e)
        {
            txtHelloBox.Text = e.Result;
            NavigateToGetVersionNumber();
        }
        #endregion

        // Visual States

        #region NavigateToGetVersionNumber
        private void NavigateToGetVersionNumber()
        {
            VisualStateManager.GoToState(this, "ShowGetVersionNumber", true);
        }
        #endregion

        #region NavigateToShowGetVersionOfADefHelpdesk
        private void NavigateToShowGetVersionOfADefHelpdesk()
        {
            VisualStateManager.GoToState(this, "ShowGetVersionOfADefHelpdesk", true);
        }
        #endregion

        #region NavigateToShowCheckUpgradeIssues
        private void NavigateToShowCheckUpgradeIssues()
        {
            VisualStateManager.GoToState(this, "ShowCheckUpgradeIssues", true);
        }
        #endregion

        // Get Current Version number of ADefHelpDesk Upgrade Advisor

        #region btnGetCurrentVersionNumber_Click
        private void btnGetCurrentVersionNumber_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            waitingIcon.IsBusy = true;
            WebServiceSoapClient objWebServiceSoapClient = new WebServiceSoapClient();
            EndpointAddress MyEndpointAddress = new EndpointAddress(strWebServiceBase + "Webservice.asmx");
            objWebServiceSoapClient.Endpoint.Address = MyEndpointAddress;

            objWebServiceSoapClient.CurrentVersionOfADefHelpDeskUpgradeAdvisorCompleted +=
                new EventHandler<CurrentVersionOfADefHelpDeskUpgradeAdvisorCompletedEventArgs>(objWebServiceSoapClient_CurrentVersionOfADefHelpDeskUpgradeAdvisorCompleted);
            objWebServiceSoapClient.CurrentVersionOfADefHelpDeskUpgradeAdvisorAsync(intPortalID, intModuleID, intUserID, strPassword);
        }

        void objWebServiceSoapClient_CurrentVersionOfADefHelpDeskUpgradeAdvisorCompleted(object sender, CurrentVersionOfADefHelpDeskUpgradeAdvisorCompletedEventArgs e)
        {
            waitingIcon.IsBusy = false;
            try
            {
                if (e.Result == "01.00.00")
                {
                    btnGetCurrentVersionNumber.Visibility = Visibility.Collapsed;
                    txtCurrentUpgradeVersionNumber.Text = "Current version is 01.00.00 ADefHelpDesk Upgrade Advisor is up to date";
                    btnContinuetoHelpDeskVersion.Visibility = Visibility.Visible;
                }
                else
                {
                    txtCurrentUpgradeVersionNumber.Text = string.Format("Current version is {0} ADefHelpDesk Upgrade Advisor is NOT up to date. Download the latest version from http://ADefHelpDesk.com to continue", e.Result);
                }
            }
            catch (Exception ex)
            {
                txtError.Text = ex.Message;
            }
        }
        #endregion

        #region btnContinuetoHelpDeskVersion_Click
        private void btnContinuetoHelpDeskVersion_Click(object sender, RoutedEventArgs e)
        {
            NavigateToShowGetVersionOfADefHelpdesk();
        }
        #endregion

        // Get Current Version of ADefHelpDesk      

        #region btnGetVersionOfADefHelpDesk_Click
        private void btnGetVersionOfADefHelpDesk_Click(object sender, RoutedEventArgs e)
        {
            waitingIcon.IsBusy = true;
            WebServiceSoapClient objWebServiceSoapClient = new WebServiceSoapClient();
            EndpointAddress MyEndpointAddress = new EndpointAddress(strWebServiceBase + "Webservice.asmx");
            objWebServiceSoapClient.Endpoint.Address = MyEndpointAddress;

            objWebServiceSoapClient.CurrentVersionOfADefHelpDeskCompleted +=
                new EventHandler<CurrentVersionOfADefHelpDeskCompletedEventArgs>(objWebServiceSoapClient_CurrentVersionOfADefHelpDeskCompleted);
            objWebServiceSoapClient.CurrentVersionOfADefHelpDeskAsync(intPortalID, intModuleID, intUserID, strPassword);
        }

        void objWebServiceSoapClient_CurrentVersionOfADefHelpDeskCompleted(object sender, CurrentVersionOfADefHelpDeskCompletedEventArgs e)
        {
            waitingIcon.IsBusy = false;
            try
            {
                if (e.Result != "")
                {
                    strCurrentVersion = e.Result;
                    btnGetVersionOfADefHelpDesk.Visibility = Visibility.Collapsed;
                    txtCurrentVersionOfADefHelpDesk.Text = string.Format("The current version of ADefHelpDesk that is installed is {0}", e.Result);
                    btnContinuetoUpgradeCheck.Visibility = Visibility.Visible;
                }
                else
                {
                    txtCurrentVersionOfADefHelpDesk.Text = "ADefHelpDesk is not installed. Download the latest version from http://ADefHelpDesk.com to continue";
                }
            }
            catch (Exception ex)
            {
                txtError.Text = ex.Message;
            }
        }
        #endregion

        #region btnContinuetoUpgradeCheck_Click
        private void btnContinuetoUpgradeCheck_Click(object sender, RoutedEventArgs e)
        {
            NavigateToShowCheckUpgradeIssues();
        }
        #endregion

        // Check for Upgrade Items

        #region btnGetUpgradeItems_Click
        private void btnGetUpgradeItems_Click(object sender, RoutedEventArgs e)
        {
            waitingIcon.IsBusy = true;
            WebServiceSoapClient objWebServiceSoapClient = new WebServiceSoapClient();
            EndpointAddress MyEndpointAddress = new EndpointAddress(strWebServiceBase + "Webservice.asmx");
            objWebServiceSoapClient.Endpoint.Address = MyEndpointAddress;

            objWebServiceSoapClient.GetUpgradeItemsCompleted += new EventHandler<GetUpgradeItemsCompletedEventArgs>(objWebServiceSoapClient_GetUpgradeItemsCompleted);
            objWebServiceSoapClient.GetUpgradeItemsAsync(intPortalID, intModuleID, intUserID, strPassword, strCurrentVersion);
        }

        void objWebServiceSoapClient_GetUpgradeItemsCompleted(object sender, GetUpgradeItemsCompletedEventArgs e)
        {
            waitingIcon.IsBusy = false;
            try
            {
                if (e.Result.Count > 0)
                {
                    btnGetUpgradeItems.Visibility = Visibility.Collapsed;
                    btnContinuetoCheckUpgradeItems.Visibility = Visibility.Visible;

                    foreach (var item in e.Result)
                    {
                        ListBoxItem objListBoxItem = new ListBoxItem();
                        objListBoxItem.Content = item.ItemDescription;
                        ToolTipService.SetToolTip(objListBoxItem, item.ItemDetails);
                        ColMethodsToCheck.Add(item.ItemMethodName);

                        lstUpgradeItems.Items.Add(objListBoxItem);
                    }
                }
                else
                {
                    lstUpgradeItems.Items.Add("There are no Upgrade Issues");
                }
            }
            catch (Exception ex)
            {
                txtError.Text = ex.Message;
            }
        }
        #endregion

        #region btnContinuetoCheckUpgradeItems_Click
        private void btnContinuetoCheckUpgradeItems_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Clear the ListBox
            lstUpgradeItems.Items.Clear();

            // Check each Method
            foreach (var Method in ColMethodsToCheck)
            {
                switch (Method)
                {
                    case "ADefHelpDeskLastSearchCurrentPagePresent":
                        ADefHelpDeskLastSearchCurrentPagePresent();
                        break;
                    default:
                        break;
                }
            }
        }

        private void ADefHelpDeskLastSearchCurrentPagePresent()
        {
            waitingIcon.IsBusy = true;
            WebServiceSoapClient objWebServiceSoapClient = new WebServiceSoapClient();
            EndpointAddress MyEndpointAddress = new EndpointAddress(strWebServiceBase + "Webservice.asmx");
            objWebServiceSoapClient.Endpoint.Address = MyEndpointAddress;

            objWebServiceSoapClient.ADefHelpDeskLastSearchCurrentPagePresentCompleted += new EventHandler<ADefHelpDeskLastSearchCurrentPagePresentCompletedEventArgs>(objWebServiceSoapClient_ADefHelpDeskLastSearchCurrentPagePresentCompleted);
            objWebServiceSoapClient.ADefHelpDeskLastSearchCurrentPagePresentAsync(intPortalID, intModuleID, intUserID, strPassword);
        }

        void objWebServiceSoapClient_ADefHelpDeskLastSearchCurrentPagePresentCompleted(object sender, ADefHelpDeskLastSearchCurrentPagePresentCompletedEventArgs e)
        {
            waitingIcon.IsBusy = false;
            try
            {
                if (e.Result)
                {
                    ListBoxItem objListBoxItem = new ListBoxItem();
                    objListBoxItem.Content = "LastSearchCurrentPage Present (passed)";
                    lstUpgradeItems.Items.Add(objListBoxItem);
                }
                else
                {
                    ListBoxItem objListBoxItem = new ListBoxItem();
                    objListBoxItem.Content = "LastSearchCurrentPage Present (failed)";
                    lstUpgradeItems.Items.Add(objListBoxItem);
                }

                // count this method as checked
                intMethodsChecked++;

                DetermineIfAllChecksComplete();
            }
            catch (Exception ex)
            {
                txtError.Text = ex.Message;
            }
        }
        #endregion

        #region DetermineIfAllChecksComplete
        private void DetermineIfAllChecksComplete()
        {
            // If all checks are complete then indicate it now
            // Otherwise there are web service methods still running or about to be run that will 
            // also call this method

            if (ColMethodsToCheck.Count == intMethodsChecked)
            {
                // All Methods have been checked
                btnContinuetoCheckUpgradeItems.Content = "All Checks Complete";

                // Reset Methods checked in case user runs checks again
                intMethodsChecked = 0;
            }
        } 
        #endregion


    }
}
