using System.Runtime.InteropServices;
using System.Text;

public class IniFile
{
    private readonly string _path;

    public IniFile(string path)
    {
        _path = path;
    }

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section,
        string key, string val, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section,
        string key, string def, StringBuilder retVal,
        int size, string filePath);

    public void Write(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, _path);
    }

    public string Read(string section, string key, string defaultValue = "0")
    {
        StringBuilder temp = new StringBuilder(255);
        GetPrivateProfileString(section, key, defaultValue, temp, 255, _path);
        return temp.ToString();
    }

    public int ReadInt(string section, string key, int defaultValue = 0)
    {
        string val = Read(section, key, defaultValue.ToString());
        return int.TryParse(val, out int result) ? result : defaultValue;
    }
}