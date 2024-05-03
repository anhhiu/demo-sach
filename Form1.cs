using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Sach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            HienThi();
        }
       public XmlDocument doc = new XmlDocument();
      public  XmlElement root;
       public string filename = @"C:\Users\Admin\source\repos\Sach\Sach\sach.xml";



        public void HienThi()// tham so la mot doi tuong data gridview
        {
            doc.Load(filename);// load tệp xml
            root = doc.DocumentElement;// xác định node gốc

            XmlNodeList ds = root.SelectNodes("sach");// lay danh sách(ds) các node có tên là sach
            int sd = 0;//lưu chỉ số dòng để hiển thị theo từng dòng trong datagridview
            foreach (XmlNode item in ds)// duyệt từng node trong danh sách vừa có
            {
                dgv.Rows.Add();// tạo 1 dòng trắng trên data gridview
                dgv.Rows[sd].Cells[0].Value = item.SelectSingleNode("@masach").Value;
                // lấy giá trị của thuộc tính masach gán vào cột đầu tiên trên dòng thứ sd
                dgv.Rows[sd].Cells[1].Value = item.SelectSingleNode("tensach").InnerText;
                dgv.Rows[sd].Cells[2].Value = item.SelectSingleNode("soluong").InnerText;
                dgv.Rows[sd].Cells[3].Value = item.SelectSingleNode("dongia").InnerText;
                sd++;// tang số dòng lên để hiển thị node tiếp theo
            }
        }

      
 

        private void btnthem_Click(object sender, EventArgs e)
        {
            doc.Load(filename);// load tệp xml
            root = doc.DocumentElement;// xác định node gốc

            //tạo nút sach. Do sach có các phần tử con hoặc thuộc tính nên mình phải dung XmlNode.
            XmlNode sach = doc.CreateElement("sach");

            //tạo nút con của sách là masach

            XmlAttribute masach = doc.CreateAttribute("masach");// tạo 1 attribute nút masach
            masach.Value = textBoxmasach.Text;//gán giá trị trên ô textbox txtMS cho node mã sách
            sach.Attributes.Append(masach);// gán node masach là node con của node sach

            XmlElement tensach = doc.CreateElement("tensach");// tạo 1 element node ten sach
            tensach.InnerText = textBoxtensach.Text;// gán giá trị trên ô textbox txttenS cho node tensach
            sach.AppendChild(tensach);//gán node ténach là node con của node sach

            XmlElement soluong = doc.CreateElement("soluong");
            soluong.InnerText = textBoxsoluong.Text;
            sach.AppendChild(soluong);

            XmlElement dongia = doc.CreateElement("dongia");
            dongia.InnerText = textBoxdongia.Text;
            sach.AppendChild(dongia);

            //sau khi tạo xong node sach, thì thêm sach vào gốc root
            root.AppendChild(sach);
            doc.Save(filename);//lưu dữ liệu
            HienThi();// hiển thị lại dữ liệu

        }


        private void btnsua_Click(object sender, EventArgs e)
        {
            doc.Load(filename);// load tệp xml 
            root = doc.DocumentElement;// xác định node gốc
            //láy vị trí cần sửa theo mã sách cũ đưa vào
            XmlNode sachCu = root.SelectSingleNode("sach[@masach ='" + textBoxmasach.Text + "']");
            if (sachCu != null)
            {
                // taoj 1 nut sachSuaMoi
                XmlNode sachSuaMoi = doc.CreateElement("sach");

                //tạo nút con của sách là masach
                XmlAttribute masach = doc.CreateAttribute("masach");
                masach.InnerText = textBoxmasach.Text;//gán giá trị cho mã sách
                sachSuaMoi.Attributes.Append(masach);

                XmlElement tensach = doc.CreateElement("tensach");
                tensach.InnerText = textBoxtensach.Text;
                sachSuaMoi.AppendChild(tensach);

                XmlElement soluong = doc.CreateElement("soluong");
                soluong.InnerText = textBoxsoluong.Text;
                sachSuaMoi.AppendChild(soluong);

                XmlElement dongia = doc.CreateElement("dongia");
                dongia.InnerText = textBoxdongia.Text;
                sachSuaMoi.AppendChild(dongia);

                //thay thế sách cũ bằng sách mới(sửa )
                root.ReplaceChild(sachSuaMoi, sachCu);
                doc.Save(filename);//lưu lại
                HienThi();


            }
        }

            private void btnxoa_Click(object sender, EventArgs e)
        {
            doc.Load(filename);// load tệp xml
            root = doc.DocumentElement;// xác định node gốc
            XmlNode sachCanXoa = root.SelectSingleNode("sach[@masach ='" + textBoxmasach.Text + "']");
            if (sachCanXoa != null)
            {
                root.RemoveChild(sachCanXoa);
                doc.Save(filename);
            }
            dgv.Rows.Clear();
            HienThi( );

        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            dgv.Rows.Clear();
            XmlNode sachCanTim = root.SelectSingleNode("sach[@masach ='" + textBoxmasach.Text.Trim().ToLower() + "']");
            if (sachCanTim != null)
            {

                // dgv.Rows.Add();//thêm một dòng mới

                //đưa dữ liệu vào dòng vừa tạo
                dgv.Rows[0].Cells[0].Value = sachCanTim.SelectSingleNode("@masach").InnerText;
                dgv.Rows[0].Cells[1].Value = sachCanTim.SelectSingleNode("tensach").InnerText;
                dgv.Rows[0].Cells[2].Value = sachCanTim.SelectSingleNode("soluong").InnerText;
                dgv.Rows[0].Cells[3].Value = sachCanTim.SelectSingleNode("dongia").InnerText;
            }

        }

       

        private void btnhienthi_Click(object sender, EventArgs e)
        {
            HienThi();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int t = dgv.CurrentCell.RowIndex;
            textBoxmasach.Text = dgv.Rows[t].Cells[0].Value.ToString();
            textBoxtensach.Text = dgv.Rows[t].Cells[1].Value.ToString();
            textBoxsoluong.Text = dgv.Rows[t].Cells[2].Value.ToString();
            textBoxdongia.Text = dgv.Rows[t].Cells[3].Value.ToString();

        }

       
    }

   

}
