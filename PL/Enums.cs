using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    internal class EngineerExperience : IEnumerable
    {
        static readonly IEnumerable<BO.EngineerExperience> s_enums =
    (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    internal class EngineerExperienceOnlyLevels : IEnumerable
    {
        // Assuming BO.EngineerExperience is your enum in the Business Layer
        static readonly IEnumerable<BO.EngineerExperience> s_enums =
            Enum.GetValues(typeof(BO.EngineerExperience))
                .Cast<BO.EngineerExperience>()
                .Where(e => e != BO.EngineerExperience.All);

        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
}
