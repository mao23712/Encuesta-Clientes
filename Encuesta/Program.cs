using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Rango
{
    public string CodigoPais { get; set; }
    public int EdadMinima { get; set; }
    public int EdadMaxima { get; set; }
    public string CategoriaEdad { get; set; }

}

class Paises
{
    private List<Rango> paises = new List<Rango>();

    public void agregarPais(Rango pais)
    {
        paises.Add(pais);
    }
    public string ObtenerPais(string pais, List<Rango> paises)
    {
        foreach (var paisCliente in paises)
        {
            if (string.Compare(pais.Substring(0, 3), paisCliente.CodigoPais.Substring(0, 3), StringComparison.OrdinalIgnoreCase) == 0)
            {
                return paisCliente.CodigoPais;
            }
        }
        return "";
    }

}
class CategoriaDeEdad
{
    private List<Rango> rangosDeEdad = new List<Rango>();

    public void AgregarRango(Rango rango)
    {
        rangosDeEdad.Add(rango);
    }

    public string ObtenerCategoriaDeEdad(int edad, List<Rango> rangosDeEdad)
    {
        foreach (var categoria in rangosDeEdad)
        {
            if (edad >= categoria.EdadMinima && edad <= categoria.EdadMaxima)
            {
                return categoria.CategoriaEdad;
            }
        }
        return "";
    }

}
class Cliente
{
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Pais { get; set; }
    public string Preferencia { get; set; }
    public DateTime Fecha { get; set; }
}
class Program
{
    static void Main(string[] args)
    {
        // Leer los datos del archivo Rango.txt
        var rangosDeEdad = new List<Rango>();
        using (var reader = new StreamReader("Rango.txt"))
        {
            while (!reader.EndOfStream)
            {
                var linea = reader.ReadLine();
                var partes = linea.Split(',');
                //agrego los datos ami lista rango
                rangosDeEdad.Add(new Rango
                {
                    CodigoPais = partes[0],
                    EdadMinima = int.Parse(partes[1]),
                    EdadMaxima = int.Parse(partes[2]),
                    CategoriaEdad = partes[3]
                });
            }
        }
        //Crear la categoria de paises
        var paises = new Paises();
        foreach(var pais in  rangosDeEdad)
        {
            paises.agregarPais(pais);
        }

        // Crear la categoría de edad
        var categoriaDeEdad = new CategoriaDeEdad();
        foreach (var rango in rangosDeEdad)
        {
            categoriaDeEdad.AgregarRango(rango);
        }

        // Leer los datos del archivo Cliente.txt y determinar la categoría de edad
        var clientes = new List<Cliente>();
        using (var reader = new StreamReader("Cliente.txt"))
        {
            while (!reader.EndOfStream)
            {
                var linea = reader.ReadLine();
                var partes = linea.Split(',');
                var cliente = new Cliente
                {
                    Nombre = partes[0],
                    Edad = int.Parse(partes[1]),
                    Pais = partes[2],
                    Preferencia = partes[3],
                    Fecha = DateTime.ParseExact(partes[4], "dd/MM/yyyy", null)
                };
                clientes.Add(cliente);
            }
        }

        // Mostrar el menú
        var opcion = 0;
        while (opcion != 4)
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("--                MENU              --");
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Mostrar lista de clientes");
            Console.WriteLine("2. Mostrar lista de rangos de edad");
            Console.WriteLine("3. Mostrar clientes jóvenes");
            Console.WriteLine("4. Salir");
            Console.WriteLine("--                                  --");
            Console.WriteLine("--------------------------------------");
            int.TryParse(Console.ReadLine(), out opcion);

            switch (opcion)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("Lista de Clientes");
                    foreach (var cliente in clientes)
                    {
                        var categoria = categoriaDeEdad.ObtenerCategoriaDeEdad(cliente.Edad, rangosDeEdad);
                        var paisCliente = paises.ObtenerPais(cliente.Pais, rangosDeEdad);
                        Console.WriteLine($"{cliente.Nombre} - {cliente.Edad} años - {paisCliente} - {cliente.Preferencia} - {categoria}");
                    }
                    Console.ReadLine();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Lista de Rangos");
                    foreach (var rango in rangosDeEdad)
                    {
                        Console.WriteLine($"{rango.CodigoPais} - {rango.EdadMinima} a {rango.EdadMaxima} años");
                    }
                    Console.ReadLine();
                    break;
                case 3:
                    Console.Clear();
                     
                    var cantidadJovenes = clientes.Where(c => categoriaDeEdad.ObtenerCategoriaDeEdad(c.Edad, rangosDeEdad) == "JOVEN").Count();
                    var clientesJovenes = clientes.FindAll(c => categoriaDeEdad.ObtenerCategoriaDeEdad(c.Edad, rangosDeEdad) == "JOVEN");
                    Console.WriteLine($"Cantidad de Clientes Jovenes: {cantidadJovenes}");
                    foreach (var cliente in clientesJovenes)
                    {
                        Console.WriteLine($"{cliente.Nombre} - {cliente.Edad} años - {cliente.Pais} - {cliente.Preferencia}");
                    }
                    Console.ReadLine();
                    break;
                case 4:
                    Console.WriteLine("Saliendo del programa...");
                    break;
                default:
                    Console.WriteLine("Opción no válida");
                    break;
            }
        }
    }
}
