using System.ComponentModel.DataAnnotations;

namespace Blog.Models;
public class Category
{
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O slug é necessário")]
    public string Slug { get; set; }
        
    public IList<Post> Posts { get; set; }
}
