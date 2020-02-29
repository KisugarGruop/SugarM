using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;

namespace SugarM.TagHelpers {
    public class PaginationHelper {
        /// <summary>
        /// Renders pagination used in listing pages.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalNumberOfRecords"></param>
        /// <param name="pageRequest">Current page request used to get the URL path of the page.</param>
        /// <param name="noOfPagesLinks">Number of pagination numbers to show.</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static HtmlString CreatePagination (int currentPage, int totalNumberOfRecords, HttpRequest pageRequest, int noOfPagesLinks = 10, int pageSize = 10) {
            StringBuilder paginationHtml = new StringBuilder ();

            // Only render the pagination markup if the total number of records is more than our page size.
            if (totalNumberOfRecords > pageSize) {
                #region Pagination Calculations

                int amountOfPages = (int) (Math.Ceiling (totalNumberOfRecords / Convert.ToDecimal (pageSize)));

                int startPage = currentPage;

                if (startPage == 1 || startPage == 2 || amountOfPages < noOfPagesLinks)
                    startPage = 1;
                else
                    startPage -= 2;

                int maxPage = startPage + noOfPagesLinks;

                if (amountOfPages < maxPage)
                    maxPage = Convert.ToInt32 (amountOfPages) + 1;

                if (maxPage - startPage != noOfPagesLinks && maxPage > noOfPagesLinks)
                    startPage = maxPage - noOfPagesLinks;

                int previousPage = currentPage - 1;
                if (previousPage < 1)
                    previousPage = 1;

                int nextPage = currentPage + 1;

                #endregion

                #region Get Current Path

                // Get current path.
                string path = pageRequest.Path.ToString ();

                int pos = path.LastIndexOf ("/") + 1;

                // Get last route value.
                string lastRouteValue = path.Substring (pos, path.Length - pos).ToLower ();

                // Removes page number from end of path if path contains a page number.
                if (lastRouteValue.StartsWith ("page"))
                    path = path.Substring (0, path.LastIndexOf ('/'));

                #endregion

                paginationHtml.Append ("<ul class='pagination pagination-sm no-margin pull-right'>");

                if (currentPage > 1)
                    paginationHtml.Append ($"<li class=\"page-item\"><a class=\"page-link\" href=\"{path}?Page={previousPage}\"><span>ก่อนหน้า</span></a></li>");

                for (int i = startPage; i < maxPage; i++) {
                    // If the current page equals one of the pagination numbers, set active state.
                    if (i == currentPage)
                        paginationHtml.Append ($"<li class=\"page-item\"><a href=\"{path}?Page={i}\" class=\"page-link\"><span>{i}</span></a></li>");
                    else
                        paginationHtml.Append ($"<li class=\"page-item\"><a href=\"{path}?Page={i}\"  class=\"page-link\"><span>{i}</span></a></li>");
                }

                if (startPage + noOfPagesLinks < amountOfPages && maxPage > noOfPagesLinks || currentPage < amountOfPages)
                    paginationHtml.Append ($"<li class=\"page-item\"><a href=\"{path}?Page={nextPage}\"  class=\"page-link\"><span>หน้าถัดไป</span></a></li>");

                paginationHtml.Append ("</ul>");
                HtmlString test = new HtmlString (paginationHtml.ToString ());
                return test;
                //return paginationHtml.ToString();
            } else {
                return null;
            }
        }
    }
}