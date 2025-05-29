using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecruitmentApp
{
    public partial class Form2 : Form
    {
        
        //setting a variable for jobseeker id as we will need it when saving jobs or applying, etc..
        public int jobSeekerID { get; set; }

        public Form2(int jobSeekerID)
        {

            InitializeComponent();

            this.jobSeekerID = jobSeekerID;

        }

        //Show all available jobs click event
        private void button1_Click(object sender, EventArgs e)
        {

            Form5 allJobs = new Form5(jobSeekerID);

            allJobs.Show();
            this.Hide();

        }

        //Show saved jobs click event
        private void button2_Click(object sender, EventArgs e)
        {

            Form6 myJobs = new Form6(jobSeekerID);

            myJobs.Show();
            this.Hide();

        }

        //settings button click event
        private void button3_Click(object sender, EventArgs e)
        {

            Form7 myProfile = new Form7(jobSeekerID);

            myProfile.Show();
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {

            
            Form8 applications = new Form8(jobSeekerID);

            applications.Show();
            this.Hide();

        }

        private void Form2_Load(object sender, EventArgs e)
        {}

    }

}
