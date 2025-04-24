using System;
using System.Collections.Generic;

namespace JuegoDeFabrica
{
    public interface IRecurso
    {
        string Nombre { get; }
        int Cantidad { get; set; }
        void Añadir(int cantidad);
    }

    public abstract class Material : IRecurso
    {
        public string Nombre { get; protected set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; protected set; }

        public Material(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Cantidad = 0;
        }

        public virtual void Añadir(int cantidad)
        {
            Cantidad += cantidad;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"¡Has obtenido {cantidad} de {Nombre}!");
            Console.ResetColor();
        }

        public override string ToString()
        {
            return $"{Nombre}: {Cantidad}";
        }
    }

    public class Madera : Material
    {
        public Madera() : base("Madera", "Material obtenido de árboles") { }
    }

    public class Piedra : Material
    {
        public Piedra() : base("Piedra", "Rocas resistentes para construcción") { }
    }

    public class Plata : Material
    {
        public Plata() : base("Plata", "Metal precioso utilizado en armas") { }
    }

    public class Inventario
    {
        private Dictionary<string, Material> _materiales; 

        public Inventario()
        {
            _materiales = new Dictionary<string, Material>
            {
                { "Madera", new Madera() },
                { "Piedra", new Piedra() },
                { "Plata", new Plata() }
            };
        }

        public Material ObtenerMaterial(string nombre)
        {
            if (_materiales.ContainsKey(nombre))
            {
                return _materiales[nombre];
            }
            throw new Exception($"Material '{nombre}' no encontrado en el inventario.");
        }

        public void MostrarInventario()
        {
            Console.WriteLine("\nINVENTARIO DE MATERIALES:");
            foreach (var material in _materiales.Values)
            {
                Console.WriteLine($"- {material}");
            }
        }

        public bool TieneSuficientes(string nombre, int cantidad)
        {
            return ObtenerMaterial(nombre).Cantidad >= cantidad;
        }

        public void Consumir(string nombre, int cantidad)
        {
            Material material = ObtenerMaterial(nombre);
            if (material.Cantidad >= cantidad)
            {
                material.Cantidad -= cantidad;
            }
            else
            {
                throw new Exception($"No hay suficiente {nombre} para consumir {cantidad}.");
            }
        }
    }
}