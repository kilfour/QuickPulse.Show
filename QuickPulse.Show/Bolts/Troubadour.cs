using System.Linq.Expressions;
using System.Reflection;

namespace QuickPulse.Show.Bolts;

public class Troubadour<T>(ManOfWealthAndTaste manOfWealthAndTaste, Puzzles puzzles)
{
    private readonly ManOfWealthAndTaste manOfWealthAndTaste = manOfWealthAndTaste;
    private readonly Puzzles puzzles = puzzles;

    public ManOfWealthAndTaste Use(Func<T, string> formatter)
    {
        puzzles.RegisterTypeFormatter(formatter);
        return manOfWealthAndTaste;
    }

    public ManOfWealthAndTaste Ignore<TProp>(Expression<Func<T, TProp>> expr)
    {
        var member = AsMemberInfo(expr);
        if (member is FieldInfo field) { puzzles.RegisterFieldToIgnore<T>(field); }
        if (member is PropertyInfo prop) { puzzles.RegisterPropertyToIgnore<T>(prop); }
        return manOfWealthAndTaste;
    }

    private static MemberInfo AsMemberInfo<TTarget, TMember>(Expression<Func<TTarget, TMember>> expression)
    {
        if (expression.Body is MemberExpression memberExpr)
        {
            return memberExpr.Member;
        }

        if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
        {
            return unaryMember.Member;
        }

        throw new ArgumentException($"Expression '{expression}' does not refer to a field or property.");
    }
}
