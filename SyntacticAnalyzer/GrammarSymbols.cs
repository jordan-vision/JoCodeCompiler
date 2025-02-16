
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

    // Translate text to grammar symbols
    public static Dictionary<string, string> lexemeToType = [];

    public static void BuildDictionary()
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
    }
}
