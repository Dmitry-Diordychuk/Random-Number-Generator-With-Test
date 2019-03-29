using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace Random_Number_Generator_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = "random_number_table.txt";
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                for(int i = 0; i < 10; i++)
                {
                    dataGridView1.Rows.Add(GetNextNumber(sr),
                                            GetNextNumber(sr)+ GetNextNumber(sr),
                                            GetNextNumber(sr)+ GetNextNumber(sr) + GetNextNumber(sr));
                }
            }

            SerialPort myPort = new SerialPort("COM3", 115200);
            myPort.Open();

            for (int i = 0, j = 0; i < 10; i++, j = j + 6)
            {
                dataGridView2.Rows.Add(GetNumberFromArduino(myPort),
                                        GetNumberFromArduino(myPort) + GetNumberFromArduino(myPort),
                                        GetNumberFromArduino(myPort) + GetNumberFromArduino(myPort) + GetNumberFromArduino(myPort));
            }
            float o = 0, l = 0;
            for (int i = 0; i < tableBinarySequance.Length; i++)
            {
                if (tableBinarySequance[i] == '0')
                    o++;
                else if (tableBinarySequance[i] == '1')
                    l++;
            }
            label1.Text = (o / l).ToString();
            o = 0;
            l = 0;
            for (int i = 0; i < arduinoBinarySequance.Length; i++)
            {
                if (arduinoBinarySequance[i] == '0')
                    o++;
                else if (arduinoBinarySequance[i] == '1')
                    l++;
            }
            label2.Text = (o / l).ToString();
        }
        public static string arduinoBinarySequance;
        public static string tableBinarySequance;
        private static string GetNumberFromArduino(SerialPort sp)
        {
            string st;
            int result;
            int i = 0;
            while (true)
            {
                if ((st = char.ConvertFromUtf32(sp.ReadChar())) != "?")
                {
                    if (st.Length > i)
                    {
                        result = st[i];
                        i++;
                    }
                    else
                    {
                        i = 0;
                        result = st[0];
                        i++;
                    }
                    while (result > 9)
                        result = result / 10;
                    arduinoBinarySequance += Convert.ToString(result, 2);
                    return result.ToString();
                }
            }
        }
        private static string GetNextNumber(StreamReader sr)
        {
            string ch = " ";
            while (ch == " ")
            {
                ch = char.ConvertFromUtf32(sr.Read());
            }
            int result = Int32.Parse(ch);
            tableBinarySequance += Convert.ToString(result, 2); 
            return ch;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string numbers = null;
            Regex regex = new Regex(@"\d+");
            MatchCollection matches = regex.Matches(textBox1.Text);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                    numbers += match.Value;
            }
            string numbersBin = null;
            foreach(char ch in numbers)
            {
                numbersBin += Convert.ToString(ch, 2);
            }

            float o = 0, l = 0;
            for (int i = 0; i < numbersBin.Length; i++)
            {
                if (numbersBin[i] == '0')
                    o++;
                else if (numbersBin[i] == '1')
                    l++;
            }
            label3.Text = (o / l).ToString();
        }
    }
}
