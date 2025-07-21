namespace SistemaGestionBiblioteca.Models
{
    /// <summary>
    /// Representa un libro en el sistema de biblioteca
    /// </summary>
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public bool EstaPrestado { get; set; } = false;
        public string? UsuarioPrestamista { get; set; }
        public DateTime? FechaPrestamo { get; set; }

        public Libro() { }

        public Libro(int id, string titulo, string autor, string isbn)
        {
            Id = id;
            Titulo = titulo;
            Autor = autor;
            ISBN = isbn;
        }

        public override string ToString()
        {
            var estado = EstaPrestado ? $"PRESTADO a {UsuarioPrestamista}" : "DISPONIBLE";
            return $"ID: {Id} | {Titulo} por {Autor} | ISBN: {ISBN} | Estado: {estado}";
        }
    }
}
