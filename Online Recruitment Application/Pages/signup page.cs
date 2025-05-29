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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RecruitmentApp
{

    public partial class Form4 : Form
    {

        public Form4()
        {InitializeComponent();}

        private void label6_Click(object sender, EventArgs e)
        {}

        private void label1_Click(object sender, EventArgs e)
        {}

        private void label2_Click(object sender, EventArgs e)
        {}

        //confirm button click event
        private void button1_Click(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            connection.Open();

            if(comboBox1.SelectedItem != null)
            {

                string selectedOption = comboBox1.SelectedItem.ToString();
                
                //if JobSeeker is selected insert his details to user table first then to JobSeeker table
                if(selectedOption == "JobSeeker")
                {

                    textBox4.Visible = true;
                    label1.Visible = true;

                    string query = "INSERT INTO JOBSEEKER(UserID, Name, Email, Password, UserType, PhoneNumber, Exprience) VALUES (@param6, @param1, @param2, @param3, @param7, @param4, @param5)";

                    //using SCOPE_IDENTITY() function to return the last generated userID
                    string query2 = "INSERT INTO [USER](Name, Email, Password, UserType, PhoneNumber) VALUES (@param1, @param2, @param3, @param5, @param4); SELECT SCOPE_IDENTITY();";
                    int userId = 0;

                    //inserting user details in user table 
                    using(SqlCommand command = new SqlCommand(query2, connection))
                    {

                        command.Parameters.AddWithValue("@param1", textBox1.Text);
                        command.Parameters.AddWithValue("@param2", textBox2.Text);
                        command.Parameters.AddWithValue("@param3", textBox3.Text);
                        command.Parameters.AddWithValue("@param5", selectedOption);
                        command.Parameters.AddWithValue("@param4", textBox5.Text);

                        //getting the last generated userID
                        userId = Convert.ToInt32(command.ExecuteScalar());

                    }
                    using(SqlCommand command = new SqlCommand(query, connection)) //inserting user details in JobSeeker table
                    {

                        command.Parameters.AddWithValue("@param6", userId);
                        command.Parameters.AddWithValue("@param1", textBox1.Text);
                        command.Parameters.AddWithValue("@param2", textBox2.Text);
                        command.Parameters.AddWithValue("@param3", textBox3.Text);
                        command.Parameters.AddWithValue("@param7", selectedOption);
                        command.Parameters.AddWithValue("@param4", textBox5.Text);
                        command.Parameters.AddWithValue("@param5", textBox4.Text);

                        int rowsAffected = command.ExecuteNonQuery();

                        if(rowsAffected > 0)
                        {
                            MessageBox.Show("User registered successfully!");

                            Form1 login = new Form1();

                            login.Show();
                            this.Close();

                        }
                        else MessageBox.Show("Registration failed. Please try again.");

                    }

                }
                else if(selectedOption == "Employer")
                {

                    textBox6.Visible = true;
                    label7.Visible = true;

                    string query = "INSERT INTO EMPLOYER(UserID, Name, Email, Password, UserType, PhoneNumber, CompanyName) VALUES (@param6, @param1, @param2, @param3, @param7, @param4, @param5)";

                    //using SCOPE_IDENTITY() function to return the last generated userID
                    string query2 = "INSERT INTO [USER](Name, Email, Password, UserType, PhoneNumber) VALUES (@param1, @param2, @param3, @param5, @param4); SELECT SCOPE_IDENTITY();";
                    int userId = 0;

                    //inserting user details in user table 
                    using(SqlCommand command = new SqlCommand(query2, connection))
                    {

                        command.Parameters.AddWithValue("@param1", textBox1.Text);
                        command.Parameters.AddWithValue("@param2", textBox2.Text);
                        command.Parameters.AddWithValue("@param3", textBox3.Text);
                        command.Parameters.AddWithValue("@param5", selectedOption);
                        command.Parameters.AddWithValue("@param4", textBox5.Text);

                        //getting the last generated userID
                        userId = Convert.ToInt32(command.ExecuteScalar());

                    }
                    using(SqlCommand command = new SqlCommand(query, connection)) //inserting user details in Employer table
                    {

                        command.Parameters.AddWithValue("@param6", userId);
                        command.Parameters.AddWithValue("@param1", textBox1.Text);
                        command.Parameters.AddWithValue("@param2", textBox2.Text);
                        command.Parameters.AddWithValue("@param3", textBox3.Text);
                        command.Parameters.AddWithValue("@param7", selectedOption);
                        command.Parameters.AddWithValue("@param4", textBox5.Text);
                        command.Parameters.AddWithValue("@param5", textBox6.Text);

                        int rowsAffected = command.ExecuteNonQuery();

                        if(rowsAffected > 0)
                        {

                            MessageBox.Show("User registered successfully!");

                            Form1 login = new Form1();

                            login.Show();
                            this.Close();

                        }
                        else MessageBox.Show("Registration failed. Please try again.");

                    }

                }

            }

            connection.Close();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {}

        private void textBox2_TextChanged(object sender, EventArgs e)
        {}

        private void textBox3_TextChanged(object sender, EventArgs e)
        {}

        private void textBox4_TextChanged(object sender, EventArgs e)
        {}

        private void textBox5_TextChanged(object sender, EventArgs e)
        {}

        private void Form4_Load(object sender, EventArgs e)
        {

            //hiding the special attriutes untill user type is selected
            textBox4.Visible = false;
            label1.Visible = false;
            textBox6.Visible = false;
            label7.Visible = false;

            //add items
            comboBox1.Items.Add("JobSeeker");
            comboBox1.Items.Add("Employer");

            comboBox1.Text = "Select a user type..."; //set default text

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {}

        private void label1_Click_1(object sender, EventArgs e)
        {}

        private void textBox6_TextChanged(object sender, EventArgs e)
        {}

        //confirm selection button click event
        private void button2_Click(object sender, EventArgs e)
        {

            //choosing between jobseeker and employer
            if(comboBox1.SelectedItem != null)
            {

                string selectedOption = comboBox1.SelectedItem.ToString();

                if(selectedOption == "JobSeeker")
                {

                    textBox4.Visible = true;
                    label1.Visible = true;

                }
                else if(selectedOption == "Employer")
                {

                    textBox6.Visible = true;
                    label7.Visible = true;

                }

            }
            else MessageBox.Show("You Must Select a type!");

        }

    }

}
