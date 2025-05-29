using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RecruitmentApp
{

    public partial class Form5 : Form
    {

        int jobSeekerId { get; set; }

        public Form5(int jobSeekerID)
        {
            
            InitializeComponent();

            this.jobSeekerId = jobSeekerID;

        }

        private void Form5_Load(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            try
            {

                //query that joins the Vacancy table with the Employer, Location, Industry tables
                string query = @"SELECT v.VACANCYID AS ID, v.TITLE AS Title, v.DESCRIPTION AS Description, v.REQUIREDEXPERIENCE AS [Required Experience],
                                v.SALARYRANGE AS [Salary Range], v.POSTINGDATE AS [Posting Date], v.EXPIRATIONDATE AS [Expiration Date],
                                e.COMPANYNAME AS [Company Name], i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.VACANCY v
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = e.USERID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID";

                //command with the query and connection
                using(SqlCommand command = new SqlCommand(query, connection))
                {

                    //create a data adapter and dataset
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();

                    //fill the dataset with the query results
                    adapter.Fill(ds, "VacancyData");

                    //bind the dataset to the DataGridView
                    dataGridView1.DataSource = ds.Tables["VacancyData"];

                }       
                
            }
            catch(Exception ex)
            {

                MessageBox.Show("Error loading vacancies: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            //load employers
            string employerQuery = @"SELECT NAME + ',' + COMPANYNAME AS FullDetails, USERID FROM dbo.EMPLOYER";

            using(SqlCommand command = new SqlCommand(employerQuery, connection))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "FullDetails";
                comboBox2.ValueMember = "USERID";
                comboBox2.Text = "Select Employer";

            }

            connection.Close();

            //add items
            comboBox1.Items.Add("City");
            comboBox1.Items.Add("Country");
            comboBox1.Items.Add("Industry");

            comboBox1.Text = "Search by..."; //set default text

        }

        //back button click event
        private void button2_Click(object sender, EventArgs e)
        {

            Form2 jobSeekerHome = new Form2(jobSeekerId);

            jobSeekerHome.Show();
            this.Close();
        
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {}

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {}

        //save job button click event
        private void button1_Click(object sender, EventArgs e)
        {

            //check if a row is selected
            if(dataGridView1.SelectedRows.Count == 0)
            {

                MessageBox.Show("Please select a vacancy first.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;

            }

            //get the vacancy ID
            int vacancyId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //check if this job already saved
            string query = "SELECT COUNT(*) FROM SAVEDJOBS WHERE VACANCYID = @param1 AND USERID = @param2";

            using(SqlCommand command = new SqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("@param1", vacancyId);
                command.Parameters.AddWithValue("@param2", jobSeekerId);

                int count = Convert.ToInt32(command.ExecuteScalar());

                //check if the job is already saved
                if(count > 0)
                {

                    MessageBox.Show("This job is already saved.", "Job Already Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {

                    string query2 = "INSERT INTO SAVEDJOBS VALUES (@param1, @param2)";

                    using(SqlCommand command2 = new SqlCommand(query2, connection))
                    {

                        command2.Parameters.AddWithValue("@param1", vacancyId);
                        command2.Parameters.AddWithValue("@param2", jobSeekerId);

                        int rowsAffected = command2.ExecuteNonQuery();

                        if(rowsAffected > 0)
                        {

                            MessageBox.Show("Job saved successfully!", "Job Saved",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                    }

                }

            }

            connection.Close();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {}

        //search button click event
        private void button4_Click(object sender, EventArgs e)
        {

            //choosing search filter
            if(comboBox1.SelectedItem != null)
            {

                
                //connection string from app.config file
                string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                string selectedOption = comboBox1.SelectedItem.ToString();

                SqlConnection connection = new SqlConnection(conn);

                connection.Open();

                if(selectedOption == "City")
                {

                    string query = @"SELECT v.VACANCYID AS ID, v.TITLE AS Title, v.DESCRIPTION AS Description, v.REQUIREDEXPERIENCE AS [Required Experience],
                                v.SALARYRANGE AS [Salary Range], v.POSTINGDATE AS [Posting Date], v.EXPIRATIONDATE AS [Expiration Date],
                                e.COMPANYNAME AS [Company Name], i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.VACANCY v
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = e.USERID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                WHERE l.City = '" + textBox1.Text + "'";

                    //command with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        //create a data adapter and dataset
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();

                        //fill the dataset with the query results
                        adapter.Fill(ds, "VacancyData");

                        //bind the dataset to the DataGridView
                        dataGridView1.DataSource = ds.Tables["VacancyData"];

                    }

                }
                else if(selectedOption == "Country")
                {

                    string query = @"SELECT v.VACANCYID AS ID, v.TITLE AS Title, v.DESCRIPTION AS Description, v.REQUIREDEXPERIENCE AS [Required Experience],
                                v.SALARYRANGE AS [Salary Range], v.POSTINGDATE AS [Posting Date], v.EXPIRATIONDATE AS [Expiration Date],
                                e.COMPANYNAME AS [Company Name], i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.VACANCY v
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = e.USERID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                WHERE l.Country = '" + textBox1.Text + "'";

                    //command with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        //create a data adapter and dataset
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();

                        //fill the dataset with the query results
                        adapter.Fill(ds, "VacancyData");

                        //bind the dataset to the DataGridView
                        dataGridView1.DataSource = ds.Tables["VacancyData"];

                    }

                }
                else if(selectedOption == "Industry")
                {

                    string query = @"SELECT v.VACANCYID AS ID, v.TITLE AS Title, v.DESCRIPTION AS Description, v.REQUIREDEXPERIENCE AS [Required Experience],
                                v.SALARYRANGE AS [Salary Range], v.POSTINGDATE AS [Posting Date], v.EXPIRATIONDATE AS [Expiration Date],
                                e.COMPANYNAME AS [Company Name], i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.VACANCY v
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = e.USERID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                WHERE i.Name = '" + textBox1.Text + "'";

                    //command with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        //create a data adapter and dataset
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();

                        //fill the dataset with the query results
                        adapter.Fill(ds, "VacancyData");

                        //bind the dataset to the DataGridView
                        dataGridView1.DataSource = ds.Tables["VacancyData"];

                    }

                }

                connection.Close();

            }
            else MessageBox.Show("You Must Select a type!");

        }

        //apply button click event
        private void button3_Click(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //check if a row is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {

                MessageBox.Show("Please select a vacancy first.", "Selection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;

            }

            //get the vacancy ID
            int vacancyId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            //check if the job is already applied
            string Query = "SELECT COUNT(*) FROM APPLICATION WHERE VacancyID = @VacancyID AND UserID = @UserID";

            using (SqlCommand command = new SqlCommand(Query, connection))
            {

                command.Parameters.AddWithValue("@VacancyID", vacancyId);
                command.Parameters.AddWithValue("@UserID", jobSeekerId);

                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count > 0)
                {

                    MessageBox.Show("You have already applied for this job.", "Already Applied",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }
                else
                {

                    //inserting the job application into the APPLICATION table
                    string query2 = "INSERT INTO APPLICATION (VacancyID, UserID) VALUES (@VacancyID, @UserID)";

                    using (SqlCommand command2 = new SqlCommand(query2, connection))
                    {

                        command2.Parameters.AddWithValue("@VacancyID", vacancyId);
                        command2.Parameters.AddWithValue("@UserID", jobSeekerId);

                        int rowsAffected = command2.ExecuteNonQuery();

                        //check if the job application was successful
                        if (rowsAffected > 0)
                        {

                            MessageBox.Show("Job Applied successfully!", "Application",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {

                            MessageBox.Show("Error applying for the job.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    }

                }

                connection.Close();

            }

        }

        //title with most applicants button click event
        private void button5_Click(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //query that joins the Vacancy table with the jobseeker and application tables and show the applicants
            string query = @"SELECT v.Title, COUNT(a.UserID) AS [No. Of Applicants]
                           FROM VACANCY v LEFT OUTER JOIN APPLICATION a ON v.VacancyID = a.VacancyID 
                           LEFT OUTER JOIN JOBSEEKER j ON a.UserID = j.UserID
                           GROUP BY v.Title
                           ORDER BY [No. Of Applicants] DESC";

            using(SqlCommand cmd = new SqlCommand(query, connection))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "ApplicationData");

                //bind the dataset to the DataGridView
                dataGridView1.DataSource = ds.Tables["ApplicationData"];

            }

            connection.Close();

        }

        //title with no applicants last month button click event
        private void button6_Click(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //query that joins the Vacancy table with application table and show the vacancy with 0 applicants
            string query = @"SELECT v.Title, COUNT(a.UserID) AS [No. Of Applicants]
                           FROM VACANCY v LEFT OUTER JOIN APPLICATION a ON v.VacancyID = a.VacancyID
                           WHERE v.VacancyID NOT IN (SELECT VacancyID FROM APPLICATION WHERE DATEAPPLIED >= DATEADD(MONTH, -1, GETDATE()))
                           GROUP BY v.Title";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "ApplicationData");

                //bind the dataset to the DataGridView
                dataGridView1.DataSource = ds.Tables["ApplicationData"];

            }

            connection.Close();

        }

        //employer with most announcements button click event
        private void button7_Click(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //query that joins the Vacancy table with employer table and show the announcements
            string query = @"SELECT e.Name , e.CompanyName, COUNT(V.VacancyID) AS [No. Of Announcements]
                           FROM VACANCY v RIGHT OUTER JOIN EMPLOYER e ON v.EmployerID = e.UserID
                           AND v.POSTINGDATE >= DATEADD(MONTH, -1, GETDATE())
                           GROUP BY e.name, e.CompanyName
                           ORDER BY [No. Of Announcements] DESC";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "AnnouncementsData");

                //bind the dataset to the DataGridView
                dataGridView1.DataSource = ds.Tables["AnnouncementsData"];

            }

            connection.Close();


        }

        //employer with no announcements last month button click event
        private void button8_Click(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //query that joins the Vacancy table with employer table and show the announcements
            string query = @"SELECT e.Name , e.CompanyName, COUNT(V.VacancyID) AS [No. Of Announcements]
                           FROM VACANCY v RIGHT OUTER JOIN EMPLOYER e ON v.EmployerID = e.UserID
                           WHERE e.UserID NOT IN (SELECT EmployerID FROM VACANCY WHERE POSTINGDATE >= DATEADD(MONTH, -1, GETDATE()))
                           GROUP BY e.name, e.CompanyName";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "AnnouncementsData");

                //bind the dataset to the DataGridView
                dataGridView1.DataSource = ds.Tables["AnnouncementsData"];

            }

            connection.Close();

        }

        //available positions at each employer last month
        private void button9_Click(object sender, EventArgs e)
        {

            //Validate combo box selection
            if(comboBox2.SelectedItem == null)
            {

                MessageBox.Show("Please select an Emloyer.");
                return;

            }

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //query that joins the Vacancy table with employer table and show the announcements
            string query = @"SELECT v.VacancyID, v.Title, v.CompanyName, v.SalaryRange, v.RequiredExperience
                           FROM VACANCY v INNER JOIN EMPLOYER e ON v.EmployerID = e.UserID
                           WHERE v.EMPLOYERID = @EmployerID";

            using(SqlCommand cmd = new SqlCommand(query, connection))
            {

                cmd.Parameters.AddWithValue("@EmployerID", Convert.ToInt32(((DataRowView)comboBox2.SelectedItem)["USERID"]));

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "VacancyData");

                //bind the dataset to the DataGridView
                dataGridView1.DataSource = ds.Tables["VacancyData"];

            }

            connection.Close();

        }

    }

}
