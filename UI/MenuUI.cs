using SistemaGestionBiblioteca.Services;
using SistemaGestionBiblioteca.Tests;

namespace SistemaGestionBiblioteca.UI
{
    /// <summary>
    /// Clase que maneja la interfaz de usuario y la interacción con el usuario
    /// </summary>
    public static class MenuUI
    {
        /// <summary>
        /// Muestra el menú principal y maneja la navegación
        /// </summary>
        public static void MostrarMenuPrincipal(BibliotecaService bibliotecaService)
        {
            bool continuar = true;
            
            Console.WriteLine("🏛️  BIENVENIDO AL SISTEMA DE GESTIÓN DE BIBLIOTECAS");
            Console.WriteLine("=" + new string('=', 60));
            
            while (continuar)
            {
                try
                {
                    MostrarOpciones();
                    
                    Console.Write("\n➤ Seleccione una opción (1-10): ");
                    string? entrada = Console.ReadLine()?.Trim();
                    
                    if (string.IsNullOrEmpty(entrada))
                    {
                        Console.WriteLine("⚠️  Por favor, ingrese una opción válida.");
                        EsperarTecla();
                        continue;
                    }

                    Console.WriteLine(); // Línea en blanco para mejor legibilidad

                    switch (entrada)
                    {
                        case "1":
                            BuscarLibro(bibliotecaService);
                            break;
                        case "2":
                            PrestarLibro(bibliotecaService);
                            break;
                        case "3":
                            DevolverLibro(bibliotecaService);
                            break;
                        case "4":
                            AgregarLibro(bibliotecaService);
                            break;
                        case "5":
                            bibliotecaService.MostrarTodosLosLibros();
                            break;
                        case "6":
                            bibliotecaService.MostrarUsuarios();
                            break;
                        case "7":
                            bibliotecaService.MostrarEstadisticas();
                            break;
                        case "8":
                            MostrarAyuda();
                            break;
                        case "9":
                            EjecutarPruebas();
                            break;
                        case "10":
                            Console.WriteLine("👋 ¡Gracias por usar el Sistema de Gestión de Bibliotecas!");
                            Console.WriteLine("   Desarrollado como proyecto final del curso.");
                            continuar = false;
                            continue;
                        default:
                            Console.WriteLine("⚠️  Opción no válida. Por favor, seleccione una opción del 1 al 10.");
                            break;
                    }

                    if (continuar)
                    {
                        EsperarTecla();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"✗ Error inesperado: {ex.Message}");
                    Console.WriteLine("  Por favor, intente nuevamente.");
                    EsperarTecla();
                }
            }
        }

        private static void MostrarOpciones()
        {
            Console.Clear();
            Console.WriteLine("🏛️  SISTEMA DE GESTIÓN DE BIBLIOTECAS");
            Console.WriteLine("=" + new string('=', 50));
            Console.WriteLine();
            Console.WriteLine("📋 OPCIONES DISPONIBLES:");
            Console.WriteLine("  1️⃣  Buscar libro por título");
            Console.WriteLine("  2️⃣  Prestar libro");
            Console.WriteLine("  3️⃣  Devolver libro");
            Console.WriteLine("  4️⃣  Agregar nuevo libro");
            Console.WriteLine("  5️⃣  Ver todos los libros");
            Console.WriteLine("  6️⃣  Ver usuarios y sus préstamos");
            Console.WriteLine("  7️⃣  Ver estadísticas del sistema");
            Console.WriteLine("  8️⃣  Ayuda e información");
            Console.WriteLine("  9️⃣  Ejecutar pruebas automáticas");
            Console.WriteLine("  🔟  Salir");
        }

        private static void BuscarLibro(BibliotecaService bibliotecaService)
        {
            Console.WriteLine("🔍 BUSCAR LIBRO POR TÍTULO");
            Console.WriteLine(new string('-', 30));
            
            Console.Write("Ingrese el título del libro (o parte de él): ");
            string? titulo = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(titulo))
            {
                Console.WriteLine("⚠️  El título no puede estar vacío.");
                return;
            }

            var librosEncontrados = bibliotecaService.BuscarLibrosPorTitulo(titulo);
            
            if (!librosEncontrados.Any())
            {
                Console.WriteLine($"❌ No se encontraron libros que contengan '{titulo}' en el título.");
                Console.WriteLine("💡 Sugerencia: Intente con una búsqueda más general o verifique la ortografía.");
                return;
            }

            Console.WriteLine($"\n✅ Se encontraron {librosEncontrados.Count} libro(s) que contienen '{titulo}':");
            Console.WriteLine(new string('-', 80));
            
            foreach (var libro in librosEncontrados.OrderBy(l => l.Titulo))
            {
                Console.WriteLine($"  {libro}");
            }
        }

        private static void PrestarLibro(BibliotecaService bibliotecaService)
        {
            Console.WriteLine("📚 PRESTAR LIBRO");
            Console.WriteLine(new string('-', 20));
            
            Console.Write("Ingrese el ID del libro: ");
            string? idTexto = Console.ReadLine()?.Trim();
            
            if (!int.TryParse(idTexto, out int id))
            {
                Console.WriteLine("⚠️  Por favor, ingrese un ID válido (número entero).");
                return;
            }

            Console.Write("Ingrese el nombre del usuario: ");
            string? usuario = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(usuario))
            {
                Console.WriteLine("⚠️  El nombre de usuario no puede estar vacío.");
                return;
            }

            bibliotecaService.PrestarLibro(id, usuario);
        }

        private static void DevolverLibro(BibliotecaService bibliotecaService)
        {
            Console.WriteLine("📖 DEVOLVER LIBRO");
            Console.WriteLine(new string('-', 20));
            
            Console.Write("Ingrese el ID del libro: ");
            string? idTexto = Console.ReadLine()?.Trim();
            
            if (!int.TryParse(idTexto, out int id))
            {
                Console.WriteLine("⚠️  Por favor, ingrese un ID válido (número entero).");
                return;
            }

            Console.Write("Ingrese el nombre del usuario: ");
            string? usuario = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(usuario))
            {
                Console.WriteLine("⚠️  El nombre de usuario no puede estar vacío.");
                return;
            }

            bibliotecaService.DevolverLibro(id, usuario);
        }

        private static void AgregarLibro(BibliotecaService bibliotecaService)
        {
            Console.WriteLine("➕ AGREGAR NUEVO LIBRO");
            Console.WriteLine(new string('-', 25));
            
            Console.Write("Ingrese el título del libro: ");
            string? titulo = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(titulo))
            {
                Console.WriteLine("⚠️  El título no puede estar vacío.");
                return;
            }

            Console.Write("Ingrese el autor del libro: ");
            string? autor = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(autor))
            {
                Console.WriteLine("⚠️  El autor no puede estar vacío.");
                return;
            }

            Console.Write("Ingrese el ISBN (opcional): ");
            string? isbn = Console.ReadLine()?.Trim() ?? string.Empty;

            bibliotecaService.AgregarLibro(titulo, autor, isbn);
        }

        private static void MostrarAyuda()
        {
            Console.WriteLine("❓ AYUDA E INFORMACIÓN DEL SISTEMA");
            Console.WriteLine(new string('=', 45));
            Console.WriteLine();
            Console.WriteLine("📖 FUNCIONALIDADES PRINCIPALES:");
            Console.WriteLine("  • Búsqueda de libros por título (búsqueda parcial)");
            Console.WriteLine("  • Préstamo de libros con límite de 3 por usuario");
            Console.WriteLine("  • Devolución de libros prestados");
            Console.WriteLine("  • Gestión de usuarios automática");
            Console.WriteLine("  • Seguimiento del estado de préstamos");
            Console.WriteLine();
            Console.WriteLine("🔍 BÚSQUEDA:");
            Console.WriteLine("  • Puede buscar por título completo o parcial");
            Console.WriteLine("  • La búsqueda no distingue mayúsculas/minúsculas");
            Console.WriteLine("  • Ejemplo: buscar 'quijote' encontrará 'El Quijote'");
            Console.WriteLine();
            Console.WriteLine("📚 PRÉSTAMOS:");
            Console.WriteLine("  • Cada usuario puede tener máximo 3 libros prestados");
            Console.WriteLine("  • Los usuarios se crean automáticamente al primer préstamo");
            Console.WriteLine("  • Un libro prestado no puede prestarse a otro usuario");
            Console.WriteLine();
            Console.WriteLine("📖 DEVOLUCIONES:");
            Console.WriteLine("  • Solo el usuario que prestó el libro puede devolverlo");
            Console.WriteLine("  • Al devolver, el libro queda disponible para otros usuarios");
            Console.WriteLine();
            Console.WriteLine("💡 CONSEJOS:");
            Console.WriteLine("  • Use la opción 5 para ver todos los libros y sus IDs");
            Console.WriteLine("  • Use la opción 6 para ver qué libros tiene cada usuario");
            Console.WriteLine("  • Use la opción 7 para ver estadísticas del sistema");
            Console.WriteLine();
            Console.WriteLine("👨‍💻 Proyecto desarrollado como evaluación final del curso.");
        }

        private static void EjecutarPruebas()
        {
            Console.WriteLine("🧪 EJECUTANDO PRUEBAS AUTOMÁTICAS");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("Esta opción ejecutará pruebas para validar:");
            Console.WriteLine("✓ Funcionalidad de búsqueda");
            Console.WriteLine("✓ Límite de 3 préstamos por usuario");
            Console.WriteLine("✓ Sistema de devoluciones");
            Console.WriteLine("✓ Validaciones de errores");
            Console.WriteLine();
            Console.Write("¿Desea continuar? (S/N): ");
            
            string? respuesta = Console.ReadLine()?.Trim().ToUpper();
            if (respuesta == "S" || respuesta == "SI" || respuesta == "Y" || respuesta == "YES")
            {
                Console.WriteLine();
                PruebasSistema.EjecutarPruebas();
            }
            else
            {
                Console.WriteLine("Pruebas canceladas.");
            }
        }

        private static void EsperarTecla()
        {
            Console.WriteLine("\n⏸️  Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }
    }
}
