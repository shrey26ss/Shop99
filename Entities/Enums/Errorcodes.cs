using System.ComponentModel;

namespace Entities.Enums
{
    public enum Errorcodes
    {
        [Description("Something went wrong.Please try after sometime.")]
        SomethingWentwrong = -1,
        [Description("Too Many Argument Passed To Sql Precedure")]
        TooManyArgumentPassedToSqlPrecedure = 8144,
        [Description("Invalid column name")]
        InvalidColumnName = 207,
        [Description("Invalid object name")]
        InvalidObjectName = 208,
        [Description("Incorrect syntax")]
        IncorrectSyntax = 156,
        [Description("Incorrect sql syntax")]
        IncorrectSyntaxNearEquals = 102,
        [Description("Could not find stored procedure")]
        CouldNotFindStoredProcedure = 2812,
        [Description("Must declare variable")]
        MustDeclareVariable = 137,
        [Description("Cannot insert duplicate Value")]
        ViolationOfUniqueKeyConstraint = 50000,
        [Description("Mobile number is already exists.")]
        CredentialAlreadyExists = 3903,
        [Description("Cannot insert duplicate Value")]
        RowAlreadyExists = 2627,
        [Description("There are more columns in the INSERT statement than values specified in the VALUES clause.")]
        MoreColumnsInTheStatementThanValuesSpecified = 109
    }
}
