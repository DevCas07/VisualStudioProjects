using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineDrawerDemo
{
    public class CustomExceptions
    {
        public static string already_occupied_key { get { return "Key is already occupied"; } }
        public static string not_existing_key { get { return "Key does not exist"; } }
        public static string no_line_end_nearby { get { return "Found no line ends near position"; } }
        public static string no_existing_lines { get { return "No lines exist"; } }
        public static string invalid_selected_node { get { return "Selected node is invalid"; } }
        public static string null_list_contents { get { return "List is null"; } }
        public static string unauthorized_directory_or_file_access { get { return "Cannot access directory/file"; } }
    }
}
