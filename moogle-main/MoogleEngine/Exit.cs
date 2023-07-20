using System.Text.RegularExpressions;

namespace MoogleEngine;
public static class Exit
{
    public static int Counter(float[] Similarity)
    {
        int counter = 0; // contador para llevar el conteo de los dobles no cero

        foreach (float sim in Similarity) // iterar sobre el array de similitud
        {
            if (sim != 0 && !(Double.IsNaN(sim))) // verificar el elemento del array es diferente de cero
            {
                counter++; // si es diferente de cero, aumentar el contador
            }
        }

        return counter; // devolver la cantidad de dobles no cero encontrados
    }
    public static string Snippet(string doc, Dictionary<string, float> query) //Llamar al texto
    {
        string text = File.ReadAllText(doc);
        string pattern = @"[^a-zA-Z0-9áéíóúÁÉÍÓÚñÑ]";
        text = Regex.Replace(text, pattern, " ");
        string[] words = text.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);


        int length = 0;

        for (int i = 0; i < words.Length; i++)
        {
            if (query.ContainsKey(words[i])) { length = i; break; }

        }

        string b = " ";
        int start = (length > 20) ? length - 20 : 0;
        int end = (length + 20 > words.Length) ? words.Length : length + 20;
        for (int i = start; i < end; i++)
        {
            b += words[i] + " ";
        }
        return b;
    }
    public static string GetName(string path) // Obtencion del nombre del texto a partir de la ruta
    {
        return Path.GetFileNameWithoutExtension(path);
    }

    public static void AddValues(float[] sim, Dictionary<string, float> text, string[] docs)
    {
        text.Clear(); // Limpiar el diccionario para la nueva busqueda
        for (int i = 0; i < sim.Length; i++) // Iteracion sobre los elementos del array 
        {
            text.Add(docs[i], sim[i]);// Se le introduce al diccionario la ruta del documento con su score
        }
    }
    public static Dictionary<string, float> OrderDic(Dictionary<string, float> texts) // Metodo que ordena el diccionario
    {
        return texts.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
    }
}