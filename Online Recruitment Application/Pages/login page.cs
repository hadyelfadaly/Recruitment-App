using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Configuration;

namespace RecruitmentApp
{

    public partial class Form1 : Form
    {

        public Form1()
        {InitializeComponent();}

        //login button click event
        private void button1_Click(object sender, EventArgs e)
        {

            //connection string from app.config file
            string conn = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            SqlConnection connection = new SqlConnection(conn);

            string query = "SELECT COUNT(*) FROM [USER] WHERE Email = @Email AND Password = @Password";

            connection.Open();

            using(SqlCommand command = new SqlCommand(query, connection))
            { 

                command.Parameters.AddWithValue("Email", textBox1.Text);
                command.Parameters.AddWithValue("Password", textBox2.Text);

                int userCount = Convert.ToInt32(command.ExecuteScalar());

                //checking if the user exists in the database
                if(userCount > 0)
                {

                    MessageBox.Show("Login successful");

                    string query2 = "SELECT UserType FROM [USER] WHERE Email = @Email AND Password = @Password";
                    string userTypeString = "";

                    //getting user type from the database
                    using(SqlCommand command1 = new SqlCommand(query2, connection))
                    {

                        command1.Parameters.AddWithValue("Email", textBox1.Text);
                        command1.Parameters.AddWithValue("Password", textBox2.Text);

                        userTypeString = Convert.ToString(command1.ExecuteScalar());
                    
                    }

                    string query3 = "SELECT UserID FROM [USER] WHERE Email = @Email AND Password = @Password";
                    int ID = 0;

                    //getting user ID from the database
                    using (SqlCommand command2 = new SqlCommand(query3, connection))
                    {

                        command2.Parameters.AddWithValue("Email", textBox1.Text);
                        command2.Parameters.AddWithValue("Password", textBox2.Text);
                        
                        ID = Convert.ToInt32(command2.ExecuteScalar());

                    }

                    if (userTypeString == "JobSeeker")
                    {

                        //open the next form here
                        Form2 jobSeekerPage = new Form2(ID);

                        jobSeekerPage.Show();

                    }
                    else if(userTypeString == "Employer")
                    {

                        //open the next form here
                        Form3 employerPage = new Form3(ID);

                        employerPage.Show();

                    }

                    this.Hide(); //close the login page

                }
                else MessageBox.Show("Invalid email or password");

            }

            connection.Close();

        }

        private void label2_Click(object sender, EventArgs e)
        {}

        private void Form1_Load(object sender, EventArgs e)
        {}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {}

        private void textBox2_TextChanged(object sender, EventArgs e)
        {}

        //signup button click event
        private void button2_Click(object sender, EventArgs e)
        {

            Form4 signup = new Form4();

            signup.Show();
            this.Hide();

        }

    }

}
