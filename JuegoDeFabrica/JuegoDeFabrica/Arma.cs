using System;
using System.Collections.Generic;

namespace JuegoDeFabrica
{
    public class Arma
    {
        public string Nombre { get; private set; }
        public bool EstaFabricada { get; private set; }
        public Dictionary<string, int> MaterialesRequeridos { get; private set; }

        public Arma(string nombre, Dictionary<string, int> materialesRequeridos)
        {
            Nombre = nombre;
            MaterialesRequeridos = materialesRequeridos;
            EstaFabricada = false;
        }

        public void Fabricar()
        {
            EstaFabricada = true;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n¡Muy bien mi explorador pulpin!¡Has fabricado {Nombre}!");
            Console.ResetColor();
        }

        public override string ToString()
        {
            string estado = EstaFabricada ? "Fabricada" : "Por fabricar";
            string info = $"{Nombre} - {estado}\n  Requiere: ";
            
            foreach (var material in MaterialesRequeridos)
            {
                info += $"{material.Value} de {material.Key}, ";
            }
            
            return info.TrimEnd(',', ' ');
        }
    }

    public class Armeria
    {
        private List<Arma> _armas; 

        public Armeria()
        {
            _armas = new List<Arma>
            {
                new Arma("Espada", new Dictionary<string, int> { 
                    { "Madera", 2 }, 
                    { "Plata", 2 } 
                }),
                
                new Arma("Hacha", new Dictionary<string, int> { 
                    { "Madera", 2 }, 
                    { "Piedra", 2 } 
                }),
                
                new Arma("Pistola", new Dictionary<string, int> { 
                    { "Plata", 2 }, 
                    { "Piedra", 4 } 
                })
            };
        }

        public bool TodasArmasFabricadas()
        {
            foreach (var arma in _armas)
            {
                if (!arma.EstaFabricada)
                {
                    return false;
                }
            }
            return true;
        }

        public void MostrarArmas()
        {
            Console.WriteLine("\n ARMAS A FABRICAR: ");
            foreach (var arma in _armas)
            {
                Console.WriteLine($"- {arma}");
            }
        }

        public List<Arma> ObtenerArmasFabricables(Inventario inventario)
        {
            List<Arma> fabricables = new List<Arma>();
            
            foreach (var arma in _armas)
            {
                if (!arma.EstaFabricada)
                {
                    bool puedeSerFabricada = true;
                    
                    foreach (var material in arma.MaterialesRequeridos)
                    {
                        if (!inventario.TieneSuficientes(material.Key, material.Value))
                        {
                            puedeSerFabricada = false;
                            break;
                        }
                    }
                    
                    if (puedeSerFabricada)
                    {
                        fabricables.Add(arma);
                    }
                }
            }
            
            return fabricables;
        }

        public void FabricarArma(Arma arma, Inventario inventario)
        {
            foreach (var material in arma.MaterialesRequeridos)
            {
                inventario.Consumir(material.Key, material.Value);
            }
            
            arma.Fabricar();
        }
    }
}