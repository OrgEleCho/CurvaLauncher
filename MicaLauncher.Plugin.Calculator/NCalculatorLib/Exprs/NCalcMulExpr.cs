#nullable enable

namespace NCalculatorLib.Exprs
{
    /// <summary>
    /// multiply expression :
    /// 
    /// <para>| unit mul_tail</para>
    /// </summary>
    public class NCalcMulExpr : NCalcBinExpr
    {
        public NCalcMulExpr(NCalcExpr left, NCalcMulTailExpr right) : base(left, right)
        {

        }
    }
}
