using SistemaGestionBiblioteca.Services;
using SistemaGestionBiblioteca.UI;

namespace SistemaGestionBiblioteca
{
    /// <summary>
    /// Aplicación principal del Sistema de Gestión de Bibliotecas
    /// Proyecto final del curso - Implementa búsqueda, límite de préstamos y gestión de devoluciones
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Configurar la codificación para caracteres especiales
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                
                // Inicializar el servicio principal
                var bibliotecaService = new BibliotecaService();
                
                // Mostrar mensaje de bienvenida y ejecutar la aplicación
                Console.WriteLine("🚀 Iniciando Sistema de Gestión de Bibliotecas...");
                Console.WriteLine("   Desarrollado como proyecto final del curso");
                Console.WriteLine();
                
                // Pausa breve para mostrar el mensaje de inicio
                Thread.Sleep(1500);
                
                // Ejecutar el menú principal
                MenuUI.MostrarMenuPrincipal(bibliotecaService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error crítico en la aplicación: {ex.Message}");
                Console.WriteLine("📧 Por favor, reporte este error si persiste.");
                Console.WriteLine("\nPresione cualquier tecla para salir...");
                Console.ReadKey();
            }
        }
    }
}
