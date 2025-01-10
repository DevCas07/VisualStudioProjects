using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

int generate_large_prime()
{
    while (true)
    {
        Random random = new Random();
        int num = random.Next(10000, 1000000);
        if (num % 2 == 0)
        {
            return num;
        }
    }
}
static long ModInverse(long a, long m)
{
    long m0 = m, y = 0, x = 1;
    if (m == 1) { return 0; }
    //Apply Extended Euclidean Algorithm
    while (a > 1)
    {
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
Dictionary<string, (long, long)> generate_keys()
{   
    //1
    long p = generate_large_prime();
    long q = generate_large_prime();
    //2
    long n = p * q;
    //3
    long phi_n = (p - 1) * (q - 1);
    //4 
    long e = 4294967297;
    //5
    //if (phi_n % e == 0) {
    //    Console.WriteLine("e must be coprime");  
    //}
    //5
    long d = ModInverse(e, phi_n);

    Dictionary<string, (long, long)> keys = new Dictionary<string, (long, long)>()
    {
        ["public-key"] = (e, n),
        ["private-key"] = (d, n)
    };
    return keys;

}
void runArgs(string[] argsArray)
{
    switch (argsArray[0])
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
        case "generate-large-prime":
            Console.WriteLine(generate_large_prime());
            break;
        case "generate-keys":
            Console.WriteLine("> ");
            break;
        default:
            break;
    }

}

while (true) //Main Loop
    {
    Console.Write("> ");
    string argStr = Console.ReadLine();
    
    if (argStr != string.Empty || argStr != "")
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



