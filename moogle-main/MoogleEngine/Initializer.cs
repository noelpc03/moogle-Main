using System.Diagnostics;
namespace MoogleEngine;

public class Initializer // Controla los procesos del buscador
{
    public static Dictionary<string, Dictionary<string, float>> Texts = new Dictionary<string, Dictionary<string, float>>(); // A cada texto se le asocia un diccionario sus palabras y su TF-IDF
    public static Dictionary<string, float> IDF = new Dictionary<string, float>(); // Contiene todas las palabras que aparecen en los textos con su IDF
    public static Dictionary<string, float> TextAndScore = new Dictionary<string, float>(); // A cada ruta de los documentos se le asocia su relevancia con la busqueda
    public static Dictionary<string, float> Query = new Dictionary<string, float>(); // Almacena las palabras de la busqueda con su TF-IDF
    public static float[] Similarity = new float[0]; // Contiene la relevancia de cada texto con la busqueda

    public static int Count = 0; // Contador de la cantidad de textos relevantes con la busqueda
    public static void Run() // Metodo que ejecuta la carga de datos y el procesamiento de los textos 
    {
        Stopwatch crono = new Stopwatch();
        crono.Start();
        DocReader.Fill(Texts, DocReader.docs); // Llena el diccionario Texts con los textos, sus palabras y la cantidad de veces que aparecen 
        DocReader.FillIDF(IDF, Texts); // Llena el diccionario IDF
        DocReader.CalculateTF(Texts); // Calculo del TF y guardado en el diccionario Texts
        DocReader.TFIDF(Texts, IDF); // Calculo del TF-IDF y guardado en el diccionario Texts
        crono.Stop();
        Console.WriteLine(crono.Elapsed);
    }
    public static void Search(string query) // Metodo que ejecuta los procesos de busqueda
    {
        SearchProcess.FillQuery(query, Query, IDF); //Llenado del diccionario Query con la cantidad de veces que aparecen las palabras en cada texto
        SearchProcess.CalculateTFQuery(Query); // Calculo del tf de las palabras del diccionario Query
        SearchProcess.CalculateTFIDFQuery(IDF, Query); // Calculo del TF-IDF y guardado en el diccionario Query
        Similarity = SearchProcess.Cosine(Texts, Query); // Calculo de la relevacia de cada texto y guardado en el array Similarity
        Count = Exit.Counter(Similarity); // Calculo de la cantidad de textos relevantes
        Exit.AddValues(Similarity, TextAndScore, DocReader.docs); // Incorporacion de la relevancia de los textos en el diccionario TextAndScore 
        TextAndScore = Exit.OrderDic(TextAndScore); // Ordenamiento del diccionario TextAndScore
    }
}