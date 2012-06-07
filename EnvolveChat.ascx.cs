using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EnvolveAPI
{
    public partial class EnvolveChatControl : System.Web.UI.UserControl
    {
        protected String myAPIKey;
        public String APIKey
        {
            set
            {
                myAPIKey = value;
            }
        }
        protected String fName;
        public String FirstName
        {
            set
            {
                fName = value;
            }
        }
        protected String lName;
        public String LastName
        {
            set
            {
                lName = value;
            }
        }
        protected String pic;
        public String ProfilePicture
        {
            set
            {
                pic = value;
            }
        }
        protected Boolean isAdmin;
        public Boolean AdminMode
        {
            set
            {
                isAdmin = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            EnvolveAPIEncoder encoder = new EnvolveAPIEncoder(this.myAPIKey);
            
            this.Literal1.Text = "envoSn=" + encoder.SiteID + ";\n" +
                "env_commandString=\"" + (this.fName == null ? encoder.getLogoutCommand() : 
                encoder.getLoginCommand(this.fName, this.lName, this.pic, this.isAdmin)) + "\";\n";
        }
    }
}