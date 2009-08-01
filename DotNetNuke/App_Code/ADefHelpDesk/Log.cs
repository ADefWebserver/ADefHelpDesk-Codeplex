﻿//
// ADefwebserver.com
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
//

using System;
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public class Log
    {
        public Log()
        {
        }

        #region InsertLog
        public static void InsertLog(int TaskID, int UserID, string LogDescription)
        {
            ADefHelpDeskDALDataContext objADefHelpDeskDALDataContext = new ADefHelpDeskDALDataContext();

            ADefHelpDesk_Log objADefHelpDesk_Log = new ADefHelpDesk_Log();
            objADefHelpDesk_Log.DateCreated = DateTime.Now;
            objADefHelpDesk_Log.LogDescription = Strings.Left(LogDescription,499);
            objADefHelpDesk_Log.TaskID = TaskID;
            objADefHelpDesk_Log.UserID = UserID;

            objADefHelpDeskDALDataContext.ADefHelpDesk_Logs.InsertOnSubmit(objADefHelpDesk_Log);
            objADefHelpDeskDALDataContext.SubmitChanges();
        } 
        #endregion
    }
}
