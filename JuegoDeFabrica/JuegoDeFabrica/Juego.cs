using System;
using System.Collections.Generic;
using System.Threading;

namespace JuegoDeFabrica
{
    public class Juego
    {
        private Jugador _jugador;
        private Mapa _mapa;
        private Armeria _armeria;
        private int _diasTranscurridos;
        private int _diasLimite;
        private bool _juegoTerminado;
        private Stack<string> _historialAcciones;

        public Juego(int diasLimite = 15)
        {
            _jugador = new Jugador();
            _mapa = new Mapa();
            _armeria = new Armeria();
            _diasTranscurridos = 1;
            _diasLimite = diasLimite;
            _juegoTerminado = false;
            _historialAcciones = new Stack<string>();
        }

        public void Iniciar()
        {
            MostrarIntroduccion();

            while (!_juegoTerminado)
            {
                MostrarInterfazPrincipal();
                ProcesarAccionPrincipal();
                VerificarFinJuego();
            }
        }

        private void MostrarIntroduccion()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("         JUEGO DE FÁBRICA DE ARMAS          ");
            Console.ResetColor();

            Console.WriteLine("\nBienvenido a tu fábrica de armas.");
            Console.WriteLine("Tu misión es fabricar una Espada, un Hacha y una Pistola");
            Console.WriteLine($"antes de que pasen {_diasLimite} días.");
            Console.WriteLine("\nDebes recolectar materiales visitando diferentes lugares,");
            Console.WriteLine("pero ten cuidado con tus decisiones, algunas pueden dañarte.");
            Console.WriteLine("\nPresiona Enter para comenzar...");
            Console.ReadLine();
        }

        private void MostrarInterfazPrincipal()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("               TU FÁBRICA                   ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nDía: {_diasTranscurridos}/{_diasLimite}");
            Console.ResetColor();

            _jugador.MostrarEstado();

            _armeria.MostrarArmas();

            if (_historialAcciones.Count > 0)
            {
                Console.WriteLine("\nÚLTIMA ACCIÓN");
                Console.WriteLine(_historialAcciones.Peek());
            }

            MostrarOpcionesPrincipales();
        }

        private void MostrarOpcionesPrincipales()
        {
            Console.WriteLine("\nACCIONES DISPONIBLES");

            List<Lugar> lugaresDisponibles = _mapa.ObtenerLugaresDisponibles();
            int opcion = 1;

            foreach (var lugar in lugaresDisponibles)
            {
                Console.WriteLine($"{opcion}. Ir a {lugar.Nombre}");
                opcion++;
            }

            Console.WriteLine($"{opcion}. Descansar hasta el día siguiente");

            List<Arma> armasFabricables = _armeria.ObtenerArmasFabricables(_jugador.Inventario);
            foreach (var arma in armasFabricables)
            {
                opcion++;
                Console.WriteLine($"{opcion}. Fabricar {arma.Nombre}");
            }

            Console.WriteLine("0. Salir del juego");
        }

        private void ProcesarAccionPrincipal()
        {
            Console.WriteLine("\nSelecciona una opción (número): ");
            string entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out int opcion))
            {
                RegistrarAccion("Opción no válida. Intenta de nuevo.");
                return;
            }

            if (opcion == 0)
            {
                ConfirmarSalida();
                return;
            }

            List<Lugar> lugaresDisponibles = _mapa.ObtenerLugaresDisponibles();

            if (opcion > 0 && opcion <= lugaresDisponibles.Count)
            {
                VisitarLugar(lugaresDisponibles[opcion - 1]);
                return;
            }

            if (opcion == lugaresDisponibles.Count + 1)
            {
                Descansar();
                return;
            }

            List<Arma> armasFabricables = _armeria.ObtenerArmasFabricables(_jugador.Inventario);
            int indiceArma = opcion - lugaresDisponibles.Count - 2;

            if (indiceArma >= 0 && indiceArma < armasFabricables.Count)
            {
                FabricarArma(armasFabricables[indiceArma]);
                return;
            }

            RegistrarAccion("Opción no válida. Intenta de nuevo.");
        }

        private void VisitarLugar(Lugar lugar)
        {
            lugar.Visitado = true;
            Console.Clear();
            lugar.MostrarDescripcion();

            RegistrarAccion($"Has visitado {lugar.Nombre}.");

            bool eligioOpcionValida = false;

            while (!eligioOpcionValida)
            {
                Console.WriteLine("\nOPCIONES");
                Console.WriteLine("1. Recolectar materiales");
                Console.WriteLine("2. Volver a la fábrica (No podras volver hasta el día siguiente)");

                Console.WriteLine("\nSelecciona una opción (número): ");
                string entrada = Console.ReadLine();

                if (!int.TryParse(entrada, out int opcion) || opcion < 1 || opcion > 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opción no válida. Por favor, elige 1 para recolectar o 2 para volver.");
                    Console.ResetColor();
                    Console.WriteLine("Presiona Enter para intentarlo de nuevo...");
                    Console.ReadLine();
                    Console.Clear();
                    lugar.MostrarDescripcion();
                }
                else
                {
                    eligioOpcionValida = true;

                    if (opcion == 1)
                    {
                        RecolectarMateriales(lugar);
                    }
                }
            }
        }

        private void RecolectarMateriales(Lugar lugar)
        {
            bool eligioOpcionValida = false;

            while (!eligioOpcionValida)
            {
                Decisiones decisiones = lugar.GenerarDecisiones();
                decisiones.MostrarOpciones();

                Console.WriteLine("\nSelecciona una opción (número): ");
                string entrada = Console.ReadLine();

                // Verificar si la entrada es válida
                if (!int.TryParse(entrada, out int opcion) || opcion < 1 || opcion > 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opción no válida. Por favor, elige un número del 1 al 4.");
                    Console.ResetColor();
                    Console.WriteLine("Presiona Enter para intentarlo de nuevo...");
                    Console.ReadLine();
                    Console.Clear();
                    lugar.MostrarDescripcion();
                }
                else
                {
                    eligioOpcionValida = true;

                    try
                    {
                        Resultado<int> resultado = decisiones.SeleccionarOpcion(opcion - 1);

                        Console.WriteLine($"\n{resultado.Mensaje}");

                        if (resultado.DañoVida > 0)
                        {
                            _jugador.RecibirDaño(resultado.DañoVida);
                        }

                        if (resultado.Recompensa > 0)
                        {
                            _jugador.RecibirMaterial(lugar.Nombre switch
                            {
                                "Bosque" => "Madera",
                                "Cantera" => "Plata",
                                "Cueva" => "Piedra",
                                _ => throw new Exception("Lugar desconocido")
                            }, resultado.Recompensa);
                        }
                        else if (resultado.Recompensa == 0 && resultado.DañoVida == 0)
                        {
                            Console.WriteLine("No obtuviste ningún material.");
                        }

                        RegistrarAccion($"Has recolectado en {lugar.Nombre}. {resultado.Mensaje}");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.ResetColor();
                    }
                }
            }

            Console.WriteLine("\nPresiona Enter para continuar...");
            Console.ReadLine();
        }

        private void Descansar()
        {
            _diasTranscurridos++;
            _jugador.Curar();
            _mapa.ReiniciarVisitas();

            RegistrarAccion("Has roncado hasta el día siguiente. Te sientes renovado, bello, potro y etéreo para continuar.");

            Console.WriteLine("\nPresiona Enter para continuar...");
            Console.ReadLine();
        }

        private void FabricarArma(Arma arma)
        {
            try
            {
                _armeria.FabricarArma(arma, _jugador.Inventario);
                RegistrarAccion($"Has fabricado {arma.Nombre}.");

                Console.WriteLine("\nPresiona Enter para continuar...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error al fabricar: {ex.Message}");
                Console.ResetColor();

                Console.WriteLine("\nPresiona Enter para continuar...");
                Console.ReadLine();
            }
        }

        private void VerificarFinJuego()
        {
            if (!_jugador.EstaVivo())
            {
                FinJuego(false, "Te has quedado sin puntos de vida, te desvivieron *snif snif*.");
                return;
            }

            if (_diasTranscurridos > _diasLimite)
            {
                FinJuego(false, $"Han pasado {_diasLimite} días y no has completado tu misión, ¿te vas para la trica?.");
                return;
            }

            if (_armeria.TodasArmasFabricadas())
            {
                FinJuego(true, "¡Has fabricado todas las armas! Tu fábrica es un éxito, no eres tan pulpin como creí.");
                return;
            }
        }

        private void FinJuego(bool victoria, string mensaje)
        {
            Console.Clear();

            if (victoria)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("¡VICTORIA!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("DERROTA");
            }

            Console.WriteLine("\n" + mensaje);
            Console.ResetColor();

            Console.WriteLine($"\nHas jugado durante {_diasTranscurridos} días.");
            _jugador.MostrarEstado();

            Console.WriteLine("\nPresiona Enter para salir...");
            Console.ReadLine();

            _juegoTerminado = true;
        }

        private void ConfirmarSalida()
        {
            Console.WriteLine("\n¿Estás seguro de que quieres salir del juego? (S/N)");
            string respuesta = Console.ReadLine().Trim().ToUpper();

            if (respuesta == "S")
            {
                _juegoTerminado = true;
                Console.WriteLine("\nGracias por jugar. ¡Hasta pronto!");
                Thread.Sleep(1500);
            }
        }

        private void RegistrarAccion(string accion)
        {
            _historialAcciones.Push(accion);
        }
    }
}