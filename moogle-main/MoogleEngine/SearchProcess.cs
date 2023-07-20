using System.Text.RegularExpressions;

namespace MoogleEngine;

public static class SearchProcess
{
    public static void FillQuery(string search, Dictionary<string, float> query, Dictionary<string, float> idf)
    {
        query.Clear(); // Limpieza del diccionario para realizar la nueva busqueda
        
        string pattern = @"[^a-zA-Z0-9áéíóúÁÉÍÓÚñÑ]"; // Caracteres a eliminar de la busqueda(los que no sean alfanumericos)
        search = Regex.Replace(search, pattern, " "); // Eliminacion de los caracteres no deseados de la consulta
        string[] words = search.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries); // Conversion de la consulta en un array de string
        foreach (var word in words) // iteracion sobre 
        {
            if (idf.ContainsKey(word))
            {
                if (!(query.ContainsKey(word)))
                {
                    query.Add(word, 1);
                }
                else
                {
                    query[word]++;
                }
            }
        }
    }
    public static void CalculateTFQuery(Dictionary<string, float> query)
    {
        float maxfreq = 0;
        foreach (var WordAndFreq in query)
        {
            maxfreq = Math.Max(maxfreq, WordAndFreq.Value);
        }
        foreach (var WordAndFreq in query)
        {
            query[WordAndFreq.Key] = query[WordAndFreq.Key] / maxfreq;
        }
    }
    public static void CalculateTFIDFQuery(Dictionary<string, float> idf, Dictionary<string, float> query)
    {
        foreach (var wordandfreq in query)
        {
            query[wordandfreq.Key] *= idf[wordandfreq.Key];
        }
    }
    public static float[] Cosine(Dictionary<string, Dictionary<string, float>> texts, Dictionary<string, float> query)
    {
        float[] similarity = new float[texts.Count];
        int count = 0;
        foreach (var text in texts)
        {
            double dotProduct = 0.0;
            double queryMagnitude = 0.0;
            double documentMagnitude = 0.0;

            foreach (var wordandnum in query)
            {
                if (text.Value.ContainsKey(wordandnum.Key))
                {
                    dotProduct += query[wordandnum.Key] * texts[text.Key][wordandnum.Key];
                    queryMagnitude += query[wordandnum.Key] * query[wordandnum.Key];
                    documentMagnitude += texts[text.Key][wordandnum.Key] * texts[text.Key][wordandnum.Key];
                }
            }
            queryMagnitude = Math.Sqrt(queryMagnitude);
            documentMagnitude = Math.Sqrt(documentMagnitude);
            similarity[count] = (float)dotProduct / (float)(queryMagnitude * documentMagnitude);
            count++;
        }
        return similarity;
    }
}