using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
public class Vocabulary
{
    public static List<(string,double)>Texts = new List<(string, double)>(); // Textos completos y su similitud 
    
    public static List<string> TxtVocabulary = new List<string>(); // Vocabulario
    
    public static List <List<string>>documents = new List<List<string>>(); // textos  sus palabras ya procesadas
    public static string [] files = new string [0]; // Nombres de textos
    
    public static List<string> CreateVocabulary(ref List<List<string>> documents, ref string[] files, ref List<(string,double)>Texts)
        {
            List<string> vocabulary = new List<string>();
            Dictionary<string, int> wordCountList = new Dictionary<string, int>();
            
            
            files = TextReader.ScantTxtFiles();
            var path = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.FullName + "/moogle-main/Content/";
            
            
            int docIndex = 1;
            foreach (var doc in files)
            {
                List<string> document = new List<string>();

                var content = File.ReadAllText(path + doc);

                Texts.Add((content, 0.0));
                
                var wordPattern = new Regex(@"\w+");

                List<string> words = new List<string>();

                //use regex MATCH for take each word inside the document.
                foreach (Match match in wordPattern.Matches(content.ToLower()))
                {
                    string word = match.Value.ToLower();
                    word = Replace(word); 
                    
                    words.Add(word);

                    if (word.Length > 0)
                    {
                        // Build the word count list.
                        if (wordCountList.ContainsKey(word))
                        {
                            wordCountList[word]++;
                        }
                        else
                        {
                            wordCountList.Add(word, 1);
                        }

                        document.Add(word);
                    }
                }

                documents.Add(document);
                docIndex++;
            }
            
            foreach (var item in wordCountList)
            {
                vocabulary.Add(item.Key);
                //PrintValues(item.Key, item.Value);
            }
           
            return vocabulary;
        }

    public static string Replace(string input)
    {
        
        input = input.Normalize(NormalizationForm.FormD);
        string pattern = @"[\p{Mn}]";
        input = Regex.Replace(input, pattern, string.Empty);
        return input;
    }


}


public class TextReader 
{
    public static string[] ScantTxtFiles()
    {
        var docs = new string[0];      
        var path = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.FullName;
        path = path + "/moogle-main/Content";
        docs = Directory.GetFiles(path, "*.txt")!
        .Select(Path.GetFileName)!
        .ToArray()!;
        return docs;
    }
    
}

public class TF_Idf
{
    public static Dictionary<string, double> vocabularyIDF = new Dictionary<string, double>();
    

    public  static Dictionary<string,double> CalculateIDF(List<string> BagOFwords, List<List<string>> documents)
    {
        if (vocabularyIDF.Count == 0)
        {
            var docIndex = 0;
            foreach (var term in BagOFwords)
            {
                double numberOfDocsContainingTerm = documents.Where(d => d.Contains(term)).Count();
                vocabularyIDF[term] =(numberOfDocsContainingTerm==0)?0:Math.Log((double)documents.Count / ((double)numberOfDocsContainingTerm-0.5),10);
                docIndex = docIndex + 1;
            }
        }
        return vocabularyIDF;
    }   
    
    public static double [][] TextMatrix = new double[0][];
    public  static double[][] TransformToTFIDFVectors(List<List<string>> documents, Dictionary<string, double> vocabularyIDF)
        {

            List<List<double>> vectors = new List<List<double>>();

            foreach (var doc in documents)
            {
                List<double> vector = new List<double>();

                foreach (var vocab in vocabularyIDF)
                {
                    double tf = doc.Where(d => d == vocab.Key).Count();

                    double tfidf = tf * vocab.Value;

                    vector.Add(tfidf);
                }

                vectors.Add(vector);
            }

            return vectors.Select(v => v.ToArray()).ToArray();
        }   
}
public class NormalizeVectors
{
    public static double[][] NormalizeMatriz(double[][] vectors)
    {
        List<double[]> normalizedVectors = new List<double[]>();
        foreach (var vector in vectors)
        {
            var normalized = NormalizeVector(vector);
            normalizedVectors.Add(normalized);
        }
        
        return normalizedVectors.ToArray();
    }
    public static double[] NormalizeVector (double[]vector)
{
    // Calcula la magnitud del vector
double magnitude = 0.0;
foreach (double element in vector)
{
    magnitude += element * element;
}
magnitude = Math.Sqrt(magnitude);

// Normaliza el vector
for (int i = 0; i < vector.Length; i++)
{
    vector[i] /= (double)magnitude;
}
return vector;
}
   
}

public class Query
{
    public static string input = "query";//Query de entrada
    
    public static List<string> QueryVocabulary = new List<string>(); //query completa en lista en lista
    public static List<string> QueryModified = new List<string>(); // query sin repeticion de palabras
    public static List<string>  filter(string query, ref List<string> QueryVocabulary )
{
    //Eliminar mayusculas
    query = query.ToLower();

    //Eliminar tildes y signos 
    query = Vocabulary.Replace(query);
    
    //Convierte el string en una lista de palabras sin signos de puntuacion
    List<string> filtered = query.Split().Select(word => new string(word.Where(c => !char.IsPunctuation(c)).ToArray())).ToList();
    
    QueryVocabulary = filtered;
    
    // Eliminar palabras repetidas
    filtered = filtered.Distinct().ToList(); 

    // Eliminar palabras que no estan en el vocabulario
    filtered.RemoveAll(word => !Vocabulary.TxtVocabulary.Contains(word)); 

    return filtered;
}

public static Dictionary<string,double> tfQuery = new Dictionary<string, double>();

public static Dictionary<string,double> TFquery(List<string> QueryModified, List<string> queryVocabulary)
{
    Dictionary<string, double> tf = new Dictionary<string, double>();

        foreach (string word in QueryModified)
        {
            int count = queryVocabulary.Count(w => w == word);
            double frequency = (double)count / queryVocabulary.Count;
            tf[word] = frequency;
        }
    return tf;
}

public static Dictionary<string,double> TFIDFQuery = new Dictionary<string, double>();

public static Dictionary<string,double> TFIDF(Dictionary<string,double>tf,Dictionary<string,double>idf)
{
     Dictionary<string, double> tfidf = new Dictionary<string, double>();

        foreach (string term in tf.Keys)
        {
            double tfidfValue = tf[term] * idf[term];
            tfidf[term] = tfidfValue;
        }
    return tfidf;
}
public static Dictionary<string,double> Vector = new Dictionary<string, double>();
public static Dictionary<string,double> VectorQuery(Dictionary<string,double>TFIDFQuery)
{
    double[] vector = new double[TFIDFQuery.Count];
    int i = 0;
    foreach (KeyValuePair<string, double> entry in TFIDFQuery)
    {
        vector[i] = entry.Value;
        i++;
    }
    vector = NormalizeVectors.NormalizeVector(vector);
    TFIDFQuery = ChangeDoubles(TFIDFQuery,vector);
    
    return TFIDFQuery;

}
private static Dictionary<string, double> ChangeDoubles(Dictionary<string, double> dict,double[] vector)
{
   Dictionary<string, double> result = new Dictionary<string, double>();

   // Recorremos el diccionario y reemplazamos los valores
   for (int i = 0; i < vector.Length; i++)
   {
       string key = dict.Keys.ElementAt(i);
       double val = vector[i];
       result[key] = val;
   }

   return result;
}
  public static List<(string,double)> VectorCosine = new List<(string, double)>();
  public static List<(string, double)> BuildVector(List<string> Vocabulary, Dictionary<string, double> Query)
{
    List<(string, double)> vector = new List<(string, double)>();
    foreach(string word in Vocabulary)
    {
        if(Query.ContainsKey(word))
        {
            vector.Add((word, Query[word]));
        }
        else
        {
            vector.Add((word, 0.0));
        }
    }
    return vector;
}   
    public static double[] GetVector(List<(string, double)> tupleList)
{
    double[] array = new double[tupleList.Count];
    for(int i = 0; i < tupleList.Count; i++)
    {
        array[i] = tupleList[i].Item2;
    }
    return array;
   }   public static double [] Vectorforcomp = new double [0];
}


public class Similarity 
{
    public static double [] SimilarityVector = new double [0];
     public static double [] Cosine(double[]queryVector,double[][] documentMatrix)
     {
         double[] similarities = new double[documentMatrix.Length];
 for (int i = 0; i < documentMatrix.Length && i<queryVector.Length; i++)
 {
     double[] documentVector = documentMatrix[i];
     double dotProduct = 0.0;
     double queryMagnitude = 0.0;
     double documentMagnitude = 0.0;
     for (int j = 0; j < documentVector.Length; j++)
     {
         dotProduct += queryVector[j] * documentVector[j];
         queryMagnitude += queryVector[j] * queryVector[j];
         documentMagnitude += documentVector[j] * documentVector[j];
     }
     queryMagnitude = Math.Sqrt(queryMagnitude);
     documentMagnitude = Math.Sqrt(documentMagnitude);
     similarities[i] = dotProduct / (queryMagnitude * documentMagnitude);
 }
 return similarities;
     }







public static List<(string,double)> Modify(List<(string,double)>Tuple, double[]vector)
{
    for (int i = 0;i < vector.Length; i++)
{
    // Obtener la tupla actual y el valor correspondiente en el array
    var ActualTuple = Tuple[i];
    var ActualVector = vector[i];

    // Actualizar el valor de la tupla con el valor del array
    Tuple[i] = (ActualTuple.Item1, ActualVector); 
    
}    
    return Tuple;


}
   



}





public class Ejecution
{
    public static void Part1 ()
    {
        Vocabulary.TxtVocabulary = Vocabulary.CreateVocabulary(ref Vocabulary.documents,ref Vocabulary.files,ref Vocabulary.Texts);
        TF_Idf.vocabularyIDF = TF_Idf.CalculateIDF(Vocabulary.TxtVocabulary,Vocabulary.documents);
        TF_Idf.TextMatrix = TF_Idf.TransformToTFIDFVectors(Vocabulary.documents,TF_Idf.vocabularyIDF);
        TF_Idf.TextMatrix = NormalizeVectors.NormalizeMatriz(TF_Idf.TextMatrix); 
        
        
    } 
    public static void Part2(string Search)
    {
        Query.input= Search;
        Query.QueryModified= Query.filter(Query.input, ref Query.QueryVocabulary);
        Query.tfQuery = Query.TFquery(Query.QueryModified,Query.QueryVocabulary);
        Query.TFIDFQuery = Query.TFIDF(Query.tfQuery,TF_Idf.vocabularyIDF);
        Query.Vector = Query.VectorQuery(Query.TFIDFQuery);
        Query.VectorCosine = Query.BuildVector(Vocabulary.TxtVocabulary,Query.TFIDFQuery);
        Query.Vectorforcomp = Query.GetVector(Query.VectorCosine);  
        Similarity.SimilarityVector = Similarity.Cosine(Query.Vectorforcomp,TF_Idf.TextMatrix);
        Vocabulary.Texts = Similarity.Modify(Vocabulary.Texts,Similarity.SimilarityVector);
        Exit.Count = Exit.Counter(Vocabulary.Texts);
    }
}

public static class Exit
{
    public static int PositionMax(List<(string, double)> list)
{
    double max = double.MinValue;
    int posicion = -1;

    for (int i = 0; i < list.Count; i++)
    {
        if (list[i].Item2 > max)
        {
            max = list[i].Item2;
            posicion = i;
        }
    }

    return posicion;
}
public static int PositionSecondMax(List<(string, double)> list)
{
    double max = double.MinValue;
    double secondmax = double.MinValue;
    int posicionSegundoMaximo = -1;
    int posicionMax = -1;

    for (int i = 0; i < list.Count; i++)
    {
        if (list[i].Item2 > max)
        {
            secondmax = max;
            max = list[i].Item2;
            posicionSegundoMaximo = posicionMax;
            posicionMax = i;
        }
        else if (list[i].Item2 > secondmax)
        {
            secondmax = list[i].Item2;
            posicionSegundoMaximo = i;
        }
    }

    return posicionSegundoMaximo;
}
public static int PositionThirdMax(List<(string, double)> lista)
{
    double max = double.MinValue;
    double secondmax = double.MinValue;
    double thirdmax = double.MinValue;
    int posicionTercerMaximo = -1;
    int posicionSegundoMaximo= -1;
    int posicionMax=-1;

    for (int i = 0; i < lista.Count; i++)
    {
        if (lista[i].Item2 > max)
        {
            thirdmax = secondmax;
            secondmax = max;
            max = lista[i].Item2;
            posicionTercerMaximo = posicionSegundoMaximo;
            posicionSegundoMaximo = posicionMax;
            posicionMax = i;
            
            
            
        }
        else if (lista[i].Item2 > secondmax )
        {
            thirdmax = secondmax;
            secondmax = lista[i].Item2;
            posicionTercerMaximo = posicionSegundoMaximo;
            posicionSegundoMaximo = i;
        }
        else if (lista[i].Item2 > thirdmax )
        {
            thirdmax = lista[i].Item2;
            posicionTercerMaximo = i;
        }
    }

    return posicionTercerMaximo;
}

public static int Count = 0;
public static int Counter(List<(string, double)> list)
{
    int counter = 0; // contador para llevar el conteo de los dobles no cero
    
    foreach ((string, double) tupla in list) // iterar sobre la lista de tuplas
    {
        if (tupla.Item2 != 0 && !(Double.IsNaN(tupla.Item2))) // verificar si el segundo elemento de la tupla es diferente de cero
        {
            counter++; // si es diferente de cero, aumentar el contador
        }
    }
    
    return counter; // devolver la cantidad de dobles no cero encontrados
}
public static float Approach(List<(string, double)> list, int position)
{
    float result = 0f; // variable para almacenar el resultado de la aproximación
    double valor = list[position].Item2; // obtener el doble del valor en la posición indicada
    result = (float)Math.Round(valor, 2); // aproximar el valor a dos decimales y convertirlo a float
    
    return result; 
}
    
    public static int SearchWord(List<string> palabrasQuery, List<string> palabrasDocumento)
{
    foreach (string palabra in palabrasQuery)
    {
        int posicion = palabrasDocumento.IndexOf(palabra);
        
        
            return posicion;
        
    }
    return -1; 
      
}

public static string GetSubstringWithWordAtPosition(string text, int position)
{
    // Eliminamos los espacios y saltos de línea al principio y final del texto
    text = text.Trim();

    // Reemplazamos los saltos de línea por espacios
    text = text.Replace("\n", " ");

    // Separar el texto en palabras
    string[] words = text.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);;


    // Encontrar el índice de la palabra en la posición entregada
    int wordIndex = position;

    // Definir los límites del substring
    int startIndex = Math.Max(0, wordIndex - 70);
    int endIndex = Math.Min(words.Length - 1, wordIndex + 69);

    // Construir el substring
    StringBuilder sb = new StringBuilder();
    for (int i = startIndex; i <= endIndex; i++)
    {
        sb.Append(words[i]);
        sb.Append(' ');
    }

    return sb.ToString().Trim();
}

    
 
 

 
} 
public  class Matrix 
{
public static int[,] MatrixMult(int[,] A, int[,] B)
    {
        int numFilasA = A.GetLength(0);
        int numColumnasA = A.GetLength(1);
        int numFilasB = B.GetLength(0);
        int numColumnasB = B.GetLength(1);

        if (numColumnasA == numFilasB)
        {
            int[,] Resultado = new int[numFilasA, numColumnasB];

            for (int i = 0; i < numFilasA; i++)
            {
                for (int j = 0; j < numColumnasB; j++)
                {
                    int valorCelda = 0;
                    for (int k = 0; k < numColumnasA; k++)
                    {
                        valorCelda += A[i, k] * B[k, j];
                    }
                    Resultado[i, j] = valorCelda;
                }
            }
            return Resultado;
        }
        else
        {
            throw new ArgumentException("Las matrices no son multiplicables.");
        }
    }
public static double[,] ScalarMultiplication(double[,] matrix, double scalar) 
{ 
    int rowCount = matrix.GetLength(0); 
    int colCount = matrix.GetLength(1); 

    double[,] result = new double[rowCount,colCount]; 

    for(int row = 0; row < rowCount; row++) 
    { 
        for(int col = 0; col < colCount; col++) 
        { 
            result[row, col] = matrix[row, col] * scalar; 
        } 
    } 

    return result; 
}

public static int[,] MatrixSum(int[,] matrizA, int[,] matrizB)
{
    int rowA = matrizA.GetLength(0);
    int colA = matrizA.GetLength(1);
    int rowB = matrizB.GetLength(0);
    int colB = matrizB.GetLength(1);

    if (rowA != rowB || colA != colB)
        throw new Exception("Las matrices deben ser del mismo tamaño para poder sumarlas.");

    int[,] matrizResultado = new int[rowA, colA];
    for (int i = 0; i < rowA; i++)
    {
        for (int j = 0; j < colA; j++)
        {
            matrizResultado[i, j] = matrizA[i, j] + matrizB[i, j];
        }
    }
    return matrizResultado;
 }


    public static double[,] MatrixInverse(double[,] matrix) 
    { 
        int order = matrix.GetLength(0); 
        double[,] inverseMatrix = new double[order, order]; 

        // Calcular el adjunto de la matriz 
        inverseMatrix = FindAdjoint(matrix); 

         // Calcular el determinante de la matriz 
        double det = FindDeterminant(matrix); 
        
        // Calcular la matriz inversa 
        for (int i=0; i < order; i++){
            for (int j=0; j < order; j++) {
                inverseMatrix[i, j] = inverseMatrix[i, j]/det; }
        }
        return inverseMatrix; 
    } 

private static double FindDeterminant(double[,] matrix) 
    { 
        int order = matrix.GetLength(0); 
  
        if (order == 1) 
            return matrix[0, 0]; 
  
        double det = 0; 
        double[,] subMatrix = new double[order - 1, order - 1]; 
  
        for (int x = 0; x < order; x++) 
        { 
            subMatrix = GetSubMatrix(matrix, 0, x); 
            det += (Math.Pow(-1, x) * matrix[0, x] * FindDeterminant(subMatrix)); 
        } 
        return det; 
    } 
  
private static double[,] FindAdjoint(double[,] matrix) 
    { 
        int order = matrix.GetLength(0); 
        double[,] adjoint = new double[order, order]; 
  
        if (order == 1) 
        { 
            adjoint[0, 0] = 1; 
            return adjoint; 
        } 
  
        for (int i = 0; i < order; i++) 
        { 
            for (int j = 0; j < order; j++) 
            { 
                double[,] subMatrix = GetSubMatrix(matrix, i, j); 
                adjoint[j, i] = ((Math.Pow(-1, i + j)) * 
                FindDeterminant(subMatrix)); 
            } 
        } 
        return adjoint; 
    } 


private static double[,] GetSubMatrix(double[,] matrix, int omit_i, int omit_j) 
    { 
        int order = matrix.GetLength(0); 
  
        double[,] subMatrix = new double[order - 1, order - 1]; 
  
        int i = 0; 
        for (int x=0; x < order; x++) 
            if (x != omit_i) 
            { 
                int j = 0; 
                for (int y=0; y < order; y++) 
                    if (y != omit_j) 
                    { 
                        subMatrix[i, j] = matrix[x, y]; 
                        j++; 
                    } 
                i++; 
            } 
        return subMatrix; 
    } 
}





    

