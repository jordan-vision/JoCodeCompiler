
namespace SyntacticAnalyzer;

public class GrammarSymbols
{
    // Non-terminal symbols
    public readonly static string START = "START";
    public readonly static string PROG = "PROG";
    public readonly static string CLASSORIMPLORFUNCREPETITION = "CLASSORIMPLORFUNCREPETITION";
    public readonly static string CLASSORIMPLORFUNC = "CLASSORIMPLORFUNC";
    public readonly static string CLASSDECL = "CLASSDECL";
    public readonly static string PARENTCLASSDEF = "PARENTCLASSDEF";
    public readonly static string CLASSMEMBERS = "CLASSMEMBERS";
    public readonly static string PARENTCLASSLIST = "PARENTCLASSLIST";
    public readonly static string IMPLDEF = "IMPLDEF";
    public readonly static string FUNCDEFREPETITION = "FUNCDEFREPETITION";
    public readonly static string FUNCDEF = "FUNCDEF";
    public readonly static string VISIBILITY = "VISIBILITY";
    public readonly static string MEMBERDECL = "MEMBERDECL";
    public readonly static string FUNCDECL = "FUNCDECL";
    public readonly static string FUNCHEAD = "FUNCHEAD";
    public readonly static string FUNCBODY = "FUNCBODY";
    public readonly static string LOCALVARDECLORSTATEMENTREPETITION = "LOCALVARDECLORSTATEMENTREPETITION";
    public readonly static string LOCALVARDECLORSTAT = "LOCALVARDECLORSTAT";
    public readonly static string ATTRIBUTEDECL = "ATTRIBUTEDECL";
    public readonly static string LOCALVARDECL = "LOCALVARDECL";
    public readonly static string VARDECL = "VARDECL";
    public readonly static string ARRAYSIZEREPETITION = "ARRAYSIZEREPETITION";
    public readonly static string STATEMENT = "STATEMENT";
    public readonly static string STATBLOCK = "STATBLOCK";
    public readonly static string STATEMENTREPETITION = "STATEMENTREPETITION";
    public readonly static string EXPR = "EXPR";
    public readonly static string RELEXPR = "RELEXPR";
    public readonly static string ARITHEXPR = "ARITHEXPR";
    public readonly static string SIGN = "SIGN";
    public readonly static string TERM = "TERM";
    public readonly static string FACTOR = "FACTOR";
    public readonly static string VARIABLE = "VARIABLE";
    public readonly static string IDNEST = "IDNEST";
    public readonly static string INDICE = "INDICE";
    public readonly static string ARRAYSIZE = "ARRAYSIZE";
    public readonly static string TYPE = "TYPE";
    public readonly static string RETURNTYPE = "RETURNTYPE";
    public readonly static string FPARAMS = "FPARAMS";
    public readonly static string FPARAMSTAILREPETITION = "FPARAMSTAILREPETITION";
    public readonly static string APARAMS = "APARAMS";
    public readonly static string APARAMSTAILREPETITION = "APARAMSTAILREPETITION";
    public readonly static string FPARAMSTAIL = "FPARAMSTAIL";
    public readonly static string APARAMSTAIL = "APARAMSTAIL";
    public readonly static string ASSIGNOP = "ASSIGNOP";
    public readonly static string RELOP = "RELOP";
    public readonly static string ADDOP = "ADDOP";
    public readonly static string MULTOP = "MULTOP";
    public readonly static string IDORSELF = "IDORSELF";
    public readonly static string FUNCTIONCALLORASSIGNSTAT = "FUNCTIONCALLORASSIGNSTAT";
    public readonly static string FUNCTIONCALLORASSIGNSTAT2 = "FUNCTIONCALLORASSIGNSTAT2";
    public readonly static string ARITHORRELEXPR = "ARITHORRELEXPR";
    public readonly static string ARITHEXPRTAIL = "ARITHEXPRTAIL";
    public readonly static string FACTORSTAIL = "FACTORSTAIL";
    public readonly static string FUNCTIONCALLORVARIABLE = "FUNCTIONCALLORVARIABLE";
    public readonly static string PARAMSORINDICES = "PARAMSORINDICES";
    public readonly static string ARRAYSIZE2 = "ARRAYSIZE2";

    public readonly static string[] NONTERMINALS = [START, PROG, CLASSORIMPLORFUNCREPETITION, CLASSORIMPLORFUNC, CLASSDECL, 
        PARENTCLASSDEF, CLASSMEMBERS, PARENTCLASSLIST, IMPLDEF, FUNCDEFREPETITION, FUNCDEF, VISIBILITY, MEMBERDECL, FUNCDECL, FUNCHEAD, 
        FUNCBODY, LOCALVARDECLORSTATEMENTREPETITION, LOCALVARDECLORSTAT, ATTRIBUTEDECL, LOCALVARDECL, VARDECL, ARRAYSIZEREPETITION, STATEMENT, 
        STATBLOCK, STATEMENTREPETITION, EXPR, RELEXPR, ARITHEXPR, SIGN, TERM, FACTOR, VARIABLE, IDNEST, INDICE, ARRAYSIZE, TYPE, RETURNTYPE, FPARAMS, 
        FPARAMSTAILREPETITION, APARAMS, APARAMSTAILREPETITION, FPARAMSTAIL, APARAMSTAIL, ASSIGNOP, RELOP, ADDOP, MULTOP, IDORSELF, 
        FUNCTIONCALLORASSIGNSTAT, FUNCTIONCALLORASSIGNSTAT2, ARITHORRELEXPR, ARITHEXPRTAIL, FACTORSTAIL, FUNCTIONCALLORVARIABLE, PARAMSORINDICES, 
        ARRAYSIZE2];

    // Terminal sybols
    public readonly static string MINUS = "MINUS";
    public readonly static string LPAREN = "LPAREN";
    public readonly static string RPAREN = "RPAREN";
    public readonly static string AST = "AST";
    public readonly static string COMMA = "COMMA";
    public readonly static string PERIOD = "PERIOD";
    public readonly static string SOL = "SOL";
    public readonly static string COLON = "COLON";
    public readonly static string EQUIV = "EQUIV";
    public readonly static string SEMI = "SEMI";
    public readonly static string LSQB = "LSQB";
    public readonly static string RSQB = "RSQB";
    public readonly static string LCUB = "LCUB";
    public readonly static string RCUB = "RCUB";
    public readonly static string PLUS = "PLUS";
    public readonly static string LT = "LT";
    public readonly static string LEQ = "LEQ";
    public readonly static string TRI = "TRI";
    public readonly static string EQUALS = "EQUALS";
    public readonly static string LAMBDA = "LAMBDA";
    public readonly static string RT = "RT";
    public readonly static string GEQ = "GEQ";
    public readonly static string AND = "AND";
    public readonly static string ATTRIBUTE = "ATTRIBUTE";
    public readonly static string CLASS = "CLASS";
    public readonly static string CONSTRUCTOR = "CONSTRUCTOR";
    public readonly static string ELSE = "ELSE";
    public readonly static string FLOAT = "FLOAT";
    public readonly static string FLOATLIT = "FLOATLIT";
    public readonly static string FUNCTION = "FUNCTION";
    public readonly static string ID = "ID";
    public readonly static string IF = "IF";
    public readonly static string IMPLEMENTATION = "IMPLEMENTATION";
    public readonly static string INT = "INT";
    public readonly static string INTLIT = "INTLIT";
    public readonly static string ISA = "ISA";
    public readonly static string LOCAL = "LOCAL";
    public readonly static string NOT = "NOT";
    public readonly static string OR = "OR";
    public readonly static string PRIVATE = "PRIVATE";
    public readonly static string PUBLIC = "PUBLIC";
    public readonly static string READ = "READ";
    public readonly static string RETURN = "RETURN";
    public readonly static string SELF = "SELF";
    public readonly static string THEN = "THEN";
    public readonly static string VOID = "VOID";
    public readonly static string WHILE = "WHILE";
    public readonly static string WRITE = "WRITE";
    public readonly static string END = "$";

    public readonly static string[] TERMINALS = [MINUS, LPAREN, RPAREN, AST, COMMA, PERIOD, SOL, COLON, EQUIV, SEMI, LSQB, RSQB, LCUB, RCUB, PLUS, LT, LEQ, 
        TRI, EQUALS, LAMBDA, RT, GEQ, AND, ATTRIBUTE, CLASS, CONSTRUCTOR, ELSE, FLOAT, FLOATLIT, FUNCTION, ID, IF, IMPLEMENTATION, INT, INTLIT, ISA, LOCAL, 
        NOT, OR, PRIVATE, PUBLIC, READ, RETURN, SELF, THEN, VOID, WHILE, WRITE, END];

    public static Dictionary<string, string> lexemeToType = []; // Translate token to grammar symbols
    public static Dictionary<string, string> nonTerminalToErrorDetails = []; // Get error from non-terminal symbol

    public static void BuildDictionaries()
    {
        lexemeToType = new() { 
            { "-", MINUS },
            { "(", LPAREN },
            { ")", RPAREN },
            { "*", AST },
            { ",", COMMA },
            { ".", PERIOD },
            { "/", SOL },
            { ":", COLON },
            { ":=", EQUIV },
            { ";", SEMI },
            { "[", LSQB },
            { "]", RSQB },
            { "{", LCUB },
            { "}", RCUB },
            { "+", PLUS },
            { "<", LT },
            { "<=", LEQ },
            { "<>", TRI },
            { "==", EQUALS },
            { "=>", LAMBDA },
            { ">", RT },
            { ">=", GEQ },
        };

        nonTerminalToErrorDetails = new()
        {
            { START, "Begin the program with a valid class, function, or implementation."},
            { PROG, "Begin the program with a valid class, function, or implementation."},
            { CLASSORIMPLORFUNCREPETITION, "Expected a valid class, function, or implementation."},
            { CLASSORIMPLORFUNC, "Expected a valid class, function, or implementation."},
            { CLASSDECL, "Invalid class declaration."},
            { PARENTCLASSDEF, "Invalid parent class definition."},
            { CLASSMEMBERS, "Expected a visibility modifier."},
            { PARENTCLASSLIST, "Use comma separated identifiers for each parent class."},
            { IMPLDEF, "Invalid implementation definition."},
            { FUNCDEFREPETITION, "Invalid function definition."},
            { FUNCDEF, "Invalid function definition."},
            { VISIBILITY, "Expected a visibility modifier."},
            { MEMBERDECL, "Expected a valid attribute or function declaration."},
            { FUNCDECL, "Invalid function declaration."},
            { FUNCHEAD, "Invalid function head."},
            { FUNCBODY, "Invalid function body."},
            { LOCALVARDECLORSTATEMENTREPETITION, "Expected a valid statement or loval variable declaration."},
            { LOCALVARDECLORSTAT, "Expected a valid statement or loval variable declaration."},
            { ATTRIBUTEDECL, "Invalud attribute declaration."},
            { LOCALVARDECL, "Invalid local variable declaration."},
            { VARDECL, "Invalid variable declaration."},
            { ARRAYSIZEREPETITION, "Use successive square brackets to define array size."},
            { STATEMENT, "Invalid statement."},
            { STATBLOCK, "Invalid statement block."},
            { STATEMENTREPETITION, "Invalid statement."},
            { EXPR, "Invalid expression"},
            { RELEXPR, "Expected a valid comparison expression."},
            { ARITHEXPR, "Expected a valid number or arithmetic expression."},
            { SIGN, "Expected \"+\" or \"-\"."},
            { TERM, "Invalid term in expression."},
            { FACTOR, "Invalid term in expression."},
            { VARIABLE, "Invalid variable."},
            { IDNEST, "Invalid identifier."},
            { INDICE, "Invalid array indexing."},
            { ARRAYSIZE, "Expected array size declaration."},
            { TYPE, "Expected a valid type identifier."},
            { RETURNTYPE, "Expected a valid type identifier or void."},
            { FPARAMS, "Invalid function paramenter."},
            { FPARAMSTAILREPETITION, "Enter comma separated identifiers for function paramenters."},
            { APARAMS, "Invalid function argument."},
            { APARAMSTAILREPETITION, "Enter comma separated identifiers for function arguments."},
            { FPARAMSTAIL, "Enter comma separated identifiers for function paramenters."},
            { APARAMSTAIL, "Enter comma separated identifiers for function arguments."},
            { ASSIGNOP, "Expected assignment operator \":=\""},
            { RELOP, "Expected comparison operator."},
            { ADDOP, "Expected addition operator."},
            { MULTOP, "Expected multiplocation operator."},
            { IDORSELF, "Expected identifier or self."},
            { FUNCTIONCALLORASSIGNSTAT, "Invalid function call or assignment statement."},
            { FUNCTIONCALLORASSIGNSTAT2, "Invalid function call or assignment statement."},
            { ARITHORRELEXPR, "Invalid expression."},
            { ARITHEXPRTAIL, "Invalid number or arithmetic expression."},
            { FACTORSTAIL, "Invalid number or arithmetic expression."},
            { FUNCTIONCALLORVARIABLE, "Invalud function call or variable."},
            { PARAMSORINDICES, "Expected parameters or indice."},
            { ARRAYSIZE2, "Invalid array size."},
        };
    }
}
