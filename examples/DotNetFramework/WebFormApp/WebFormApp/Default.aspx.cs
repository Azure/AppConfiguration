using System;
using System.Web.UI.WebControls;

namespace WebFormApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            body.Attributes["bgcolor"] = Global.Configuration["TestApp:Settings:BackgroundColor"];
            message.Text = Global.Configuration["TestApp:Settings:Message"];
            message.Font.Size = FontUnit.Point(int.Parse(Global.Configuration["TestApp:Settings:FontSize"]));
            message.ForeColor = System.Drawing.Color.FromName(Global.Configuration["TestApp:Settings:FontColor"]);
        }
    }
}
