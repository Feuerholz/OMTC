using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OMTC.Model;
using Newtonsoft.Json.Linq;
using System.IO;

namespace OMTC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            APIAccessor.ApiKey = apikeybox.Text;
            Match match = new Match(blueteambox.Text, redteambox.Text);
            JArray matchJSON = APIAccessor.RetrieveMatchDataAsync(matchidbox.Text).Result;
            match.FillMaps(matchJSON);
            string tablepath = "./" + matchidbox.Text + ".txt";
            StreamWriter file = new StreamWriter(@tablepath);
            file.Write(match.ToString());
            file.Close();
        }
    }
}
