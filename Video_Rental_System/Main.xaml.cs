using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Video_Rental_System
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Main : Window
    {
        // Fields
        Customer_Data Obj_Customer = new Customer_Data();
        Video Obj_video = new Video();
        public int CustomerID;
        public int MovieID;
        public int RentID;

        SqlConnection con;
        //Database connection
        String Constr = @"Data Source=LAPTOP-RAKIOMBV\SQLEXPRESS;Initial Catalog=MusicRent;Integrated Security=True";
        SqlCommand command;
        SqlDataReader reader;
        DataTable tbl = new DataTable();
        int cpy = 0;
        public Main()
        {
            //Below code is used fpr open the project in the centre of the screen.
            this.InitializeComponent();
            base.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private void Tab_Customer_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Customer");
            // load the Customer Data from the table to Grid
            DG_Customer.ItemsSource = Obj_Customer.LoadCustomerData().DefaultView;
            DateTime now = DateTime.Now;


        }

        private void Tab_Video_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Videoo");
            // load the Video Data from the table to Grid
            VideoDG.ItemsSource = Obj_video.LoadvideoData().DefaultView;


        }


        private void Tab_Rented_Loaded(object sender, RoutedEventArgs e)
        {
            // load the Rented Data from the table to Grid

            RentedDG.ItemsSource = Obj_video.LoadRentedData().DefaultView;

            //
        }

        private void Update_Customer_Click(object sender, RoutedEventArgs e)
        {
            //update the whole record of the Customer and pass the data to the Update_Customer function using the object of the class
            string text = FirstName_TextBox.Text;
            Obj_Customer.Update_Customer(CustomerID, FirstName_TextBox.Text, LastName_TextBox.Text, Address_TextBox.Text, Convert.ToInt32(Phone_TextBox.Text));
            //pop up customer update message.
            MessageBox.Show("Customer Updated");
            DG_Customer.ItemsSource = Obj_Customer.LoadCustomerData().DefaultView;
            FirstName_TextBox.Text = "";
            LastName_TextBox.Text = "";
            Address_TextBox.Text = "";
            Phone_TextBox.Text = "";
            CustDFK_TextBox.Text = "";
        }

        private void Update_Video_Click(object sender, RoutedEventArgs e)
        {
            //update the whole record of the VIdeo and pass the data to the Update_Video function using the object of the class
            int rent = Convert.ToInt32(Rental_Cost_TextBox.Text);


            Obj_video.Update_video(MovieID, Rating_TextBox.Text, Title_TextBox.Text, Year_TextBox.Text, rent, Copies_TextBox.Text, Plot_TextBox.Text, Genre_TextBox.Text);
            String qry = "";
            int copy = Convert.ToInt32(Copies_TextBox.Text.ToString()) - cpy;
            qry = "select * from tbMovies where  MovieID='" + MovieID + "'";
            DataTable tbl = new DataTable();
            tbl = this.getRecords(qry);
            int olcpy = Convert.ToInt32(tbl.Rows[0]["MovieCopy"].ToString());
            olcpy = olcpy + copy;
            MessageBox.Show("Copies left " + olcpy.ToString());
            qry = "update tbMovies set MovieCopy=" + olcpy + " where MovieID='" + MovieID + "'";

            this.runIDU(qry);


            MessageBox.Show("Movie Updated");


            VideoDG.ItemsSource = Obj_video.LoadvideoData().DefaultView;
            Rating_TextBox.Text = "";
            Title_TextBox.Text = "";
            Year_TextBox.Text = "";
            Rental_Cost_TextBox.Text = "";
            Copies_TextBox.Text = "";
            Plot_TextBox.Text = "";
            Genre_TextBox.Text = "";
            MovieDFK_TextBox.Text = "";
        }

        private void SelectCustomerRow(object sender, MouseButtonEventArgs e)
        {
            // get the data of Customer from data grid after double clicking and pass the whole record to the DataRowView in Array format
            DataRowView view = (DataRowView)DG_Customer.SelectedItems[0];
            CustomerID = Convert.ToInt32(view["CustID"]);

            CustDFK_TextBox.Text = Convert.ToString(CustomerID);

            // this code Automatically get current date
            DateRented_TextBox.Text = DateTime.Today.ToString("dd-MM-yyyy");

            FirstName_TextBox.Text = Convert.ToString(view["FirstName"]);
            LastName_TextBox.Text = Convert.ToString(view["LastName"]);
            Address_TextBox.Text = Convert.ToString(view["Address"]);
            Phone_TextBox.Text = Convert.ToString(view["Phone"]);
            DG_Customer.ItemsSource = Obj_Customer.LoadCustomerData().DefaultView;
        }

        private void SelectVideoRow(object sender, MouseButtonEventArgs e)
        {
            // get the data of video from data grid after double clicking and pass the whole record to the DataRowView in Array format
            DataRowView view = (DataRowView)VideoDG.SelectedItems[0];
            MovieID = Convert.ToInt32(view["MovieID"]);

            // this code Automatically get current date
            DateRented_TextBox.Text = DateTime.Today.ToString("dd-MM-yyyy");

            MovieDFK_TextBox.Text = Convert.ToString(MovieID);

            Rating_TextBox.Text = Convert.ToString(view["Rating"]);
            Title_TextBox.Text = Convert.ToString(view["Title"]);
            Year_TextBox.Text = Convert.ToString(view["Year"]);
            Rental_Cost_TextBox.Text = Convert.ToString(view["Rental_Cost"]);
            Copies_TextBox.Text = Convert.ToString(view["Copies"]);
            cpy = Convert.ToInt32(view["Copies"]);
            Plot_TextBox.Text = Convert.ToString(view["Plot"]);
            Genre_TextBox.Text = Convert.ToString(view["Genre"]);
            VideoDG.ItemsSource = Obj_video.LoadvideoData().DefaultView;
        }

        private void Delete_Customer_Click(object sender, RoutedEventArgs e)
        {
            int customerID = CustomerID;
            if (MessageBox.Show("Are you sure you want to delete the User?", "User", MessageBoxButton.YesNo).ToString() == "Yes")
            {
                //first of the all select the record from the Rented Movie is he already has a movie on rent or not if he has a movie on rent then he can't be able to delete the record from the table
                String qry = "";
                qry = "select * from RentedMovies where CustIDFK=" + customerID + "";
                tbl = this.getRecords(qry);
                if (tbl.Rows.Count > 0)
                {
                    //display the message if he has a movie on rent 
                    MessageBox.Show("Sorry,this customer didn't return the movie.");
                }
                else
                {
                    //if the customer does not have a movie on rent the cusotmer reocrd will be deleted from the table using the delete query and object of the Customer class 
                    Obj_Customer.Delete_Customer(customerID);

                    //after deleting the record the grid must be reload 
                    DG_Customer.ItemsSource = Obj_Customer.LoadCustomerData().DefaultView;

                    FirstName_TextBox.Text = "";
                    LastName_TextBox.Text = "";
                    Address_TextBox.Text = "";
                    Phone_TextBox.Text = "";
                    CustDFK_TextBox.Text = "";
                }
            }
        }


        private void Delete_Video_Click(object sender, RoutedEventArgs e)
        {
            int movieID = MovieID;
            if (MessageBox.Show("Are you sure you want to delete the Video?", "User", MessageBoxButton.YesNo).ToString() == "Yes")
            {
                string qry1 = "Select Count(*) from RentedMovies where MovieIDFK=" + movieID + " and DateReturned = 'Date Returned'";
                //object of Connection class to create the Connection with sql Server
                con = new SqlConnection(Constr);
                // open the Connection Using OPen using of the SQLConnection Class
                con.Open();
                // Command is the Object of the SqlCommand Class for executing a  Command
                command = new SqlCommand(qry1, con);
                // this function is used to execute the Command on the Connection
                int y = Convert.ToInt32(command.ExecuteScalar());
                if (y == 0)
                {
                    String qry = "";
                    //delete the video using the video id from the Tables using the Video class function
                    Obj_video.Delete_video(MovieID);

                    //after that record also delete from tbMovies from the table
                    qry = "";
                    qry = "delete from tbMovies where MovieID=" + movieID + "";
                    this.runIDU(qry);
                    MessageBox.Show("Video Deleted");

                    //reload the data to Grid after deleteing the record
                    VideoDG.ItemsSource = Obj_video.LoadvideoData().DefaultView;
                    Rating_TextBox.Text = "";
                    Title_TextBox.Text = "";
                    Year_TextBox.Text = "";
                    Rental_Cost_TextBox.Text = "";
                    Copies_TextBox.Text = "";
                    Plot_TextBox.Text = "";
                    MovieDFK_TextBox.Text = "";
                    DateRented_TextBox.Text = "";
                    DateReturned_TextBox.Text = "";
                    CustDFK_TextBox.Text = "";
                    Genre_TextBox.Text = "";
                }
                else
                {
                    //it will be show the pop up message hwen a customer didn't return a movie but you trying to delete his account.
                    MessageBox.Show("Sorry, the movie is on rent so you can't delete it until it returns back.");
                }
            }
        }
        private void Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            if (((FirstName_TextBox.Text != "") && ((LastName_TextBox.Text != "") && (Address_TextBox.Text != ""))) && Phone_TextBox.Text != "")
            {
                //add the customer record in the tables using the object of the Customer record file
                Obj_Customer.Add_Customer(FirstName_TextBox.Text, LastName_TextBox.Text, Address_TextBox.Text, Phone_TextBox.Text);

                // pass the data to grid using the LoadCustomerData Function
                DG_Customer.ItemsSource = Obj_Customer.LoadCustomerData().DefaultView;
                FirstName_TextBox.Text = "";
                LastName_TextBox.Text = "";
                Address_TextBox.Text = "";
                Phone_TextBox.Text = "";
            }
        }


        private void Add_Video_Click(object sender, RoutedEventArgs e)
        {
            if (((Rating_TextBox.Text != "") && ((Title_TextBox.Text != "") && ((Year_TextBox.Text != "") && ((Rental_Cost_TextBox.Text != "") && ((Copies_TextBox.Text != "") && (Plot_TextBox.Text != "")))))) && (Genre_TextBox.Text != ""))
            {
                //insert the record using the object of the video classs
                Obj_video.Add_video(Rating_TextBox.Text, Title_TextBox.Text, Year_TextBox.Text, Rental_Cost_TextBox.Text, Copies_TextBox.Text, Plot_TextBox.Text, Genre_TextBox.Text);
                String qry = "";
                int id = 1;
                // select the data from the movies tables and get the movie ID from the table
                qry = "select * from Movies";
                tbl = this.getRecords(qry);
                if (tbl.Rows.Count > 0)
                {
                    id = Convert.ToInt32(tbl.Rows[tbl.Rows.Count - 1]["MovieID"]);
                }
                qry = "";
                //inser the record after getting the record in the Movies tables to keep the record of the no of copies of the movies
                qry = "insert into tbMovies(MovieID,MovieCopy)values('" + id.ToString() + "'," + Convert.ToInt32(Copies_TextBox.Text.ToString()) + ")";
                this.runIDU(qry);
                MessageBox.Show("Video Record is Saved");

                //reload the data in the Grid
                VideoDG.ItemsSource = Obj_video.LoadvideoData().DefaultView;
                Rating_TextBox.Text = "";
                Title_TextBox.Text = "";
                Year_TextBox.Text = "";
                Rental_Cost_TextBox.Text = "";
                Copies_TextBox.Text = "";
                Plot_TextBox.Text = "";
                Genre_TextBox.Text = "";

            }
        }



        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            // this is used to close the window
            base.Close();

        }

        private void Issue_Movie_Click(object sender, RoutedEventArgs e)
        {
            // try {

            // get the record of the Customer How much MOvie he already have on rent if he has more then 2 movies on rent then he can't be able to get the movie on rent anymore

            String qry = "";
            int count = 1;
            // this task is done by using select query
            qry = "select * from RentedMovies where CustIDFK=" + Convert.ToInt32(CustDFK_TextBox.Text.ToString()) + "";
            tbl = this.getRecords(qry);
            for (int x = 0; x <= tbl.Rows.Count - 1; x++)
            {
                if (tbl.Rows[x]["DateReturned"].ToString().Equals("Date Returned"))
                {
                    count = count + 1;
                }
            }
            //MessageBox.Show(count.ToString());
            if (count > 2)
            {
                // if he already have more than 2 movie on rent then this message will be print
                MessageBox.Show("You Can't get Rental Movie Any More ");
            }
            else
            {
                // this query is used to get the copies of the movie is available now 
                int cnt = 1;
                qry = "";
                qry = "select * from tbMovies where MovieID='" + MovieDFK_TextBox.Text.ToString() + "'";
                tbl = this.getRecords(qry);

                int rent = Convert.ToInt32(tbl.Rows[tbl.Rows.Count - 1]["MovieCopy"]);
                MessageBox.Show("Customer Rented = " + rent.ToString() + "movies ");

                //after giving the movie on rent the counting must be decreased by 1
                rent = rent - 1;

                if (rent >= 0)
                {
                    // after decreasing the Counting the record mudt be updated  using the update query
                    qry = "";
                    qry = "update tbMovies set MovieCopy=" + rent + " where MovieID='" + MovieDFK_TextBox.Text.ToString() + "'";
                    this.runIDU(qry);

                    //   MessageBox.Show("copy set");

                    // Movie  Ratting 

                    // this query is used to keep the record of the movie that how much times it is issued on the rent 

                    qry = "";
                    qry = "select * from MovieRatting where MovieID='" + MovieDFK_TextBox.Text.ToString() + "'";
                    tbl = this.getRecords(qry);
                    if (tbl.Rows.Count > 0)
                    {
                        cnt = Convert.ToInt32(tbl.Rows[tbl.Rows.Count - 1]["Cnt"]);
                        cnt++;
                        qry = "";
                        //update the record using update query
                        qry = "update MovieRatting set Cnt=" + cnt + " where MovieID='" + MovieDFK_TextBox.Text.ToString() + "'";
                        this.runIDU(qry);
                        //MessageBox.Show("MOvie Ratting Done");

                    }
                    else
                    {
                        //if this is first time giving on rental then it's counting must be start from 1 and inser the record
                        qry = "";
                        qry = "insert into MovieRatting (MovieID,Cnt) values('" + MovieDFK_TextBox.Text.ToString() + "'," + cnt + ")";
                        this.runIDU(qry);


                    }




                    // customer Ratting  to keep the record of the customer that how many movies a customer rented 

                    //using select query we get the counting of the customer using the select query 
                    qry = "";
                    qry = "select * from CustRatting where CustID='" + CustDFK_TextBox.Text.ToString() + "'";
                    tbl = this.getRecords(qry);
                    if (tbl.Rows.Count > 0)
                    {
                        cnt = Convert.ToInt32(tbl.Rows[tbl.Rows.Count - 1]["Cnt"]);
                        cnt++;
                        //update the record using update query to keep the record updated
                        qry = "";
                        qry = "update CustRatting set Cnt=" + cnt + " where CustID='" + CustDFK_TextBox.Text.ToString() + "'";
                        this.runIDU(qry);
                    }
                    else
                    {
                        //if the customer is not registered then his counting must start from 1
                        cnt = 1;
                        qry = "";
                        qry = "insert into CustRatting(CustID,Cnt) values('" + CustDFK_TextBox.Text.ToString() + "'," + cnt + ")";
                        this.runIDU(qry);


                    }
                    qry = "";
                    // insert the Record to the Rented MOvies table after issuing the Movie on Rent  using RUNIDU Function
                    qry = "insert into RentedMovies(MovieIDFK,CustIDFK,DateRented,DateReturned) values(" + Convert.ToInt32(MovieDFK_TextBox.Text.ToString()) + "," + Convert.ToInt32(CustDFK_TextBox.Text.ToString()) + ",'" + DateRented_TextBox.Text.ToString() + "','Date Returned')";
                    this.runIDU(qry);

                    MessageBox.Show("Movies is Issued on Rent");

                    RentedDG.ItemsSource = Obj_video.LoadRentedData().DefaultView;


                    MovieDFK_TextBox.Text = "";
                    CustDFK_TextBox.Text = "";
                    DateRented_TextBox.Text = "";
                    DateReturned_TextBox.Text = "";
                    Rating_TextBox.Text = "";
                    Title_TextBox.Text = "";
                    Year_TextBox.Text = "";
                    Rental_Cost_TextBox.Text = "";
                    Copies_TextBox.Text = "";
                    Plot_TextBox.Text = "";
                    Genre_TextBox.Text = "";




                    FirstName_TextBox.Text = "";
                    LastName_TextBox.Text = "";
                    Address_TextBox.Text = "";
                    Phone_TextBox.Text = "";


                }
                else
                {
                    // pop up message when no nore copies of movies in the storage.
                    MessageBox.Show("All Copies are rented out ");
                }


            }



        }


        public void runIDU(String qry)
        {
            //object of Connection class to create the Connection with sql Server
            con = new SqlConnection(Constr);
            // open the Connection Using OPen using of the SQLConnection Class
            con.Open();
            // Command is the Object of the SqlCommand Class for executing a  Command
            command = new SqlCommand(qry, con);
            // this function is used to execute the Command on the Connection
            int y = command.ExecuteNonQuery();

            // Close the Connection using CLose function of the SQLCOnnection Classs
            con.Close();
        }

        public DataTable getRecords(String qry)
        {
            //object of Connection class to create the Connection with sql Server
            con = new SqlConnection(Constr);

            // open the Connection Using OPen using of the SQLConnection Class
            con.Open();

            // Command is the Object of the SqlCommand Class for executing a  Command
            command = new SqlCommand(qry, con);

            // this function is used to execute the Command on the Connection
            reader = command.ExecuteReader();

            // create the objec of the Datatable Class
            DataTable dt = new DataTable();

            //passs  the whole Data to the Reader Class
            dt.Load(reader);

            // Close the Connection using CLose function of the SQLCOnnection Classs
            con.Close();

            //return the Data Table Object
            return dt;
        }

        private void Copies_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {


        }

        private void Year_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                // get the Current Date of the System for Displaying the Charges automatically
                DateTime now = DateTime.Now;

                // get the Current Year
                int yr = now.Year;

                // get the Difference from Current Year and Movie Launched Year
                int diff = yr - Convert.ToInt32(Year_TextBox.Text.ToString());

                // if the Difference between 0 to 5 then the charges is 5 dollar other wise the Charges is 2 dollar

                int charge = Obj_video.year(diff);
                Rental_Cost_TextBox.Text = charge.ToString();

            }
            catch (Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }

        }


        private void Tab_Rented_GotFocus(object sender, RoutedEventArgs e)
        {

        }



        private void Dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            /*DataRowView view = (DataRowView)dg.SelectedItems[0];
            RentID = Convert.ToInt32(view["RMID"]);
            MovieDFK_TextBox.Text = Convert.ToString(view["MovieIDFK"]);
            CustDFK_TextBox.Text = Convert.ToString(view["CustIDFK"]);
            DateRented_TextBox.Text = Convert.ToString(view["DateRented"]);
            DateReturned_TextBox.Text= Convert.ToString(view["DateReturned"]);



            dg.ItemsSource = Obj_video.LoadRentedData().DefaultView;
            */


        }

        private void VideoDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Return_Movie_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(RentID.ToString());

            // get the date of the Movie Rented Date  for calculating the  charges for the movie rental

            DateTime old = Convert.ToDateTime(DateRented_TextBox.Text.ToString());

            // get the date of the Movie Return Date  for calculating the  charges for the movie rental

            DateTime nw = Convert.ToDateTime(DateReturned_TextBox.Text.ToString());

            //  get the differnece between the 2 dates using total days function in C#
            int diffrn = Convert.ToInt32((nw - old).TotalDays.ToString());

            //increased by one to calculate the exact charges
            diffrn++;



            String qry = "";
            // Movie  Ratting 
            qry = "";
            // this query is used to get the charges of the movie using the get records function  using movie id from movies table
            qry = "select * from Movies where MovieID=" + Convert.ToInt32(MovieDFK_TextBox.Text.ToString()) + "";
            tbl = this.getRecords(qry);

            // pass the cost to the local variable cst variable for printing and calculating the charges 
            int cst = Convert.ToInt32(tbl.Rows[tbl.Rows.Count - 1]["Rental_Cost"]);



            //MessageBox.Show("Cost is===" + cst);

            //MessageBox.Show("diff is===" + diffrn);

            // pass the charges to totalcharges as a local variable 
            int totalcharges = cst * diffrn;

            // increase the counting value after getting back from the customer using select query and get the last value from the tbmovies using Movie ID
            qry = "";
            qry = "select * from tbMovies where MovieID='" + MovieDFK_TextBox.Text.ToString() + "'";
            tbl = this.getRecords(qry);

            int rent = Convert.ToInt32(tbl.Rows[tbl.Rows.Count - 1]["MovieCopy"]);
            rent = rent + 1;
            //after getting the value of No of copies from MovieCopy then increased by one after getting the back movie from the Customer

            if (rent >= 0)
            {
                qry = "";
                //update the record using update query in the Movie Copy
                qry = "update tbMovies set MovieCopy=" + rent + " where MovieID='" + MovieDFK_TextBox.Text.ToString() + "'";
                this.runIDU(qry);

            }

            //after getting the movie back then delete the record from the RentedMovies tables
            qry = "";
            qry = "Update  RentedMovies set DateReturned='" + DateReturned_TextBox.Text.ToString() + "' where RMID=" + RentID + "";
            this.runIDU(qry);

            //display the pop up for total Charges to Pay
            MessageBox.Show("Your Total Charges to pay is $" + totalcharges);

            RentedDG.ItemsSource = Obj_video.LoadRentedData().DefaultView;

            MovieDFK_TextBox.Text = "";
            CustDFK_TextBox.Text = "";
            DateRented_TextBox.Text = "";
            DateReturned_TextBox.Text = "";

            Rating_TextBox.Text = "";
            Title_TextBox.Text = "";
            Year_TextBox.Text = "";
            Rental_Cost_TextBox.Text = "";
            Copies_TextBox.Text = "";
            Plot_TextBox.Text = "";
            Genre_TextBox.Text = "";




            FirstName_TextBox.Text = "";
            LastName_TextBox.Text = "";
            Address_TextBox.Text = "";
            Phone_TextBox.Text = "";








        }

        private void RentedDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // get the data after Double CLicking on the Grid and Pass the Record to DataRowView Class object and then get the data in the Array Format
            DataRowView view = (DataRowView)RentedDG.SelectedItems[0];
            //conver the id to Numeric
            RentID = Convert.ToInt32(view["RMID"]);
            MovieDFK_TextBox.Text = Convert.ToString(view["MovieIDFK"]);
            CustDFK_TextBox.Text = Convert.ToString(view["CustIDFK"]);
            DateRented_TextBox.Text = Convert.ToString(view["DateRented"]);
            DateReturned_TextBox.Text = DateTime.Today.ToString("dd-MM-yyyy");

            //reload the Grid after any updation
            RentedDG.ItemsSource = Obj_video.LoadRentedData().DefaultView;
        }

        private void Customer_Rate_Click(object sender, RoutedEventArgs e)
        {
            // this is used to open the Customer Ratiing Page
            //new CustomerRate().ShowDialog();
            //this.Hide();

            String qry = "";
            qry = "SELECT CustRatting.CustID,Customer.FirstName,Customer.LastName,CustRatting.Cnt from CustRatting INNER JOIN Customer on CustRatting.CustID = Customer.CustID order by CustRatting.Cnt DESC";





            // SELECT MovieRatting.MovieID,Movies.Title,MovieRatting.Cnt from MovieRatting INNER JOIN Movies on MovieRatting.MovieID = Movies.MovieID order by MovieRatting.Cnt DESC;


            DataTable tbl = new DataTable();
            tbl = this.getRecords(qry);
            String msg = "";
            msg = tbl.Rows[0]["FirstName"] + " " + tbl.Rows[0]["LastName"] + " rented most movies i.e. " + tbl.Rows[0]["Cnt"] + " movies";
            MessageBox.Show(msg);


        }



        private void Movie_Rate_Click(object sender, RoutedEventArgs e)
        {
            // this is used to open the Movie ranking Page
            // new MovieRate().ShowDialog();
            // this.Hide();

            String qry = "";
            qry = "SELECT MovieRatting.MovieID,Movies.Title,MovieRatting.Cnt from MovieRatting INNER JOIN Movies on MovieRatting.MovieID = Movies.MovieID order by MovieRatting.Cnt DESC";





            // ;


            DataTable tbl = new DataTable();
            tbl = this.getRecords(qry);
            String msg = "";
            msg = "The most popular movie is " + tbl.Rows[0]["Title"] + " (ID " + tbl.Rows[0]["MovieID"] + ") , rented " + tbl.Rows[0]["Cnt"] + " times";
            MessageBox.Show(msg);


        }

        private void Rental_Cost_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // for deleting the movie data from the grid which is returned.
        private void Delete_Movie_Rent_Click(object sender, RoutedEventArgs e)
        {
            String qry = "";
            qry = "select * from RentedMovies where RMID=" + RentID + "";
            DataTable tbl = new DataTable();
            tbl = this.getRecords(qry);
            if (tbl.Rows[0]["DateReturned"].ToString().Equals("Date Returned"))
            {
                // pop up message when the customer didn't returned the movie
                MessageBox.Show("You Can't Delete the Movie Record it is on Rental Movie");
            }
            else
            {
                qry = "";
                qry = "delete from RentedMovies where RMID=" + RentID + "";
                this.runIDU(qry);

            }
            RentedDG.ItemsSource = Obj_video.LoadRentedData().DefaultView;
            MovieDFK_TextBox.Text = "";
            CustDFK_TextBox.Text = "";
            DateRented_TextBox.Text = "";
            DateReturned_TextBox.Text = "";

        }
    }
}
