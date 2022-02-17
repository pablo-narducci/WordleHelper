using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace WordleHelper.IO
{
    public interface IDefinitionGetter
    {
        string GetDefinition(string word);
    }

    public class DictionaryFactory
    {
        public static IDefinitionGetter Create()
        {
            return new DictionaryApi();
        }
    }

    public class DictionaryApi : IDefinitionGetter
    {
        public string GetDefinition(string word)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://api.dictionaryapi.dev/api/v2/entries/en/")
            };

            HttpResponseMessage response = client.GetAsync(word).Result;
            if (!response.IsSuccessStatusCode)
            {
                return new WordModel().ToString();
            }

            var jsonString = response.Content.ReadAsStringAsync().Result;
            IEnumerable<WordModel> list = JsonSerializer.Deserialize<IEnumerable<WordModel>>(jsonString);
            var wordDef = list.FirstOrDefault();

            return wordDef.ToString();
        }
    }

    public class WordModel
    {
        public string word { get; set; }

        public IEnumerable<MeaningModel> meanings { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    public class MeaningModel
    {
        public string partOfSpeech { get; set; }
        public IEnumerable<DefinitionModel> definitions { get; set; }
    }

    public class DefinitionModel
    {
        public string definition { get; set; }
        public string example { get; set; }
        
        /*
        public string[] synonyms { get; set; }

        public string[] antonyms { get; set; }
        */
    }
}
