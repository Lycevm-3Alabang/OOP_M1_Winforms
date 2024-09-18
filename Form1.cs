using WinFormsApp1.Repositories;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        #region Fields
        #endregion

        #region Ctor
        public Form1()
        {
            InitializeComponent();

        }
        #endregion

        #region Event Handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.Visible = true;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;  // Adjusts width to fill available space
            }

            RefreshDataGridView();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form2 form = new();
            form.ShowDialog(this);

            RefreshDataGridView();
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                {

                    var obj = dataGridView1.Rows[e.RowIndex].DataBoundItem;
                    Student? selectedStudent = UnitOfWork.StudentRepository.Get().FirstOrDefault(s => s.ID == (int)obj.GetType().GetProperty("ID").GetValue(obj));
                    if (selectedStudent == null)
                    {
                        MessageBox.Show("Invalid selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Form2 form = new(selectedStudent);
                        form.ShowDialog(this);
                        RefreshDataGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

        #region Private Methods
        private void RefreshDataGridView(IEnumerable<Student>? students = default)
        {
            students ??= UnitOfWork.StudentRepository.Get() ?? [];
            dataGridView1.DataSource = students.Select(s => new StudentModel(s.ID, s.Name, s.Section, s.Course)).ToArray();
        }

        #endregion

        record StudentModel(int ID,string Name, string Section, string Course);

    }
}
