// https://freeprog.tistory.com/category/C%23

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace datetimepicker_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initializeDateTimePicker();
        }

        // DateTimePicker format 초기 셋팅하기
        private void initializeDateTimePicker()
        {
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt1 = dateTimePicker1.Value;
            Console.WriteLine(dt1.ToString());

            Console.WriteLine(dt1.Date.ToString()); // 시간 무시됨. 00:00:00 으로 됨.
            Console.WriteLine(dt1.GetDateTimeFormats().Length);

            Console.WriteLine("Year == {0}", dt1.Year);
            Console.WriteLine("Month == {0}", dt1.Month);
            Console.WriteLine("Day == {0}", dt1.Day);

            String dt = dt1.ToString("yyyy-MM-dd");
            Console.WriteLine("my datetime format1 == {0}", dt);
        }

        // 내일로 이동
        private void button1_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.AddDays(1); 
        }

        // 요일 구하기
        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(dateTimePicker1.Value.DayOfWeek);
            Console.WriteLine((int)dateTimePicker1.Value.DayOfWeek);

            Console.WriteLine(dateTimePicker1.Value.ToString("dddd"));

            Console.WriteLine(dateTimePicker1.Value.ToString("ddd"), new System.Globalization.CultureInfo("ko-KR"));

            // "en-US" 효과없음 ???
            Console.WriteLine(dateTimePicker1.Value.ToString("dddd"), new System.Globalization.CultureInfo("en-US")); 
        }

        // 선택된 날짜 시간 구하기 (24시간)
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime dt1 = dateTimePicker1.Value;
            Console.WriteLine("DayOfYear = {0}", dt1.DayOfYear.ToString());
            Console.WriteLine("DateTime.ToShortDateString() = {0}", dt1.ToShortDateString());

            Console.WriteLine(dt1.ToString("yyyy-MM-dd HH:mm:ss"));  // HH  --> 24시간 형식

            //for (int i = 0; i < dt1.GetDateTimeFormats().Length; i++)
            //{
            //    Console.WriteLine(dt1.GetDateTimeFormats()[i]);
            //    if (i == 10) break;
            //}
        }

        // String to DateTime
        private void button4_Click(object sender, EventArgs e)
        {
            String dt3 = "1988-05-21";
            Console.WriteLine("String == {0}", dt3);

            DateTime newdt = Convert.ToDateTime(dt3);  // 방법 1

            Console.WriteLine("new DateTime == {0}", newdt); // 오전 12:00:00 == 00:00:00
            Console.WriteLine("1초전 new datetime = {0}", newdt.AddSeconds(-1));

            Console.WriteLine("2시간 전 new datetime = {0}", newdt.AddHours(-2)); // 2시간 이전
            Console.WriteLine("7일전 new datetime = {0}", newdt.AddDays(-7));

            Console.WriteLine("DateTimeKind = {0}", newdt.Kind);

            Console.WriteLine("-----------------------------------------------------");

            String dt4 = "1988-06-01 00:00:00";
            Console.WriteLine("String == {0}", dt4);

            DateTime newdt2 = DateTime.Parse(dt4);  // 방법 2

            Console.WriteLine("new datetime == {0}", newdt2); // 오전 12:00:00 == 00:00:00

            Console.WriteLine(newdt2.ToString("yyyy-MM-dd HH:mm:ss"));  // HH  --> 24시간 형식

            Console.WriteLine("1초전 new datetime = {0}", newdt2.AddSeconds(-1));
            Console.WriteLine("1초전 new datetime = {0}", newdt2.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss"));

        }

        // TimeZone & Culture 정보 구하기
        private void button5_Click(object sender, EventArgs e)
        {
            DateTime dt1 = dateTimePicker1.Value;

            Console.WriteLine("TimeZoneInfo.Local.DisplayName = {0}", TimeZoneInfo.Local.DisplayName);
            Console.WriteLine(System.Threading.Thread.CurrentThread.CurrentCulture.Name);

            String dt2 = dt1.ToString("yyyy-MM-dd hh:mm:ss");
            Console.WriteLine("my datetime format2 == {0}", dt2);
            Console.WriteLine(dt1.ToString("yyyy-MM-dd HH:mm:ss"));  // HH  --> 24시간 형식
            Console.WriteLine(dt1.ToString("yyyy-MM-dd hh:mm:ss tt"));  // hh  --> 14시간 형식, tt --> am/pm

            Console.WriteLine(dt1.ToString("yyyy-MM-dd hh:mm:ss tt", new System.Globalization.CultureInfo("en-US")));  
        }

        // 현재 날짜, 시간 구하기,   2시간전, 2달전 구하기
        private void button6_Click(object sender, EventArgs e)
        {
            DateTime nowdt = DateTime.Now;

            Console.WriteLine("Now == {0}", nowdt);

            Console.WriteLine("2시간전 time (초 단위) = {0}", nowdt.AddHours(-2));
            Console.WriteLine("2시간전 time (분 단위) = {0}", nowdt.AddHours(-2).ToString("yyyy-MM-dd HH:mm"));

            Console.WriteLine("2달전 time (24시간) = {0}", nowdt.AddMonths(-2).ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}