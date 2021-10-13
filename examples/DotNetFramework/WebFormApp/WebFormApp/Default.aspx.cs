using System;
using System.Web.UI.WebControls;

namespace WebFormApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Read configuration from the IConfiguration object loaded from Azure App Configuration
            string messageText = Global.Configuration["TestApp:Settings:Message"] ?? "Please add the key \"TestApp:Settings:Message\" in your Azure App Configuration store.";
            string messageFontSize = Global.Configuration["TestApp:Settings:FontSize"] ?? "20";
            string messageFontColor = Global.Configuration["TestApp:Settings:FontColor"] ?? "Black";
            string backgroundColor = Global.Configuration["TestApp:Settings:BackgroundColor"] ?? "White";

            message.Text = messageText;
            message.Font.Size = FontUnit.Point(int.Parse(messageFontSize));
            message.ForeColor = System.Drawing.Color.FromName(messageFontColor);
            body.Attributes["bgcolor"] = backgroundColor;
        }
    }
}
