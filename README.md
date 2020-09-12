# Compiladores2020
Proyecto... Walter Orozco/Ricardo Chian

# Laboratorio A :white_check_mark:

El laboratorio consistia en realizar un Analisis Sintactico Descendente Recursivo, el objetivo del mismo es realizar el analizis sobre el lenguaje asignado que en este caso es MiniC, además de que en la gramatica nos toco las producciones de IF, FOR y EXPR.

# Manejo de Errores :no_entry_sign:

- El analizador de MiniC cuenta con un manejo de errores el cual consiste en una gramática que se va verificando con la información que contiene el archivo y realizando recursividad en cada uno de los parseos, para realizar un match entre el token esperado y el token que se esta leyendo del archivo, si en dado caso no realiza el match entonces se producira un error.
- Luego de encontrar el error el programa se recupera de la siguiente forma, guarda el error que se encontro respecto a los match token y luego realiza con un nuevo token el parseo nuevamente para que el programa siga con su flujo y este no se quede parado.
- La estrategia implementada para este laboratorio consistio de primero realizar una revision de la gramática, rediseñarla y luego realizar los parseos respecto a los no terminales y realizar un lookahad, un mathtoken y un backtracking para manejar los errores y verificar uno a uno la entrada de cada token del archivo analizado con el flujo del programa. 

# Gramática Original :bulb:

Program → Decl+
- Decl → VariableDecl | FunctionDecl
- VariableDecl  → variable ;
- Variable → Type ident
- Type → int | double | bool | string | ident | Type []
- FunctionDecl → Type ident ( Formals ) Stmt* | void ident ( Formals ) Stmt*
- Formals → Variable+ , | eps
- Stmt → IfStmt | ForStmt | Expr ;
- IfStmt → if ( Expr ) Stmt < else Stmt >
- ForStmt → for ( <Expr> ; Expr ; <Expr> ) Stmt
- Expr → LValue = Expr | Constant | LValue | this | ( Expr ) | Expr + Expr | Expr - Expr | 
- Expr * Expr | Expr / Expr | Expr % Expr | - Expr | Expr < Expr | Expr <= Expr |
- Expr > Expr | Expr >= Expr | Expr == Expr | Expr != Expr | Expr && Expr |
- Expr || Expr | ! Expr | New (ident) | 
- LValue → ident | Expr . ident | Expr [ Expr ]
- Constant → intConstant | doubleConstant | boolConstant | stringConstant | null

# Gramática Nueva :new:

- Program → Decl
- Decl → VariableDecl Decl’ | FunctionDecl Decl’
- Decl’ → VariableDecl Decl’ | FunctionDecl Decl’ | eps
- VariableDecl  → Variable ;
- Variable → Type ident
- Type → Type’ Type2
- Type’ → int | double | bool | string | ident 
- Type2 → [] Type2 | eps
- FunctionDecl → Type ident ( Formals ) Stmt’ | void ident ( Formals ) Stmt’
- Stmt’ → Stmt Stmt’ | eps
- Formals → Variable Variable’ , | eps
- Variable’ →  Variable Variable’ | eps
- Stmt → IfStmt | ForStmt | Expr ;
- IfStmt → if ( Expr ) Stmt IfStmt’
- IfStmt’ → else Stmt | eps
- ForStmt → for ( ForStmt’  ; Expr ; ForStmt’  ) Stmt
- ForStmt’ → Expr | eps
- Expr →  Expr’ Expr1
- Expr1 → || Expr’ Expr1 | eps
- Expr’ → Expr2’ Expr2
- Expr2 → && Expr2’ Expr2 | eps
- Expr2’ → Expr3’ Expr3
- Expr3 → == Expr3’ Expr3 | != Expr3’ Expr3 | eps
- Expr3’ → Expr4’ Expr4
- Expr4 → < Expr4’ Expr4 | > Expr4’ Expr4 | <= Expr4’ Expr4 | >= Expr4’ Expr4 | eps
- Expr4’ →  Expr5’ Expr5
- Expr5 → + Expr5’ Expr5 | - Expr5’ Expr5 | eps
- Expr5’ → Expr6’ Expr6
- Expr6 → * Expr6’ Expr6 | / Expr6’ Expr6 | % Expr6’ Expr6 | eps
- Expr6’ → LValue = Expr |  Constant | LValue | this | - Expr | ! Expr | ( Expr ) |  New (ident) | eps
- LValue → ident | Expr LValue’
- LValue’ → . ident | [ Expr ]
- Constant → intConstant | doubleConstant | boolConstant | stringConstant | null


# Comentarios :newspaper:

- Para el rediseño de la gramática se realizaron varios cambios debido a que traia expresiones con *,+ y <>.
- Se cambiaron tambien debido a que existia recursividad, factorizacion y ambiguedad que se podian eliminar.
- Finalmente se agregaron las nuevas producciones que no contienen ningun tipo de problema que podria cambiar la gramática para el analizador.
