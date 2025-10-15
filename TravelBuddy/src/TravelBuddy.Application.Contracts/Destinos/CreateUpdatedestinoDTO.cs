using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;


namespace TravelBuddy.Destinos;
public class CreateUpdatedestinoDTO
{
    [Required]
    [StringLength(100)]
    public string Pais { get; set; }
    [Required]
    [StringLength(100)]
    public string Ciudad { get; set; }
    [StringLength(50)]
    public string Coordenadas { get; set; }
    [StringLength(200)]
    public string Foto { get; set; }

    public int Poblacion { get; set; }  
}       
    