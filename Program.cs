using ImGuiNET;
using System;
using System.Text;
using System.Numerics;

partial class Program
{
    public string inputText = ""; 
    public string encryptedOutput = ""; 
    public string encryptionStep = ""; 
    private readonly Encrypt encrypt = new Encrypt();
    private readonly ReEncrypt reEncrypt = new ReEncrypt();

    protected override void Render()
    {

        ImGui.Begin("Шифр: Поточный шифр"); 
        
        if (ImGui.Button("Выход"))
            Close(); 

        ImGui.Separator(); 

        ImGui.Text("Шифр: Поточный шифр");
        ImGui.Text("Введите текст для зашифровки / дешифровки пишется слитно");
        ImGui.InputTextMultiline("Исходный текст", ref inputText, 1000, new Vector2(-1, ImGui.GetTextLineHeightWithSpacing() * 5));
        ImGui.Text("Введите ключевое слово");
        //ImGui.SliderInt("Сдвиг (шаг)", ref encryptionStep, 1, 32 , $"Шаг: %d");
        ImGui.InputTextMultiline("Ключевое слово", ref encryptionStep, 1000, new Vector2(-1, ImGui.GetTextLineHeightWithSpacing() * 5));

        ImGui.Spacing();

        if (ImGui.Button("Зашифровать"))
        {
            encryptedOutput = encrypt.RemakeText(inputText, encryptionStep);
        }
        ImGui.SameLine(); 
        if (ImGui.Button("Дешифровать"))
        {
           encryptedOutput = reEncrypt.RemakeText(inputText, encryptionStep);
        }

        ImGui.Separator(); 

        ImGui.Text("Результат:");
        ImGui.InputTextMultiline("Зашифрованный / Дешифрованный текст", ref encryptedOutput, (uint)encryptedOutput.Length + 100, new Vector2(-1, ImGui.GetTextLineHeightWithSpacing() * 5), ImGuiInputTextFlags.ReadOnly);
        
        ImGui.End();
    }
}
public class Encrypt
{
   public string RemakeText(string inputText, string key)
    {
        byte[] inputTextBytes = Encoding.UTF8.GetBytes(inputText);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] result = new byte[inputTextBytes.Length];

        for (int i = 0; i < inputTextBytes.Length; i++)
        {
            int k = i % keyBytes.Length;
            result[i] = (byte)(inputTextBytes[i] ^ keyBytes[k]);
        }
        string resultString = BitConverter.ToString(result).Replace("-", "");
        return resultString;
    }
}

public class ReEncrypt
{
     public string RemakeText(string inputText, string key)
    {
        byte[] inputTextBytes = FromHex(inputText);
        var keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length == 0) 
        return "Ошибка: пустой ключ / Неверный ключ";

        var result = new byte[inputTextBytes.Length];
        for (int i = 0; i < inputTextBytes.Length; i++)
        {
            int k = i % keyBytes.Length;

            result[i] = (byte)(inputTextBytes[i] ^ keyBytes[i % keyBytes.Length]);
        }
        return Encoding.UTF8.GetString(result);
    }

    private static byte[] FromHex(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return Array.Empty<byte>();
        hex = hex.Trim();
        if (hex.Length % 2 != 0) throw new FormatException();
        var bytes = new byte[hex.Length / 2];
        for (int i = 0; i < bytes.Length; i++)
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        return bytes;
    }
}


        //byte [] bytes = System.Text.Encoding.UTF8.GetBytes(A1);
        //string A = Encoding.UTF8.GetString(bytes);