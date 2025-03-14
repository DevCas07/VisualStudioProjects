using System.Data.SqlTypes;
using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using CommandLineInterpreterLibrary;

bool showInfoMessage = true;
Dictionary<string, (long, long)> keys = new Dictionary<string, (long, long)>()
{
    {"", (2, 2) }
};
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
    using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("Primes", limitRange * sizeof(int)))
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

            writer.WriteLine(prime); //Write first line with an integer representing the amount of prime numbers in the entire file

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

long retrieveRandomPrime()
{
    if (!File.Exists(filePrimesPath))
    {
        generate_primes_MemoryMappedFile(filePrimesPath);
    }

    using (StreamReader reader = new StreamReader(filePrimesPath))
    {
        Random rand = new Random();
        string tempLine = reader.ReadLine();

        if (tempLine == null)
        {
            return 2;
        }

        int line1 = int.Parse(tempLine);

        int defaultGroupSize = 500;
        int groupSize = defaultGroupSize;
        long groupAmount = (long)Math.Round((double)(line1 / (double)groupSize), 0);
        
        long randomGroupIndex = rand.NextInt64(0, groupAmount);
        int groupindex = 0;

        int[] primesArray = new int[groupSize];
        List<long> primeList = new List<long>();

        while (reader.EndOfStream == false)
        {
            for (int groupindexTemp = 0; groupindexTemp < groupAmount; groupindex++) //Handles the current group to do chunks of 1000 prime numbers per group to ease RAM usage 
            {
                Array.Clear(primesArray);
                //Console.WriteLine($"Index: {groupindex}");
                for (int i = 0; i < groupSize; i++) //Does chunks of n prime numbers to ease RAM usage
                {
                    string line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line)) { line = ""; }

                    primesArray[i] = int.Parse(line);
                }

                if (groupindex == randomGroupIndex) //Get random prime from primes chunk and add it to prime list
                {
                    long randomIndex = rand.NextInt64(0, primesArray.Length);

                    while (string.IsNullOrEmpty(primesArray[randomGroupIndex].ToString()))
                    {
                        randomIndex = rand.NextInt64(0, primesArray.Length);
                    }

                    long randomPrime = primesArray[randomIndex];
                    return randomPrime;
                }
            }
            return 2;
        }

        return 2;
    }
}
long retrieveRandomPrime_Old(int minPrime = 168, int limitPrime = 1230)
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
Dictionary<string, (long, long)> generate_keys(long e = 65537) // int minPrime = 168, int maxPrime = 1230 //RSA Generation Algorithm, e is public exponent, d is private exponent
{
    //1
    long p = retrieveRandomPrime();
    long q = retrieveRandomPrime();

    while (p == q)
    {
        q = retrieveRandomPrime();
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
    long d = keys["private-key"].Item1;
    //Convert into byteArray
    byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
    Console.WriteLine($"Step1 ByteArray: {plaintextBytes}\n");
    //Convert byteArray into biginteger

    //Test to convert byteText back into plaintext
    Console.WriteLine($"backward conversion text: {Encoding.ASCII.GetString(plaintextBytes)}\n");

    BigInteger plaintextInt = new BigInteger(plaintextBytes);
    Console.WriteLine($"Step2 plaintextInt: {plaintextInt}\n");
    //Encrypt biginteger into ciphertext
    BigInteger ciphertextInt = BigInteger.ModPow(plaintextInt, e, n);
    Console.WriteLine($"Step3 ciphertextInt: {ciphertextInt}\n");
    //Convert into string
    string ciphertext = ciphertextInt.ToString();
    Console.WriteLine($"Encryption Output, Step4 ciphertext: {ciphertext}\n\n");
    
    //Test decryption -------------
    //Parse cipher text into BigInteger
    BigInteger ciphertextInt2 = BigInteger.Parse(ciphertext);
    Console.WriteLine($"Step1 ciphertextInt: {ciphertextInt2}\n");
    //Decrypt ciphertext
    BigInteger plaintextInt2 = BigInteger.ModPow(ciphertextInt2, d, n);
    Console.WriteLine($"Step2 plaintextInt: {plaintextInt2}\n");
    //Convert into byteArray
    byte[] plaintextBytes2 = plaintextInt.ToByteArray();
    Console.WriteLine($"Step3 plaintextBytes: {plaintextBytes2}\n");
    //Remove leading zeros if neccesarry
    if (plaintextBytes.Length > 0 && plaintextBytes2[plaintextBytes2.Length - 1] == 0)
    {
        Array.Resize(ref plaintextBytes2, plaintextBytes2.Length - 1);
    }
    Console.WriteLine("STep4 removed leading zeros\n");
    //Convert byteArray into string
    string plaintext2 = Encoding.UTF8.GetString(plaintextBytes2);
    Console.WriteLine($"Decryption Output, Step5 plaintext: {plaintext2}\n");

    return ciphertext;
}
string decrypt(string ciphertext, long d, long n)
{
    //Parse cipher text into BigInteger
    BigInteger ciphertextInt = BigInteger.Parse(ciphertext);
    Console.WriteLine($"Step1 ciphertextInt: {ciphertextInt}");
    //Decrypt ciphertext
    BigInteger plaintextInt = BigInteger.ModPow(ciphertextInt, d, n);
    Console.WriteLine($"Step2 plaintextInt: {plaintextInt}");
    //Convert into byteArray
    byte[] plaintextBytes = plaintextInt.ToByteArray();
    Console.WriteLine($"Step3 plaintextBytes: {plaintextBytes}");
    //Remove leading zeros if neccesarry
    if (plaintextBytes.Length > 0 && plaintextBytes[plaintextBytes.Length - 1] == 0)
    {
        Array.Resize(ref plaintextBytes, plaintextBytes.Length - 1);
    }
    Console.WriteLine("STep4 removed leading zeros");
    //Convert byteArray into string
    string plaintext = Encoding.UTF8.GetString(plaintextBytes);
    Console.WriteLine($"Step5 plaintext: {plaintext}");

    return plaintext;

}

Interpreter interpreter = new Interpreter();
interpreter.interpreterCharacter = '>';

while (true) //Main Loop
{
    interpreter.RunInterpreter();
    
    if (interpreter.checkArgument("exit", false))
    {
        Environment.Exit(0);
    }
    else if (interpreter.checkArgument("cls", false))
    {
        Console.Clear();
    }
    else if (interpreter.checkArgument("info", false))
    {
        foreach (var str in infoList)
        {
            Console.WriteLine(str);
        }
    }
    else if (interpreter.checkArgument("encrypt", true))
    {
        if (interpreter.checkArgument("-set", false))
        {
            //Parameters 
            interpreter.checkParameter("plaintext", out plaintext, "Hello World!");
            interpreter.checkParameter("e", out string temp_e, keys["public-key"].Item1.ToString());
            interpreter.checkParameter("n", out string temp_n, keys["public-key"].Item2.ToString());
            //--------------------

            ciphertext = encrypt(plaintext, long.Parse(temp_e), long.Parse(temp_n));
            Console.WriteLine($"Encrypted ciphertext: {ciphertext}");
        }
        else if (interpreter.checkArgument("-get", false))
        {
            ciphertext = encrypt(plaintext, keys["public-key"].Item1, keys["public-key"].Item2);
            Console.WriteLine($"Encrypted ciphertext: {ciphertext}");
        }
        else { Console.WriteLine("Unkown command or missing arguments"); }
    }
    else if (interpreter.checkArgument("decrypt", true))
    {
        if (interpreter.checkArgument("-set", true))
        {
            //Parameters 
            interpreter.checkParameter("ciphertext", out string temp_ciphertext);
            interpreter.checkParameter("d", out string temp_d);
            interpreter.checkParameter("n", out string temp_n);
            //--------------------

            plaintext = encrypt(temp_ciphertext, int.Parse(temp_d), int.Parse(temp_n));
            Console.WriteLine($"Encrypted plaintext: {plaintext}");
        }
        else if (interpreter.checkArgument("-get", false))
        {
            plaintext = decrypt(ciphertext, keys["private-key"].Item1, keys["private-key"].Item2);
            Console.WriteLine($"Decrypted plaintext: {plaintext}");
        }
        else { Console.WriteLine("Unkown command or missing arguments"); }
    }
    else if (interpreter.checkArgument("prime", true))
    {
        if (interpreter.checkArgument("-get", false))
        {
            long prime = retrieveRandomPrime();
            Console.WriteLine($"{prime}");
        }
        else if (interpreter.checkArgument("-generate", true))
        {
            if (!File.Exists(filePrimesPath))
            {
                interpreter.checkParameter("limit-range", out string temp_limitrange, "300");

                generate_primes_MemoryMappedFile(filePrimesPath, long.Parse(temp_limitrange));

                Console.WriteLine("Primes file has been generated");
            }
            else
            {
                Console.WriteLine("Cannot override existing primes file");
            }
        }
        else if (interpreter.checkArgument("-reset", false))
        {
            if (File.Exists(filePrimesPath))
            {
                File.Delete(filePrimesPath);
                Console.WriteLine("Primes file has been deleted");
            }
            else
            {
                Console.WriteLine("Primes file not found");
            }
        }
        else { Console.WriteLine("Unkown command or missing arguments"); }

    }
    else if (interpreter.checkArgument("keys", true))
    {
        if (interpreter.checkArgument("-get", false))
        {
            Console.WriteLine($"Public-key: {keys["public-key"]} : ('e', 'n') ");
            Console.WriteLine($"Private-key: {keys["private-key"]} : ('d', 'n')");
        }
        else if (interpreter.checkArgument("-set", true))
        {
            interpreter.checkParameter("public-exponent", out string temp_e, "65537");
            interpreter.checkParameter("private-exponent", out string temp_d);
            interpreter.checkParameter("modulous", out string temp_n);

            keys["public-key"] = (long.Parse(temp_e), long.Parse(temp_n));
            keys["private-key"] = (long.Parse(temp_d), long.Parse(temp_n));

            Console.WriteLine("Key-pair successfully set");

        }
        else if (interpreter.checkArgument("-generate-pair", false))
        {
            interpreter.checkParameter("e", out string temp_e, "65537");

            keys = generate_keys(long.Parse(temp_e));
            Console.WriteLine("Keys have succesfully been generated");
        }
        else { Console.WriteLine("Unkown command or missing arguments"); }
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