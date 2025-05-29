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
    public partial class Form3 : Form
    {

        int employerId { get; set; }

        public Form3(int employerId)
        {

            InitializeComponent();

            this.employerId = employerId;

        }

        //settings button click event
        private void button1_Click(object sender, EventArgs e)
        {

            Form9 employerProfile = new Form9(employerId);

            employerProfile.Show();
            this.Hide();

        }

        //show all posted jobs click event
        private void button2_Click(object sender, EventArgs e)
        {

            Form10 postedJobs = new Form10(employerId);

            postedJobs.Show();
            this.Hide();

        }

        //show all applicants click event
        private void button3_Click(object sender, EventArgs e)
        {

            Form12 applicantsPage = new Form12(employerId);

            applicantsPage.Show();
            this.Hide();

        }

        private void Form3_Load(object sender, EventArgs e)
        {}

    }

}
