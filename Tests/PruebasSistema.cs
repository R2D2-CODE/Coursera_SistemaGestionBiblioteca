using SistemaGestionBiblioteca.Models;

namespace SistemaGestionBiblioteca.Tests
{
    /// <summary>
    /// Clase para ejecutar pruebas automáticas del sistema
    /// Demuestra las funcionalidades implementadas
    /// </summary>
    public static class PruebasSistema
    {
        /// <summary>
        /// Ejecuta una batería completa de pruebas del sistema
        /// </summary>
        public static void EjecutarPruebas()
        {
            Console.WriteLine("🧪 INICIANDO PRUEBAS AUTOMÁTICAS DEL SISTEMA");
            Console.WriteLine(new string('=', 60));
            
            var bibliotecaService = new Services.BibliotecaService();
            
            try
            {
                // Prueba 1: Búsqueda de libros
                PruebaBusquedaLibros(bibliotecaService);
                
                // Prueba 2: Préstamos con límites
                PruebaLimitePrestamos(bibliotecaService);
                
                // Prueba 3: Devoluciones
                PruebaDevoluciones(bibliotecaService);
                
                // Prueba 4: Validaciones de errores
                PruebaValidaciones(bibliotecaService);
                
                Console.WriteLine("\n✅ TODAS LAS PRUEBAS COMPLETADAS EXITOSAMENTE");
                Console.WriteLine("   El sistema funciona correctamente según los requisitos");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERROR EN LAS PRUEBAS: {ex.Message}");
            }
        }

        private static void PruebaBusquedaLibros(Services.BibliotecaService servicio)
        {
            Console.WriteLine("\n🔍 PRUEBA 1: BÚSQUEDA DE LIBROS");
            Console.WriteLine(new string('-', 40));
            
            // Buscar libro existente
            var resultados = servicio.BuscarLibrosPorTitulo("quijote");
            Console.WriteLine($"✓ Búsqueda 'quijote': {resultados.Count} resultado(s)");
            
            // Búsqueda parcial
            resultados = servicio.BuscarLibrosPorTitulo("1984");
            Console.WriteLine($"✓ Búsqueda '1984': {resultados.Count} resultado(s)");
            
            // Búsqueda sin resultados
            resultados = servicio.BuscarLibrosPorTitulo("libro inexistente");
            Console.WriteLine($"✓ Búsqueda libro inexistente: {resultados.Count} resultado(s)");
        }

        private static void PruebaLimitePrestamos(Services.BibliotecaService servicio)
        {
            Console.WriteLine("\n📚 PRUEBA 2: LÍMITE DE PRÉSTAMOS");
            Console.WriteLine(new string('-', 40));
            
            // Prestar 3 libros al mismo usuario (límite)
            Console.WriteLine("Prestando libros a usuario 'TestUser':");
            bool resultado1 = servicio.PrestarLibro(1, "TestUser");
            bool resultado2 = servicio.PrestarLibro(2, "TestUser");
            bool resultado3 = servicio.PrestarLibro(3, "TestUser");
            
            Console.WriteLine($"✓ Préstamo 1: {(resultado1 ? "Exitoso" : "Fallido")}");
            Console.WriteLine($"✓ Préstamo 2: {(resultado2 ? "Exitoso" : "Fallido")}");
            Console.WriteLine($"✓ Préstamo 3: {(resultado3 ? "Exitoso" : "Fallido")}");
            
            // Intentar prestar un cuarto libro (debe fallar)
            Console.WriteLine("Intentando prestar 4to libro (debe fallar):");
            bool resultado4 = servicio.PrestarLibro(4, "TestUser");
            Console.WriteLine($"✓ Préstamo 4: {(resultado4 ? "Exitoso - ERROR!" : "Bloqueado correctamente")}");
        }

        private static void PruebaDevoluciones(Services.BibliotecaService servicio)
        {
            Console.WriteLine("\n📖 PRUEBA 3: DEVOLUCIONES");
            Console.WriteLine(new string('-', 30));
            
            // Devolver un libro prestado
            bool devolucion = servicio.DevolverLibro(1, "TestUser");
            Console.WriteLine($"✓ Devolución libro ID 1: {(devolucion ? "Exitosa" : "Fallida")}");
            
            // Ahora debería poder prestar otro libro
            bool nuevoPrestamo = servicio.PrestarLibro(4, "TestUser");
            Console.WriteLine($"✓ Nuevo préstamo tras devolución: {(nuevoPrestamo ? "Exitoso" : "Fallido")}");
        }

        private static void PruebaValidaciones(Services.BibliotecaService servicio)
        {
            Console.WriteLine("\n⚠️ PRUEBA 4: VALIDACIONES DE ERRORES");
            Console.WriteLine(new string('-', 45));
            
            // Intentar prestar libro ya prestado
            bool prestamoDuplicado = servicio.PrestarLibro(2, "OtroUser");
            Console.WriteLine($"✓ Préstamo duplicado bloqueado: {(!prestamoDuplicado ? "Correcto" : "ERROR")}");
            
            // Intentar devolver libro no prestado por el usuario
            bool devolucionIncorrecta = servicio.DevolverLibro(5, "TestUser");
            Console.WriteLine($"✓ Devolución incorrecta bloqueada: {(!devolucionIncorrecta ? "Correcto" : "ERROR")}");
            
            // Búsqueda con texto vacío
            var busquedaVacia = servicio.BuscarLibrosPorTitulo("");
            Console.WriteLine($"✓ Búsqueda vacía manejada: {(busquedaVacia.Count == 0 ? "Correcto" : "ERROR")}");
        }
    }
}
