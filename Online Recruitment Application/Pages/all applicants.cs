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
    public partial class Form12 : Form
    {

        int employerId { get; set; }

        public Form12(int employerId)
        {
            InitializeComponent();

            this.employerId = employerId;

        }

        private void Form12_Load(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //query that joins the Vacancy table with the jobseeker, Location, Industry and application tables and show the applicants
            string query = @"SELECT DISTINCT a.VACANCYID AS [Vacancy ID], a.DATEAPPLIED AS [Application Date], j.NAME AS [Job Seeker Name],
                                j.EXPRIENCE AS Expereience, i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.APPLICATION a
                                INNER JOIN dbo.JOBSEEKER j ON a.USERID = j.USERID
                                INNER JOIN dbo.VACANCY v ON a.VACANCYID = v.VACANCYID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = '"+employerId+"'";

            using(SqlCommand cmd = new SqlCommand(query, connection))
            {

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "ApplicationData");

                //bind the dataset to the DataGridView
                dataGridView1.DataSource = ds.Tables["ApplicationData"];

            }

            connection.Close();

            //add items
            comboBox1.Items.Add("Experience");
            comboBox1.Items.Add("Vacancy");
            comboBox1.Items.Add("Skill");

            comboBox1.Text = "Search by..."; //set default text

        }

        //back button click event
        private void button1_Click(object sender, EventArgs e)
        {

            Form3 employerPage = new Form3(employerId);

            employerPage.Show();
            this.Close();

        }

        //search button click event
        private void button2_Click(object sender, EventArgs e)
        {

            //choosing search filter
            if (comboBox1.SelectedItem != null)
            {

                //connection string from app.config file
                string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
                string selectedOption = comboBox1.SelectedItem.ToString();

                SqlConnection connection = new SqlConnection(conn);

                connection.Open();

                if(selectedOption == "Experience")
                {

                    string query = @"SELECT DISTINCT a.VACANCYID AS [Vacancy ID], a.DATEAPPLIED AS [Application Date], j.NAME AS [Job Seeker Name],
                                j.EXPRIENCE AS Expereience, i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.APPLICATION a
                                INNER JOIN dbo.JOBSEEKER j ON a.USERID = j.USERID
                                INNER JOIN dbo.VACANCY v ON a.VACANCYID = v.VACANCYID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = '"+employerId+"' " +
                                "WHERE j.EXPRIENCE = '" + textBox1.Text+"'";

                    //command with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        //create a data adapter and dataset
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();

                        //fill the dataset with the query results
                        adapter.Fill(ds, "ApplicationData");

                        //bind the dataset to the DataGridView
                        dataGridView1.DataSource = ds.Tables["ApplicationData"];

                    }

                }
                else if(selectedOption == "Vacancy")
                {

                    string query = @"SELECT DISTINCT a.VACANCYID AS [Vacancy ID], a.DATEAPPLIED AS [Application Date], j.NAME AS [Job Seeker Name],
                                j.EXPRIENCE AS Expereience, i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.APPLICATION a
                                INNER JOIN dbo.JOBSEEKER j ON a.USERID = j.USERID
                                INNER JOIN dbo.VACANCY v ON a.VACANCYID = v.VACANCYID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = '" + employerId + "' " +
                                "WHERE v.TITLE = '" + textBox1.Text + "'";

                    //command with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        //create a data adapter and dataset
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();

                        //fill the dataset with the query results
                        adapter.Fill(ds, "ApplicationData");

                        //bind the dataset to the DataGridView
                        dataGridView1.DataSource = ds.Tables["ApplicationData"];

                    }

                }
                else if(selectedOption == "Skill")
                {

                    //query to get skill id
                    string SkillQuery = "SELECT SKILLID FROM SKILL WHERE NAME = '"+textBox1.Text+"'";  
                    SqlCommand command = new SqlCommand(SkillQuery, connection);
                    int skillID = Convert.ToInt32(command.ExecuteScalar());

                    string query = @"SELECT DISTINCT a.VACANCYID AS [Vacancy ID], a.DATEAPPLIED AS [Application Date], j.NAME AS [Job Seeker Name],
                                j.EXPRIENCE AS Expereience, i.NAME as Industry, (l.CITY + ',' + l.COUNTRY) AS Location
                                FROM dbo.APPLICATION a
                                INNER JOIN dbo.JOBSEEKER j ON a.USERID = j.USERID
                                INNER JOIN dbo.VACANCY v ON a.VACANCYID = v.VACANCYID
                                INNER JOIN dbo.INDUSTRY i ON v.INDUSTRYID = i.INDUSTRYID
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                INNER JOIN dbo.EMPLOYER e ON v.EMPLOYERID = '" + employerId + "' " +
                                "WHERE j.USERID IN (SELECT USERID FROM USERSKILL WHERE SKILLID = '" + skillID + "')";

                    //command with the query and connection
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {

                        //create a data adapter and dataset
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        //fill the dataset with the query results
                        adapter.Fill(ds, "ApplicationData");

                        //bind the dataset to the DataGridView
                        dataGridView1.DataSource = ds.Tables["ApplicationData"];

                    }

                }

                connection.Close();

            }
            else MessageBox.Show("You Must Select a type!");

        }

    }

}
