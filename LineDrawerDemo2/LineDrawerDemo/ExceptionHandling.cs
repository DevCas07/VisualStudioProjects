using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineDrawerDemo
{
    internal class ExceptionHandling
    {
        Dictionary<string, string> exceptionList = new Dictionary<string, string>
        {
            { "test_id", "test_title, test_message" }, //Test message, old
            { "already_occupied_key", "Key is already occupied" },
            { "not_existing_key", "Key does not exist" },
            { "no_line_end_nearby", "Found no line ends near position" },
            { "no_existing_lines", "No lines exist" },
            { "invalid_selected_node", "Selected node is invalid" },
            { "2", "" },
            { "3", "" },
        }; //Exception list with exception id and exception message
        public void generateException(string id)
        {
            //throw new NotImplementedException();

            //string[] ex = exceptionList[id].Split(',');

            //MessageBox.Show(ex[1], ex[0]);
            MessageBox.Show(exceptionList[id], id);
        }
    }
}
