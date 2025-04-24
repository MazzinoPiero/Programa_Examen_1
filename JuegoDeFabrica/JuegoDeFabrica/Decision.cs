using System;
using System.Collections.Generic;
using System.Linq;

namespace JuegoDeFabrica
{
    public class Resultado<T>
    {
        public T Recompensa { get; private set; }
        public int DañoVida { get; private set; }
        public string Mensaje { get; private set; }

        public Resultado(T recompensa, int dañoVida, string mensaje)
        {
            Recompensa = recompensa;
            DañoVida = dañoVida;
            Mensaje = mensaje;
        }
    }

    public class Decision
    {
        public string Descripcion { get; private set; }
        public Resultado<int> Resultado { get; private set; }

        public Decision(string descripcion, Resultado<int> resultado)
        {
            Descripcion = descripcion;
            Resultado = resultado;
        }
    }

    public class PoolDecisiones
    {
        private Dictionary<string, Dictionary<string, Queue<string>>> _decisiones;
        private Dictionary<string, Dictionary<string, Queue<string>>> _descripcionesOpciones;

        public PoolDecisiones()
        {
            _decisiones = new Dictionary<string, Dictionary<string, Queue<string>>>();
            _descripcionesOpciones = new Dictionary<string, Dictionary<string, Queue<string>>>();

            InicializarDecisiones("Madera");
            InicializarDecisiones("Piedra");
            InicializarDecisiones("Plata");

            AgregarDescripcionOpcion("Madera", "muyMala", "Talar el árbol más grande y frondoso del bosque");
            AgregarDescripcionOpcion("Madera", "muyMala", "Usar tu hacha con toda tu fuerza contra ese tronco nudoso");
            AgregarDescripcionOpcion("Madera", "muyMala", "Golpear repetidamente ese árbol que tiene un extraño zumbido");

            AgregarDescripcionOpcion("Madera", "mala", "Recoger ramas caídas que parecen un poco húmedas");
            AgregarDescripcionOpcion("Madera", "mala", "Intentar cortar un árbol que parece enfermo");
            AgregarDescripcionOpcion("Madera", "mala", "Arrancar corteza de los árboles cercanos");

            AgregarDescripcionOpcion("Madera", "buena", "Recolectar ramas caídas que parecen estar en buen estado");
            AgregarDescripcionOpcion("Madera", "buena", "Cortar un árbol pequeño pero sano");
            AgregarDescripcionOpcion("Madera", "buena", "Buscar troncos abandonados por otros leñadores");

            AgregarDescripcionOpcion("Madera", "muyBuena", "Seguir el consejo del anciano del bosque sobre dónde encontrar la mejor madera");
            AgregarDescripcionOpcion("Madera", "muyBuena", "Explorar esa extraña luz entre los árboles");
            AgregarDescripcionOpcion("Madera", "muyBuena", "Examinar el claro donde caen los rayos del sol");

            AgregarDescripcionOpcion("Piedra", "muyMala", "Golpear esa pared que parece inestable");
            AgregarDescripcionOpcion("Piedra", "muyMala", "Excavar profundamente en esa zona con polvo extraño");
            AgregarDescripcionOpcion("Piedra", "muyMala", "Extraer rocas del techo de la cueva");

            AgregarDescripcionOpcion("Piedra", "mala", "Recoger piedras pequeñas del suelo húmedo");
            AgregarDescripcionOpcion("Piedra", "mala", "Intentar partir rocas blandas y quebradizas");
            AgregarDescripcionOpcion("Piedra", "mala", "Buscar en la zona oscura de la cueva");

            AgregarDescripcionOpcion("Piedra", "buena", "Extraer rocas de la pared principal");
            AgregarDescripcionOpcion("Piedra", "buena", "Recoger piedras de tamaño mediano del suelo");
            AgregarDescripcionOpcion("Piedra", "buena", "Picar en la veta gris que se ve prometedora");

            AgregarDescripcionOpcion("Piedra", "muyBuena", "Examinar el brillo peculiar de esa pared");
            AgregarDescripcionOpcion("Piedra", "muyBuena", "Seguir el consejo del viejo minero");
            AgregarDescripcionOpcion("Piedra", "muyBuena", "Investigar esos cristales que reflejan la luz");

            AgregarDescripcionOpcion("Plata", "muyMala", "Tocar ese mineral brillante con las manos desnudas");
            AgregarDescripcionOpcion("Plata", "muyMala", "Acercarte a esa extraña niebla en la cantera");
            AgregarDescripcionOpcion("Plata", "muyMala", "Excavar en esa zona marcada con símbolos de peligro");

            AgregarDescripcionOpcion("Plata", "mala", "Recoger minerales que brillan de forma opaca");
            AgregarDescripcionOpcion("Plata", "mala", "Extraer de la veta que parece muy superficial");
            AgregarDescripcionOpcion("Plata", "mala", "Buscar en las zonas ya trabajadas por otros");

            AgregarDescripcionOpcion("Plata", "buena", "Examinar la veta que brilla discretamente");
            AgregarDescripcionOpcion("Plata", "buena", "Picar en la pared que tiene vetas grises");
            AgregarDescripcionOpcion("Plata", "buena", "Buscar en la zona donde hay herramientas abandonadas");

            AgregarDescripcionOpcion("Plata", "muyBuena", "Seguir el arroyo hasta su origen mineral");
            AgregarDescripcionOpcion("Plata", "muyBuena", "Inspeccionar esa roca con vetas brillantes bajo la luz");
            AgregarDescripcionOpcion("Plata", "muyBuena", "Examinar el mapa antiguo que encontraste");

            AgregarDecision("Madera", "muyMala", "Intentas talar un árbol, pero este cae sobre ti");
            AgregarDecision("Madera", "muyMala", "Tu hacha se rompe y una astilla te hiere en el proceso");
            AgregarDecision("Madera", "muyMala", "Despiertas un nido de avispas al golpear un árbol");

            AgregarDecision("Madera", "mala", "El árbol que intentas talar está podrido por dentro");
            AgregarDecision("Madera", "mala", "Tu hacha rebota en la corteza sin conseguir nada");
            AgregarDecision("Madera", "mala", "Te distraes persiguiendo a una ardilla y pierdes tiempo");

            AgregarDecision("Madera", "buena", "Consigues talar una rama de buen tamaño");
            AgregarDecision("Madera", "buena", "Encuentras un tronco caído en buenas condiciones");
            AgregarDecision("Madera", "buena", "Un leñador local te regala un pedazo de madera");

            AgregarDecision("Madera", "muyBuena", "Encuentras un árbol perfecto y consigues mucha madera");
            AgregarDecision("Madera", "muyBuena", "Descubres un almacén abandonado con madera cortada");
            AgregarDecision("Madera", "muyBuena", "Tu técnica mejorada te permite obtener más de un solo árbol");

            AgregarDecision("Piedra", "muyMala", "Provocas un pequeño derrumbe que te golpea");
            AgregarDecision("Piedra", "muyMala", "Te cortas con una roca afilada al intentar extraerla");
            AgregarDecision("Piedra", "muyMala", "Inhalas polvo tóxico de la cueva");

            AgregarDecision("Piedra", "mala", "Las rocas son demasiado blandas y se deshacen al tocarlas");
            AgregarDecision("Piedra", "mala", "No logras extraer ninguna roca de utilidad");
            AgregarDecision("Piedra", "mala", "Tu pico rebota en la pared sin efecto");

            AgregarDecision("Piedra", "buena", "Extraes una roca de buen tamaño");
            AgregarDecision("Piedra", "buena", "Encuentras varias piedras ya extraídas");
            AgregarDecision("Piedra", "buena", "Descubres una pequeña veta fácil de extraer");

            AgregarDecision("Piedra", "muyBuena", "Encuentras un filón abundante y fácil de extraer");
            AgregarDecision("Piedra", "muyBuena", "Un derrumbe controlado te da acceso a muchas rocas");
            AgregarDecision("Piedra", "muyBuena", "Descubres rocas de alta calidad en la superficie");

            AgregarDecision("Plata", "muyMala", "Tocas plata sin refinar que resulta estar contaminada");
            AgregarDecision("Plata", "muyMala", "Te intoxicas con los gases de la fundición");
            AgregarDecision("Plata", "muyMala", "La cantera tiene trampas y caes en una de ellas");

            AgregarDecision("Plata", "mala", "La veta de plata estaba agotada");
            AgregarDecision("Plata", "mala", "El mineral que encontraste no es plata, sino pirita");
            AgregarDecision("Plata", "mala", "Tus herramientas no son lo suficientemente fuertes");

            AgregarDecision("Plata", "buena", "Encuentras una pequeña veta de plata");
            AgregarDecision("Plata", "buena", "Un minero te regala un poco de plata");
            AgregarDecision("Plata", "buena", "Logras extraer una cantidad decente de mineral");

            AgregarDecision("Plata", "muyBuena", "Descubres una rica veta de plata de alta calidad");
            AgregarDecision("Plata", "muyBuena", "Encuentras plata ya refinada y lista para usar");
            AgregarDecision("Plata", "muyBuena", "Un antiguo depósito minero tiene plata abandonada");
        }

        private void InicializarDecisiones(string tipoMaterial)
        {
            _decisiones[tipoMaterial] = new Dictionary<string, Queue<string>>
            {
                { "muyMala", new Queue<string>() },
                { "mala", new Queue<string>() },
                { "buena", new Queue<string>() },
                { "muyBuena", new Queue<string>() }
            };

            _descripcionesOpciones[tipoMaterial] = new Dictionary<string, Queue<string>>
            {
                { "muyMala", new Queue<string>() },
                { "mala", new Queue<string>() },
                { "buena", new Queue<string>() },
                { "muyBuena", new Queue<string>() }
            };
        }

        private void AgregarDecision(string tipoMaterial, string calidad, string descripcion)
        {
            _decisiones[tipoMaterial][calidad].Enqueue(descripcion);
        }

        private void AgregarDescripcionOpcion(string tipoMaterial, string calidad, string descripcion)
        {
            _descripcionesOpciones[tipoMaterial][calidad].Enqueue(descripcion);
        }

        public string ObtenerDecisionAleatoria(string tipoMaterial, string calidad)
        {
            if (!_decisiones.ContainsKey(tipoMaterial) || !_decisiones[tipoMaterial].ContainsKey(calidad))
            {
                throw new Exception($"No se encontró el tipo de material '{tipoMaterial}' o la calidad '{calidad}'");
            }

            Queue<string> cola = _decisiones[tipoMaterial][calidad];

            if (cola.Count == 0)
            {
                return $"Decisión genérica para {tipoMaterial} ({calidad})";
            }

            string decision = cola.Dequeue();
            cola.Enqueue(decision);

            return decision;
        }

        public string ObtenerDescripcionOpcionAleatoria(string tipoMaterial, string calidad)
        {
            if (!_descripcionesOpciones.ContainsKey(tipoMaterial) || !_descripcionesOpciones[tipoMaterial].ContainsKey(calidad))
            {
                throw new Exception($"No se encontró el tipo de material '{tipoMaterial}' o la calidad '{calidad}'");
            }

            Queue<string> cola = _descripcionesOpciones[tipoMaterial][calidad];

            if (cola.Count == 0)
            {
                return $"Opción genérica para {tipoMaterial} ({calidad})";
            }

            string descripcion = cola.Dequeue();
            cola.Enqueue(descripcion);

            return descripcion;
        }
    }

    public class Decisiones
    {
        private List<Decision> _opciones;
        private static PoolDecisiones _pool = new PoolDecisiones();
        private string _tipoMaterial;
        private static Random _random = new Random();

        public Decisiones(string tipoMaterial)
        {
            _tipoMaterial = tipoMaterial;
            _opciones = new List<Decision>();

            CrearDecision("muyMala", 0, -1);
            CrearDecision("mala", 0, 0);
            CrearDecision("buena", 1, 0);
            CrearDecision("muyBuena", 2, 0);

            _opciones = _opciones.OrderBy(x => _random.Next()).ToList();
        }

        private void CrearDecision(string calidad, int cantidadMaterial, int dañoVida)
        {
            string mensaje = _pool.ObtenerDecisionAleatoria(_tipoMaterial, calidad);
            string descripcionOpcion = _pool.ObtenerDescripcionOpcionAleatoria(_tipoMaterial, calidad);

            Resultado<int> resultado = new Resultado<int>(cantidadMaterial, dañoVida, mensaje);
            _opciones.Add(new Decision(descripcionOpcion, resultado));
        }

        public void MostrarOpciones()
        {
            Console.WriteLine("\nOPCIONES DE RECOLECCIÓN:");
            for (int i = 0; i < _opciones.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_opciones[i].Descripcion}");
            }
        }

        public Resultado<int> SeleccionarOpcion(int indice)
        {
            if (indice < 0 || indice >= _opciones.Count)
            {
                throw new Exception("Opción no válida");
            }

            return _opciones[indice].Resultado;
        }
    }
}