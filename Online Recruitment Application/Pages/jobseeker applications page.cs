using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecruitmentApp
{
    public partial class Form8 : Form
    {

        int jobSeekerId { get; set; }

        public Form8(int jobSeekerId)
        {

            InitializeComponent();

            this.jobSeekerId = jobSeekerId;

        }

        private void Form8_Load(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            try
            {

                //query that joins the Vacancy table with the Employer, Location, Industry tables and show the saved jobs of the employer
                string query = @"SELECT DISTINCT v.VACANCYID AS ID, v.TITLE AS Title, v.DESCRIPTION AS Description, v.REQUIREDEXPERIENCE AS [Required Experience],
                                v.SALARYRANGE AS [Salary Range], v.POSTINGDATE AS [Posting Date], v.EXPIRATIONDATE AS [Expiration Date],
                                e.COMPANYNAME AS [Company Name], i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.VACANCY v
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = e.USERID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                INNER JOIN dbo.APPLICATION a ON v.VACANCYID = a.VACANCYID
                                WHERE v.VACANCYID IN (SELECT VACANCYID FROM APPLICATION WHERE USERID = '" + jobSeekerId + "')";

                //command with the query and connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    //create a data adapter and dataset
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();

                    //fill the dataset with the query results
                    adapter.Fill(ds, "SavedJobsData");

                    //bind the dataset to the DataGridView
                    dataGridView1.DataSource = ds.Tables["SavedJobsData"];

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error loading vacancies: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            connection.Close();

        }

        //back button click event
        private void button1_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2(jobSeekerId);

            form2.Show();
            this.Close();

        }

    }

}
