using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Video_Rental_System
{
    public class Video
    {
        //Database connection
        private SqlConnection Conn = new SqlConnection(@"Data Source=KING-PC\SQLEXPRESS;Initial Catalog=MusicRent;Integrated Security=True");
        private SqlCommand Cmd = new SqlCommand();
        private SqlDataReader SqlReader;
        private string Str;

        // Add_video() method is used for insert movie data into the movie tables after getting this data from textboxes.
        public void Add_video(string Rating_TextBox, string Title_TextBox, string Year_TextBox, string Rental_Cost_TextBox, string Copies_TextBox, string Plot_TextBox, string Genre_TextBox)
        {
            this.Cmd.Parameters.Clear();
            try
            {
                this.Cmd.Parameters.Clear();
                this.Cmd.Connection = this.Conn;
                this.Str = "Insert into Movies(Rating,Title,Year,Rental_Cost,Copies,Plot,Genre) Values(@Rating, @Title, @Year, @Rental_Cost, @Copies, @Plot, @Genre)";
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@Rating", Rating_TextBox), new SqlParameter("@Title", Title_TextBox), new SqlParameter("@Year", Year_TextBox), new SqlParameter("@Rental_Cost", Rental_Cost_TextBox), new SqlParameter("@Copies", Copies_TextBox), new SqlParameter("@Plot", Plot_TextBox), new SqlParameter("@Genre", Genre_TextBox) };
                this.Cmd.Parameters.Add(parameterArray[0]);
                this.Cmd.Parameters.Add(parameterArray[1]);
                this.Cmd.Parameters.Add(parameterArray[2]);
                this.Cmd.Parameters.Add(parameterArray[3]);
                this.Cmd.Parameters.Add(parameterArray[4]);
                this.Cmd.Parameters.Add(parameterArray[5]);
                this.Cmd.Parameters.Add(parameterArray[6]);
                this.Cmd.CommandText = this.Str;
                this.Conn.Open();
                this.Cmd.ExecuteNonQuery();
                this.Conn.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Database Exception" + exception.Message);
                this.Conn.Close();
            }
        }

        // Delete_video() is used for delete video in the movie tables.
        public void Delete_video(int MovieID)
        {
            try
            {
                Cmd.Parameters.Clear();
                this.Cmd.Connection = this.Conn;
                this.Str = "Delete from Movies where MovieID like @MovieID";
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@MovieID", MovieID) };
                this.Cmd.Parameters.Add(parameterArray[0]);
                this.Cmd.CommandText = this.Str;
                this.Conn.Open();
                this.Cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Database Exception" + exception.Message);
            }
            finally
            {
                if (this.Conn != null)
                {
                    this.Conn.Close();
                }
            }
        }
        //Loadvideodata() method is used to load the video data into the data grid from video table.
        public DataTable LoadvideoData()
        {
            DataTable table = new DataTable();
            try
            {
                this.Cmd.Connection = this.Conn;
                this.Str = "Select * from Movies";
                this.Cmd.CommandText = this.Str;
                this.Conn.Open();
                this.SqlReader = this.Cmd.ExecuteReader();
                if (this.SqlReader.HasRows)
                {
                    table.Load(this.SqlReader);
                }
                this.Conn.Close();
                return table;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Database Exception" + exception.Message);
                this.Conn.Close();
                return null;
            }
        }
        //LoadRenteddata() method is used to load the rented data into the data grid from rented table.
        public DataTable LoadRentedData()
        {
            DataTable table = new DataTable();
            try
            {
                this.Cmd.Connection = this.Conn;
                this.Str = "Select RMID,MovieIDFK,CustIDFK,DateRented,DateReturned from RentedMovies";
                this.Cmd.CommandText = this.Str;
                this.Conn.Open();
                this.SqlReader = this.Cmd.ExecuteReader();
                if (this.SqlReader.HasRows)
                {
                    table.Load(this.SqlReader);
                }
                this.Conn.Close();
                return table;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Database Exception" + exception.Message);
                this.Conn.Close();
                return null;
            }
        }
        //LoadCustomerRankData() method is used to load the best customer data into the grid from CustRatting table.
        public DataTable LoadCustomerRankData()
        {
            DataTable table = new DataTable();
            try
            {
                this.Cmd.Connection = this.Conn;
                this.Str = "Select * from CustRatting ORDER BY Cnt DESC";
                this.Cmd.CommandText = this.Str;
                this.Conn.Open();
                this.SqlReader = this.Cmd.ExecuteReader();
                if (this.SqlReader.HasRows)
                {
                    table.Load(this.SqlReader);
                }
                this.Conn.Close();
                return table;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Database Exception" + exception.Message);
                this.Conn.Close();
                return null;
            }
        }
        //LoadMovieRankData() method is used to load the most popular movie's data into the grid from Movieratting table.
        public DataTable LoadMovieRankData()
        {
            DataTable table = new DataTable();
            try
            {
                this.Cmd.Connection = this.Conn;
                this.Str = "Select * from MovieRatting ORDER BY Cnt DESC";
                this.Cmd.CommandText = this.Str;
                this.Conn.Open();
                this.SqlReader = this.Cmd.ExecuteReader();
                if (this.SqlReader.HasRows)
                {
                    table.Load(this.SqlReader);
                }
                this.Conn.Close();
                return table;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Database Exception" + exception.Message);
                this.Conn.Close();
                return null;
            }
        }

        public int year(int diff)
        {
            if (diff >= 0 && diff <= 5)
            {
                //it will display the cost of rent
                return 5;
            }
            else if (diff > 5)
            {
                //it will display the cost of ren
                return 2;
            }
            else
            {
                return 0;
            }
        }

        // Update_video() method is used for update movie data into the movie tables after getting this data from textboxes..
        public void Update_video(int MovieID, string Rating, string Title, string Year, int Rental_Cost, string Copies, string Plot, string Genre)
        {
            try
            {
                Cmd.Parameters.Clear();
                Cmd.Connection = Conn;
                Str = "Update Movies Set Rating = @Rating, Title = @Title, Year = @Year, Rental_Cost = @Rental_Cost, Copies = @Copies, Plot= @Plot, Genre = @Genre where MovieID = @MovieID";
                SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@MovieID", MovieID), new SqlParameter("@Rating", Rating), new SqlParameter("@Title", Title), new SqlParameter("@Year", Year), new SqlParameter("@Rental_Cost", Rental_Cost), new SqlParameter("@Copies", Copies), new SqlParameter("@Plot", Plot), new SqlParameter("@Genre", Genre) };
                Cmd.Parameters.Add(parameterArray[0]);
                Cmd.Parameters.Add(parameterArray[1]);
                Cmd.Parameters.Add(parameterArray[2]);
                Cmd.Parameters.Add(parameterArray[3]);
                Cmd.Parameters.Add(parameterArray[4]);
                Cmd.Parameters.Add(parameterArray[5]);
                Cmd.Parameters.Add(parameterArray[6]);
                Cmd.Parameters.Add(parameterArray[7]);
                Cmd.CommandText = Str;
                Conn.Open();
                Cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Database Exception" + exception.Message);
            }
            finally
            {
                if (this.Conn != null)
                {
                    this.Conn.Close();
                }
            }
        }

    }
}
