
public class MatrixOperations
{
    public static int[,] MatrixMult(int[,] A, int[,] B) // Multiplicacion de matrices 
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
    public static double[,] ScalarMultiplication(double[,] matrix, double scalar) // Multiplicacion de una matriz por un escalar
    {
        int rowCount = matrix.GetLength(0);
        int colCount = matrix.GetLength(1);

        double[,] result = new double[rowCount, colCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                result[row, col] = matrix[row, col] * scalar;
            }
        }

        return result;
    }

    public static int[,] MatrixSum(int[,] matrizA, int[,] matrizB) // Suma de dos matrices
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


    public static double[,] MatrixInverse(double[,] matrix) // Inversa de una matriz
    {
        int order = matrix.GetLength(0);
        double[,] inverseMatrix = new double[order, order];

        inverseMatrix = FindAdjoint(matrix);

        double det = FindDeterminant(matrix);

        for (int i = 0; i < order; i++)
        {
            for (int j = 0; j < order; j++)
            {
                inverseMatrix[i, j] = inverseMatrix[i, j] / det;
            }
        }
        return inverseMatrix;
    }

    private static double FindDeterminant(double[,] matrix) // Calculo del determinante
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

    private static double[,] FindAdjoint(double[,] matrix) // Calculo de la matriz adjunta
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
        for (int x = 0; x < order; x++)
            if (x != omit_i)
            {
                int j = 0;
                for (int y = 0; y < order; y++)
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

