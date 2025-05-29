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
    public partial class Form6 : Form
    {

        int jobSeekerId { get; set; }
        public Form6(int jobSeekerId)
        {
            
            InitializeComponent();

            this.jobSeekerId = jobSeekerId;

        }

        private void Form6_Load(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            try
            {

                //query that joins the Vacancy table with the Employer, Location, Industry tables and show the saved jobs of the employer
                string query = @"SELECT v.VACANCYID AS ID, v.TITLE AS Title, v.DESCRIPTION AS Description, v.REQUIREDEXPERIENCE AS [Required Experience],
                                v.SALARYRANGE AS [Salary Range], v.POSTINGDATE AS [Posting Date], v.EXPIRATIONDATE AS [Expiration Date],
                                e.COMPANYNAME AS [Company Name], i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.VACANCY v
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = e.USERID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                WHERE v.VACANCYID IN (SELECT VACANCYID FROM SAVEDJOBS WHERE USERID = '"+jobSeekerId+"')";

                //command with the query and connection
                using(SqlCommand command = new SqlCommand(query, connection))
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
            catch(Exception ex)
            {

                MessageBox.Show("Error loading vacancies: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            connection.Close();

        }

        //back button click event
        private void button1_Click(object sender, EventArgs e)
        {

            Form2 jobSeekerHome = new Form2(jobSeekerId);

            jobSeekerHome.Show();
            this.Close();

        }

        //remove button click event
        private void button2_Click(object sender, EventArgs e)
        {

            //check if a row is selected
            if(dataGridView1.SelectedRows.Count == 0)
            {

                MessageBox.Show("Please select a vacancy first.", "Selection Required",MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;

            }

            //get the vacancy ID
            int vacancyId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            string query = "DELETE FROM SAVEDJOBS WHERE VacancyID = '"+vacancyId+"'";
            SqlCommand command = new SqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();

            if(rowsAffected > 0) MessageBox.Show("Job Removed successfully!", "Removal", MessageBoxButtons.OK, MessageBoxIcon.Information);

            connection.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {}

        //apply button click event
        private void button3_Click(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //check if a row is selected
            if(dataGridView1.SelectedRows.Count == 0)
            {

                MessageBox.Show("Please select a vacancy first.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }

            //get the vacancy ID
            int vacancyId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            //check if the job is already applied
            string Query = "SELECT COUNT(*) FROM APPLICATION WHERE VacancyID = @VacancyID AND UserID = @UserID";

            using(SqlCommand command = new SqlCommand(Query, connection))
            {

                command.Parameters.AddWithValue("@VacancyID", vacancyId);
                command.Parameters.AddWithValue("@UserID", jobSeekerId);

                int count = Convert.ToInt32(command.ExecuteScalar());

                if(count > 0)
                {

                    MessageBox.Show("You have already applied for this job.", "Already Applied",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }
                else 
                {

                    //inserting the job application into the APPLICATION table
                    string query2 = "INSERT INTO APPLICATION (VacancyID, UserID) VALUES (@VacancyID, @UserID)";

                    using(SqlCommand command2 = new SqlCommand(query2, connection))
                    {

                        command2.Parameters.AddWithValue("@VacancyID", vacancyId);
                        command2.Parameters.AddWithValue("@UserID", jobSeekerId);

                        int rowsAffected = command2.ExecuteNonQuery();

                        //check if the job application was successful
                        if(rowsAffected > 0) MessageBox.Show("Job Applied successfully!", "Application",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("Error applying for the job.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);   

                    }

                }

                connection.Close();

            }

        }

    }

}
