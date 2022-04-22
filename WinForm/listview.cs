using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListView_sort2
{
    public partial class Form1 : Form
    {
        private int sortColumn = -1;
        private String[] listview_columnTitle = { "aa", "bb", "cc" };

        public Form1()
        {
            InitializeComponent();
        }
 
        private void Form1_Load(object sender, EventArgs e)
        {

            start_ListView();

            add_items();
        }

        private void start_ListView()
        {
            // throw new NotImplementedException();

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            int listview_width = listView1.ClientSize.Width;
            int col_width = listview_width / 3;

            listView1.Columns.Add(listview_columnTitle[0], col_width, HorizontalAlignment.Center);
            listView1.Columns.Add(listview_columnTitle[1], col_width, HorizontalAlignment.Center);
            listView1.Columns.Add(listview_columnTitle[2], col_width, HorizontalAlignment.Center);
        }

        private void add_items()
        {
            ListViewItem newitem = new ListViewItem(new String[] { "c1", "5", "2" });
            listView1.Items.Add(newitem);
            ListViewItem newitem2 = new ListViewItem(new String[] { "b1", "4", "3" });
            listView1.Items.Add(newitem2);
            ListViewItem newitem3 = new ListViewItem(new String[] { "c2", "3", "4" });
            listView1.Items.Add(newitem3);
            ListViewItem newitem4 = new ListViewItem(new String[] { "e1", "0", "5" });
            listView1.Items.Add(newitem4);
            ListViewItem newitem5 = new ListViewItem(new String[] { "d1", "1", "1" });
            listView1.Items.Add(newitem5);
        }


        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine whether the column is the same as the last column clicked.
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                listView1.Sorting = SortOrder.Ascending;
                listView1.Columns[sortColumn].Text = listview_columnTitle[sortColumn] + " ▲";
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (listView1.Sorting == SortOrder.Ascending)
                {
                    listView1.Sorting = SortOrder.Descending;
                    listView1.Columns[sortColumn].Text = listview_columnTitle[sortColumn] + " ▼";
                }
                else
                {
                    listView1.Sorting = SortOrder.Ascending;
                    listView1.Columns[sortColumn].Text = listview_columnTitle[sortColumn] + " ▲";

                }
            }

            // Call the sort method to manually sort.
            listView1.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            this.listView1.ListViewItemSorter = new MyListViewComparer(e.Column, listView1.Sorting);
        
        }
    }


    class MyListViewComparer : IComparer
    {
        private int col;
        private SortOrder order;
        public MyListViewComparer() {
            col=0;
            order = SortOrder.Ascending;
        }
        public MyListViewComparer(int column, SortOrder order) 
        {
            col=column;
            this.order = order;
        }
        public int Compare(object x, object y) 
        {
            int returnVal= -1;
            returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                    ((ListViewItem)y).SubItems[col].Text);
            // Determine whether the sort order is descending.
            if (order == SortOrder.Descending)
                // Invert the value returned by String.Compare.
                returnVal *= -1;
            return returnVal;
        }

    }
}