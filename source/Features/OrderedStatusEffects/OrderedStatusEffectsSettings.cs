using static BattleTech.StatCollection;

namespace MechEngineer.Features.OrderedStatusEffects
{
    internal class OrderedStatusEffectsSettings : ISettings
    {
        public bool Enabled { get; set; } = true;
        public string EnabledDescription => "Orders status effects, so e.g. install order of mechComponents does not matter for additive and multiplicative effects. Affects all statistic data.";

        public StatOperation[] Order = {
            StatOperation.Int_Add,
            StatOperation.Float_Add,

            StatOperation.Int_Multiply,
            StatOperation.Int_Multiply_Float,
            StatOperation.Float_Multiply,
            StatOperation.Float_Multiply_Int,

            StatOperation.Int_Subtract,
            StatOperation.Float_Subtract,

            StatOperation.Int_Divide,
            StatOperation.Int_Divide_Float,
            StatOperation.Float_Divide,
            StatOperation.Float_Divide_Int,

            StatOperation.Int_Divide_Denom,
            StatOperation.Int_Divide_Denom_Float,
            StatOperation.Float_Divide_Denom,
            StatOperation.Float_Divide_Denom_Int
        };
        public string OrderDescription => "The order in which operations are sorted. Operations not part of the list are deemded unsortable and only sortable operations after such a unsortable operations are sorted. For Example: '* + SET / * +' =sort=> '* + SET + * /'.";
    }
}