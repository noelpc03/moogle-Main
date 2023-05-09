Moogle es un buscador de archivos .txt, el cual mediante una consulta debe entregar un grupo de nombres de textos ordenados por relevancia(máximo 3) mostrando una pequeña porción de los mismos donde salgan elementos de la query. Para determinar esta relevancia se utiliza el calculo del TF-IDF, así como también la función coseno. 

Al comenzar a compilar el programa, lo primero que hace es crear el vocabulario(todas las palabras diferentes que aparecen en los textos guardados en la carpeta Content). En este proceso se guardan los títulos en un array para después mostrarlos en la búsqueda, son guardados los textos en una lista de tupla de string y double en la que a cada texto se le asocia un valor de similitud(0.0) y también se colocan todas las palabras en su orden de aparición en los textos en una lista de lista de string para poder sacar mas tarde la cantidad de veces que se repite una palabra en un texto. De todas las palabras son eliminados todos los símbolos(la ñ se mantiene), mayúsculas y signos antes de colocarlas en el vocabulario. 

Después se realiza el calculo del TF-IDF, comenzando por el calculo de los IDF de las palabras del vocabulario y guardándolo en un diccionario. Luego se calcula el tf de cada palabra en cada documento para así llegar al TF-IDF del vocabulario en cada archivo. Esta información es guardada en una matriz cuyos valores son normalizados. 

Llegado a este punto el Moogle es lanzado en el navegador (3.5 mb de .txt se demoran en cargar 7 min) a la espera de que el usuario introduzca una consulta. Esta es normalizada y convertida a una lista para así facilitar el calculo de su tf. Este se guarda en un diccionario y con el diccionario de los idf del vocabulario se halla su IDF. A este le añaden las palabras del vocabulario que no aparecen en la query y se le asigna el 0. Estos valores son llevados a vector para así calcular la similitud, mediante el coseno, con cada uno de los vectores de la matriz(cada vector corresponde a un texto). El vector resultante muestra el parentesco de la consulta con cada texto, el cual se le introduce a la lista de tupla. 

Al saber cual es la importancia de la query para cada texto, el siguiente paso es la creación de los objetos del método query que es el encargado de mostrar la salida(cada objeto tiene un titulo, un snippet y un score). Para esta tarea se encuentra un método que dependiendo de la cantidad de textos con similitud diferente de 0 dice la cantidad que se deben crear:si es 0 no hay ningún documento que satisface a la consulta y se crea un objeto con este mensaje, si es 1 se crea un objeto con la información del texto relevante y así sucesivamente hasta llegar hasta 3(si hay mas de 3 se muestran los 3 mas importantes). 

(Esta explicación es análoga para los 3 objetos)
Para la muestra del título del primer objeto se usa un método que busca el texto más relevante en la lista de tupla y después toma su título del array que los contiene. Luego, para sacar el snippet, el programa va a buscar una palabra de la query en la lista de listas de palabras que se creo al principio(toma la lista correspondiente al texto mas relevante) y tomara su posición que será el centro del substring que se mostrará. Este se sacará de la lista de tupla y va a tener como máximo 140 palabras(70 hacia la izquierda y 69 hacia la derecha) siempre evitando que se vaya de los limites del texto. 

Normalización de un vector: 

La fórmula matemática para normalizar un vector es:

v_norm = v / ||v||

Donde v es el vector original y ||v|| es la norma del vector, que se calcula como la raíz cuadrada de la suma de los cuadrados de sus componentes:
|
|v|| = sqrt(v1^2 + v2^2 + ... + vn^2)

El resultado de esta fórmula es un nuevo vector v_norm que tiene la misma dirección que el vector original, pero su longitud se encuentra entre 0 y 1. 

TF-IDF:
La fórmula matemática para calcular el tf-idf es:

tf-idf(t, d) = tf(t, d) * idf(t)


Donde:

- tf(t, d) es la frecuencia de término (tf) del término t en el documento d.
- idf(t) es la frecuencia inversa de documento (idf) del término t en el corpus.

La fórmula completa para calcular idf es:

idf(t) = log(N / df(t))

Donde:
- N es el número total de documentos en el corpus.
- df(t) es el número de documentos que contienen el término t.

Por lo tanto, la fórmula completa para calcular tf-idf es:

tf-idf(t, d) = frecuencia de término (tf) del término t en el documento d * log(N / número de documentos que contienen el término t). 

Función coseno:
La fórmula matemática para calcular la similitud del coseno entre dos vectores A y B es:

similitud del coseno = (A · B) / (||A|| ||B||)

Donde:

- A · B es el producto punto de los vectores A y B.
- ||A|| es la norma euclidiana del vector A.
- ||B|| es la norma euclidiana del vector B.


