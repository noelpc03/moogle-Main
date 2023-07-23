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
        // Normalizacion del texto
        string text = File.ReadAllText(doc);
        string pattern = @"[^a-zA-Z0-9áéíóúÁÉÍÓÚñÑ]";
        text = Regex.Replace(text, pattern, " ");
        string[] words = text.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);


        int position = 0; //Posicion de una de las palabras de la query
        
        for (int i = 0; i < words.Length; i++) //Iteracion sobre el texto
        {
            if (query.ContainsKey(words[i])) { position = i; break; } // Si la query contiene la palabra guarda la posicion y termina de iterar

        }

        string b = " "; // String para mostrar 
        int start = (position > 20) ? position - 20 : 0; // Inicio de snippet
        int end = (position + 20 > words.Length) ? words.Length : position + 20; // Final del snippet
        for (int i = start; i < end; i++) // Iteracion sobre las palabras del texto dentro del conjunto de palabras a mostrar
        {
            b += words[i] + " "; // Al string se le va agregando cada palabra con un espacio
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