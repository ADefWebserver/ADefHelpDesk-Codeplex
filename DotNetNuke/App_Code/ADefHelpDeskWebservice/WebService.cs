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
using System.Web;
using System.Web.Services;

namespace ADefWebserver.Modules.ADefHelpDeskWebservice
{
    [WebService(Namespace = "http://ADefHelpDesk.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebService : System.Web.Services.WebService
    {
        #region UpgradeItems
        [Serializable]
        public class UpgradeItems
        {
            public string Item { get; set; }
            public string ItemDescription { get; set; }
            public string ItemDetails { get; set; }
            public string ItemMethodName { get; set; }
        }
        #endregion

        #region CurrentUpgradeVersion
        [WebMethod]
        public string CurrentUpgradeVersion()
        {
            return "01.00.00";
        }
        #endregion

        #region UpgradeCheck
        [WebMethod]
        public List<UpgradeItems> UpgradeCheck(string CurrentVersion)
        {
            List<UpgradeItems> ColUpgradeItems = new List<UpgradeItems>();

            switch (CurrentVersion)
            {
                case "01.00.00":
                case "01.10.00":
                case "01.30.00":
                case "01.40.00":
                    // There are no upgrade items for 01.40.00 and earlier
                    break;
                default:
                    // Upgrades for 01.50.00 and later
                    ColUpgradeItems.Add
                        (new UpgradeItems
                        {
                            Item = "Check ADefHelpDesk_LastSearch",
                            ItemDescription = "Ensure ADefHelpDesk_LastSearch is updated",
                            ItemDetails = "Ensure ADefHelpDesk_LastSearch has a CurrentPage colum",
                            ItemMethodName = "ADefHelpDeskLastSearchCurrentPagePresent"
                        });
                    break;
            }
            return ColUpgradeItems;
        }
        #endregion
    }
}