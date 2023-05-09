Moogle es un buscador de archivos .txt, el cual mediante una consulta debe entregar un grupo de nombres de textos ordenados por relevancia(m�ximo 3) mostrando una peque�a porci�n de los mismos donde salgan elementos de la query. Para determinar esta relevancia se utiliza el calculo del TF-IDF, as� como tambi�n la funci�n coseno. 

Al comenzar a compilar el programa, lo primero que hace es crear el vocabulario(todas las palabras diferentes que aparecen en los textos guardados en la carpeta Content). En este proceso se guardan los t�tulos en un array para despu�s mostrarlos en la b�squeda, son guardados los textos en una lista de tupla de string y double en la que a cada texto se le asocia un valor de similitud(0.0) y tambi�n se colocan todas las palabras en su orden de aparici�n en los textos en una lista de lista de string para poder sacar mas tarde la cantidad de veces que se repite una palabra en un texto. De todas las palabras son eliminados todos los s�mbolos(la � se mantiene), may�sculas y signos antes de colocarlas en el vocabulario. 

Despu�s se realiza el calculo del TF-IDF, comenzando por el calculo de los IDF de las palabras del vocabulario y guard�ndolo en un diccionario. Luego se calcula el tf de cada palabra en cada documento para as� llegar al TF-IDF del vocabulario en cada archivo. Esta informaci�n es guardada en una matriz cuyos valores son normalizados. 

Llegado a este punto el Moogle es lanzado en el navegador (3.5 mb de .txt se demoran en cargar 7 min) a la espera de que el usuario introduzca una consulta. Esta es normalizada y convertida a una lista para as� facilitar el calculo de su tf. Este se guarda en un diccionario y con el diccionario de los idf del vocabulario se halla su IDF. A este le a�aden las palabras del vocabulario que no aparecen en la query y se le asigna el 0. Estos valores son llevados a vector para as� calcular la similitud, mediante el coseno, con cada uno de los vectores de la matriz(cada vector corresponde a un texto). El vector resultante muestra el parentesco de la consulta con cada texto, el cual se le introduce a la lista de tupla. 

Al saber cual es la importancia de la query para cada texto, el siguiente paso es la creaci�n de los objetos del m�todo query que es el encargado de mostrar la salida(cada objeto tiene un titulo, un snippet y un score). Para esta tarea se encuentra un m�todo que dependiendo de la cantidad de textos con similitud diferente de 0 dice la cantidad que se deben crear:si es 0 no hay ning�n documento que satisface a la consulta y se crea un objeto con este mensaje, si es 1 se crea un objeto con la informaci�n del texto relevante y as� sucesivamente hasta llegar hasta 3(si hay mas de 3 se muestran los 3 mas importantes). 

(Esta explicaci�n es an�loga para los 3 objetos)
Para la muestra del t�tulo del primer objeto se usa un m�todo que busca el texto m�s relevante en la lista de tupla y despu�s toma su t�tulo del array que los contiene. Luego, para sacar el snippet, el programa va a buscar una palabra de la query en la lista de listas de palabras que se creo al principio(toma la lista correspondiente al texto mas relevante) y tomara su posici�n que ser� el centro del substring que se mostrar�. Este se sacar� de la lista de tupla y va a tener como m�ximo 140 palabras(70 hacia la izquierda y 69 hacia la derecha) siempre evitando que se vaya de los limites del texto. 

Normalizaci�n de un vector: 

La f�rmula matem�tica para normalizar un vector es:

v_norm = v / ||v||

Donde v es el vector original y ||v|| es la norma del vector, que se calcula como la ra�z cuadrada de la suma de los cuadrados de sus componentes:
|
|v|| = sqrt(v1^2 + v2^2 + ... + vn^2)

El resultado de esta f�rmula es un nuevo vector v_norm que tiene la misma direcci�n que el vector original, pero su longitud se encuentra entre 0 y 1. 

TF-IDF:
La f�rmula matem�tica para calcular el tf-idf es:

tf-idf(t, d) = tf(t, d) * idf(t)


Donde:

- tf(t, d) es la frecuencia de t�rmino (tf) del t�rmino t en el documento d.
- idf(t) es la frecuencia inversa de documento (idf) del t�rmino t en el corpus.

La f�rmula completa para calcular idf es:

idf(t) = log(N / df(t))

Donde:
- N es el n�mero total de documentos en el corpus.
- df(t) es el n�mero de documentos que contienen el t�rmino t.

Por lo tanto, la f�rmula completa para calcular tf-idf es:

tf-idf(t, d) = frecuencia de t�rmino (tf) del t�rmino t en el documento d * log(N / n�mero de documentos que contienen el t�rmino t). 

Funci�n coseno:
La f�rmula matem�tica para calcular la similitud del coseno entre dos vectores A y B es:

similitud del coseno = (A � B) / (||A|| ||B||)

Donde:

- A � B es el producto punto de los vectores A y B.
- ||A|| es la norma euclidiana del vector A.
- ||B|| es la norma euclidiana del vector B.


