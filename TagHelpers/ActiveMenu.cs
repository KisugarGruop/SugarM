using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SugarM.TagHelpers {
    [HtmlTargetElement (Attributes = "is-active-route")]
    public class ActiveMenu : TagHelper {
        private IDictionary<string, string> _routeValues;

        /// <summary>The name of the action method.</summary>
        /// <remarks>Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-<c>null</c>.</remarks>
        [HtmlAttributeName ("asp-action")]
        public string Action { get; set; }

        /// <summary>The name of the controller.</summary>
        /// <remarks>Must be <c>null</c> if <see cref="P:Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper.Route" /> is non-<c>null</c>.</remarks>
        [HtmlAttributeName ("asp-controller")]
        public string Controller { get; set; }

        /// <summary>Additional parameters for the route.</summary>
        [HtmlAttributeName ("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues {
            get {
                if (this._routeValues == null)
                    this._routeValues = (IDictionary<string, string>) new Dictionary<string, string> ((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
                return this._routeValues;
            }
            set {
                this._routeValues = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.AspNetCore.Mvc.Rendering.ViewContext" /> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process (TagHelperContext context, TagHelperOutput output) {
            base.Process (context, output);

            if (ShouldBeActive ()) {
                MakeActive (output);
            }

            output.Attributes.RemoveAll ("is-active-route");
        }

        private bool ShouldBeActive () {
            string currentController = ViewContext.RouteData.Values["Controller"].ToString ();
            string currentAction = ViewContext.RouteData.Values["Action"].ToString ();
            string CarTypeDetail = "CarTypeDetail";
            string CarBrand = "CarBrand";
            string CarTypeMinor = "CarTypeMinor";
            if (!string.IsNullOrWhiteSpace (Controller) && Controller.ToLower () != currentController.ToLower ()) {
                if (Controller.ToLower () == CarTypeDetail.ToLower () || Controller.ToLower () == CarBrand.ToLower () || Controller.ToLower () == CarTypeMinor.ToLower ()) {
                    return true;
                }
                return false;
            }

            foreach (KeyValuePair<string, string> routeValue in RouteValues) {
                if (!ViewContext.RouteData.Values.ContainsKey (routeValue.Key) ||
                    ViewContext.RouteData.Values[routeValue.Key].ToString () != routeValue.Value) {
                    return false;
                }
            }

            return true;
        }

        private void MakeActive (TagHelperOutput output) {
            var classAttr = output.Attributes.FirstOrDefault (a => a.Name == "is-active-route");
            if (classAttr == null) {
                classAttr = new TagHelperAttribute ("class", "");
                output.Attributes.Add (classAttr);
            } else if (classAttr.Value == null || classAttr.Value.ToString ().IndexOf ("kt-menu__item") < 0) {
                output.Attributes.SetAttribute ("class", classAttr.Value == null ?
                    "kt-menu__item" :
                    classAttr.Value.ToString () + "kt-menu__item kt-menu__item--active");

            }
        }
    }
}