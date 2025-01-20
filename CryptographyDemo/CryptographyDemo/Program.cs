using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

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

    for (long i = 0; i <= maxPrimeRange; i++) {
        isPrime[i] = true;
    }

    long p_temp = 2;
    for (long multiple = p_temp * p_temp; multiple <= maxPrimeRange; multiple += 2) {
        isPrime[multiple] = false;
    }

    for (long p = 3; p <= maxPrimeRange; p = p + 2) {
        if (isPrime[p]) {
            for (long multiple = p * p; multiple <= maxPrimeRange; multiple += p) {
                isPrime[multiple] = false;
            }
        }
    }

    List<long> primes = new List<long>();
    for (long i = minPrimeRange; i <= maxPrimeRange; i++) {
        if (isPrime[i]) {
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
void generate_primes_MemoryMappedFile(long limitRange = 50, string filePath)
{
    using (MemoryMappedFile mmf = MemoryMappedFile.CreateNew("Primes", limitRange + 1,))
    {
        using (var accessor = mmf.CreateViewAccessor()) {
            for (long i = 0; i <= limitRange; i++) { //Mark all positions as true
                accessor.Write(i, true);
            }
            accessor.Write(0, false);
            accessor.Write(1, false);
            

            for (long p = 2; p <= limitRange; p = p + 2) { //Sieves through which numbers are multiplies and therefore non primes
                if (accessor.ReadBoolean(p) == true) {
                    for (long multiple = p * p; multiple <= limitRange; multiple += p) {
                        accessor.Write(multiple, false);
                    }
                }
            }

            //Creates new file and fills it with primes from the MemoryMapped file
            using (StreamWriter writer = new StreamWriter(filePath)) {
                for (long i = 1; i < limitRange + 1; i++) {
                    if (accessor.ReadBoolean(i) == true) {
                        writer.WriteLine(i.ToString());
                    }
                }
            }
        }
    }
}

long retrieveRandomPrime(int minRange = 100, int limitRange = 1000)
{
    Random random = new Random();
    long primeNumInOrder = random.Next(minRange, limitRange);

    if (!File.Exists(filePrimesPath))
    {
        generate_primes_MemoryMappedFile(limitRange, filePrimesPath);
    }
    using (StreamReader reader = new StreamReader(filePrimesPath))
    {
        long a = 0;
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

        //If this point is reached then primeNumInOrder is larger than primes.txt available primes
        Console.WriteLine("Specified prime has not been generated, try and increase generation limit, largest generated prime has been returned");
        return a;

        throw new ArgumentOutOfRangeException(nameof(primeNumInOrder));
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
Dictionary<string, (long, long)> generate_keys(int minPrimeRange = 100, int maxPrimeRange = 1000, long e = 65537) //RSA Generation Algorithm, e is public exponent, d is private exponent
{   
    //1
    long p = retrieveRandomPrime(minPrimeRange, maxPrimeRange);
    long q = retrieveRandomPrime(minPrimeRange, maxPrimeRange);

    while (p == q) {
        q = retrieveRandomPrime(minPrimeRange, maxPrimeRange);
    }
    //2
    long n = p * q;
    //3
    long phi_n = (p - 1) * (q - 1);
    //4 
    //long e = 65537;
    //5
    if (phi_n % e == 0) {
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
                foreach (var str in infoList) {
                    Console.WriteLine(str); }
            break;
            case "generate":
                if (argsArray.Length == 1) { Console.WriteLine("Subarguments needed"); break; }
                    else { switch (argsArray[1]) { //First subargument
                        case "prime":
                            if (argsArray.Length < (2 + 2)) { // '2' main arguments + '2' sub-subargument
                                Console.WriteLine(retrieveRandomPrime()); }
                            else { Console.WriteLine(retrieveRandomPrime((int)long.Parse(argsArray[2]), (int)long.Parse(argsArray[3]))); }
                            break;
                        case "key-pair":
                            if (argsArray.Length < (2 + 3)) { // '2' main arguments + '3' sub-subargument
                                keys = generate_keys(); }
                            else { keys = generate_keys((int)long.Parse(argsArray[2]), (int)long.Parse(argsArray[3]), (int)long.Parse(argsArray[4])); }
                            Console.WriteLine($"Public-key: {keys["public-key"]} : ('e', 'n') ");
                            Console.WriteLine($"Private-key: {keys["private-key"]} : ('d', 'n')");
                            break;
                        } 
                    }
                break;
            case "encrypt": //Maybe remake so the order of parameters isn't hardcoded and can be dynamic ------------------------------------------------
                if (argsArray.Length == 1) { Console.WriteLine("Subarguments needed"); break; }
                    else { switch (argsArray[1]) { //First subargument
                        case "get-vars" or "get":
                            if (argsArray.Length > 2) // '2' main arguments
                                { Console.WriteLine("Invalid amount of arguments"); break; }
                            else {
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
                            else {

                            string[] strings = { 
                                argsArray[2], // e public exponent
                                argsArray[3], //n modulus
                                argsArray[4] // plaintext
                            };

                            
                            foreach (var str in strings)
                            {
                                string[] tempStr = str.Split(':');

                                switch (tempStr[0])
                                {
                                    case "e-exp" or "e":

                                        break;
                                    case "n-modulo" or "n":

                                        break;
                                    case "msg" or "message" or "text":

                                        break;
                                }
                            }
                            
                            var watch2 = System.Diagnostics.Stopwatch.StartNew();

                            ciphertext = encrypt(argsArray[4], long.Parse(argsArray[2]), long.Parse(argsArray[3]));
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
                    else { switch (argsArray[1]) { //First subargument
                        case "prime":
                            if (argsArray.Length < (2 + 2)) { // '2' main arguments + '2' sub-subargument
                                Console.WriteLine(retrieveRandomPrime()); }
                            else { Console.WriteLine(retrieveRandomPrime((int)long.Parse(argsArray[2]), (int)long.Parse(argsArray[3]))); }
                            break;
                        case "key-pair":
                            if (argsArray.Length < (2 + 3)) { // '2' main arguments + '3' sub-subargument
                                keys = generate_keys(); }
                            else { keys = generate_keys((int)long.Parse(argsArray[2]), (int)long.Parse(argsArray[3]), long.Parse(argsArray[4])); }
                            Console.WriteLine($"Public-key: {keys["public-key"]} : ('e', 'n') ");
                            Console.WriteLine($"Private-key: {keys["private-key"]} : ('d', 'n')");
                            break;
                        } 
                    }
                break;
            case "vars":
                if (argsArray.Length == 1) { Console.WriteLine("Subarguments needed"); break; }
                    else { switch (argsArray[1]) { //First subargument
                        case "get":
                            if (argsArray.Length < (2 + 2)) { // '2' main arguments + '2' sub-subargument
                                Console.WriteLine(retrieveRandomPrime()); }
                            else { Console.WriteLine(retrieveRandomPrime((int)long.Parse(argsArray[2]), (int)long.Parse(argsArray[3]))); }
                            break;
                        case "set":
                            if (argsArray.Length < (2 + 3)) { // '2' main arguments + '3' sub-subargument
                                keys = generate_keys(); }
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

while (true) //Main Loop
    {
    if (showInfoMessage == true) {
        Console.WriteLine("Type 'info' to list all available commands");
        showInfoMessage = false; 
    }

    Console.Write("> ");
    string argStr = "";
    argStr = Console.ReadLine().ToString();

    if (!string.IsNullOrEmpty(argStr))
    {
        string[] argsArray = argStr.Split(' ');
        runArgs(argsArray);
        
        //Console.WriteLine(argsArray.Length);
        //foreach (var arg in argsArray)
        //{
        //    Console.WriteLine(arg);
        //}
    } else { continue; }
}



