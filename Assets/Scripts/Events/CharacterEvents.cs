using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Events
{
    public class CharacterEvents
    {
        // character damaged and damaged value
        public static UnityAction<GameObject, int> characterDamaged;

        // character healed and healed value
        public static UnityAction<GameObject, int> characterHealed;
    }
}
