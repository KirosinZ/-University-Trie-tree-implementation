using System.Windows.Forms;

namespace STRIALG_TRIE
{
    public partial class WordsForm : Form
    {
        public enum InputFormat
        {
            Letter,
            Word,
            Words
        }

        public string Output { get; private set; } = "";
        public InputFormat Targetinput { get; }

        void Exit()
        {
            if (Targetinput == InputFormat.Words || Targetinput == InputFormat.Letter &&  textBox1.Text.IsLetter() || Targetinput == InputFormat.Word && textBox1.Text.IsWord())
            {
                Output = textBox1.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(this, "Введенные данные не соответствуют формату.", "Ошибка!", MessageBoxButtons.OK);
            }
        }

        public WordsForm(string Caption, string Instruction, InputFormat format = InputFormat.Words)
        {
            InitializeComponent();

            Text = Caption;
            label1.Text = Instruction;
            button1.Text = Caption;
            Targetinput = format;
            button1.Click += (o, e) => Exit();
        }
    }
}
