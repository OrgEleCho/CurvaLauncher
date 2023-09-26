#nullable enable

namespace NCalculatorLib.Exprs
{
    /// <summary>
    /// add expression :
    /// 
    /// <para>| mul plus_tail</para>
    /// <para>| unit plus_tail</para>
    /// </summary>
    public class NCalcPlusExpr : NCalcBinExpr
    {
        public NCalcPlusExpr(NCalcExpr left, NCalcPlusTailExpr tail) : base(left, tail)
        {

        }
    }
}
