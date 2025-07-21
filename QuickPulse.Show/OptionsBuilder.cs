using System.Linq.Expressions;
using System.Reflection;

namespace QuickPulse.Show;

public class OptionsBuilder<T>
{
    private readonly TypeBuilder typeBuilder;
    private readonly Options options;

    public OptionsBuilder(TypeBuilder typeBuilder, Options options)
    {
        this.typeBuilder = typeBuilder;
        this.options = options;
    }

    public TypeBuilder Use(Func<T, string> formatter)
    {
        options.RegisterTypeFormatter(formatter);
        return typeBuilder;
    }

    public TypeBuilder Ignore<TProp>(Expression<Func<T, TProp>> expr)
    {
        var member = AsMemberInfo(expr);
        if (member is FieldInfo field) { options.RegisterFieldToIgnore<T>(field); }
        if (member is PropertyInfo prop) { options.RegisterPropertyToIgnore<T>(prop); }
        return typeBuilder;
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
