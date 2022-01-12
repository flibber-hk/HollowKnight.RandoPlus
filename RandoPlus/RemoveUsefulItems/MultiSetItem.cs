using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ItemChanger;
using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerCore.StringLogic;
using RandomizerMod.RC;
using RandomizerMod.Settings;

namespace RandoPlus.RemoveUsefulItems
{
    /// <summary>
    /// Logic item that increments each term by 1, if it isn't at least 1 already
    /// </summary>
    /// <param name="Name">The name of the item.</param>
    /// <param name="Terms">The terms to increment.</param>
    public sealed record MultiSetItem(string Name, Term[] Terms) : LogicItem(Name)
    {
        public override void AddTo(ProgressionManager pm)
        {
            foreach (Term term in Terms)
            {
                if (!pm.Has(term.Id, 1))
                {
                    pm.Incr(term, 1);
                }
            }
        }

        public override IEnumerable<Term> GetAffectedTerms()
        {
            return Terms;
        }
    }
}
