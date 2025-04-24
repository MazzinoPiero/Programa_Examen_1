using System;
using System.Collections.Generic;

namespace JuegoDeFabrica
{
    public class Lugar
    {
        public string Nombre { get; protected set; }
        public string Descripcion { get; protected set; }
        public bool Visitado { get; set; }
        protected string TipoMaterial;

        public Lugar(string nombre, string descripcion, string tipoMaterial)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            TipoMaterial = tipoMaterial;
            Visitado = false;
        }

        public virtual void MostrarDescripcion()
        {
            Console.WriteLine($"\n {Nombre} ");
            Console.WriteLine(Descripcion);
            Console.WriteLine($"Aquí puedes encontrar: {TipoMaterial}");
        }

        public virtual Decisiones GenerarDecisiones()
        {
            return new Decisiones(TipoMaterial);
        }
    }

    public class Bosque : Lugar
    {
        public Bosque() : base("Bosque", "Un frondoso bosque lleno de árboles milenarios.", "Madera") { }
    }

    public class Cantera : Lugar
    {
        public Cantera() : base("Cantera", "Una cantera rica en vetas de plata.", "Plata") { }
    }

    public class Cueva : Lugar
    {
        public Cueva() : base("Cueva", "Una profunda cueva con abundantes formaciones rocosas.", "Piedra") { }
    }

    public class Mapa
    {
        private List<Lugar> _lugares;

        public Mapa()
        {
            _lugares = new List<Lugar>
            {
                new Bosque(),
                new Cantera(),
                new Cueva()
            };
        }

        public List<Lugar> ObtenerLugaresDisponibles()
        {
            List<Lugar> disponibles = new List<Lugar>();
            
            foreach (var lugar in _lugares)
            {
                if (!lugar.Visitado)
                {
                    disponibles.Add(lugar);
                }
            }
            
            return disponibles;
        }

        public void ReiniciarVisitas()
        {
            foreach (var lugar in _lugares)
            {
                lugar.Visitado = false;
            }
        }
    }
}