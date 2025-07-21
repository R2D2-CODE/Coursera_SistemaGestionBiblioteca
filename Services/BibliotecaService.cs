using SistemaGestionBiblioteca.Models;

namespace SistemaGestionBiblioteca.Services
{
    /// <summary>
    /// Servicio principal para la gestión de la biblioteca
    /// Implementa las operaciones CRUD y reglas de negocio
    /// </summary>
    public class BibliotecaService
    {
        private readonly List<Libro> _libros;
        private readonly Dictionary<string, Usuario> _usuarios;
        private int _siguienteId = 1;

        public BibliotecaService()
        {
            _libros = new List<Libro>();
            _usuarios = new Dictionary<string, Usuario>(StringComparer.OrdinalIgnoreCase);
            InicializarDatosPrueba();
        }

        /// <summary>
        /// Inicializa datos de prueba para demostrar funcionalidad
        /// </summary>
        private void InicializarDatosPrueba()
        {
            AgregarLibro("El Quijote", "Miguel de Cervantes", "978-84-376-0494-7");
            AgregarLibro("Cien años de soledad", "Gabriel García Márquez", "978-84-376-0495-4");
            AgregarLibro("1984", "George Orwell", "978-84-376-0496-1");
            AgregarLibro("El Principito", "Antoine de Saint-Exupéry", "978-84-376-0497-8");
            AgregarLibro("To Kill a Mockingbird", "Harper Lee", "978-84-376-0498-5");
            AgregarLibro("The Great Gatsby", "F. Scott Fitzgerald", "978-84-376-0499-2");
            AgregarLibro("Pride and Prejudice", "Jane Austen", "978-84-376-0500-5");
            AgregarLibro("The Catcher in the Rye", "J.D. Salinger", "978-84-376-0501-2");
        }

        /// <summary>
        /// Agrega un nuevo libro a la colección
        /// </summary>
        public void AgregarLibro(string titulo, string autor, string isbn)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(autor))
                {
                    throw new ArgumentException("El título y autor son obligatorios");
                }

                var libro = new Libro(_siguienteId++, titulo.Trim(), autor.Trim(), isbn.Trim());
                _libros.Add(libro);
                Console.WriteLine($"✓ Libro agregado exitosamente: {titulo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error al agregar libro: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca libros por título (búsqueda parcial)
        /// </summary>
        public List<Libro> BuscarLibrosPorTitulo(string titulo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo))
                {
                    return new List<Libro>();
                }

                return _libros.Where(libro => 
                    libro.Titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error en la búsqueda: {ex.Message}");
                return new List<Libro>();
            }
        }

        /// <summary>
        /// Obtiene o crea un usuario
        /// </summary>
        private Usuario ObtenerOCrearUsuario(string nombreUsuario)
        {
            if (!_usuarios.TryGetValue(nombreUsuario, out Usuario? usuario))
            {
                usuario = new Usuario(nombreUsuario);
                _usuarios[nombreUsuario] = usuario;
                Console.WriteLine($"➤ Nuevo usuario registrado: {nombreUsuario}");
            }
            return usuario;
        }

        /// <summary>
        /// Presta un libro a un usuario con validaciones de límite
        /// </summary>
        public bool PrestarLibro(int idLibro, string nombreUsuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreUsuario))
                {
                    Console.WriteLine("✗ El nombre de usuario es obligatorio");
                    return false;
                }

                var libro = _libros.FirstOrDefault(l => l.Id == idLibro);
                if (libro == null)
                {
                    Console.WriteLine($"✗ Libro con ID {idLibro} no encontrado");
                    return false;
                }

                if (libro.EstaPrestado)
                {
                    Console.WriteLine($"✗ El libro '{libro.Titulo}' ya está prestado a {libro.UsuarioPrestamista}");
                    return false;
                }

                var usuario = ObtenerOCrearUsuario(nombreUsuario);
                
                if (!usuario.PuedePrestarMasLibros())
                {
                    Console.WriteLine($"✗ {nombreUsuario} ha alcanzado el límite de préstamos ({Usuario.LIMITE_PRESTAMOS} libros)");
                    Console.WriteLine($"  Libros prestados actualmente: {usuario.LibrosPrestados.Count}");
                    return false;
                }

                // Realizar el préstamo
                libro.EstaPrestado = true;
                libro.UsuarioPrestamista = nombreUsuario;
                libro.FechaPrestamo = DateTime.Now;
                usuario.LibrosPrestados.Add(idLibro);

                Console.WriteLine($"✓ Libro '{libro.Titulo}' prestado exitosamente a {nombreUsuario}");
                Console.WriteLine($"  Libros disponibles para {nombreUsuario}: {usuario.LibrosDisponiblesParaPrestamo()}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error al prestar libro: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Devuelve un libro prestado
        /// </summary>
        public bool DevolverLibro(int idLibro, string nombreUsuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreUsuario))
                {
                    Console.WriteLine("✗ El nombre de usuario es obligatorio");
                    return false;
                }

                var libro = _libros.FirstOrDefault(l => l.Id == idLibro);
                if (libro == null)
                {
                    Console.WriteLine($"✗ Libro con ID {idLibro} no encontrado");
                    return false;
                }

                if (!libro.EstaPrestado)
                {
                    Console.WriteLine($"✗ El libro '{libro.Titulo}' no está prestado");
                    return false;
                }

                if (!string.Equals(libro.UsuarioPrestamista, nombreUsuario, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"✗ El libro '{libro.Titulo}' está prestado a {libro.UsuarioPrestamista}, no a {nombreUsuario}");
                    return false;
                }

                // Realizar la devolución
                libro.EstaPrestado = false;
                libro.UsuarioPrestamista = null;
                libro.FechaPrestamo = null;

                if (_usuarios.TryGetValue(nombreUsuario, out Usuario? usuario))
                {
                    usuario.LibrosPrestados.Remove(idLibro);
                    Console.WriteLine($"✓ Libro '{libro.Titulo}' devuelto exitosamente por {nombreUsuario}");
                    Console.WriteLine($"  Libros disponibles para {nombreUsuario}: {usuario.LibrosDisponiblesParaPrestamo()}");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error al devolver libro: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Muestra todos los libros en la colección
        /// </summary>
        public void MostrarTodosLosLibros()
        {
            try
            {
                Console.WriteLine("\n📚 COLECCIÓN COMPLETA DE LIBROS:");
                Console.WriteLine(new string('=', 80));

                if (!_libros.Any())
                {
                    Console.WriteLine("No hay libros en la colección.");
                    return;
                }

                var librosDisponibles = _libros.Where(l => !l.EstaPrestado).ToList();
                var librosPrestados = _libros.Where(l => l.EstaPrestado).ToList();

                Console.WriteLine($"\n📖 DISPONIBLES ({librosDisponibles.Count}):");
                foreach (var libro in librosDisponibles.OrderBy(l => l.Titulo))
                {
                    Console.WriteLine($"  {libro}");
                }

                Console.WriteLine($"\n📚 PRESTADOS ({librosPrestados.Count}):");
                foreach (var libro in librosPrestados.OrderBy(l => l.Titulo))
                {
                    Console.WriteLine($"  {libro}");
                }

                Console.WriteLine($"\nTotal de libros: {_libros.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error al mostrar libros: {ex.Message}");
            }
        }

        /// <summary>
        /// Muestra información de todos los usuarios registrados
        /// </summary>
        public void MostrarUsuarios()
        {
            try
            {
                Console.WriteLine("\n👥 USUARIOS REGISTRADOS:");
                Console.WriteLine(new string('=', 50));

                if (!_usuarios.Any())
                {
                    Console.WriteLine("No hay usuarios registrados.");
                    return;
                }

                foreach (var kvp in _usuarios.OrderBy(u => u.Key))
                {
                    var usuario = kvp.Value;
                    Console.WriteLine($"  {usuario}");
                    
                    if (usuario.LibrosPrestados.Any())
                    {
                        Console.WriteLine("    Libros prestados:");
                        foreach (var idLibro in usuario.LibrosPrestados)
                        {
                            var libro = _libros.FirstOrDefault(l => l.Id == idLibro);
                            if (libro != null)
                            {
                                Console.WriteLine($"      - {libro.Titulo} (ID: {libro.Id})");
                            }
                        }
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error al mostrar usuarios: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene estadísticas del sistema
        /// </summary>
        public void MostrarEstadisticas()
        {
            try
            {
                var totalLibros = _libros.Count;
                var librosDisponibles = _libros.Count(l => !l.EstaPrestado);
                var librosPrestados = _libros.Count(l => l.EstaPrestado);
                var totalUsuarios = _usuarios.Count;

                Console.WriteLine("\n📊 ESTADÍSTICAS DEL SISTEMA:");
                Console.WriteLine(new string('=', 40));
                Console.WriteLine($"Total de libros: {totalLibros}");
                Console.WriteLine($"Libros disponibles: {librosDisponibles}");
                Console.WriteLine($"Libros prestados: {librosPrestados}");
                Console.WriteLine($"Usuarios registrados: {totalUsuarios}");
                
                if (totalLibros > 0)
                {
                    var porcentajeOcupacion = (double)librosPrestados / totalLibros * 100;
                    Console.WriteLine($"Tasa de ocupación: {porcentajeOcupacion:F1}%");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error al mostrar estadísticas: {ex.Message}");
            }
        }
    }
}
