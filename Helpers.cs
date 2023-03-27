using Microsoft.AspNetCore.Html;        // для HtmlString
using Microsoft.AspNetCore.Mvc.Rendering;   // для IHtmlHelper
namespace SkachkiWebApp
{
    public static class Helpers
    {
        public static HtmlString ShowTableIppodroms(this IHtmlHelper html)
        {
            string result = "<table>";
            result += "<tr><th>Id</th><th>Address</th><th>Description</th></tr>";
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var ippo in db.Ippodroms.ToList())
                {
                    result += $"<tr><th>{ippo.Id}</th><th>{ippo.Address}</th><th>{ippo.Description}</th></tr>";
                }
            }
            result = $"{result}</table>";
            return new HtmlString(result);
        }
    }
}
