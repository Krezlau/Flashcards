﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class DeckActivity
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Day { get; set; }

        [Required]
        public int ReviewedFlashcardsCount { get; set; }

        [Required]
        public int MinutesSpentLearning { get; set; }

        [Required]
        [ForeignKey("Deck")]
        public int DeckId { get; set; }

        public virtual Deck Deck { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null || !this.GetType().Equals(obj.GetType())) return false;

            DeckActivity da = (DeckActivity)obj;

            return da.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
