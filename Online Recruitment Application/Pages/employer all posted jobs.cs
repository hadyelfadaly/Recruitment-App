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

    public partial class Form10 : Form
    {

        int employerId { get; set; }

        public Form10(int employerId)
        {

            InitializeComponent();

            this.employerId = employerId;

        }

        private void Form10_Load(object sender, EventArgs e)
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
                                INNER JOIN dbo.LOCATION l ON v.LOCATIONID = l.LOCATIONID
                                WHERE v.EMPLOYERID = '"+employerId+"'";

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
            catch(Exception ex)
            {

                MessageBox.Show("Error loading vacancies: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            connection.Close();

        }

        //save changes button click event
        private void button2_Click(object sender, EventArgs e)
        {

            string newDesc = "", newReqExp = "", newSalRange = "", newExpirationDate = "";

            //get the new values from the datagridview
            if(dataGridView1.CurrentRow != null)
            {

                if(dataGridView1.CurrentRow.Cells["Description"].Value != null) newDesc = dataGridView1.CurrentRow.Cells["Description"].Value.ToString();
                if(dataGridView1.CurrentRow.Cells["Required Experience"].Value != null) newReqExp = dataGridView1.CurrentRow.Cells["Required Experience"].Value.ToString();
                if(dataGridView1.CurrentRow.Cells["Salary Range"].Value != null) newSalRange = dataGridView1.CurrentRow.Cells["Salary Range"].Value.ToString();
                if(dataGridView1.CurrentRow.Cells["Expiration Date"].Value != null) newExpirationDate = dataGridView1.CurrentRow.Cells["Expiration Date"].Value.ToString();

            }

            //updating vacancy table
            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);
            connection.Open();

            string query = @"UPDATE VACANCY SET DESCRIPTION = @Description, REQUIREDEXPERIENCE = @RequiredExperience, SALARYRANGE = @SalaryRange, EXPIRATIONDATE = @ExpirationDate
                            WHERE VACANCYID = @VacancyID";

            using(SqlCommand cmd = new SqlCommand(query, connection))
            {

                cmd.Parameters.AddWithValue("@Description", newDesc); //get newDesc from the datagrid
                cmd.Parameters.AddWithValue("@RequiredExperience", newReqExp);//get newReqExp from the datagrid
                cmd.Parameters.AddWithValue("@SalaryRange", newSalRange); //get newSalRange from the datagrid
                cmd.Parameters.AddWithValue("@ExpirationDate", newExpirationDate);//get newExpirationDate from the datagrid
                cmd.Parameters.AddWithValue("@VacancyID", dataGridView1.CurrentRow.Cells["ID"].Value); //use the correct vacancy id

                int rowsAffected = cmd.ExecuteNonQuery();

                if(rowsAffected > 0) MessageBox.Show("Vacancy updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("Error updating vacancy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            connection.Close();

        }

        //post a new vacancy button click event
        private void button3_Click(object sender, EventArgs e)
        {

            Form11 postVacancy = new Form11(employerId);

            postVacancy.Show();
            this.Close();

        }


        private void button1_Click(object sender, EventArgs e)
        {}

        //back button click event
        private void button4_Click(object sender, EventArgs e)
        {

            Form3 form3 = new Form3(employerId);

            form3.Show();
            this.Close();

        }

        //remove vacancy button click event
        private void button1_Click_1(object sender, EventArgs e)
        {

            //check if a row is selected
            if (dataGridView1.SelectedRows.Count == 0)
            {

                MessageBox.Show("Please select a vacancy first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;

            }

            //get the vacancy ID
            int vacancyId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            string query2 = "DELETE FROM APPLICATION WHERE VacancyID = '" + vacancyId + "'";
            SqlCommand command2 = new SqlCommand(query2, connection);
            int rowsAffected2 = command2.ExecuteNonQuery();

            string query3 = "DELETE FROM SAVEDJOBS WHERE VacancyID = '" + vacancyId + "'";
            SqlCommand command3 = new SqlCommand(query3, connection);
            int rowsAffected3 = command3.ExecuteNonQuery();

            string query4 = "DELETE FROM VACANCYSKILL WHERE VacancyID = '" + vacancyId + "'";
            SqlCommand command4 = new SqlCommand(query4, connection);
            int rowsAffected4 = command4.ExecuteNonQuery();

            string query = "DELETE FROM VACANCY WHERE VacancyID = '" + vacancyId + "'";
            SqlCommand command = new SqlCommand(query, connection);
            int rowsAffected = command.ExecuteNonQuery();

            
            if(rowsAffected > 0)
                MessageBox.Show("Vacancy Removed successfully!", "Removal", MessageBoxButtons.OK, MessageBoxIcon.Information);

            connection.Close();

        }

    }

}
