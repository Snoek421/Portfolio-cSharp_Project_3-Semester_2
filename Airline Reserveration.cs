using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string customerName;
        bool allSeatsFull = false;
        int waitingListCount = 0;
        bool waitingListActive = false;
        bool waitingListFull = false;
        string[] waitingListArray = new string[10];
        string[,] seatIdList = new string[5, 3] { { "A0", "A1", "A2" }, { "B0", "B1", "B2" }, { "C0", "C1", "C2" }, { "D0", "D1", "D2" }, { "E0", "E1", "E2" } };
        string[,] seatsListArray = new string[5, 3];


        //////////////////////////// Start of Methods //////////////////////////

        //EmptySeatcount method for checking empty seats, returning the count of them, and setting the allSeatsFull boolean to the currently accurate value
        private int EmptySeatCount()
        {

            int emptyIndexes = 0;
            for (int i = 0; i < 5; i++) // loop through all entries of seating array, check if all seats are empty
            {
                for (int j = 0; j < 3; j++)
                {
                    if (string.IsNullOrEmpty(seatsListArray[i, j]))
                    {
                        emptyIndexes++;
                    }
                }
            }
            if (emptyIndexes == 0)
            {
                allSeatsFull = true; //if no empty seats available then set allSeatsFull to true
                return emptyIndexes;
            }
            else
            {
                allSeatsFull = false;
                return emptyIndexes;
            }
        }

        // ShowSeats method to show seats cleanly in the correct box
        private void ShowSeats(int emptySeats)
        {
            if (emptySeats == 15) //if all seats are empty then display the seat Ids in left rich textbox
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        rtxAllSeats.Text += seatIdList[i, j] + Environment.NewLine;
                    }
                }
            }
            else  //otherwise, loop through arrays and display each seat ID or seat ID + booked name in left rich textbox
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (string.IsNullOrEmpty(seatsListArray[i, j])) //if array index for given seat position is empty, show seat id
                        {
                            rtxAllSeats.Text += seatIdList[i, j] + Environment.NewLine;
                        }
                        else
                        {
                            rtxAllSeats.Text += seatsListArray[i, j] + Environment.NewLine; //if array has entry for that seat position, show entry
                        }
                    }
                }
            }
        }
        
        // GetRowAndColumn method to retrieve the selected row and column in their listboxes and convert/store them as the array coordinates they represent
        private void GetRowAndColumn(out int seatRow, out int seatColumn)
        {
            seatRow = -1;
            seatColumn = -1;
            switch (lsbSeatRow.SelectedItem.ToString()) //get the letter row that was selected and obtain the corresponding index number
            {
                case "A":
                    seatRow = 0;
                    break;
                case "B":
                    seatRow = 1;
                    break;
                case "C":
                    seatRow = 2;
                    break;
                case "D":
                    seatRow = 3;
                    break;
                case "E":
                    seatRow = 4;
                    break;
                default:
                    break;
            }
            string seatColumnSelection = lsbSeatColumn.SelectedItem.ToString();//get the value for the selected column number
            seatColumn = Convert.ToInt16(seatColumnSelection); //convert selected value string to int
        }

        ////////////////////// Start of button handlers //////////////////////////////
        
        private void btnShowAllSeats_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false; //turn label visibility off at start of any action
            lblMessage.Text = ""; //clear bottom label text at start of any action
            rtxAllSeats.Clear(); //clear rich textbox before populating it

            int emptySeats = EmptySeatCount(); //check empty seats
            ShowSeats(emptySeats); //show all seats
        }

        private void btnFillAll_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
            for (int i=0;i<5;i++)
            {
                for (int j=0;j<3;j++)
                {
                    seatsListArray[i, j] = seatIdList[i,j] + " - Casey Snoek"; //for each index of 2d array, fill with cooresponding seat ID and my name
                }
            }
            allSeatsFull = true; //change seats available to false
        }

        private void btnShowWaitingList_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
            rtxWaitingList.Clear(); //Clear rich textbox before populating it
            foreach (string person in waitingListArray)
            {
                rtxWaitingList.Text += person + Environment.NewLine; //for each index in waiting list array, display the person's name in the right rich textbox
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
            customerName = txtNameInput.Text.Trim(); //take text (or lack of text) from name input box
            if (string.IsNullOrEmpty(customerName)) //if name input is empty or null then show message saying to please input a name before booking
            {
                lblMessage.Visible = true; //before any label message text is changed, make sure label is visible
                lblMessage.Text = "No name has been entered. Please enter a name before attempting to book a seat.";
                return;
            }
            else if (lsbSeatRow.SelectedIndex == -1 || lsbSeatColumn.SelectedIndex == -1) //if seat row or column was not selected, show message saying to select a row and column before booking
            {
                lblMessage.Visible = true;
                lblMessage.Text = "No seat has been selected. Please select a row and column before attempting to book a seat.";
                return;
            }
            else if (allSeatsFull == true) //if all seats are full then check if waiting list is full
            {
                if (waitingListFull == true) //if waiting list is full then show message saying seats and waiting list are full
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "No seats are available and waiting list is full.";
                    return;
                }

                else if (waitingListFull == false) //if seats are full but waiting list is not full
                {
                    for (int i = 0; i < waitingListArray.Length; i++)
                    {
                        /*if (waitingListArray[i] == customerName) //disabled for debugging purposes (easy repeated entry to fill up waiting list)
                        {
                            lblMessage.Visible=true;
                            lblMessage.Text = "Your name is already on the waiting list.";
                            return;
                        }*/

                        if (string.IsNullOrEmpty(waitingListArray[i])) // check each waiting list index until empty one is found
                        {
                            waitingListArray[i] = txtNameInput.Text; //commit input name into
                            waitingListActive = true;
                            waitingListCount++; //increase the waiting list count by one
                            if (waitingListCount == 10)
                            {
                                waitingListFull = true;
                            }
                            lblMessage.Visible = true;
                            lblMessage.Text = "Your name has been added to the waiting list.";

                            rtxWaitingList.Clear();
                            foreach (string person in waitingListArray) //clear and re-fill the waiting list textbox
                            {
                                rtxWaitingList.Text += person + Environment.NewLine; //for each index in waiting list array, display the person's name in the right rich textbox
                            }

                            break;
                        }
                    }
                }
            }
            else if (allSeatsFull == false)
            {
                GetRowAndColumn(out int seatRow, out int seatColumn); //get the selected row and column from the listboxes and store them as the array coordinates they represent

                if (!string.IsNullOrEmpty(seatsListArray[seatRow, seatColumn])) //if seat is not empty, display message that seat is already booked
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Your selected seat is already booked.";
                    return;
                }
                else if (string.IsNullOrEmpty(seatsListArray[seatRow, seatColumn])) //if selected seat is empty
                {
                    seatsListArray[seatRow, seatColumn] = seatIdList[seatRow, seatColumn] + " - " + customerName; //commit customer's name to selected seat index
                    lblMessage.Visible = true;
                    lblMessage.Text = "Seat has been successfully booked. Congratulations!";

                    int emptySeats = EmptySeatCount(); //after committing new entry into seats array, use method to count empty seats and update the allSeatsFull boolean

                    if (rtxAllSeats.Lines.Length < 15) //if less than 15 lines of text in left textbox (show all seats button not clicked and user hasn't booked all 15 seats
                    {
                        rtxAllSeats.Text += seatsListArray[seatRow, seatColumn] + Environment.NewLine; // add newly booked seat into the textbox one per line
                    }
                    else if (rtxAllSeats.Lines.Length >= 15) //if left textbox has 15 rows of text or more
                    {
                        rtxAllSeats.Clear(); //clear textbox and call the method to update all shown seats.
                        ShowSeats(emptySeats);
                    }

                }

            }


        }

        private void btnAddWaitingList_Click(object sender, EventArgs e)
        {
            lblMessage.Visible=false;
            lblMessage.Text = "";
            int emptySeats = EmptySeatCount(); //check if seats are available

            if (allSeatsFull == false) //if seats are available, show message that seats are available
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Seats are available";
                return;
            }
            if (allSeatsFull == true && waitingListFull == true) //if all seats are full and waiting list is full, show message that waiting list is full
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Waiting list is full.";
            }
            else if (allSeatsFull && waitingListFull == false)
            {
                customerName = txtNameInput.Text.Trim(); //take text (or lack of text) from name input box
                if (string.IsNullOrEmpty(customerName)) //if name input is empty or null then show message saying to please input a name before adding to waiting list
                {
                    lblMessage.Visible = true; //before any label message text is changed, make sure label is visible
                    lblMessage.Text = "No name has been entered. Please enter a name before attempting to add it to the waiting list.";
                    return;
                }
                else
                {
                    for (int i = 0; i < waitingListArray.Length; i++)
                    {
                        /*if (waitingListArray[i] == customerName) //disabled for debugging purposes (easy repeated entry to fill up waiting list)
                        {
                            lblMessage.Visible=true;
                            lblMessage.Text = "Your name is already on the waiting list.";
                            return;
                        }*/

                        if (string.IsNullOrEmpty(waitingListArray[i])) // check each waiting list index until empty one is found
                        {
                            waitingListArray[i] = txtNameInput.Text; //commit input name into
                            waitingListActive = true;
                            waitingListCount++; //increase the waiting list count by one
                            if (waitingListCount == 10)
                            {
                                waitingListFull = true;
                            }
                            lblMessage.Visible = true;
                            lblMessage.Text = "Your name has been added to the waiting list.";

                            rtxWaitingList.Clear();
                            foreach (string person in waitingListArray)
                            {
                                rtxWaitingList.Text += person + Environment.NewLine; //for each index in waiting list array, display the person's name in the right rich textbox
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            GetRowAndColumn(out int seatRow, out int seatColumn);
            if (string.IsNullOrEmpty(seatsListArray[seatRow, seatColumn]))
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Seat is EMPTY.";
                txtStatusBox.Text = "Empty";
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Seat is OCCUPIED: " + seatsListArray[seatRow, seatColumn];
                txtStatusBox.Text = seatsListArray[seatRow, seatColumn];
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (lsbSeatRow.SelectedIndex == -1 || lsbSeatColumn.SelectedIndex == -1) //if seat row or column was not selected, show message saying to select a row and column before booking
            {
                lblMessage.Visible = true;
                lblMessage.Text = "No seat has been selected. Please select a row and column before attempting to cancel a booking.";
                return;
            }
                GetRowAndColumn(out int seatRow, out int seatColumn); //get selected row and column coordinates of seat to cancel

            if (!string.IsNullOrEmpty(seatsListArray[seatRow, seatColumn])) // if selected seat is booked then show messagebox asking for user confirmation
            {
                string message = "Are you sure you want to cancel the booking of seat " + lsbSeatRow.SelectedItem.ToString() + lsbSeatColumn.SelectedItem.ToString() + " ?";
                string caption = "Please click Yes to continue or No to go back.";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo; 
                DialogResult result;
                //above code is preparing strings and properties to show in the messagebox and to store the user's resulting clicked button

                result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question); //show messagebox with properties and strings from above


                string seatSwapStorage = ""; 
                if (result == DialogResult.Yes) //if user confirms cancellation of seat then >>
                {
                    for (int i=0; i<waitingListArray.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(waitingListArray[i])) //find first name stored in waiting list
                        {
                            waitingListActive = true;
                            seatSwapStorage = waitingListArray[i]; //store first name in waiting list in temporary string for swapping to seat
                            waitingListArray[i] = ""; // change first name in waiting list into empty string
                            for (int j = 0; j < waitingListArray.Length; j++)
                            {
                                if (j == 9) //on last index, escape loop before causing an error
                                {
                                    waitingListArray[j] = "";
                                    break;
                                }
                                waitingListArray[j] = waitingListArray[j + 1]; //move each stored waiting list name down by one index
                            }
                            break; //escape loop
                        }
                        else if (string.IsNullOrEmpty(waitingListArray[i])) //if no names are found in waiting list then continue deleting booked seat
                        {
                            break; //escape loop
                        }
                    }
                    if (waitingListActive == true)
                    {                         
                        rtxAllSeats.Text = rtxAllSeats.Text.Replace(seatsListArray[seatRow, seatColumn], seatIdList[seatRow,seatColumn] + " - " + seatSwapStorage); //find and replace the selected seat's line in the rich textbox with the correctly formatted new entry
                        seatsListArray[seatRow, seatColumn] = seatIdList[seatRow, seatColumn] + " - " + seatSwapStorage; //commit the name stored in seatSwapStorage into the seat list array in the correct format
                        rtxWaitingList.Clear(); //clear waiting list text box
                        foreach (string person in waitingListArray)
                        {
                            rtxWaitingList.Text += person + Environment.NewLine; // re-fill waiting list text box with waiting list after moving all names down by 1
                        }

                        lblMessage.Visible = true;
                        lblMessage.Text = "Seat booking cancelled, the seat has been given to the first member on the waiting list."; //display message saying why the seat is not empty
                        waitingListFull = false;
                    }
                    else
                    {
                        rtxAllSeats.Text = rtxAllSeats.Text.Replace(seatsListArray[seatRow, seatColumn], seatIdList[seatRow, seatColumn]); //replace respective seat's line in seat chart with the seat ID
                        seatsListArray[seatRow, seatColumn] = ""; //change entry in seats array to be empty string
                        lblMessage.Visible = true;
                        lblMessage.Text = "Seat booking has been successfully cancelled. Seat is now available."; //display message saying booking was cancelled
                    }
                }
                else if(result == DialogResult.No) //if user cancels cancellation then escape the function
                {
                    return;
                }

            }
        }
    }
}




