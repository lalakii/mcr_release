string[] clArgs = Environment.GetCommandLineArgs();
if (clArgs == null || clArgs.Length < 2)
{
    Console.WriteLine("mcr: {0}, doc: https://github.com/lalakii/mcr_release", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
    return;
}
if (clArgs.Length != 7) { Console.WriteLine("Parameter error"); return; }
Dictionary<string, string> parameter = clArgs.Skip(1)
            .Select((value, index) => new { value, index })
            .GroupBy(x => x.index / 2)
            .ToDictionary(x => x.First().value, x => x.Last().value);
string biosPath = null;
byte[] mcData = null, mcOldData = null, biosData = null;
foreach (KeyValuePair<string, string> item in parameter)
{
    string value = item.Value;
    if (File.Exists(value))
    {
        byte[] data = File.ReadAllBytesAsync(value, CancellationToken.None).GetAwaiter().GetResult();
        if ("-U".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
        {
            biosPath = value;
            biosData = data;
        }
        else if ("-SRC".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
        {
            mcOldData = data;
        }
        else if ("-DEST".Equals(item.Key, StringComparison.OrdinalIgnoreCase))
        {
            mcData = data;
        }
    }
}
if (biosData == null || mcOldData == null || mcData == null)
{
    Console.WriteLine("Can't find the file.");
    return;
}
int offset = mcData.Length - mcOldData.Length;
if (offset != 0)
{
    int emptySize = Math.Abs(offset);
    if (offset == emptySize)
    { Update(ref mcOldData, emptySize); }
    else
    { Update(ref mcData, emptySize); }
}
int indexOfMcOld = -1;
for (int i = 0; i <= biosData.Length - mcOldData.Length; i++)
{
    int j = 0;
    for (; j < mcOldData.Length; j++)
    {
        if (mcOldData[j] != biosData[i + j]) { break; }
    }
    if (j == mcOldData.Length) { indexOfMcOld = i; }
}
if (indexOfMcOld != -1)
{
    Buffer.BlockCopy(mcData, 0, biosData, indexOfMcOld, mcOldData.Length);
    File.Move(biosPath, biosPath + "-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + ".bak");
    if (!File.Exists(biosPath))
    {
        File.WriteAllBytesAsync(biosPath, biosData, CancellationToken.None).GetAwaiter().GetResult();
        Console.WriteLine("Done.");
        return;
    }
}
Console.WriteLine("Nothing happens.");
static void Update(ref byte[] src, int emptySize)
{
    int offset = src.Length;
    Array.Resize(ref src, offset + emptySize);
    for (int k = offset; k < src.Length; k++)
    {
        src[k] = 0xff;
    }
}