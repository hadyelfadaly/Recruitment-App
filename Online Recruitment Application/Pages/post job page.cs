using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecruitmentApp
{

    public partial class Form11 : Form
    {

        int employerId { get; set; }
        List<string> skillsList = new List<string>(); //list to store skills

        public Form11(int employerId)
        {

            InitializeComponent();

            this.employerId = employerId;

        }

        private void label1_Click(object sender, EventArgs e)
        {}

        private void Form11_Load(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            try
            {

                //load industries
                string industryQuery = @"SELECT INDUSTRYID, NAME FROM dbo.INDUSTRY";

                using (SqlCommand command = new SqlCommand(industryQuery, connection))
                {

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "NAME";
                    comboBox2.ValueMember = "INDUSTRYID";
                    comboBox2.Text = "Select Industry";

                }

                //load locations as combined city/country 
                string locationQuery = @"SELECT LOCATIONID, CITY + ', ' + COUNTRY AS FullLocation FROM dbo.LOCATION";

                using (SqlCommand command = new SqlCommand(locationQuery, connection))
                {

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "FullLocation";
                    comboBox1.ValueMember = "LOCATIONID";
                    comboBox1.Text = "Select Location";

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error loading form data: " + ex.Message);

            }

            connection.Close();

        }

        //confirm button click event
        private void button1_Click(object sender, EventArgs e)
        {

            //Validate combo box selections
            if(comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
            {

                MessageBox.Show("Please select a location and industry.");
                return;

            }

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            //query to get company name
            string coquery = @"SELECT COMPANYNAME FROM dbo.EMPLOYER WHERE USERID = '"+employerId+"'";
            SqlCommand sqlCommand = new SqlCommand(coquery, connection);
            string companyName = sqlCommand.ExecuteScalar().ToString();

            //inserting job details into table vacancy
            string query = @"INSERT INTO dbo.VACANCY (TITLE, DESCRIPTION, SALARYRANGE, REQUIREDEXPERIENCE, EXPIRATIONDATE, EMPLOYERID, LOCATIONID, INDUSTRYID, COMPANYNAME)
                            VALUES (@Title, @Description, @SalaryRange, @RequiredExperience, @ExpirationDate, @EmployerId, @Location, @Industry, @CoName)";
            int rowsAffected = 0, rowsAffected2 = 0;

            using (SqlCommand command = new SqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("@Title", textBox1.Text);
                command.Parameters.AddWithValue("@Description", textBox2.Text);
                command.Parameters.AddWithValue("@SalaryRange", textBox4.Text);
                command.Parameters.AddWithValue("@RequiredExperience", textBox3.Text);
                command.Parameters.AddWithValue("@ExpirationDate", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@EmployerId", employerId);
                command.Parameters.AddWithValue("@Location", ((DataRowView)comboBox1.SelectedItem)["LocationID"]);
                command.Parameters.AddWithValue("@Industry", ((DataRowView)comboBox2.SelectedItem)["IndustryID"]);
                command.Parameters.AddWithValue("@CoName", companyName);

                rowsAffected = command.ExecuteNonQuery();

            }

            //query to get the current vacancy id
            string query2 = @"SELECT VACANCYID FROM dbo.VACANCY WHERE EMPLOYERID = @EmployerId AND TITLE = @Title AND REQUIREDEXPERIENCE = @RequiredExperience";
            int vacancyId = 0;

            using(SqlCommand command = new SqlCommand(query2, connection))
            {

                command.Parameters.AddWithValue("@EmployerId", employerId);
                command.Parameters.AddWithValue("@Title", textBox1.Text);
                command.Parameters.AddWithValue("@RequiredExperience", textBox3.Text);

                vacancyId = Convert.ToInt32(command.ExecuteScalar());

            }

            //query to get skill id
            string querySkill = @"SELECT SKILLID FROM dbo.SKILL WHERE NAME = @SkillName";
            List<int> SkillsIDS = new List<int>();

            for(int i = 0; i < skillsList.Count; i++)
            {

                using(SqlCommand command = new SqlCommand(querySkill, connection))
                {

                    command.Parameters.AddWithValue("@SkillName", skillsList[i]);

                    //getting the skill id
                    int skillId = Convert.ToInt32(command.ExecuteScalar());

                    SkillsIDS.Add(skillId);

                }

            }

            //inserting skills into table vacancySkills
            string query3 = @"INSERT INTO dbo.VACANCYSKILL (VACANCYID, SKILLID) VALUES (@VacancyId, @SkillId)";

            for(int i = 0; i < SkillsIDS.Count; i++)
            {

                using (SqlCommand command = new SqlCommand(query3, connection))
                {

                    command.Parameters.AddWithValue("@VacancyId", vacancyId);
                    command.Parameters.AddWithValue("@SkillId", SkillsIDS[i]);

                    rowsAffected2 = command.ExecuteNonQuery();

                }

            }


            if(rowsAffected > 0 || rowsAffected2 > 0)
            {

                MessageBox.Show("Job Posted successfully!");

                Form10 form10 = new Form10(employerId);

                form10.Show();
                this.Close();

            }
            else MessageBox.Show("Posting failed. Please try again.");

            connection.Close();     

        }

        //add button click event
        private void button2_Click(object sender, EventArgs e)
        {

            //store current skill
            string skill = textBox5.Text.Trim();

            //make sure textBox isnt empty
            if(!string.IsNullOrEmpty(skill))
            {

                //check if skill already exists
                if(!skillsList.Contains(skill))
                {

                    skillsList.Add(skill);
                    textBox5.Clear();

                }
                else MessageBox.Show("Skill already added.");
            }
            else MessageBox.Show("Please enter a skill.");

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {}

        private void textBox4_TextChanged(object sender, EventArgs e)
        {}

    }

}
