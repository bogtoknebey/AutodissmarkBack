using Autodissmark.ExternalServices.TextToSpeach.Contracts;
using System.Text;

namespace Autodissmark.ExternalServices.TextToSpeach.ApihostAPI;

public class TextToSpeach : ITextToSpeach
{
    static async Task SendData(string url, string jsonData)
    {
        using (HttpClient client = new HttpClient())
        {
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Data sent successfully. responseStr: {responseStr}");
            }
            else
            {
                Console.WriteLine($"Failed to send data. Status code: {response.StatusCode}");
            }
        }
    }

    public async Task<byte[]?> GetAudioByText(string text)
    {
        string url = "https://apihost.ru/tts.php";
        string jsonData = "{\"data\":[{\"lang\":\"ru-RU\",\"speaker\":\"4\",\"emotion\":\"good\",\"text\":\"Романы на экраны и ты ты порочишь её криками а А ты варлам шаламов просто простопросто так не дарован а\\nА налетай покупай потребляй чё чё пидор на сделай пару\\nПару раз упал и больше больше борозд чем от оспы\",\"rate\":\"1.1\",\"pitch\":\"1.1\",\"type\":\"wav\",\"pause\":\"0\"}]}";

        try
        {
            await SendData(url, jsonData);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return new byte[0];
    }

    async Task<byte[]?> ITextToSpeach.GetAudioByText(string text)
    {
        throw new NotImplementedException();
    }
}
