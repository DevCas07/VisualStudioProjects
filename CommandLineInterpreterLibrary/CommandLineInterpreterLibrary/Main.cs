using System;

namespace CommandLineInterpreterLibrary
{
    public class Interpreter
    {
        public bool showInfoMessage { get; set; } = true;
        public string[] infoList { get; set; } = { "> info //lists all available commands" };
        public string infoMessage { get; set; } = "Type 'info' to list all available commands";
        public string[] argumentStrings { get; set; } = { "" };
        public char interpreterCharacter { get; set; } = '>';
        internal int index { get; set; }
        internal Dictionary<string, bool> requiredParameters { get; set; } = new Dictionary<string, bool>();
        internal bool requireParameters { get; set; } = true;
        //public Dictionary<string, string> parameterValues { get; set; }
        internal int parameterIndex { get; set; } = 0;
        internal int totalParameterAmount { get; set; } = 0;



        public void RunInterpreter(bool showInterpreterCharecter = true) //string[] Args
        {
            //IntepreterCode
            resetArguments();

            if (showInfoMessage == true) {
                Console.WriteLine(infoMessage);
                showInfoMessage = false;
            }
            if (showInterpreterCharecter) { 
                Console.Write($"{interpreterCharacter} "); 
            }

            string argStr = "";
            argStr = Console.ReadLine().ToString();

            if (!string.IsNullOrEmpty(argStr)) {
                argumentStrings = argStr.Split(' ');
            }
        }
        
        public string[] argumentStringsExample = new string[] { //Example Argument Array
        "do",
        "exit:true",
        "msg:hello_world",
        "number:1234"
        };

        //public void InitialiseArguments()

        public void checkParameter(string parameterId, out string outParameter, char seperator = ':') //Index specifies which argument in the argsArray is considered last argument before parameters
        {
            //if (!isFirstParameter) { isFirstParameter = true; }
            outParameter = "";
            if (index < 0 || index >= argumentStrings.Length) //Checks if index is out of range
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
            }

            for (int i = index; i < argumentStrings.Length; i++)
            {
                string[] tempStr = argumentStrings[i].Split(seperator);

                if (tempStr.Length == 2 && parameterId.Equals(tempStr[0], StringComparison.OrdinalIgnoreCase)) {
                    if (requireParameters) { //Check if required parameters are enabled
                        requiredParameters[tempStr[0]] = true; 
                        parameterIndex = parameterIndex + 1;

                        if (parameterIndex >= totalParameterAmount) { //Check if all required parameters have passed
                            checkIfRequiredParametersPassed();
                        }
                    }

                    outParameter = tempStr[1];
                    //parameterValues.Add(parameterId, outParameter); //Maybe add output values to a dictionary instead of a single varible
                    //return true;
                }
            }
            //return false;
        }
        public bool checkArgument(string argumentId) //Index specifies which argument in the argsArray is considered last argument before parameters
        {
            //Add system that checks if amount of argument is to few or to many -------------------------------------------
            if (index < 0 || index >= argumentStrings.Length) //Checks if index is out of range
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
            }

            for (int i = index; i < argumentStrings.Length; i++)
            {
                if (argumentId.Equals(argumentStrings[i], StringComparison.OrdinalIgnoreCase))
                {
                    index = i + 1;
                    return true;
                }
            }
            return false;
        }
        internal void resetArguments()
        {
            requireParameters = false;
            parameterIndex = 0;
            requiredParameters.Clear();
            totalParameterAmount = 0;
            index = 0; 
        }
        public void InitialiseRequiredParameters(string[] requiredParameterIds, int totParameterAmount) //Maybe add a default value funtionality
        {
            requireParameters = true;

            for (int i = 0; i < requiredParameterIds.Length; i++)
            {
                requiredParameters.Add(requiredParameterIds[i], false);
            }
        }
        internal void checkIfRequiredParametersPassed()
        {
            foreach (var parameter in requiredParameters)
            {
                if (!parameter.Value)
                {
                    throw new ArgumentException();
                    //break;
                }
            }
        }
    }
}
