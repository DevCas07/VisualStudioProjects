using System;
using System.Runtime.CompilerServices;

namespace CommandLineInterpreterLibrary
{
    public class Interpreter
    {
        public bool showInfoMessage { get; set; } = true;
        public string[] infoList { get; set; } = { "> info //lists all available commands" };
        public string infoMessage { get; set; } = "Type 'info' to list all available commands";
        public string[] argumentStrings { get; set; } = { "" };
        public char interpreterCharacter { get; set; } = '>';
        internal int index { get; set; } = 0;
        internal Dictionary<string, bool> requiredParameters { get; set; } = new Dictionary<string, bool>();
        internal bool requireParameters { get; set; } = true;
        //public Dictionary<string, string> parameterValues { get; set; }
        internal int parameterIndex { get; set; } = 0;
        internal int totalParameterAmount { get; set; } = 0;
        internal bool isFirstParameter = true;
        internal bool isFirstArgument = true;
        public bool debugMode { get; set; } = false;



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

        public void checkParameter(string parameterId, out string outParameterValue, string defaultOutParameterValue = "0", char seperator = ':') //Index specifies which argument in the argsArray is considered last argument before parameters
        {
            outParameterValue = defaultOutParameterValue;
            if (index < 0 || index > argumentStrings.Length) //Checks if index is out of range
            {
                //Console.WriteLine($"index: {index}");
                //Console.ReadLine();

                if (isFirstParameter) { //Only display once message
                    isFirstParameter = false;
                    Console.WriteLine("Index is out of range / To few parameters");
                }
                //throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range / To few parameters");
            }
            else
            {
                for (int i = index; i < argumentStrings.Length; i++)
                {
                    string[] tempStr = argumentStrings[i].Split(seperator);

                    if (tempStr.Length == 2 && parameterId.Equals(tempStr[0], StringComparison.OrdinalIgnoreCase)) { //Checks if split was successful and split index 0 is equals to desired parameter id
                         //if (requireParameters) { //Check if required parameters are enabled
                         //   requiredParameters[tempStr[0]] = true;
                         //   parameterIndex = parameterIndex + 1;

                         //   if (parameterIndex >= totalParameterAmount) { //Check if all required parameters have passed
                         //       checkIfRequiredParametersPassed();
                        //}
                        //}

                        //Console.WriteLine($"tempStr: {tempStr}");
                        //Console.WriteLine($"tempStr.length: {tempStr.Length}");
                        //Console.WriteLine($"tempStr[0]: {tempStr[0]}");
                        //Console.WriteLine($"tempStr[1]: {tempStr[1]}");

                        //outParameter = tempStr[1];

                        if (!string.IsNullOrEmpty(tempStr[1])) //Returns the data value of parameter if it is not null or an empty string
                        {
                            outParameterValue = tempStr[1];
                        }
                        //else //Returns data for parameter as defualt data if data of parameter is null or and empty strings
                        //{
                        //    outParameterValue = defaultOutParameterValue;
                        //}
                    }

                    //parameterValues.Add(parameterId, outParameter); //Maybe add output values to a dictionary instead of a single varible
                    //return true;
                }
                //return false;
            }
        }
        
        public bool checkArgument(string argumentId, bool requireArguments) //Index specifies which argument in the argsArray is considered last argument before parameters
        {
            //Console.WriteLine(argumentId);
            //Add system that checks if amount of argument is to few or to many -------------------------------------------
            if (index < 0 || index > argumentStrings.Length) { //Checks if index is out of range
                Console.WriteLine("Index is out of range");
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
            }
            //Console.WriteLine($"argumentid: {argumentId}");
            //Console.WriteLine($"argumentid: {requireArguments}");

            for (int i = index; i < argumentStrings.Length; i++) {
                if (argumentId.Equals(argumentStrings[i], StringComparison.OrdinalIgnoreCase)) {
                    index = i + 1;
                    return true;
                }
                else if (requireArguments == true && (argumentStrings.Length - index) < 2) { //If diffrence is below below it returns true, if not it retuns false, number analysis has been made to determine the formula
                    //Console.WriteLine("To few arguments");
                    //throw new Exception("To few arguments");
                    return false;
                }
            }
            return false;
        }
        internal void resetArguments()
        {
            //requireParameters = false;
            //parameterIndex = 0;
            //requiredParameters.Clear();
            //totalParameterAmount = 0;
            index = 0;
            isFirstParameter = true;
        }
        internal void InitialiseRequiredParameters(string[] requiredParameterIds, int totParameterAmount) //Maybe add a default value funtionality
        {
            requireParameters = true;
            totalParameterAmount = totParameterAmount;

            for (int i = 0; i < requiredParameterIds.Length; i++)
            {
                //Remake system to have it include the option to have multiple variants of the parameter id at the same parameter index
                //string[] strings = requiredParameterIds[i];
                //for (int j = 0; j < requiredParameterIds[]; j++) {
                //    requiredParameters.Add(requiredParameterIds[i], false);
                //}
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
        //public void initialiseExceptions()
        //{
            //if (argumentStrings.length < 1)
            //{
                //throw new Exception();
            //}
        //}
    }
}
