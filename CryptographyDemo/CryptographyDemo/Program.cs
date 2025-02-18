﻿using System.Data.SqlTypes;
using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using CommandLineInterpreterLibrary;

bool showInfoMessage = true;
Dictionary<string, (long, long)> keys = new Dictionary<string, (long, long)>();
string filePath = Directory.GetCurrentDirectory().ToString();
string filePrimesPath = Directory.GetCurrentDirectory().ToString() + @"\primes.txt";
string plaintext = "";
string ciphertext = "";

string[] infoList = {
    "----------------------------",
    "< ... > - required, { ... } - optional   //Groupings",
    "----------------------------",
    "> test    //debug",
    "> exit / close   //Exits the application",
    "> clear / cls    //Clears the screen",
    "> info    //Lists all available commands",
    "> generate prime {<min prime range> <max prime range>}   //Output: Single prime number, optional parameters / default parameters (min=100, max=1000), absolute max range of prime is 1 000 000 000 ...",
    "> generate key-pair {<min prime range> <max prime range> <'e' public exponent>}   //Generate public and private keys with RSA algorithm, optional parameters / default parameters (min=100, max=1000, e=65537), Public-key output: ('e' Public exponent), ('n' Modulus), Private-key output: ('e' Private exponent), ('n' Modulus)",
    "> encrypt <'e' Public exponent> <'n' Modulus> <cipher text string>   //Encrypt text using RSA key, Output: Encrypted ciphertext from plaintext",
    "> decrypt <'d' Private exponent> <'n' Modulus> <cipher text string>   //Decrypt cipher text using RSA key, Output: Decrypted ciphertext to plaintext",
    "----------------------------"
};

string[] argumentStrings = new string[] { //Example Argument Array
        "do",
        "exit:true",
        "msg:hello_world",
        "number:1234"
    };

bool checkParameter(string parameterId, int index, out string outParameter, char seperator = ':') //Index specifies which argument in the argsArray is considered last argument before parameters
{
    outParameter = "";
    if (index < 0 || index >= argumentStrings.Length) //Checks if index is out of range
    {
        throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
    }

    for (int i = index; i < argumentStrings.Length; i++)
    {
        string[] tempStr = argumentStrings[i].Split(seperator);
        
        if (tempStr.Length == 2 && parameterId.Equals(tempStr[0], StringComparison.OrdinalIgnoreCase))
        {
            outParameter = tempStr[1];
            return true;
        }
    }
    return false;
}
bool checkArgument(string argumentId, int index) //Index specifies which argument in the argsArray is considered last argument before parameters, Maybe add int amountRequiredParameters
{
    if (index < 0 || index >= argumentStrings.Length) //Checks if index is out of range
    {
        throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
    }
        
    for (int i = index; i < argumentStrings.Length; i++)
    {
        if (argumentId.Equals(argumentStrings[i], StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
    }
    return false;
}
long generate_prime_boolArray(long minPrimeRange = 100, long maxPrimeRange = 1000) //old
{
    //string filePath = Directory.GetCurrentDirectory().ToString();
    //StreamReader primeReader = new StreamReader(filePath + @"\primes.txt");
    //StreamWriter primeWriter = new StreamWriter(filePath + @"\primes.txt");
    //primeReader.ReadLineAsync().Wait();

    //if (File.Exists(filePath + "\\primes.txt"))
    //{

    //}

    var watch = System.Diagnostics.Stopwatch.StartNew();

    bool[] isPrime = new bool[maxPrimeRange + 1];

    for (long i = 0; i <= maxPrimeRange; i++)
    {
        isPrime[i] = true;
    }

    for (long p = 2; p <= maxPrimeRange; p = p + 2)
    {
        if (isPrime[p])
        {
            for (long multiple = p * p; multiple <= maxPrimeRange; multiple += p)
            {
                isPrime[multiple] = false;
            }
        }
    }

    List<long> primes = new List<long>();
    for (long i = minPrimeRange; i <= maxPrimeRange; i++)
    {
        if (isPrime[i])
        {
            primes.Add(i);
        }
    }

    var completion_time = watch.ElapsedMilliseconds;
    Console.WriteLine($"Completed in {completion_time} ms");

    //Clears array and list for garbage collector
    isPrime = Array.Empty<bool>();
    primes.Clear();

    //Randomly chooses a prime number present in the list
    Random random = new Random();
    long numBase = random.Next(0, primes.Count - 1);

    long prime = primes[(int)numBase];

    return prime;
}
void generate_primes_MemoryMappedFile(string filePath, long limitRange = 300) //limitRange - max generation range
{
    using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("Primes", limitRange + 1))
    {
        using (var accessor = mmf.CreateViewAccessor())
        {
            //bool[] isPrime = new bool[limitRange + 1];

            for (long i = 0; i <= limitRange; i++)
            { //Mark all positions as true
                accessor.Write(i, true);
                //isPrime[i] = true;
            }
            accessor.Write(0, false);
            accessor.Write(1, false);

            for (long p = 2; p <= limitRange;)
            { //Sieves through which numbers are multiplies and therefore non primes
                if (accessor.ReadBoolean(p) == true)
                {
                    for (long multiple = p * p; multiple <= limitRange; multiple += p)
                    {
                        accessor.Write(multiple, false);
                        //isPrime[multiple] = false;
                    }
                }
                // next p
                p = p + 1;

                while (accessor.ReadBoolean(p) == false) //check if marked as non-prime, skips numbers for p already marked as non-prime
                {
                    p = p + 1;
                    if (p >= limitRange) { break; }
                }
            }

            //Creates new file and fills it with primes from the MemoryMapped file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                long prime = 0;
                for (long a = 0; a < limitRange; a++)
                {
                    if (accessor.ReadBoolean(a) == true)
                    {
                        prime = prime + 1;
                    }
                }

                writer.WriteLine(prime);

                for (long i = 1; i < limitRange + 1; i++)
                {
                    if (accessor.ReadBoolean(i) == true)
                    {
                        writer.WriteLine(i.ToString());
                    }
                }

                writer.WriteLine();
            }
        }
    }
}

long retrieveRandomPrime(int minPrime = 168, int limitPrime = 1230)
{
    Random random = new Random();
    long primeNumInOrder = random.Next(minPrime, limitPrime);

    if (!File.Exists(filePrimesPath))
    {
        generate_primes_MemoryMappedFile(filePrimesPath, limitPrime);
    }
    using (StreamReader reader = new StreamReader(filePrimesPath))
    {
        long a = 0;
        string tempLine = reader.ReadLine();
        if (tempLine == null) { return 2; }

        long limit = long.Parse(tempLine);
        if (limit > limitPrime)
        {
            //string tempLine = reader.ReadLine();
            for (long i = 1; i < primeNumInOrder; i++)
            {
                a = i;
                string line = reader.ReadLine();
                if (line == null) { return 2; }

                if (i == primeNumInOrder)
                {
                    return long.Parse(line);
                }

            }
        }

        //If this point is reached then primeNumInOrder is larger than primes.txt available primes
        Console.WriteLine("Specified prime has not been generated, try and increase generation limit, largest generated prime has been returned");
        return a;

        //throw new ArgumentOutOfRangeException(nameof(primeNumInOrder));
    }
}
static long ModInverse(long a, long m) //Reverse Modulo Function 
{
    long m0 = m, y = 0, x = 1;
    if (m == 1) { return 0; }
    //Apply Extended Euclidean Algorithm
    while (a > 1) {
        long q = a / m;
        long t = m;

        //m is remainder
        m = a % m;
        a = t;
        t = y;

        //Update x and y
        y = x - q * y;
        x = t;
    }

    //Make x positive
    if (x < 0) { x += m0; }
    return x;

}
Dictionary<string, (long, long)> generate_keys(int minPrime = 168, int maxPrime = 1230, long e = 65537) // int minPrime = 168, int maxPrime = 1230 //RSA Generation Algorithm, e is public exponent, d is private exponent
{
    //1
    long p = retrieveRandomPrime(minPrime, maxPrime);
    long q = retrieveRandomPrime(minPrime, maxPrime);

    while (p == q)
    {
        q = retrieveRandomPrime(minPrime, maxPrime);
    }
    //2
    long n = p * q;
    //3
    long phi_n = (p - 1) * (q - 1);
    //4 
    //long e = 65537;
    //5
    if (phi_n % e == 0)
    {
        Console.WriteLine("e must be coprime");
    }
    //6
    long d = ModInverse(e, phi_n);

    Dictionary<string, (long, long)> keys = new Dictionary<string, (long, long)>() {
        ["public-key"] = (e, n),
        ["private-key"] = (d, n)
    };
    return keys;

}

string encrypt(string plaintext, long e, long n)
{
    //Convert into byteArray
    byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
    //Convert byteArray into biginteger
    BigInteger plaintextInt = new BigInteger(plaintextBytes);
    //Encrypt biginteger into ciphertext
    BigInteger ciphertextInt = BigInteger.ModPow(plaintextInt, e, n);
    //Convert into string
    string ciphertext = ciphertextInt.ToString();

    return ciphertext;
}
string decrypt(string ciphertext, long d, long n)
{
    //Parse cipher text into BigInteger
    BigInteger ciphertextInt = BigInteger.Parse(ciphertext);
    //Decrypt ciphertext
    BigInteger plaintextInt = BigInteger.ModPow(ciphertextInt, d, n);
    //Convert into byteArray
    byte[] plaintextBytes = plaintextInt.ToByteArray();
    //Remove leading zeroz in neccesarry
    if (plaintextBytes.Length > 0 && plaintextBytes[plaintextBytes.Length - 1] == 0)
    {
        Array.Resize(ref plaintextBytes, plaintextBytes.Length - 1);
    }
    //Convert byteArray into string
    string plaintext = Encoding.UTF8.GetString(plaintextBytes);

    return plaintext;

}
void runArgs(string[] argsArray)
{
    Random rand = new Random();
    switch (argsArray[0]) //Initial argument
    {
        case "test":
            Console.WriteLine("test");
            break;
        case "exit" or "close":
            Environment.Exit(0);
            break;
        case "clear" or "cls":
            Console.Clear();
            break;
        case "info":
            foreach (var str in infoList)
            {
                Console.WriteLine(str);
            }
            break;
        case "generate":
            if (argsArray.Length == 1) { Console.WriteLine("Subarguments needed"); break; }
            else
            {
                switch (argsArray[1])
                { //First subargument
                    case "prime":
                        if (argsArray.Length < (2 + 2))
                        { // '2' main arguments + '2' sub-subargument
                            Console.WriteLine(retrieveRandomPrime());
                        }
                        else
                        {
                            //--------------------------------------
                            string[] strings = {
                                    argsArray[2], // minPrime
                                    argsArray[3], // maxPrime
                                };

                            long minPrime = 0;
                            long maxPrime = 0;

                            foreach (var str in strings)
                            {
                                string[] tempStr = str.Split(':');

                                switch (tempStr[0])
                                {
                                    case "min" or "minPrime":
                                        minPrime = long.Parse(tempStr[1]);
                                        break;
                                    case "max" or "maxPrime":
                                        maxPrime = long.Parse(tempStr[1]);
                                        break;
                                }
                            }
                            //--------------------------------------

                            Console.WriteLine(retrieveRandomPrime((int)minPrime, (int)maxPrime));
                        }
                        break;
                    case "key-pair":
                        if (argsArray.Length < (2 + 3))
                        { // '2' main arguments + '3' sub-subargument
                            keys = generate_keys();
                        }
                        else
                        {

                            //--------------------------------------
                            string[] strings = {
                                argsArray[2], // minPrime
                                argsArray[3], // maxPrime
                                argsArray[4] // e public exponent
                            };

                            long minPrime = 0;
                            long maxPrime = 0;
                            long e = 0;

                            foreach (var str in strings)
                            {
                                string[] tempStr = str.Split(':');

                                switch (tempStr[0])
                                {
                                    case "minPrime":
                                        minPrime = long.Parse(tempStr[1]);
                                        break;
                                    case "maxPrime":
                                        maxPrime = long.Parse(tempStr[1]);
                                        break;
                                    case "e-exp" or "e":
                                        e = long.Parse(tempStr[1]);
                                        break;
                                }
                            }

                            //--------------------------------------

                            keys = generate_keys((int)minPrime, (int)maxPrime, (int)e);
                        }
                        Console.WriteLine($"Public-key: {keys["public-key"]} : ('e', 'n') ");
                        Console.WriteLine($"Private-key: {keys["private-key"]} : ('d', 'n')");
                        break;
                    case "new-prime-file" or "primes":
                        if (argsArray.Length < (2 + 1))
                        { // '2' main arguments + '1' sub-subargument
                            generate_primes_MemoryMappedFile(filePrimesPath);
                            Console.WriteLine("Generated new prime file");
                        }
                        else
                        {
                            long maxPrimeRange = 0;
                            string[] tempStr = argsArray[2].Split(':');
                            switch (tempStr[0])
                            {
                                case "limitRange" or "limitPrimeRange" or "range":
                                    if (File.Exists(filePrimesPath))
                                    {
                                        File.Delete(filePrimesPath);
                                    }
                                    maxPrimeRange = long.Parse(tempStr[1]);
                                    generate_primes_MemoryMappedFile(filePrimesPath, maxPrimeRange);

                                    Console.WriteLine($"Generated new prime file, limitRange:{tempStr[1]}");
                                    break;
                            }
                        }
                        break;
                }
            }
            break;

        case "encrypt": //Maybe remake so the order of parameters isn't hardcoded and can be dynamic ------------------------------------------------
            if (argsArray.Length == 1) { Console.WriteLine("Subarguments needed"); break; }
            else
            {
                switch (argsArray[1])
                { //First subargument
                    case "get-vars" or "get":
                        if (argsArray.Length > 2) // '2' main arguments
                        { Console.WriteLine("Invalid amount of arguments"); break; }
                        else
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();

                            Console.WriteLine($"Public-key: {keys["public-key"]} : ('e', 'n') ");
                            ciphertext = encrypt(plaintext, keys["public-key"].Item1, keys["public-key"].Item2);
                            Console.WriteLine($" Encrypted ciphertext: {ciphertext}");

                            var completion_time = watch.ElapsedMilliseconds;
                            Console.WriteLine($"Completed in {completion_time} ms");
                        }
                        break;
                    case "set-parameters" or "set":
                        if (argsArray.Length < (2 + 3)) // '2' main arguments + '3' sub-subparameters
                        { Console.WriteLine("Missing parameters"); break; }
                        else if (argsArray.Length > (2 + 3)) //To many arguments
                        { Console.WriteLine("To many subarguments"); break; }
                        else
                        {

                            //--------------------------------------
                            string[] strings = {
                                argsArray[2], // e public exponent
                                argsArray[3], //n modulus
                                argsArray[4] // plaintext
                            };

                            long e = 0;
                            long n = 0;
                            string plaintext = "";

                            foreach (var str in strings)
                            {
                                string[] tempStr = str.Split(':');

                                switch (tempStr[0])
                                {
                                    case "e-exp" or "e":
                                        e = long.Parse(tempStr[1]);
                                        break;
                                    case "n-modulo" or "n":
                                        n = long.Parse(tempStr[1]);
                                        break;
                                    case "msg" or "message" or "text":
                                        plaintext = tempStr[1];
                                        break;
                                }
                            }

                            //--------------------------------------
                            var watch2 = System.Diagnostics.Stopwatch.StartNew();

                            ciphertext = encrypt(plaintext, e, n);
                            Console.WriteLine($" Encrypted ciphertext: {ciphertext}");

                            var completion_time2 = watch2.ElapsedMilliseconds;
                            Console.WriteLine($"Completed in {completion_time2} ms");
                        }
                        break;
                }
            }
            break;
        case "decrypt":
            if (argsArray.Length == 1) { Console.WriteLine("Subarguments needed"); break; }
            else
            {
                switch (argsArray[1])
                { //First subargument
                    case "prime":
                        if (argsArray.Length < (2 + 2))
                        { // '2' main arguments + '2' sub-subargument
                            Console.WriteLine(retrieveRandomPrime());
                        }
                        else { Console.WriteLine(retrieveRandomPrime((int)long.Parse(argsArray[2]), (int)long.Parse(argsArray[3]))); }
                        break;
                    case "key-pair":
                        if (argsArray.Length < (2 + 3))
                        { // '2' main arguments + '3' sub-subargument
                            keys = generate_keys();
                        }
                        else { keys = generate_keys((int)long.Parse(argsArray[2]), (int)long.Parse(argsArray[3]), long.Parse(argsArray[4])); }
                        Console.WriteLine($"Public-key: {keys["public-key"]} : ('e', 'n') ");
                        Console.WriteLine($"Private-key: {keys["private-key"]} : ('d', 'n')");
                        break;
                }
            }
            break;
        case "vars":
            if (argsArray.Length == 1) { Console.WriteLine("Subarguments needed"); break; }
            else
            {
                switch (argsArray[1])
                { //First subargument
                    case "get":
                        if (argsArray.Length < (2 + 2))
                        { // '2' main arguments + '2' sub-subargument
                            Console.WriteLine(retrieveRandomPrime());
                        }
                        else { Console.WriteLine(retrieveRandomPrime((int)long.Parse(argsArray[2]), (int)long.Parse(argsArray[3]))); }
                        break;
                    case "set":
                        if (argsArray.Length < (2 + 3))
                        { // '2' main arguments + '3' sub-subargument
                            keys = generate_keys();
                        }
                        else { keys = generate_keys((int)long.Parse(argsArray[2]), (int)long.Parse(argsArray[3]), long.Parse(argsArray[4])); }
                        Console.WriteLine($"Public-key: {keys["public-key"]} : ('e', 'n') ");
                        Console.WriteLine($"Private-key: {keys["private-key"]} : ('d', 'n')");
                        break;
                }
            }
            break;
        default:
            break;
    }

}

Interpreter interpreter = new Interpreter();
interpreter.interpreterCharacter = '>';

while (true) //Main Loop
{
    interpreter.RunInterpreter();

    if (interpreter.checkArgument("write", true))
    {
        if (interpreter.checkArgument("default"))
        {
            Console.WriteLine("Hello world!");
        }
        else if (interpreter.checkArgument("custom"))
        {
            //interpreter.InitialiseRequiredParameters(["a", "b"], 2); //Initialises any required parameters

            interpreter.checkParameter("a", out string temp1);
            interpreter.checkParameter("b", out string temp2);

            Console.WriteLine($"A={temp1}, B={temp2}");
        }
        else { Console.WriteLine("Unkown command"); }
    }
    else if (interpreter.checkArgument("test"))
    {
        Console.WriteLine("test succeded");
    }
    else if (interpreter.checkArgument("exit"))
    {
        Environment.Exit(0);
    }
    else if (interpreter.checkArgument("cls"))
    {
        Console.Clear();
    }
    else { Console.WriteLine("Unkown command"); }

    //if (showInfoMessage == true)
    //{
    //    Console.WriteLine("Type 'info' to list all available commands");
    //  showInfoMessage = false;
    //}

    //Console.Write("> ");
    //string argStr = "";
    //argStr = Console.ReadLine().ToString();
    //
    //if (!string.IsNullOrEmpty(argStr))
    //{
    //string[] argsArray = argStr.Split(' ');

    //runArgs(argsArray);

    //Console.WriteLine(argsArray.Length);
    //foreach (var arg in argsArray)
    //{
    //    Console.WriteLine(arg);
    //}
    //}
    //else { continue; }

}