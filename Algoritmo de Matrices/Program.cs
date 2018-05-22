using System;

namespace Practica_v1
{
    class Program
    {
        static void Main(string[] args)
         {
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Bienvenido al Programa de Leslie\n");
             
            bool continuar = true;
            while(continuar == true ){
            Console.WriteLine("Ingrese al Sistema un numero que se encuentre en el rango del intervalo cerrado 2,6\n");
             
             var orden = int.Parse(Console.ReadLine());
                  if(orden >=2 && orden <=6 ){

                    int f = orden;
                    int c = orden;
                    Random generar = new Random();
                    int [,] matriz = new int [f, c];

                    for (int i=0; i<f; ++i)
                    for(int j=0; j<c; j++)
                    matriz [i, j] = generar.Next(10);

                    var columna = "";
                    for (int i=0; i<f; i++){
                        columna = "";
                        for(int j=0; j<c; j++ ){                    
                            columna = columna + "["+matriz[j,i] +"]";
                            }
                        Console.WriteLine(columna + "\n");
                    }

                if ((orden%2 == 0) && (orden%3 == 0)) {
                    int sumaTotalFilas = 0;
                    for (int i=0; i<f; i++){
                        int sumaFila = 0;
                        for(int j=0; j<c; j++ ){ 
                            sumaFila =  sumaFila + matriz[j,i];
                            sumaTotalFilas = sumaTotalFilas + sumaFila;
                        }     
                        Console.WriteLine("La suma de la fila "+(i+1)+" es "+sumaFila+ "\n");                
                    }
                    Console.WriteLine("La suma total de las filas es "+sumaTotalFilas+ "\n");
                } else {
                    int productoTotalColumnas = 1;
                    for (int i=0; i<f; i++){
                        int productoColumna = 1;
                        for(int j=0; j<c; j++ ){ 
                            productoColumna =  productoColumna * matriz[i,j];
                            productoTotalColumnas = productoTotalColumnas * productoColumna;
                        }     
                        Console.WriteLine("El producto de la columna "+(i+1)+" es "+productoColumna+"\n");                
                    } 
                    Console.WriteLine("El producto total de las columnas es "+productoTotalColumnas+"\n");
                }
                    }else{
                        Console.WriteLine("El numero Ingresado esta fuera del rango aceptado\n");
                    }
            Console.WriteLine("Ingresa Y para continuar o S para salir\n");
            var continuando = Console.ReadLine();
            
            if(continuando == "Y".ToLower())
            {
                continuar = true;
                Console.Clear();
            }else{
                continuar = false;
                 Console.WriteLine("Bye Bye");
                 }
            }

        Console.ReadKey();
       
        }
    }
}
