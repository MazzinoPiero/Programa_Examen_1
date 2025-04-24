using System;

namespace JuegoDeFabrica
{
    public class Jugador
    {
        public int PuntosVida { get; private set; }
        public int VidaMaxima { get; private set; }
        public Inventario Inventario { get; private set; }

        public Jugador(int vidaMaxima = 3)
        {
            VidaMaxima = vidaMaxima;
            PuntosVida = VidaMaxima;
            Inventario = new Inventario();
        }

        public bool EstaVivo()
        {
            return PuntosVida > 0;
        }

        public void RecibirDaño(int cantidad)
        {
            PuntosVida -= cantidad;
            
            if (cantidad > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"¡Has perdido {cantidad} punto(s) de vida!");
                Console.ResetColor();
            }
            
            if (PuntosVida <= 0)
            {
                PuntosVida = 0;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("¡Has agotado todos tus puntos de vida!");
                Console.ResetColor();
            }
        }

        public void Curar()
        {
            int cantidadCurada = VidaMaxima - PuntosVida;
            
            if (cantidadCurada > 0)
            {
                PuntosVida = VidaMaxima;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"¡Te has recuperado completamente! (+{cantidadCurada} puntos de vida)");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Ya estás en perfecta salud.");
            }
        }

        public void RecibirMaterial(string tipoMaterial, int cantidad)
        {
            try
            {
                Material material = Inventario.ObtenerMaterial(tipoMaterial);
                material.Añadir(cantidad);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error al recibir material: {ex.Message}");
                Console.ResetColor();
            }
        }

        public void MostrarEstado()
        {
            Console.WriteLine("\nESTADO DEL JUGADOR");
            
            Console.Write("Vida: ");
            
            if (PuntosVida <= VidaMaxima / 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (PuntosVida <= 2 * VidaMaxima / 3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            
            Console.WriteLine($"{PuntosVida}/{VidaMaxima}");
            Console.ResetColor();
            
            Inventario.MostrarInventario();
        }
    }
}