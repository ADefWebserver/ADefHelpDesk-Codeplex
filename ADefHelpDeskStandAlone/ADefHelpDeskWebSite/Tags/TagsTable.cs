//
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
using System.Web;
using System.Web.Caching;

namespace ADefWebserver.Modules.ADefHelpDesk
{
    public static class CategoriesTable
    {
        #region GetCategoriesTable
        public static IQueryable<ADefHelpDesk_Category> GetCategoriesTable(int PortalID, bool RequestorCatagories)
        {
            IQueryable<ADefHelpDesk_Category> Categories;
            object objCategoriesTable;
 
            // Get Table out of Cache
            if (RequestorCatagories)
            {
                objCategoriesTable = HttpContext.Current.Cache.Get(String.Format("RequestorCategoriesTable_{0}", PortalID.ToString()));
            }
            else
            {
                objCategoriesTable = HttpContext.Current.Cache.Get(String.Format("CategoriesTable_{0}", PortalID.ToString()));
            }

            // Is the table in the cache?
            if (objCategoriesTable == null)
            {
                if (RequestorCatagories)
                {
                    // Get the table from the database
                    Categories = GetEntireRequestorTable(PortalID);
                    HttpContext.Current.Cache.Add(String.Format("RequestorCategoriesTable_{0}", PortalID.ToString()), Categories, null, Cache.NoAbsoluteExpiration,
                        Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }
                else
                {
                    // Get the table from the database
                    Categories = GetEntireTable(PortalID);
                    HttpContext.Current.Cache.Add(String.Format("CategoriesTable_{0}", PortalID.ToString()), Categories, null, Cache.NoAbsoluteExpiration,
                        Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }
            }
            else
            {
                // Use the cache version of the table
                Categories = (IQueryable<ADefHelpDesk_Category>)objCategoriesTable;
            }

            return Categories;
        } 
        #endregion

        #region GetEntireRequestorTable
        private static IQueryable<ADefHelpDesk_Category> GetEntireRequestorTable(int PortalID)
        {
            dnnHelpDeskDALDataContext CategoryAdminDALDataContext = new dnnHelpDeskDALDataContext();

            IQueryable<ADefHelpDesk_Category> EntireTable = (from AdefWebserverCategories in CategoryAdminDALDataContext.ADefHelpDesk_Categories
                                                            where AdefWebserverCategories.PortalID == PortalID
                                                            where AdefWebserverCategories.RequestorVisible == true
                                                            select AdefWebserverCategories).ToList().AsQueryable();

            return EntireTable;
        }
        #endregion

        #region GetEntireTable
        private static IQueryable<ADefHelpDesk_Category> GetEntireTable(int PortalID)
        {
            dnnHelpDeskDALDataContext CategoryAdminDALDataContext = new dnnHelpDeskDALDataContext();

            IQueryable<ADefHelpDesk_Category> EntireTable = (from AdefWebserverCategories in CategoryAdminDALDataContext.ADefHelpDesk_Categories
                                                            where AdefWebserverCategories.PortalID == PortalID
                                                            select AdefWebserverCategories).ToList().AsQueryable();

            return EntireTable;
        }
        #endregion
    }
}
