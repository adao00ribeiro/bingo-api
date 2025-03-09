using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace bingo_api.src.DTOs.Request;

public record RoundBulkRequestDto
{
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da cartela deve ser maior que zero.")]
        public decimal CardValue { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateOnly StartedDate { get; set; }

        [Required(ErrorMessage = "A data final é obrigatória.")]
        public DateOnly FinishedDate { get; set; }

        [Required(ErrorMessage = "A Horario inicial é obrigatória.")]
         [DefaultValue("07:00")]
        public TimeOnly StartedTime { get; set; }

        [Required(ErrorMessage = "A Horario Final é obrigatória.")]
        [DefaultValue("23:00")]
        public TimeOnly FinishedTime { get; set; } 
        
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("4")]
        public int TimeBetweenBalls { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("10")]
        public int TimeBetweenRounds { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("90")]
        public int MaxBalls { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("3")]
        public int CardRows { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("5")]
        public int CardColumns { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DefaultValue("ea66ba0c-da9c-45f1-8d73-b08b137195f6")]
        public Guid RoomId { get; set; }
        public IEnumerable<PrizeRequestDto>? Prizes { get; set; }
}
