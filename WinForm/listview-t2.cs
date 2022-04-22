using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ListView_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;

            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.CheckBoxes = true;

            // listView1.LabelEdit = true;

            listView1.Columns.Add("a", 100); 
            listView1.Columns.Add("b");
            listView1.Columns.Add("c", 50, HorizontalAlignment.Right);
            listView1.Columns.Add("d", 50, HorizontalAlignment.Center);

            /*
            //  String[] aa = {"aa1", "aa2"}; 와 같이 일부 항목만 입력해도 ListView 에 표시되지만,
            // 나중에  item.SubItems[index].Text 로 나중에 접근시 ArgumentOutOfRangeException 발생한다.
            // 모두 입력하는게 좋다.
             */
            String[] aa = {"aa1", "aa2", "", "" };
            ListViewItem newitem = new ListViewItem(aa);
            listView1.Items.Add(newitem);

            newitem = new ListViewItem(new String[] { "bb1", "bb2", "bb3", ""});
            listView1.Items.Add(newitem);

        }

        // ListViewItem 추가하기
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                ListViewItem item;

                if (i % 2 == 0)
                {
                    item = new ListViewItem(); // 첫칸 빈칸 입력
                }
                else
                {
                    item = new ListViewItem("첫칸");
                }

                item.SubItems.Add(i.ToString());
                item.SubItems.Add("");  // 첫칸과 달리, 빈칸 입력시 "" 필수.
                item.SubItems.Add(i.ToString());
                listView1.Items.Add(item);
            }
        }


        // 체크박스 toggle
        private void button2_Click(object sender, EventArgs e)
        {
            // listView1.CheckBoxes = false; // default 설정.
            listView1.CheckBoxes = !listView1.CheckBoxes;

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                {
                    Console.WriteLine(" cheched row index = {0}, {1}", item.Index, item.SubItems[0].Text);
                }
            }
        }


        // Column 추가 삭제 반복
        private void button3_Click(object sender, EventArgs e)
        {
            String msg = String.Format("칼럼수 == {0}", listView1.Columns.Count);
            Console.WriteLine(msg);

            if (listView1.Columns.Count == 4)
            {
                listView1.Columns.RemoveAt(3);
            }
            else
            {
                listView1.Columns.Add("추가 컬럼");
                listView1.Columns.Add("추가 컬럼2", 150, HorizontalAlignment.Center);
                listView1.Columns.Insert(2, "insert컬럼");  // index 2 번 자리에 column 추가.
            }
        }


        // 기본정보 및 내용 출력하기
        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine("총 row 수 = {0}", listView1.Items.Count);
            Console.WriteLine("총 column 수  = {0}", listView1.Columns.Count);

            //각 행의 subitem 갯수는 컬럼수와 동일
            Console.WriteLine(" 2번째 행의 subitem 갯수 = {0}", listView1.Items[1].SubItems.Count);

            Console.WriteLine(" 2번째 행, 3번째 subitem 내용 = {0}", listView1.Items[1].SubItems[2].Text);

            Console.WriteLine(" --------------------------------");


            ListView.ColumnHeaderCollection colheaderColl = listView1.Columns;

            foreach (ColumnHeader colHeader in colheaderColl)
            {
                Console.WriteLine("ColumnHeader Name = {0} ; Text = {1}", colHeader.Name, colHeader.Text);
            }

            Console.WriteLine("3번째 칼럼 제목 = {0}", colheaderColl[2].Text);
        }

        // 선택한 행 정보얻기 및 삭제 (selected row ) ; 방법 1
        private void button5_Click(object sender, EventArgs e)
        {
            var idxColl = listView1.SelectedIndices; // 선택된 줄들의 모음 반환
            Console.WriteLine("선택된 rows count == {0}", idxColl.Count);

            // index 기준 삭제시에는 역순으로 삭제해야함.
            for (int i = idxColl.Count - 1; i >= 0; i--)
            {
                int idx = idxColl[i];

                listView1.Items[idx].Remove();  // listView1.Items.RemoveAt(idx); 
                Console.WriteLine("{0} 번째 row 삭제됨", idx);
            }

        }

        // 선택한 행 정보얻기 및 삭제 (selected row ) ; 방법 2
        private void button6_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection itemColl = listView1.SelectedItems;

            foreach (ListViewItem item in itemColl)
            {
                Console.WriteLine("선택한 row 의 첫 컬럼 내용 == {0}", item.Text); 

                // index 는 가변적임 (row 삭제시마다 바뀜) --> 그냥 index 기준으로 삭제하면 안됨.
                Console.WriteLine("선택한 row index = {0}", item.Index); 

                // item 의 모든 subitem 출력
                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    Console.WriteLine("선택된 row 의 {0}번째 subitem 내용 == {1}", i, item.SubItems[i].Text);
                }

                listView1.Items.Remove(item);  
            }
        }

        // checked 행 삭제 방법 1
        private void button7_Click(object sender, EventArgs e)
        {
            Console.WriteLine(" cheched row 총 수 = {0}", listView1.CheckedIndices.Count);

            // index 역순으로 삭제해야함...
            for (int i = listView1.CheckedIndices.Count - 1 ; i >= 0; i--)
            {
                int idx = listView1.CheckedIndices[i];
                Console.WriteLine("삭제되는 checked row index = {0}", idx );
                listView1.Items.RemoveAt(idx); // listView1.Items[idx].Remove();
            }

        }

        // checked 행 삭제 방법 2
        private void button8_Click(object sender, EventArgs e)
        {
            Console.WriteLine(" cheched row 총 수 = {0}", listView1.CheckedItems.Count);

            foreach (ListViewItem item in listView1.CheckedItems)
            {
                // row 삭제되면서 index 계속 바뀜. -- item 기준으로 삭제해야 정확히 삭제됨.
                Console.WriteLine("삭제되는 checked row index = {0}", item.Index); 
                listView1.Items.Remove(item);
            }
        }

        // 배경색 변경
        private void button9_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Index % 2 == 0)
                {
                    item.BackColor = Color.Yellow; // new Color();
                }
                else
                {
                    item.ForeColor = Color.Red;

                    // UseItemStyleForSubItems = true ; default
                    item.UseItemStyleForSubItems = false;  // subitem 각각에 Font, Color 지정가능.
                    item.SubItems[0].ForeColor = Color.Blue;
                    item.SubItems[1].BackColor = Color.Cyan;
                }
            }
        }

        // ContextMenuStrip
        private void button10_Click(object sender, EventArgs e)
        {
            //this.components = new System.ComponentModel.Container();
            //this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);

        }

        // ContextMenuStrip 에서 foreground color 선택한 경우...
        private void foregroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Console.WriteLine(MousePosition.X);

            // contextmenustrip 오픈위해 Right click 하는 순간, 해당 row 가 selected 됨.
            // 여러개 selected 한후, selected 영역내에서 Right click 시에도 적용가능..
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                item.UseItemStyleForSubItems = false;
                item.SubItems[1].ForeColor = Color.Red;
                item.SubItems[1].Font = new Font("Fixsys", 10, FontStyle.Bold);
            }
        }

        // listview1 의 데이터 입력 영역의 Mouse click event 만 처리한다.*************
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("x= {0}, y= {1}", e.X, e.Y);
            Console.WriteLine("PointToScreen(e.Location) = {0}", PointToScreen(e.Location));

            // mouse 위치 2 차이나는 이유 모르겠음.?????
            Console.WriteLine("MousePosition.X = {0}, MousePosition.Y = {1}", MousePosition.X, MousePosition.Y);
            Console.WriteLine("PointToClient(MousePosition) = {0}", PointToClient(MousePosition));
            Console.WriteLine("---------------------------");

            if (e.Button == MouseButtons.Right)
            {
                Console.WriteLine(e.Location.ToString());
                Console.WriteLine("x= {0}, y= {1}", e.X, e.Y);

                Console.WriteLine("PointToScreen(e.Location) = {0}", PointToScreen(e.Location));

                ListViewItem clickedItem = listView1.GetItemAt(e.X, e.Y);
                Console.WriteLine(clickedItem.Index);
            }
        }

        // MousePosition --> screen 좌표
        private void listView1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("MousePosition.X = {0}, MousePosition.Y = {1}", MousePosition.X, MousePosition.Y);
            Console.WriteLine("PointToClient(MousePosition) = {0}", PointToClient(MousePosition));
            

            Console.WriteLine("Form location == {0}", this.Location);
            Console.WriteLine("listview1 location = {0}", listView1.Location);
            Console.WriteLine("---------------------------");
        }

        // contextmenustrip 클릭한 위치의 item, subitem 구하기
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Point ptInListView = PointToClient(MousePosition);

            // ListViewItem item = listView1.GetItemAt(ptInListView.X, ptInListView.Y);

            ListViewHitTestInfo hitInfo = listView1.HitTest(ptInListView.X, ptInListView.Y);

            // HitTest 부분이 listview 의 item 영역이 아니면, ListViewHitTestInfo.Item = null 이다.
            if (hitInfo.Item != null)
            {
                Console.WriteLine("hittest subitem index == {0}", hitInfo.Item.SubItems.IndexOf(hitInfo.SubItem));
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine(" -------------db click =============");
        }

    }
}

