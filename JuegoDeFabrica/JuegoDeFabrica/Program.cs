using System;
using System.Collections.Generic;
using System.Threading;

namespace JuegoDeFabrica
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Juego de Fábrica de Armas";
            
            try 
            {
                Juego juego = new Juego();
                juego.Iniciar();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("¡Ha ocurrido un error inesperado!");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Detalles: {ex.StackTrace}");
                Console.ResetColor();
                Console.WriteLine("\nPresiona enter tecla para salir...");
                Console.ReadLine();
            }
        }
    }
}