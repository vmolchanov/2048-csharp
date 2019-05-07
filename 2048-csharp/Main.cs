using System.Windows.Forms;

class Program
{
    public static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Game2048.MainScreen());
    }
}