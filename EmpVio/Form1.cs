using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using System.Data.SqlClient;
using System.Collections;
using Transitions;


namespace EmpVio
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sql s = new sql(this);
            s.loademployees();
            s.loadviolationtb();
            s.loadviolators();
        }

        private void refreshBTN_Click(object sender, EventArgs e)
        {
            sql s = new sql(this);
            s.loademployees();
        }

        private void empaddBTN_Click(object sender, EventArgs e)
        {
            sql s = new sql(this);
            s.addemp();
            s.loademployees();
        }

        private void metroTextButton1_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selecteditems = empGRID.SelectedRows;

            ArrayList x = new ArrayList(selecteditems.Count);

            foreach (DataGridViewRow selecteditem in selecteditems)
            {
                x.Add(selecteditem.Cells["id"].Value.ToString());
            }
            sql s = new sql(this);
            foreach (string i in x)
            {
                string id = i;
                string query="";
             

                if (kryptonCheckBox1.Checked == true)
                {
                    sql.sqlcon.Open();
                    string find = "select * from employeetb where employee = '" + empname.Text + "'";
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd = new SqlCommand(find, sql.sqlcon);
                    SqlDataReader read = sqlcmd.ExecuteReader();
                    if (read.HasRows == true)
                    {
                        read.Close();
                        sql.sqlcon.Close();
                        MetroMessageBox.Show(this, "Employee already exist! Data not saved", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        read.Close();
                        sql.sqlcon.Close();
                        query = "update employeetb set employee = '" + empname.Text + "' where id = '" + id + "'";
                        s.updateemp(query);
                    }                   
                }
                else
                {
                }
                if (kryptonCheckBox2.Checked == true)
                {
                    query = "update employeetb set department = '" + department.Text + "' where id = '" + id + "'";
                    s.updateemp(query);
                }
                else
                {

                }

            }
            s.loademployees();
            kryptonCheckBox1.Checked = false;
            kryptonCheckBox2.Checked = false;

        }

        private void empGRID_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            sql s = new sql(this);
            s.autorow(sender,e);
        }

        private void metroTextButton2_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this,"Do you want to delete this record?", "Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.No)
            {
                return;
            }
            DataGridViewSelectedRowCollection selecteditems = empGRID.SelectedRows;
            ArrayList list = new ArrayList(selecteditems.Count);
            foreach (DataGridViewRow x in selecteditems)
            {
               list.Add(x.Cells["id"].Value.ToString());
            }
            sql s = new sql(this);
            foreach (string x in list)
            {
                string query = "delete from employeetb where id = '" + x + "'";
                s.deleteemp(query);
            }
            s.loademployees();
        }

        private void metroTextButton6_Click(object sender, EventArgs e)
        {
            sql s = new sql(this);
            s.loadviolationtb();
        }

        private void metroTextButton5_Click(object sender, EventArgs e)
        {
            sql s = new sql(this);
            if(points.Text == "")
            {
                points.Text = "0";
            }
            s.addviolation();
            s.loadviolationtb();
        }

        private void metroTextButton4_Click(object sender, EventArgs e)
        {
            if (points.Text == "")
            {
                points.Text = "0";
            }
            DataGridViewSelectedRowCollection selecteditems = violationGrid.SelectedRows;
            ArrayList list = new ArrayList(selecteditems.Count);
            foreach(DataGridViewRow selecteditem in selecteditems)
            {
                list.Add(selecteditem.Cells["id"].Value.ToString());
            }
            sql s = new sql(this);
            foreach(string x in list)
            {
                if (kryptonCheckBox3.Checked == true)
                {
                    sql.sqlcon.Open();
                    string find = "select * from violationtb where violation = '" + violation.Text + "'";
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd = new SqlCommand(find, sql.sqlcon);
                    SqlDataReader read = sqlcmd.ExecuteReader();
                    if (read.HasRows == true)
                    {
                        read.Close();
                        sql.sqlcon.Close();
                        MetroMessageBox.Show(this, "Violation already exist! Data not saved", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        read.Close();
                        sql.sqlcon.Close();
                        string str = "update violationtb set violation = '" + violation.Text + "' where id = '" + x + "'";
                        s.updateviolation(str);
                    }
                }
                else
                {

                }
                if (kryptonCheckBox4.Checked == true)
                {
                    string str = "update violationtb set points = '" + points.Text + "' where id = '" + x + "'";
                    s.updateviolation(str);
                }
                else
                {

                }
            }
            kryptonCheckBox3.Checked = false;
            kryptonCheckBox4.Checked = false;   
            s.loadviolationtb();
        }

        private void metroTextButton3_Click(object sender, EventArgs e)
        {
           if(MetroMessageBox.Show(this, "Do you want to delete selected rows?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            DataGridViewSelectedRowCollection selecteditems = violationGrid.SelectedRows;
            ArrayList list = new ArrayList(selecteditems.Count);
            foreach (DataGridViewRow selecteditem in selecteditems)
            {
                list.Add(selecteditem.Cells["id"].Value.ToString());
            }
            sql s = new sql(this);
            foreach (string x in list)
            {
                s.deleteviolation(x);
            }
            s.loadviolationtb();
        }

        private void violationGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            sql s = new sql(this);
            s.autorow(sender, e);
        }

        private void metroTextButton10_Click(object sender, EventArgs e)
        {
            sql s = new sql(this);
            s.loadviolators();
        }

        private void selectviolator_MouseDown(object sender, MouseEventArgs e)
        {
            sql s = new sql(this);
            s.genviolator();
        }

        private void selectviolation_MouseDown(object sender, MouseEventArgs e)
        {
            sql s = new sql(this);
            s.genviolation();
        }

        private void metroTextButton9_Click(object sender, EventArgs e)
        {
            sql s = new sql(this);
            s.addviolators();
            s.loadviolators();
        }

        private void vdategen_ValueChanged(object sender, EventArgs e)
        {
            vdate.Text = vdategen.Text;
        }

        private void vdategen_MouseDown(object sender, MouseEventArgs e)
        {
            vdate.Text = vdategen.Text;
        }

        private void violatorGRID_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            sql s = new sql(this);
            s.autorow(sender, e);
        }

        private void metroTextButton8_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selecteditems = violatorGRID.SelectedRows;
            ArrayList list = new ArrayList(selecteditems.Count);
            foreach(DataGridViewRow selecteditem in selecteditems)
            {
                list.Add(selecteditem.Cells["id"].Value.ToString());
            }
            sql s = new sql(this);
            foreach(string x in list)
            {
                if (kryptonCheckBox5.Checked == true)
                {
                    string str = "declare @empid as varchar(50) = (select id from employeetb where employee = '" + selectviolator.Text + "')"
                        +"update violatorstb set empid = @empid where id = '" + x + "'";
                    s.updateviolator(str);
                }
                else
                {
                }
                if (kryptonCheckBox6.Checked == true)
                {
                    string str = "declare @vioid as varchar(50)= (select id from violationtb where violation = '" + selectviolation.Text + "')"
                        + "update violatorstb set vioid = @vioid where id = '" + x + "'";
                    s.updateviolator(str);
                }
                else
                {
                }
                if (kryptonCheckBox7.Checked == true)
                {
                    string str =  "update violatorstb set vdate = '" + vdate.Text + "' where id = '" + x + "'";
                    s.updateviolator(str);
                }
                else
                {
                }
            }
            kryptonCheckBox5.Checked = false;
            kryptonCheckBox6.Checked = false;
            kryptonCheckBox7.Checked = false;
            s.loadviolators();
        }

        private void metroTextButton7_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this,"Do you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            DataGridViewSelectedRowCollection selecteditems = violatorGRID.SelectedRows;
            ArrayList list = new ArrayList(selecteditems.Count);
            foreach(DataGridViewRow selecteditem in selecteditems)
            {
                list.Add(selecteditem.Cells["id"].Value.ToString());
            }
            sql s = new EmpVio.sql(this);
            foreach (string x in list)
            {
                s.deleteviolators(x);
            }
            s.loadviolators();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            sql s = new sql(this);
            s.usetrans();

            string str = "select b.EMPLOYEE,sum(c.POINTS) as points from VIOLATORSTB as a "+
"inner join employeetb as b " +
"on a.empid = b.ID " +
"inner join violationtb as c " +
"on a.vioid = c.ID " +
"group by b.employee order by sum(c.POINTS) DESC";
            summaryGrid.DataSource = null;
            s.summarygridcontent(str);

            string ch = "select b.EMPLOYEE,sum(c.POINTS) as points from VIOLATORSTB as a " +
"inner join employeetb as b " +
"on a.empid = b.ID " +
"inner join violationtb as c " +
"on a.vioid = c.ID " +
"group by b.employee order by sum(c.POINTS) ASC";
            s.charting(ch, "employee","Top Employees");
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            sql s = new sql(this);
            s.usetrans();

            string str = "select c.violation,sum(c.POINTS) as points from VIOLATORSTB as a " +
"inner join employeetb as b " +
"on a.empid = b.ID " +
"inner join violationtb as c " +
"on a.vioid = c.ID " +
"group by c.violation order by sum(c.POINTS)desc";
            summaryGrid.DataSource = null;
            s.summarygridcontent(str);

            string ch = "select c.violation,sum(c.POINTS) as points from VIOLATORSTB as a " +
"inner join employeetb as b " +
"on a.empid = b.ID " +
"inner join violationtb as c " +
"on a.vioid = c.ID " +
"group by c.violation order by sum(c.POINTS) asc";
            s.charting(ch, "violation","Top Violations");
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            sql s = new sql(this);
            s.usetrans();

            string str = "select b.department,sum(c.POINTS) as points from VIOLATORSTB as a " +
"inner join employeetb as b " +
"on a.empid = b.ID " +
"inner join violationtb as c " +
"on a.vioid = c.ID " +
"group by b.department order by sum(c.POINTS) DESC";
            summaryGrid.DataSource = null;
            s.summarygridcontent(str);

            string ch = "select b.department,sum(c.POINTS) as points from VIOLATORSTB as a " +
"inner join employeetb as b " +
"on a.empid = b.ID " +
"inner join violationtb as c " +
"on a.vioid = c.ID " +
"group by b.department order by sum(c.POINTS) ASC";
            s.charting(ch, "department","Top Departments");
        }

        private void summaryGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            sql s = new sql(this);
            s.autorow(sender,e);
        }

        private void points_Leave(object sender, EventArgs e)
        {
            if (sql.IsNumeric(points.Text)==true)
            {
            }
            else
            {
                MetroMessageBox.Show(this, "Invalid points", "Numeric Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                points.Focus();
            }
        }

        private void violationGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((violationGrid.RowCount >= 0) && (e.RowIndex>=0))
            {
                DataGridViewRow row = violationGrid.Rows[e.RowIndex];
                violation.Text = row.Cells["violation"].Value.ToString();
                points.Text = row.Cells["points"].Value.ToString();
            }
        }

        private void empGRID_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((empGRID.RowCount >= 0) && (e.RowIndex >= 0))
            {
                DataGridViewRow row = empGRID.Rows[e.RowIndex];
                empname.Text = row.Cells["employee"].Value.ToString();
                department.Text = row.Cells["department"].Value.ToString();
            }
        }

        private void violatorGRID_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((violatorGRID.RowCount >= 0) && (e.RowIndex >= 0))
            {
                DataGridViewRow row = violatorGRID.Rows[e.RowIndex];
                selectviolation.Text = row.Cells["violation"].Value.ToString();
                selectviolator.Text = row.Cells["employee"].Value.ToString();
                vdate.Text = row.Cells["vdate"].Value.ToString();
            }
        }

     

        private void kryptonCheckButton1_Click(object sender, EventArgs e)
        {
            if (kryptonCheckButton1.Checked == true)
            {
                kryptonCheckButton1.Text = "Chart";
                chart1.Visible = true;
                summaryGrid.Visible = false;
            }
            else
            {
                kryptonCheckButton1.Text = "Grid";
                chart1.Visible = false;
                summaryGrid.Visible = true;
            }
        }
    }
}
