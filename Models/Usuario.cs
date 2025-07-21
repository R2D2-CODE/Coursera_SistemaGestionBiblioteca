namespace SistemaGestionBiblioteca.Models
{
    /// <summary>
    /// Representa un usuario del sistema de biblioteca
    /// </summary>
    public class Usuario
    {
        public string Nombre { get; set; } = string.Empty;
        public List<int> LibrosPrestados { get; set; } = new List<int>();
        public const int LIMITE_PRESTAMOS = 3;

        public Usuario(string nombre)
        {
            Nombre = nombre;
        }

        /// <summary>
        /// Verifica si el usuario puede tomar más libros prestados
        /// </summary>
        public bool PuedePrestarMasLibros()
        {
            return LibrosPrestados.Count < LIMITE_PRESTAMOS;
        }

        /// <summary>
        /// Obtiene la cantidad de libros disponibles para préstamo
        /// </summary>
        public int LibrosDisponiblesParaPrestamo()
        {
            return LIMITE_PRESTAMOS - LibrosPrestados.Count;
        }

        public override string ToString()
        {
            return $"{Nombre} - Libros prestados: {LibrosPrestados.Count}/{LIMITE_PRESTAMOS}";
        }
    }
}
