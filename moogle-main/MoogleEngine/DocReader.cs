using System.Text.RegularExpressions;

namespace MoogleEngine;
public static class DocReader // Carga de Datos y Preprocesamiento.
{
    public static string path = Path.Join("..", "Content"); // Ruta del documento
    public static string[] docs = Directory.GetFiles(path, "*.txt"); // Array que guarda la ruta de cada documento 

    private static string[] BagOfWords(string doc) // Lee el texto, lo normaliza y lo convierte en un arrat de string
    {
        string text = File.ReadAllText(doc); // Lectura del texto
        string pattern = @"[^a-zA-Z0-9áéíóúÁÉÍÓÚñÑ]"; // Caracteres a eliminar de los textos(todo lo que no sea alfanumerico)
        text = Regex.Replace(text, pattern, " "); // Eliminacion de los caracteres no deseados en el texto
        string[] words = text.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries); // Conversion del texto en un array de string eliminando los espacios en blanco
        return words;
    }
    public static void Fill(Dictionary<string, Dictionary<string, float>> Texts, string[] docs) 
    {
        foreach (var doc in docs) // Iterar por los elementos del array que tiene las rutas de los documentos 
        {
            Texts.Add(doc, new Dictionary<string, float>()); // de la ruta y un diccionario al diccionario Texts
            foreach (var word in BagOfWords(doc)) // Iterar sobre las palabras de cada texto
            {
                if (!(Texts[doc].ContainsKey(word))) // Si no esta se agrega con el numero 1
                {
                    Texts[doc].Add(word, 1);
                }
                else // Si esta se le adiciona 1 
                {
                    Texts[doc][word]++;
                }
            }
        }
    }
    public static void FillIDF(Dictionary<string, float> IDF, Dictionary<string, Dictionary<string, float>> Texts)
    {
        foreach (var text in Texts) // Iterar sobre los textos 
        {
            foreach (var WordAndFreq in text.Value) // Iterar sobre las palabras
            {
                if (!IDF.ContainsKey(WordAndFreq.Key)) // Si no esta se agrega con el numero 1
                {
                    IDF.Add(WordAndFreq.Key, 1);
                }
                else // Si esta se le adiciona 1(textos en los que aparece)
                {
                    IDF[WordAndFreq.Key]++;
                }
            }
        }
        foreach (var word in IDF) // Iteracion sobre las palabras diferentes que aparecen en los documentos 
        {
            IDF[word.Key] = (float)Math.Log10(docs.Length / IDF[word.Key]); // Calculo del IDF
        }
    }
    public static void CalculateTF(Dictionary<string, Dictionary<string, float>> Texts) 
    {
        foreach (var text in Texts) // Iteracion por cada texto
        {
            float maxfreq = 0; // frecuencia maxima 
            foreach (var WordAndFreq in text.Value) // Iteracion por cada palabra de cada texto
            {
                maxfreq = Math.Max(maxfreq, WordAndFreq.Value); // Hallar la frecuancia maxima en cada texto
            }
            foreach (var WordAndFreq in text.Value) // iteracion por cada palabra de cada texto
            {
                Texts[text.Key][WordAndFreq.Key] = text.Value[WordAndFreq.Key] / maxfreq; // Calculo del TF de cada palabra
            }
        }
    }
    public static void TFIDF(Dictionary<string, Dictionary<string, float>> Texts, Dictionary<string, float> IDF)
    {
        foreach (var text in Texts) // Iteracion por cada texto
        {
            foreach (var wordandfreq in text.Value) // Iteracion por cada palabra de cada texto
            {
                Texts[text.Key][wordandfreq.Key] = Texts[text.Key][wordandfreq.Key] * IDF[wordandfreq.Key]; // Calculo del TF-IDF de cada palabra de los textos
            }
        }
    }
}